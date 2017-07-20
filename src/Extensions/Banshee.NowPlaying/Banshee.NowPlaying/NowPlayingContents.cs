//
// NowPlayingContents.cs
//
// Author:
//   Aaron Bockover <abockover@novell.com>
//
// Copyright (C) 2008 Novell, Inc.
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using Gtk;

using Banshee.Gui;
using Banshee.Gui.Widgets;
using Banshee.ServiceStack;
using Hyena;

namespace Banshee.NowPlaying
{
    public class NowPlayingContents : Table, IDisposable
    {
        private static XOverlayVideoDisplay video_display;

        public static void CreateVideoDisplay ()
        {
            if (video_display == null) {
                video_display = new XOverlayVideoDisplay ();
            }
        }

        public static void SetVideoContext ()
        {
            video_display.SetVideoContext ();
        }

        private EventBox video_event;

        private TrackInfoDisplay track_info_display;

        public NowPlayingContents () : base (1, 1, false)
        {
            NoShowAll = true;

            CreateVideoDisplay ();

            video_event = new EventBox ();
            video_event.VisibleWindow = false;
            video_event.CanFocus = true;
            video_event.AboveChild = true;
            video_event.Add (video_display);
            video_event.Events |= Gdk.EventMask.PointerMotionMask |
                    Gdk.EventMask.ButtonPressMask |
                    Gdk.EventMask.ButtonMotionMask |
                    Gdk.EventMask.KeyPressMask |
                    Gdk.EventMask.KeyReleaseMask;

            //TODO stop tracking mouse when no more in menu
            video_event.ButtonPressEvent += OnButtonPress;
            video_event.ButtonReleaseEvent += OnButtonRelease;
            video_event.MotionNotifyEvent += OnMouseMove;
            video_event.KeyPressEvent += OnKeyPress;

            video_display.IdleStateChanged += OnVideoDisplayIdleStateChanged;

            Attach (video_event, 0, 1, 0, 1,
                AttachOptions.Expand | AttachOptions.Fill,
                AttachOptions.Expand | AttachOptions.Fill, 0, 0);

            track_info_display = new NowPlayingTrackInfoDisplay ();
            Attach (track_info_display, 0, 1, 0, 1,
                AttachOptions.Expand | AttachOptions.Fill,
                AttachOptions.Expand | AttachOptions.Fill, 0, 0);

            CheckIdle ();
        }

        protected override void Dispose (bool disposing)
        {
            if (disposing) {
                video_event.ButtonPressEvent -= OnButtonPress;
                video_event.ButtonReleaseEvent -= OnButtonRelease;
                video_event.MotionNotifyEvent -= OnMouseMove;
                video_event.KeyPressEvent -= OnKeyPress;

                video_display.IdleStateChanged -= OnVideoDisplayIdleStateChanged;

                video_display = null;
            }

            base.Dispose (disposing);
        }

        protected override void OnShown ()
        {
            base.OnShown ();
            video_event.GrabFocus ();
        }

        private void CheckIdle ()
        {
            Log.Debug ("Contents.CheckIdle ()");

            video_event.Visible = video_display.Visible = !video_display.IsIdle;
            track_info_display.Visible = !video_event.Visible;
        }

        private void OnVideoDisplayIdleStateChanged (object o, EventArgs args)
        {
            CheckIdle ();
        }

        [GLib.ConnectBefore]
        void OnMouseMove (object o, MotionNotifyEventArgs args)
        {
            if (ServiceManager.PlayerEngine.InDvdMenu) {
                ServiceManager.PlayerEngine.NotifyMouseMove (args.Event.X, args.Event.Y);
            }
        }

        [GLib.ConnectBefore]
        void OnButtonPress (object o, ButtonPressEventArgs args)
        {
            switch (args.Event.Type) {
                case Gdk.EventType.TwoButtonPress:
                    var iaservice = ServiceManager.Get<InterfaceActionService> ();
                    var action = iaservice.ViewActions["FullScreenAction"] as Gtk.ToggleAction;
                    if (action != null && action.Sensitive) {
                        action.Active = !action.Active;
                    }
                    break;
                case Gdk.EventType.ButtonPress:
                    video_event.GrabFocus ();
                    if (ServiceManager.PlayerEngine.InDvdMenu) {
                        ServiceManager.PlayerEngine.NotifyMouseButtonPressed ((int)args.Event.Button, args.Event.X, args.Event.Y);
                    }
                    break;
            }
        }

        [GLib.ConnectBefore]
        void OnButtonRelease (object o, ButtonReleaseEventArgs args)
        {
            switch (args.Event.Type) {
                case Gdk.EventType.ButtonRelease:
                    if (ServiceManager.PlayerEngine.InDvdMenu) {
                        ServiceManager.PlayerEngine.NotifyMouseButtonReleased ((int)args.Event.Button, args.Event.X, args.Event.Y);
                    }
                    break;
            }
        }

        [GLib.ConnectBefore]
        void OnKeyPress (object o, KeyPressEventArgs args)
        {
            if (!ServiceManager.PlayerEngine.InDvdMenu) {
                return;
            }
            switch (args.Event.Key) {
                case Gdk.Key.leftarrow:
                case Gdk.Key.KP_Left:
                case Gdk.Key.Left:
                    ServiceManager.PlayerEngine.NavigateToLeftMenu ();
                    args.RetVal = true;
                    break;
                case Gdk.Key.rightarrow:
                case Gdk.Key.KP_Right:
                case Gdk.Key.Right:
                    ServiceManager.PlayerEngine.NavigateToRightMenu ();
                    args.RetVal = true;
                    break;
                case Gdk.Key.uparrow:
                case Gdk.Key.KP_Up:
                case Gdk.Key.Up:
                    ServiceManager.PlayerEngine.NavigateToUpMenu ();
                    args.RetVal = true;
                    break;
                case Gdk.Key.downarrow:
                case Gdk.Key.KP_Down:
                case Gdk.Key.Down:
                    ServiceManager.PlayerEngine.NavigateToDownMenu ();
                    args.RetVal = true;
                    break;
                case Gdk.Key.Break:
                case Gdk.Key.KP_Enter:
                case Gdk.Key.Return:
                    ServiceManager.PlayerEngine.ActivateCurrentMenu ();
                    args.RetVal = true;
                    break;
            }
        }
    }
}

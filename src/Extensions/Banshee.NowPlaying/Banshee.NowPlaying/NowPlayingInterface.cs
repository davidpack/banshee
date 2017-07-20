//
// NowPlayingInterface.cs
//
// Author:
//   Aaron Bockover <abockover@novell.com>
//
// Copyright 2008-2010 Novell, Inc.
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

using Mono.Unix;
using Gtk;

using Banshee.Gui;
using Banshee.PlatformServices;
using Banshee.ServiceStack;
using Banshee.Sources;
using Banshee.Sources.Gui;

namespace Banshee.NowPlaying
{
    public class NowPlayingInterface : VBox, ISourceContents
    {
        private NowPlayingSource source;
        private Hyena.Widgets.RoundedFrame frame;
        private Gtk.Window fullscreen_window;
        private Gtk.Window primary_window;
        private FullscreenAdapter fullscreen_adapter;
        private ScreensaverManager screensaver;
        
        internal NowPlayingContents Contents { get; private set; }

        public NowPlayingInterface ()
        {
            GtkElementsService service = ServiceManager.Get<GtkElementsService> ();
            primary_window = service.PrimaryWindow;

            Contents = new NowPlayingContents ();

            fullscreen_window = new FullscreenWindow (primary_window);
            fullscreen_window.Hidden += OnFullscreenWindowHidden;

            frame = new Hyena.Widgets.RoundedFrame ();
            frame.SetFillColor (new Cairo.Color (0, 0, 0));
            frame.DrawBorder = false;
            frame.Child = Contents;
            frame.Show ();

            PackStart (frame, true, true, 0);

            fullscreen_adapter = new FullscreenAdapter ();
            fullscreen_adapter.SuggestUnfullscreen += OnAdapterSuggestUnfullscreen;
            screensaver = new ScreensaverManager ();
        }

        protected override void Dispose (bool disposing)
        {
            if (disposing) {
                fullscreen_adapter.SuggestUnfullscreen -= OnAdapterSuggestUnfullscreen;
                fullscreen_adapter.Dispose ();
                screensaver.Dispose ();
            }
            base.Dispose (disposing);
        }

        private void MoveVideoExternal ()
        {
            Contents.Reparent (fullscreen_window);
            Contents.Show ();
        }

        private void MoveVideoInternal ()
        {
            Contents.Reparent (frame);
            Contents.Show ();
        }

#region Video Fullscreen Override

        private ViewActions.FullscreenHandler previous_fullscreen_handler;
        private bool primary_window_is_fullscreen;

        private void DisableFullscreenAction ()
        {
            InterfaceActionService service = ServiceManager.Get<InterfaceActionService> ();
            Gtk.ToggleAction action = service.ViewActions["FullScreenAction"] as Gtk.ToggleAction;
            if (action != null) {
                action.Active = false;
            }
        }

        internal void OverrideFullscreen ()
        {
            FullscreenHandler (false);

            InterfaceActionService service = ServiceManager.Get<InterfaceActionService> ();
            if (service?.ViewActions == null) {
                return;
            }

            previous_fullscreen_handler = service.ViewActions.Fullscreen;
            primary_window_is_fullscreen = (primary_window.Window.State & Gdk.WindowState.Fullscreen) != 0;
            service.ViewActions.Fullscreen = FullscreenHandler;
            DisableFullscreenAction ();
        }

        internal void RelinquishFullscreen ()
        {
            FullscreenHandler (false);

            InterfaceActionService service = ServiceManager.Get<InterfaceActionService> ();
            if (service?.ViewActions == null) {
                return;
            }

            service.ViewActions.Fullscreen = previous_fullscreen_handler;
        }

        private void OnFullscreenWindowHidden (object o, EventArgs args)
        {
            MoveVideoInternal ();
            DisableFullscreenAction ();
        }

        private bool is_fullscreen;

        private void FullscreenHandler (bool fullscreen)
        {
            // Note: Since the video window is override-redirect, we
            // need to fullscreen the main window, so the window manager
            // actually knows we are actually doing stuff in fullscreen
            // here. The original primary window fullscreen state is
            // stored, so when we can restore it appropriately

            is_fullscreen = fullscreen;

            if (fullscreen) {
                primary_window.Fullscreen ();
                fullscreen_window.Show ();
                fullscreen_adapter.Fullscreen (fullscreen_window, true);
                screensaver.Inhibit ();
                MoveVideoExternal ();
            } else {
                MoveVideoInternal ();
                screensaver.UnInhibit ();
                fullscreen_adapter.Fullscreen (fullscreen_window, false);
                fullscreen_window.Hide ();
                if (!primary_window_is_fullscreen) {
                    primary_window.Unfullscreen ();
                }
            }
        }

        private void OnAdapterSuggestUnfullscreen (object o, EventArgs args)
        {
            if (is_fullscreen) {
                FullscreenHandler (false);
            }
        }

#endregion

#region ISourceContents

        public bool SetSource (ISource src)
        {
            this.source = src as NowPlayingSource;
            return this.source != null;
        }

        public ISource Source {
            get { return source; }
        }

        public void ResetSource ()
        {
            source = null;
        }

        public Widget Widget {
            get { return this; }
        }

#endregion

    }
}

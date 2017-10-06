// 
// HeaderBox.cs
// 
// Author:
//   Aaron Bockover <abockover@novell.com>
// 
// Copyright 2010 Novell, Inc.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;

using Gtk;

using Hyena.Gui;

namespace Banshee.MediaPanel
{
    public class HeaderBox : VBox
    {
        [Flags]
        public enum HighlightFlags
        {
            None = 0,
            Background = 1,
            TopLine = 2,
            BottomLine = 4
        }

        private Dictionary<Widget, HighlightFlags> highlight_widgets = new Dictionary<Widget, HighlightFlags> ();

        private Alignment header;
        private Label header_label;
        private string header_label_text;

        protected HeaderBox (IntPtr raw) : base (raw)
        {
        }

        public HeaderBox ()
        {
            BorderWidth = 5;
            Spacing = 5;
            RedrawOnAllocate = true;
            AppPaintable = true;

            header = new Alignment (0.0f, 0.5f, 1.0f, 1.0f) {
                LeftPadding = 10,
                RightPadding = 10,
                TopPadding = 5,
                BottomPadding = 5
            };
            header_label = new Label () { Xalign = 0.0f };
            header.Add (header_label);
            header.ShowAll ();
            PackStart (header, false, false, 0);
        }

        public void PackStartHighlighted (Widget child, bool expand, bool fill, uint padding, HighlightFlags highlightFlags)
        {
            PackStart (child, expand, fill, padding);
            if (highlightFlags != HighlightFlags.None) {
                highlight_widgets.Add (child, highlightFlags);
            }
        }

        protected override void OnRemoved (Widget widget)
        {
            base.OnRemoved (widget);
            highlight_widgets.Remove (widget);
        }

        public string Title {
            get { return header_label_text; }
            set {
                header_label_text = value;
                header_label.Markup = String.Format ("<span font_desc=\"Droid Sans Bold\" " +
                    "size=\"large\" foreground=\"#616261\">{0}</span>",
                    GLib.Markup.EscapeText (value));
            }
        }
    }
}


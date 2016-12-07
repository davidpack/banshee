//
// SearchableTreeView.cs
//
// Author:
//   Nicholas Little <arealityfarbetween@googlemail.com>
//
// Copyright (C) 2017
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
using Hyena.Data.Gui;
using Hyena.Query;
using Hyena.Widgets;
using Banshee.Collection.Database;
using Hyena.Collections;

namespace Banshee.Collection.Gui
{
    public class SearchableTreeView<T> : TreeView<T>
    {
        private QueryHelper<T> query_helper;
        
        protected SearchableTreeView () : base ()
        {
            query_helper = new QueryHelper<T> (this);
        }

        protected SearchableTreeView (IntPtr raw) : base (raw)
        {
        }

        public virtual bool SelectOnRowFound {
            get { return true; }
        }

        protected override bool OnKeyPressEvent (Gdk.EventKey press)
        {
            return query_helper.OnKeyPressEvent (press);
        }

        private void OnPopupKeyPressed (object sender, KeyPressEventArgs args)
        {
            query_helper.OnPopupKeyPressed (sender, args);
        }
    }
}

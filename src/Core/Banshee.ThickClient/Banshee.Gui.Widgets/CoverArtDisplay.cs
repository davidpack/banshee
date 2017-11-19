//
// CoverArtdisplay.cs
//
// Author:
//   Gabriel Burt <gburt@novell.com>
//
// Copyright (C) 2009 Novell, Inc.
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

using Cairo;

using Banshee.Collection;
using Banshee.Collection.Gui;

namespace Banshee.Gui.Widgets
{
    public class CoverArtDisplay : TrackInfoDisplay
    {
        protected override int ArtworkSizeRequest {
            get { return Allocation.Width; }
        }

        protected override void RenderTrackInfo (Context cr, TrackInfo track, bool renderTrack, bool renderArtistAlbum)
        {
        }

        protected override bool CanRenderIdle {
            get { return true; }
        }

        protected override void RenderIdle (Context cr)
        {
            ArtworkRenderer.RenderThumbnail (cr,
                image:      null, // Uses the Banshee vector logo
                dispose:    false,
                x:          0,
                y:          0,
                width:      ArtworkSizeRequest,
                height:     ArtworkSizeRequest,
                drawBorder: false,
                radius:     0,
                fill:       true,
                fillColor:  BackgroundColor);
        }
    }
}

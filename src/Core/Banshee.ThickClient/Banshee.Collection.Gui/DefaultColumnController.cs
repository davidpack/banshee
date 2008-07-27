//
// DefaultColumnController.cs
//
// Author:
//   Aaron Bockover <abockover@novell.com>
//
// Copyright (C) 2007-2008 Novell, Inc.
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

using Hyena.Query;
using Hyena.Gui;
using Hyena.Gui.Theming;
using Hyena.Gui.Theatrics;
using Hyena.Data.Gui;

using Banshee.Gui;
using Banshee.ServiceStack;
using Banshee.MediaEngine;
using Banshee.Collection;
using Banshee.Query;

namespace Banshee.Collection.Gui
{
    public class DefaultColumnController : PersistentColumnController
    {
        public DefaultColumnController () : this (true)
        {
        }
        
        public DefaultColumnController (bool loadDefault) : base (String.Format ("{0}.{1}", 
            Application.ActiveClient.ClientId, "track_view_columns"))
        {
            CreateDefaultColumns ();

            if (loadDefault) {
                AddDefaultColumns ();
                DefaultSortColumn = TrackColumn;
                Load ();
            }
        }
        
        public void AddDefaultColumns ()
        {
            AddRange (
                IndicatorColumn,
                TrackColumn,
                TitleColumn,
                ArtistColumn,
                AlbumColumn,
                RatingColumn,
                DurationColumn,
                GenreColumn,
                YearColumn,
                FileSizeColumn,
                ComposerColumn,
                PlayCountColumn,
                SkipCountColumn,
                DiscColumn,
                LastPlayedColumn,
                LastSkippedColumn,
                DateAddedColumn,
                UriColumn,
                MimeTypeColumn
            );
        }

        private void CreateDefaultColumns ()
        {
            indicator_column = new Column (null, "indicator", new ColumnCellStatusIndicator (null), 0.05, true, 30, 30);

            // Visible-by-default column
            track_column        = Create (BansheeQuery.TrackNumberField, 0.10, true, new ColumnCellTrackNumber (null, true));
            title_column        = CreateText (BansheeQuery.TitleField, 0.25, true);
            artist_column       = CreateText (BansheeQuery.ArtistField, 0.225, true);
            album_column        = CreateText (BansheeQuery.AlbumField, 0.225, true);

            // Others
            genre_column        = CreateText (BansheeQuery.GenreField, 0.25);

            duration_column     = Create (BansheeQuery.DurationField, 0.10, true, new ColumnCellDuration (null, true));
            year_column         = Create (BansheeQuery.YearField, 0.15, false, new ColumnCellPositiveInt (null, true));
            file_size_column    = Create (BansheeQuery.FileSizeField, 0.15, false, new ColumnCellFileSize (null, true));
            disc_column         = Create (BansheeQuery.DiscField, 0.10, false, new ColumnCellPositiveInt (null, true));
            rating_column       = Create (BansheeQuery.RatingField, 0.15, false, new ColumnCellRating (null, true));

            composer_column     = CreateText (BansheeQuery.ComposerField, 0.25);
            comment_column      = CreateText (BansheeQuery.CommentField, 0.25);
            play_count_column   = CreateText (BansheeQuery.PlayCountField, 0.15);
            skip_count_column   = CreateText (BansheeQuery.SkipCountField, 0.15);
            uri_column          = CreateText (BansheeQuery.UriField, 0.15);
            mime_type_column    = CreateText (BansheeQuery.MimeTypeField, 0.15);

            last_played_column  = Create (BansheeQuery.LastPlayedField, 0.15, false, new ColumnCellDateTime (null, true));
            last_skipped_column = Create (BansheeQuery.LastSkippedField, 0.15, false, new ColumnCellDateTime (null, true));
            date_added_column   = Create (BansheeQuery.DateAddedField, 0.15, false, new ColumnCellDateTime (null, true));
        }

        private SortableColumn CreateText (QueryField field, double width)
        {
            return CreateText (field, width, false);
        }

        private SortableColumn CreateText (QueryField field, double width, bool visible)
        {
            return Create (field, width, visible, new ColumnCellText (field.PropertyName, true));
        }

        private SortableColumn Create (QueryField field, double width, bool visible, ColumnCell cell)
        {
            cell.Property = field.PropertyName;
            SortableColumn col = new SortableColumn (
                field.Label,
                cell,
                width, field.Name, visible
            );
            col.Field = field;

            return col;
        }
        
#region Column Properties

        private Column indicator_column;
        public Column IndicatorColumn {
            get { return indicator_column; }
        }
        
        private SortableColumn track_column;
        public SortableColumn TrackColumn {
            get { return track_column; }
        }
        
        private SortableColumn title_column;
        public SortableColumn TitleColumn {
            get { return title_column; }
        }

        private SortableColumn artist_column;
        public SortableColumn ArtistColumn {
            get { return artist_column; }
        }
        
        private SortableColumn album_column;
        public SortableColumn AlbumColumn {
            get { return album_column; }
        }
        
        private SortableColumn duration_column;
        public SortableColumn DurationColumn {
            get { return duration_column; }
        }
        
        private SortableColumn genre_column;
        public SortableColumn GenreColumn {
            get { return genre_column; }
        }
        
        private SortableColumn year_column;
        public SortableColumn YearColumn {
            get { return year_column; }
        }

        private SortableColumn file_size_column;
        public SortableColumn FileSizeColumn {
            get { return file_size_column; }
        }
        
        private SortableColumn composer_column;
        public SortableColumn ComposerColumn {
            get { return composer_column; }
        }
        
        private SortableColumn comment_column;
        public SortableColumn CommentColumn {
            get { return comment_column; }
        }
        
        private SortableColumn play_count_column;
        public SortableColumn PlayCountColumn {
            get { return play_count_column; }
        }
        
        private SortableColumn skip_count_column;
        public SortableColumn SkipCountColumn {
            get { return skip_count_column; }
        }
        
        private SortableColumn disc_column;
        public SortableColumn DiscColumn {
            get { return disc_column; }
        }
        
        private SortableColumn rating_column;
        public SortableColumn RatingColumn {
            get { return rating_column; }
        }
        
        private SortableColumn last_played_column;
        public SortableColumn LastPlayedColumn {
            get { return last_played_column; }
        }
        
        private SortableColumn last_skipped_column;
        public SortableColumn LastSkippedColumn {
            get { return last_skipped_column; }
        }
        
        private SortableColumn date_added_column;
        public SortableColumn DateAddedColumn {
            get { return date_added_column; }
        }
        
        private SortableColumn uri_column;
        public SortableColumn UriColumn {
            get { return uri_column; }
        }
        
        private SortableColumn mime_type_column;
        public SortableColumn MimeTypeColumn {
            get { return mime_type_column; }
        }

#endregion

    }
}

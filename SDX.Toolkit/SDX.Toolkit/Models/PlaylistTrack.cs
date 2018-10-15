using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.Media.Playback;

using Newtonsoft.Json;
using System.ComponentModel;
using GalaSoft.MvvmLight;
using Windows.UI.Xaml;

namespace SDX.Toolkit.Models
{
    public sealed class PlaylistTrack: DependencyObject
    {

        public static readonly DependencyProperty ArtistNameProperty =
            DependencyProperty.Register("ArtistName", typeof(string), typeof(PlaylistTrack), new PropertyMetadata(""));

        public string ArtistName
        {
            get { return (string)GetValue(ArtistNameProperty); }
            set { SetValue(ArtistNameProperty, value); }
        }

        public static readonly DependencyProperty TrackTitleProperty =
    DependencyProperty.Register("TrackTitle", typeof(string), typeof(PlaylistTrack), new PropertyMetadata(""));

        public string TrackTitle
        {
            get { return (string)GetValue(TrackTitleProperty); }
            set { SetValue(TrackTitleProperty, value); }
        }

        public static readonly DependencyProperty MediaSourceUriProperty =
    DependencyProperty.Register("MediaSourceUri", typeof(string), typeof(PlaylistTrack), new PropertyMetadata(""));

        public string MediaSourceUri
        {
            get { return (string)GetValue(MediaSourceUriProperty); }
            set { SetValue(MediaSourceUriProperty, value); }
        }

        public static readonly DependencyProperty CoverArtSourceUriProperty =
    DependencyProperty.Register("CoverArtSourceUri", typeof(string), typeof(PlaylistTrack), new PropertyMetadata(""));

        public string CoverArtSourceUri
        {
            get { return (string)GetValue(CoverArtSourceUriProperty); }
            set { SetValue(CoverArtSourceUriProperty, value); }
        }

        public static readonly DependencyProperty SelectedCoverArtSourceUriProperty =
        DependencyProperty.Register("SelectedCoverArtSourceUri", typeof(string), typeof(PlaylistTrack), new PropertyMetadata(""));

        public string SelectedCoverArtSourceUri
        {
            get { return (string)GetValue(SelectedCoverArtSourceUriProperty); }
            set { SetValue(SelectedCoverArtSourceUriProperty, value); }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.Media.Playback;

using Newtonsoft.Json;


namespace SDX.Toolkit.Models
{
    public sealed class PlaylistTrack
    {
        public string ArtistName;
        public string TrackTitle;
        public string MediaSourceUri;
        public string CoverArtSourceUri;
    }
}

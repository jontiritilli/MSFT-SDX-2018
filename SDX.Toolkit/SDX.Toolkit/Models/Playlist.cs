using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDX.Toolkit.Models
{
    public sealed class Playlist
    {
        // public properties
        public int SelectedIndex = 0;
        public ObservableCollection<PlaylistTrack> Tracks = new ObservableCollection<PlaylistTrack>();


        // static methods
        public static Playlist CreateDefault()
        {
            return new Playlist();
        }
    }
}

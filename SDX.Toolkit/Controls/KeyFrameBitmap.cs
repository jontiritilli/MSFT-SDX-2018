using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace SDX.Toolkit.Controls
{
    public class KeyFrameBitmap
    {
        #region Private Members

        private int _animationOrder = 0;
        private int _frameDurationInMilliseconds;
        private bool _isLoaded = false;
        private Uri _sourceUri = null;
        private BitmapImage _keyFrame = null;

        #endregion

        #region Constructors / Initialization

        public KeyFrameBitmap()
        {
        }

        public KeyFrameBitmap(int animationOrder, int frameDurationInMilliseconds, string assetsFilename)
        {
            _animationOrder = animationOrder;
            _frameDurationInMilliseconds = frameDurationInMilliseconds;
            _sourceUri = new Uri(@"ms-appx:///" + assetsFilename);
            _keyFrame = null;

            Initialization = LoadBitMapFromFile(_sourceUri);
        }

        #endregion

        #region Public Properties

        // used for async image loading
        public Task Initialization { get; private set; }

        public int AnimationOrder
        {
            get { return _animationOrder; }
            set { _animationOrder = value; }
        }

        public int FrameDurationInMilliseconds
        {
            get { return _frameDurationInMilliseconds; }
            set { _frameDurationInMilliseconds = value; }
        }

         public Uri SourceUri
        {
            get { return _sourceUri; }
            set { _sourceUri = value; }
        }

        public BitmapImage KeyFrame
        {
            get { return _keyFrame; }
            set { _keyFrame = value; }
        }

        public bool IsLoaded
        {
            get { return _isLoaded; }
            set { _isLoaded = value; }
        }

        #endregion

        #region Event Handlers

        //private void OnImageLoaded(object sender, RoutedEventArgs e)
        //{
        //    _isLoaded = true;
        //}

        //private void OnImageFailed(object sender, RoutedEventArgs e)
        //{
        //    _isLoaded = false;
        //}

        #endregion

        #region UI Helpers

        private async Task LoadBitMapFromFile(Uri uri)
        {
            // create the keyframe
            this.KeyFrame = new BitmapImage();

            try
            {
                // get a file pointer
                StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(uri);

                // load the file async, but wait on it
                using (var stream = await file.OpenReadAsync())
                {
                    await _keyFrame.SetSourceAsync(stream);

                    _isLoaded = true;
                }
            }
            catch (Exception e)
            {
                _isLoaded = false;
            }
        }

        #endregion
    }
}

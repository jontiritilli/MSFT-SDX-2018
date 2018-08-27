using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;


namespace SurfaceGoDemo.Helpers
{
    public static class BitmapHelper
    {
        public static async Task<BitmapImage> LoadBitMapFromFileAsync(Uri uri, int decodeWidth)
        {
            // create the bitmap
            BitmapImage bitmapImage = new BitmapImage() { DecodePixelWidth = decodeWidth };

            try
            {
                // get a file pointer
                StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(uri);

                // load the file async, but wait on it
                using (var stream = await file.OpenReadAsync())
                {
                    // wait for the source to be loaded
                    await bitmapImage.SetSourceAsync(stream);
                }
            }
            catch (Exception ex)
            {
                bitmapImage = null;
            }

            return bitmapImage;
        }

        public static async Task<BitmapImage> LoadBitMapFromFileAsync(StorageFile file, int decodeWidth)
        {
            // create the bitmap
            BitmapImage bitmapImage = new BitmapImage() { DecodePixelWidth = decodeWidth };

            if (null != file)
            {
                try
                {
                    // load the file async, but wait on it
                    using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.Read))
                    {
                        // wait for the source to be loaded
                        await bitmapImage.SetSourceAsync(stream);
                    }
                }
                catch (Exception e)
                {
                    bitmapImage = null;
                }
            }

            return bitmapImage;
        }
    }
}


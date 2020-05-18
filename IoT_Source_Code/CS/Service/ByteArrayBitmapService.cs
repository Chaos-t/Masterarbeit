
using Foreground.Controller;
using System;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace Foreground.Service
{
    /// <summary>
    /// Extension methods to transform pictures to byte array and vice versa.
    /// </summary>
    internal static class ByteArrayBitmapService
    {
        public static async Task<byte[]> AsByteArray(this StorageFile file)
        {
            IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.Read);
            var reader = new Windows.Storage.Streams.DataReader(fileStream.GetInputStreamAt(0));
            await reader.LoadAsync((uint)fileStream.Size);

            byte[] pixels = new byte[fileStream.Size];

            reader.ReadBytes(pixels);

            return pixels;
        }


        public static byte[] AsByteArray(this WriteableBitmap bitmap)
        {
            using (Stream stream = bitmap.PixelBuffer.AsStream())
            {
                MemoryStream memoryStream = new MemoryStream();
                stream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        public static async Task<byte[]> StandardBild()
        {
            StorageFile file = await SaveController.LoadFileFromAssets("leeres_Profil.png");
            return await AsByteArray(file);
        }

        public static async Task<BitmapImage> AsBitmapImage(this byte[] byteArray)
        {

            if (byteArray == null || byteArray.Length == 0)
            {
                byteArray = await StandardBild();
            }
            using (var stream = new InMemoryRandomAccessStream())
            {
                await stream.WriteAsync(byteArray.AsBuffer());
                var image = new BitmapImage();
                stream.Seek(0);
                image.SetSource(stream);
                return image;
            }


        }

        private static async Task<BitmapImage> AsBitmapImage(this StorageFile file)
        {
            var stream = await file.OpenAsync(FileAccessMode.Read);
            var bitmapImage = new BitmapImage();
            await bitmapImage.SetSourceAsync(stream);
            return bitmapImage;
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace _106_A2_M1.Services
{
    public static class QRImageHelper
    {
        public static BitmapImage ConvertByteArrayToBitmap(byte[] imageData)
        {
            if (imageData != null && imageData.Length > 0)
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.StreamSource = new MemoryStream(imageData);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();

                return bitmap;
            }

            return null; // or throw an exception, depending on your requirements
        }
    }
}

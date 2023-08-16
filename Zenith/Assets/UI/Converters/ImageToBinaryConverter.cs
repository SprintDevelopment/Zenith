using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Zenith.Assets.UI.Converters
{
    public class ImageToBinaryConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var byteArrayImage = value as byte[];

            if (byteArrayImage != null && byteArrayImage.Length > 0)
            {
                var memoryStream = new MemoryStream(byteArrayImage);

                var bitmapImg = new BitmapImage();

                bitmapImg.BeginInit();
                bitmapImg.StreamSource = memoryStream;
                bitmapImg.EndInit();

                return bitmapImg;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var bitmapImg = value as BitmapImage;

            var input = bitmapImg.StreamSource;

            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream memoryStream = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                    memoryStream.Write(buffer, 0, read);

                return memoryStream.ToArray();
            }
        }
    }
}

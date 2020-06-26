using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Wpf_AeroSphere_test_task
{
    public static class Convert_images//располагаются функции для конвертирования байтов и битовых карт  к ImageSource
    {
        static public ImageSource Convert_to_ImageSource(byte[] image_bytes, int index_of_end)//принимает массив байт и индекс конца интервала  массива для преобразования
        {
            BitmapImage bmp_img = new BitmapImage();
            MemoryStream ms = new MemoryStream(image_bytes, 0, index_of_end);
            bmp_img.BeginInit();
            bmp_img.StreamSource = ms;
            bmp_img.EndInit();
            return bmp_img as ImageSource;
        }

        static public ImageSource Convert_to_ImageSource(Bitmap bmp)//преобразует bitmap к ImageSource
        {
            IntPtr h_bmp = bmp.GetHbitmap();
            return Imaging.CreateBitmapSourceFromHBitmap(h_bmp, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        }
    }
}

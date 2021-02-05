using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace aBasics.WpfBasics {

    /// <summary>
    /// Hilfsklasse zu WPF und Bilder
    /// </summary>
    public static class Images {

        public static ImageSource ImageSourceFromFile(string file) {
            using ( Image drawingImgage = Image.FromFile(file) ) {
                return ImageToImageSource(drawingImgage);
            }
        }

        public static ImageSource ImageToImageSource(Image image) {
            // copied from stack overflow
            IntPtr bmpPt = IntPtr.Zero;
            using ( Bitmap bitmap = new Bitmap(image) ) {
                try {
                    bmpPt = bitmap.GetHbitmap();
                    BitmapSource bitmapSource =
                     System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                           bmpPt,
                           IntPtr.Zero,
                           Int32Rect.Empty,
                           BitmapSizeOptions.FromEmptyOptions());

                    //freeze bitmapSource and clear memory to avoid memory leaks
                    bitmapSource.Freeze();
                    return bitmapSource;
                }
                finally {
                    DeleteObject(bmpPt);
                }
            }
        }

        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool DeleteObject(IntPtr value);
    }
}
using System.Drawing;
using System.Drawing.Imaging;

namespace Chucksoft.Core.Drawing
{
    public interface IImageConverter
    {
        /// <summary>
        /// Converts to gray scale.
        /// </summary>
        /// <param name="original">The original.</param>
        /// <returns></returns>
        Bitmap ConvertToGrayScale(Bitmap original);

        /// <summary>
        /// Converts the byte array to bitmap.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns></returns>
        Bitmap ConvertByteArrayToBitmap(byte[] bytes);

        /// <summary>
        /// Converts the bitmapto bytes.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        byte[] ConvertBitmaptoBytes(Image image, ImageFormat format);
    }
}
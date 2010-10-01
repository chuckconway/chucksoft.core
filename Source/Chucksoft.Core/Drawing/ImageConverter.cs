using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace Chucksoft.Core.Drawing
{
    public class ImageConverter : IImageConverter
    {
        /// <summary>
        /// Converts to gray scale.
        /// </summary>
        /// <param name="original">The original.</param>
        /// <returns></returns>
        public  Bitmap ConvertToGrayScale(Bitmap original)
        {
            unsafe
            {
                //create an empty bitmap the same size as original
                Bitmap newBitmap = new Bitmap(original.Width, original.Height);

                //lock the original bitmap in memory
                BitmapData originalData = original.LockBits(
                   new Rectangle(0, 0, original.Width, original.Height),
                   ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

                //lock the new bitmap in memory
                BitmapData newData = newBitmap.LockBits(
                   new Rectangle(0, 0, original.Width, original.Height),
                   ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);

                //set the number of bytes per pixel
                int pixelSize = 3;

                for (int y = 0; y < original.Height; y++)
                {
                    //get the data from the original image
                    byte* oRow = (byte*)originalData.Scan0 + (y * originalData.Stride);

                    //get the data from the new image
                    byte* nRow = (byte*)newData.Scan0 + (y * newData.Stride);

                    for (int x = 0; x < original.Width; x++)
                    {
                        //create the grayscale version
                        byte grayScale =
                           (byte)((oRow[x * pixelSize] * .11) + //B
                           (oRow[x * pixelSize + 1] * .59) +  //G
                           (oRow[x * pixelSize + 2] * .3)); //R

                        //set the new image's pixel to the grayscale version
                        nRow[x * pixelSize] = grayScale; //B
                        nRow[x * pixelSize + 1] = grayScale; //G
                        nRow[x * pixelSize + 2] = grayScale; //R
                    }
                }

                //unlock the bitmaps
                newBitmap.UnlockBits(newData);
                original.UnlockBits(originalData);

                return newBitmap;
            }

            ////create a blank bitmap the same size as original
            //Bitmap newBitmap = new Bitmap(original.Width, original.Height);

            ////get a graphics object from the new image
            //Graphics g = Graphics.FromImage(newBitmap);

            ////create the grayscale ColorMatrix
            //ColorMatrix colorMatrix = new ColorMatrix(
            //   new[]
            //       {
            //         new[] {.3f, .3f, .3f, 0, 0},
            //         new[] {.59f, .59f, .59f, 0, 0},
            //         new[] {.11f, .11f, .11f, 0, 0},
            //         new float[] {0, 0, 0, 1, 0},
            //         new float[] {0, 0, 0, 0, 1}
            //      });

            ////create some image attributes
            //ImageAttributes attributes = new ImageAttributes();

            ////set the color matrix attribute
            //attributes.SetColorMatrix(colorMatrix);

            //g.SmoothingMode = SmoothingMode.HighQuality;
            //g.CompositingQuality = CompositingQuality.HighQuality;
            //g.InterpolationMode = InterpolationMode.High;

            ////draw the original image on the new image
            ////using the grayscale color matrix
            //g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
            //   0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);

            ////dispose the Graphics object
            //g.Dispose();
            //original.Dispose();
            //return newBitmap;
        }


        /// <summary>
        /// Converts the byte array to bitmap.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns></returns>
        public Bitmap ConvertByteArrayToBitmap(byte[] bytes)
        {
            Bitmap image;

            using (MemoryStream memStream = new MemoryStream(bytes))
            {
                image = new Bitmap(memStream);
            }

            return image;
        }

        /// <summary>
        /// Converts the bitmapto bytes.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public byte[] ConvertBitmaptoBytes(Image image, ImageFormat format)
        {
            byte[] imageBytes;
            using (MemoryStream outStream = new MemoryStream())
            {
                image.Save(outStream, format);
                outStream.Position = 0;
                imageBytes = new byte[outStream.Length];
                outStream.Read(imageBytes, 0, (int)outStream.Length);
            }

            return imageBytes;
        }
    }
}

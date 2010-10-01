using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Chucksoft.Core.Drawing
{
    public static class ImageResizer
    {
        /// <summary>
        /// Rotates the specified b.
        /// </summary>
        /// <param name="b">The b.</param>
        /// <param name="angle">The angle.</param>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public static byte[] Rotate(this byte[] b, float angle, ImageFormat format)
        {
            ImageConverter converter = new ImageConverter(); 

            Bitmap image = b.ConvertByteArrayToBitmap();
            image = Rotate(image, angle);
            byte[] rotatedImage = converter.ConvertBitmaptoBytes(image, format);
            image.Dispose();
            return rotatedImage;
        }

        /// <summary>
        /// Rotate90s the degrees left.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns></returns>
        public static byte[] Rotate90DegreesLeft(this byte[] bytes)
        {
            const RotateFlipType flipType = RotateFlipType.Rotate270FlipNone;
            return Rotate90Degrees(bytes, flipType);
        }

        /// <summary>
        /// Rotate90s the degrees right.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns></returns>
        public static byte[] Rotate90DegreesRight(this byte[] bytes)
        {
            const RotateFlipType flipType = RotateFlipType.Rotate90FlipNone;
            return Rotate90Degrees(bytes, flipType);
        }

        /// <summary>
        /// Rotate90s the degrees.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="flipType">Type of the flip.</param>
        /// <returns></returns>
        private static byte[] Rotate90Degrees(byte[] bytes, RotateFlipType flipType)
        {
            ImageConverter converter = new ImageConverter();
            Bitmap bitmap = ConvertByteArrayToBitmap(bytes);
            bitmap.RotateFlip(flipType);

            byte[] bitmaptoBytes = converter.ConvertBitmaptoBytes(bitmap, ImageFormat.Jpeg);
            bitmap.Dispose();

            return bitmaptoBytes;
        }

        /// <summary>
        /// Rotates the specified image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="angle">The angle.</param>
        /// <returns></returns>
        public static Bitmap Rotate(Image image, float angle)
        {
            //if (image == null)
            //{
            //    throw new ArgumentNullException("image");
            //}
            //const double pi2 = Math.PI / 2.0;

            //// Why can't C# allow these to be const, or at least readonly
            //// *sigh*  I'm starting to talk like Christian Graus :omg:
            //double oldWidth = image.Width;
            //double oldHeight = image.Height;

            //// Convert degrees to radians
            //double theta = angle * Math.PI / 180.0;
            //double lockedTheta = theta;

            //// Ensure theta is now [0, 2pi)
            //while (lockedTheta < 0.0)
            //{
            //    lockedTheta += 2*Math.PI;
            //}

            //#region Explaination of the calculations
            //*
            // * The trig involved in calculating the new width and height
            // * is fairly simple; the hard part was remembering that when 
            // * PI/2 <= theta <= PI and 3PI/2 <= theta < 2PI the width and 
            // * height are switched.
            // * 
            // * When you rotate a rectangle, r, the bounding box surrounding r
            // * contains for right-triangles of empty space.  Each of the 
            // * triangles hypotenuse's are a known length, either the width or
            // * the height of r.  Because we know the length of the hypotenuse
            // * and we have a known angle of rotation, we can use the trig
            // * function identities to find the length of the other two sides.
            // * 
            // * sine = opposite/hypotenuse
            // * cosine = adjacent/hypotenuse
            // * 
            // * solving for the unknown we get
            // * 
            // * opposite = sine * hypotenuse
            // * adjacent = cosine * hypotenuse
            // * 
            // * Another interesting point about these triangles is that there
            // * are only two different triangles. The proof for which is easy
            // * to see, but its been too long since I've written a proof that
            // * I can't explain it well enough to want to publish it.  
            // * 
            // * Just trust me when I say the triangles formed by the lengths 
            // * width are always the same (for a given theta) and the same 
            // * goes for the height of r.
            // * 
            // * Rather than associate the opposite/adjacent sides with the
            // * width and height of the original bitmap, I'll associate them
            // * based on their position.
            // * 
            // * adjacent/oppositeTop will refer to the triangles making up the 
            // * upper right and lower left corners
            // * 
            // * adjacent/oppositeBottom will refer to the triangles making up 
            // * the upper left and lower right corners
            // * 
            // * The names are based on the right side corners, because thats 
            // * where I did my work on paper (the right side).
            // * 
            // * Now if you draw this out, you will see that the width of the 
            // * bounding box is calculated by adding together adjacentTop and 
            // * oppositeBottom while the height is calculate by adding 
            // * together adjacentBottom and oppositeTop.
            // */
            //#endregion

            //double adjacentTop, oppositeTop;
            //double adjacentBottom, oppositeBottom;

            //// We need to calculate the sides of the triangles based
            //// on how much rotation is being done to the bitmap.
            ////   Refer to the first paragraph in the explaination above for 
            ////   reasons why.
            //if ((lockedTheta >= 0.0 && lockedTheta < pi2) ||
            //    (lockedTheta >= Math.PI && lockedTheta < (Math.PI + pi2)))
            //{
            //    adjacentTop = Math.Abs(Math.Cos(lockedTheta)) * oldWidth;
            //    oppositeTop = Math.Abs(Math.Sin(lockedTheta)) * oldWidth;

            //    adjacentBottom = Math.Abs(Math.Cos(lockedTheta)) * oldHeight;
            //    oppositeBottom = Math.Abs(Math.Sin(lockedTheta)) * oldHeight;
            //}
            //else
            //{
            //    adjacentTop = Math.Abs(Math.Sin(lockedTheta)) * oldHeight;
            //    oppositeTop = Math.Abs(Math.Cos(lockedTheta)) * oldHeight;

            //    adjacentBottom = Math.Abs(Math.Sin(lockedTheta)) * oldWidth;
            //    oppositeBottom = Math.Abs(Math.Cos(lockedTheta)) * oldWidth;
            //}

            //double newWidth = adjacentTop + oppositeBottom;
            //double newHeight = adjacentBottom + oppositeTop;

            //int nWidth = (int)Math.Floor(newWidth);
            //int nHeight = (int)Math.Floor(newHeight);
            //Bitmap rotatedBmp = new Bitmap(nWidth, nHeight);

            //using (Graphics g = Graphics.FromImage(rotatedBmp))
            //{
            //    // This array will be used to pass in the three points that 
            //    // make up the rotated image
            //    Point[] points;

            //    /*
            //     * The values of opposite/adjacentTop/Bottom are referring to 
            //     * fixed locations instead of in relation to the
            //     * rotating image so I need to change which values are used
            //     * based on the how much the image is rotating.
            //     * 
            //     * For each point, one of the coordinates will always be 0, 
            //     * nWidth, or nHeight.  This because the Bitmap we are drawing on
            //     * is the bounding box for the rotated bitmap.  If both of the 
            //     * corrdinates for any of the given points wasn't in the set above
            //     * then the bitmap we are drawing on WOULDN'T be the bounding box
            //     * as required.
            //     */
            //    if (lockedTheta >= 0.0 && lockedTheta < pi2)
            //    {
            //        points = new[] { 
            //                                 new Point( (int) oppositeBottom, 0 ), 
            //                                 new Point( nWidth, (int) oppositeTop ),
            //                                 new Point( 0, (int) adjacentBottom )
            //                             };

            //    }
            //    else if (lockedTheta >= pi2 && lockedTheta < Math.PI)
            //    {
            //        points = new[] { 
            //                                 new Point( nWidth, (int) oppositeTop ),
            //                                 new Point( (int) adjacentTop, nHeight ),
            //                                 new Point( (int) oppositeBottom, 0 )						 
            //                             };
            //    }
            //    else if (lockedTheta >= Math.PI && lockedTheta < (Math.PI + pi2))
            //    {
            //        points = new[] { 
            //                                 new Point( (int) adjacentTop, nHeight ), 
            //                                 new Point( 0, (int) adjacentBottom ),
            //                                 new Point( nWidth, (int) oppositeTop )
            //                             };
            //    }
            //    else
            //    {
            //        points = new[] { 
            //                                 new Point( 0, (int) adjacentBottom ), 
            //                                 new Point( (int) oppositeBottom, 0 ),
            //                                 new Point( (int) adjacentTop, nHeight )		
            //                             };
            //    }

            //    g.SmoothingMode = SmoothingMode.HighQuality;
            //    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            //    g.CompositingQuality = CompositingQuality.HighQuality;
            //    g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            //    g.DrawImage(image, points);
            //}

            //create a new empty bitmap to hold rotated image
            Bitmap returnBitmap = new Bitmap(image.Width, image.Height);
            //make a graphics object from the empty bitmap
            Graphics g = Graphics.FromImage(returnBitmap);
            //move rotation point to center of image
            g.TranslateTransform((float)image.Width / 2, (float)image.Height / 2);
            //rotate
            g.RotateTransform(angle);
            //move image back
            g.TranslateTransform(-(float)image.Width / 2, -(float)image.Height / 2);
            //draw passed in image onto graphics object
            g.DrawImage(image, new Point(0, 0));

            return returnBitmap;

        }

        /// <summary>
        /// Resizes the specified bitmap.
        /// </summary>
        /// <param name="bitmap">The bitmap.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public static byte[] Resize(this byte[] bitmap, int width, int height, ImageFormat format)
        {
            ImageConverter converter = new ImageConverter();

            Bitmap image = ConvertByteArrayToBitmap(bitmap);
            image = Resize(image, width, height);
            byte[] rotatedBits = converter.ConvertBitmaptoBytes(image, format);
            image.Dispose();

            return rotatedBits;
        }
        

        /// <summary>
        /// Converts the byte array to bitmap.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns></returns>
        public static Bitmap ConvertByteArrayToBitmap(this byte[] bytes)
        {
            ImageConverter converter = new ImageConverter();
            Bitmap image = converter.ConvertByteArrayToBitmap(bytes);
            return image;
        }

        /// <summary>
        /// Resizes the bitmap, pass in the new sizes
        /// </summary>
        /// <param name="bitmap">bitmap to be resized</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <returns></returns>
        public static Bitmap Resize(this Bitmap bitmap, int width, int height)
        {
            Bitmap result = new Bitmap(width, height);
            using (Graphics graphic = Graphics.FromImage(result))
            {
                graphic.SmoothingMode = SmoothingMode.HighQuality;
                graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphic.CompositingQuality = CompositingQuality.HighQuality;
                graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;

                graphic.DrawImage(bitmap, 0, 0, width, height);
            }

            return result;
        }

    }
}

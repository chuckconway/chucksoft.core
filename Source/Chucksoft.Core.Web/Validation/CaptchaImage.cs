using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Chucksoft.Core.Cryptography;

namespace Chucksoft.Core.Web.Validation
{
    public class CaptchaImage
    {
        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text { get; private set; }

        /// <summary>
        /// Gets or sets the color of the background.
        /// </summary>
        /// <value>The color of the background.</value>
        public Color BackgroundColor { get; private set; }

        /// <summary>
        /// Gets or sets the image.
        /// </summary>
        /// <value>The image.</value>
        public Bitmap Image { get; private set; }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>The width.</value>
        public int Width { get; private set; }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>The height.</value>
        public int Height { get; private set; }

        /// <summary>
        /// Gets the random color.
        /// </summary>
        /// <value>The random color.</value>
        public Color RandomColor
        {
            get
            {
                //return Color.FromArgb(RandomNumber.Next(0, 255), RandomNumber.Next(0, 255), RandomNumber.Next(0, 255));
                return Color.FromArgb(255, 255, 255);
            }
        }

        /// <summary>
        /// Gets the random string alignment.
        /// </summary>
        /// <value>The random string alignment.</value>
        public StringAlignment RandomStringAlignment
        {
            get
            {
                string[] values = Enum.GetNames(typeof(StringAlignment));
                int index = Randomizer.NextNumber(0, values.Length - 1);

                return (StringAlignment)Enum.Parse(typeof(StringAlignment), values[index]);
            }
        }

        /// <summary>
        /// Gets the color of the complementary.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns></returns>
        public Color GetComplementaryColor(Color color)
        {
            return Color.Gray;
        }

        /// <summary>
        /// Gets the random hatch style.
        /// </summary>
        /// <value>The random hatch style.</value>
        public HatchStyle RandomHatchStyle
        {
            get
            {
                string[] values = Enum.GetNames(typeof(HatchStyle));
                int index = Randomizer.NextNumber(0, values.Length - 1);

                return (HatchStyle)Enum.Parse(typeof(HatchStyle), values[index]);
            }
        }

        /// <summary>
        /// Gets the font family.
        /// </summary>
        /// <value>The font family.</value>
        public FontFamily FontFamily
        {
            get
            {
                int index = Randomizer.NextNumber(_fonts.Length - 1);

                return _fonts[index];
            }
        }

        // Internal properties.

        /// <summary>
        /// Initializes a new instance of the <see cref="CaptchaImage"/> class.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="bc">The bc.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public CaptchaImage(string s, Color bc, int width, int height)
        {
            Text = s;
            BackgroundColor = bc;
            Width = width;
            Height = height;
            GenerateImage();
        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="CaptchaImage"/> is reclaimed by garbage collection.
        /// </summary>
        ~CaptchaImage()
        {
            Dispose(false);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                // Dispose of the bitmap.
                Image.Dispose();
        }

        private readonly FontFamily[] _fonts = {
                                                   new FontFamily("Times New Roman"),
                                                   new FontFamily("Georgia"),
                                                   new FontFamily("Arial"),
                                                   new FontFamily("Comic Sans MS")
                                               };

        // ====================================================================
        // Creates the bitmap image.
        // ====================================================================
        /// <summary>
        /// Generates the image.
        /// </summary>
        private void GenerateImage()
        {
            // Create a new 32-bit bitmap image.
            Bitmap bitmap = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);

            // Create a graphics object for drawing.
            Graphics g = Graphics.FromImage(bitmap);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle rect = new Rectangle(0, 0, Width, Height);

            // Fill in the background.
            HatchBrush hatchBrush = new HatchBrush(RandomHatchStyle, Color.WhiteSmoke, Color.LightGray);

            g.FillRectangle(hatchBrush, rect);

            // Set up the text font.
            SizeF size;
            float fontSize = rect.Height + 1;
            Font font;

            // Adjust the font size until the text fits within the image.
            do
            {
                fontSize--;
                font = new Font(FontFamily, fontSize, FontStyle.Regular);
                size = g.MeasureString(Text, font);
            } while (size.Width > rect.Width);

            // Set up the text format.
            StringFormat format = new StringFormat { Alignment = RandomStringAlignment, LineAlignment = RandomStringAlignment };

            // Create a path using the text and warp it randomly.
            GraphicsPath path = new GraphicsPath();
            path.AddString(Text, font.FontFamily, (int)font.Style, font.Size, rect, format);
            const float v = 4F;
            PointF[] points =  {
                                   new PointF(Randomizer.NextNumber(rect.Width) / v, Randomizer.NextNumber(rect.Height) / v),
                                   new PointF(rect.Width - Randomizer.NextNumber(rect.Width) / v, Randomizer.NextNumber(rect.Height) / v),
                                   new PointF(Randomizer.NextNumber(rect.Width) / v,rect.Height - Randomizer.NextNumber(rect.Height) / v),
                                   new PointF(rect.Width - Randomizer.NextNumber(rect.Width) / v,rect.Height - Randomizer.NextNumber(rect.Height) / v)
                               };

            Matrix matrix = new Matrix();
            matrix.Translate(0F, 0F);
            path.Warp(points, rect, matrix, WarpMode.Perspective, 0F);

            // Draw the text.
            hatchBrush = new HatchBrush(RandomHatchStyle, Color.Gray, Color.LightGray);
            g.FillPath(hatchBrush, path);

            // Add some random noise.
            int m = Math.Max(rect.Width, rect.Height);
            for (int i = 0; i < (int)(rect.Width * rect.Height / 30F); i++)
            {
                int x = Randomizer.NextNumber(rect.Width);
                int y = Randomizer.NextNumber(rect.Height);
                int w = Randomizer.NextNumber(m / 50);
                int h = Randomizer.NextNumber(m / 50);
                g.FillEllipse(hatchBrush, x, y, w, h);
            }

            Brush brush = new SolidBrush(Color.FromArgb(100, 68, 68, 68));
            g.FillPath(brush, path);

            const double distort = 8d;

            // Copy the image so that we're always using the original for source color
            using (Bitmap copy = (Bitmap)bitmap.Clone())
            {
                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < Width; x++)
                    {
                        // Adds a simple wave
                        int newX = (int)(x + (distort * Math.Sin(Math.PI * y / 84.0)));
                        int newY = (int)(y + (distort * Math.Cos(Math.PI * x / 44.0)));
                        if (newX < 0 || newX >= Width) newX = 0;
                        if (newY < 0 || newY >= Height) newY = 0;
                        bitmap.SetPixel(x, y, copy.GetPixel(newX, newY));
                    }
                }
            }

            // Clean up.
            font.Dispose();
            hatchBrush.Dispose();
            g.Dispose();

            // Set the image.
            Image = bitmap;
        }
    }
}
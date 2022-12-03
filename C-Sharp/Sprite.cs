using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rotatable
{
    public class RotateablePictureBox : PictureBox
    {
        private Bitmap img;
        SpritePixel[,] spritePixels = new SpritePixel[0, 0];

        [
        Category("Appearance"),
        Description("The image to display."),
        ]
        public Bitmap image
        {
            get
            {
                return img;
            }
            set
            {
                img = value;
                Console.WriteLine(value);

                //this calls the set size method
                this.Size = this.Size;
                this.Refresh();
            }
        }

        private float angle = 0;
        private double cosTheta = 1;
        double sinTheta = 0;
        /// <summary>
        /// Angle is in degrees
        /// </summary>
        public float Angle
        {
            get
            {
                return angle * (float)(180 / Math.PI);
            }
            set
            {
                angle = value * ((float)Math.PI / 180);
                cosTheta = Math.Cos(angle);
                sinTheta = Math.Sin(angle);
                this.Refresh();
            }
        }

        private void setSize(object sender, EventArgs e)
        {
            if (img != null)
            {
                Console.WriteLine("in");
                int crosslength = (int)(Math.Sqrt((img.Width * img.Width) + (img.Height * img.Height)));
                this.Size = new Size(crosslength, crosslength);
                Console.WriteLine(crosslength + " " + this.Size.ToString());
            }
        }


        public RotateablePictureBox()
        {
            this.Resize += new EventHandler(setSize);
            Angle = 0;
            image = null;
            this.Location = new Point(100, 100);
            this.Size = new Size(100, 100);
            BackColor = Color.Transparent;
            this.Paint += new PaintEventHandler(this.Sprite_Paint);
        }

        public RotateablePictureBox(Bitmap pic = null)
        {
            Angle = 0;
            image = pic;
            this.Location = new Point(100, 100);
            if (pic != null)
            {
                int crosslength = (int)(Math.Sqrt((pic.Width * pic.Width) + (pic.Height * pic.Height)));
                this.Size = new Size(crosslength, crosslength);
            }
            else
            {
                this.Size = new Size(100, 100);
            }
            BackColor = Color.Transparent;
            this.Paint += new PaintEventHandler(this.Sprite_Paint);
        }

        private void Sprite_Paint(object sender, PaintEventArgs e)
        {
            if (image != null)
            {
                DrawImage(e);
            }
            else
            {
                e.Graphics.FillRectangle(new SolidBrush(BackColor), this.Location.X, this.Location.Y, Width, Height);
            }
        }

        private class SpritePixel
        {
            public Color color;
            public float x;
            public float y;
            public SpritePixel(float X, float Y, Color color)
            {
                this.color = color;
                this.x = X;
                this.y = Y;
            }
        }

        private void DrawImage(PaintEventArgs e)
        {
            spritePixels = new SpritePixel[img.Width, img.Height];
            for (int i = 0; img.Width > i; i++)
            {
                for (int j = 0; img.Height > j; j++)
                {
                    float x = i - img.Width / 2;
                    float y = j - img.Height / 2;
                    Point rotatedPoint = RotatePoint(new Point((int)Math.Round((double)x, 0), (int)Math.Round((double)y, 0)), new Point(0, 0));
                    spritePixels[i, j] = new SpritePixel(rotatedPoint.X, rotatedPoint.Y, img.GetPixel(i, j));
                }
            }

            for (int i = 0; spritePixels.GetLength(0) > i; i++)
            {
                for (int j = 0; spritePixels.GetLength(1) > j; j++)
                {
                    SpritePixel pixel = spritePixels[i, j];
                    e.Graphics.FillRectangle(new SolidBrush(pixel.color), new RectangleF(pixel.x + this.Width / 2, pixel.y + this.Height / 2, 1, 1));
                }
            }
        }

        /// <summary>
        /// Rotates one point around another
        /// </summary>
        /// <param name="pointToRotate">The point to rotate.</param>
        /// <param name="centerPoint">The center point of rotation.</param>
        /// <param name="angleInDegrees">The rotation angle in degrees.</param>
        /// <returns>Rotated point</returns>

        private Point RotatePoint(Point pointToRotate, Point centerPoint)
        {
            return new Point
            {
                X =
                    (int)
                    (cosTheta * (pointToRotate.X - centerPoint.X) -
                    sinTheta * (pointToRotate.Y - centerPoint.Y) + centerPoint.X),
                Y =
                    (int)
                    (sinTheta * (pointToRotate.X - centerPoint.X) +
                    cosTheta * (pointToRotate.Y - centerPoint.Y) + centerPoint.Y)
            };
        }
    }
}

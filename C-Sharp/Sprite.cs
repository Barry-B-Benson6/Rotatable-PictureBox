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
        //Contains the private variables
        #region Private Properties
        private Bitmap img;
        private SpritePixel[,] spritePixels = new SpritePixel[0, 0];
        private float angle = 0;
        private double cosTheta = 1;
        private double sinTheta = 0;
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
        #endregion

        //Contains the publically accessible properties for the class
        #region Public Properties

        [
            Category("Appearance"),
            Description("The image to display."),
        ]
        /// <summary>
        /// The image to display.
        /// </summary>
        public Bitmap image
        {
            get
            {
                return img;
            }
            set
            {
                img = value;
                setSize(new object(), new EventArgs());
                this.Refresh();
            }
        }

        [
            Category("Appearance"),
            Description("The angle at which the image is rotated. angle is measured in degrees going clockwise.")
        ]
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

        #endregion

        //Contains the constructors for the class
        #region Constructors
        public RotateablePictureBox()
        {
            initialize(null);
        }

        public RotateablePictureBox(Bitmap pic)
        {
            initialize(pic);
        }

        private void initialize(Bitmap pic)
        {
            this.Resize += new EventHandler(setSize);
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
        #endregion

        //Contains functions triggered by events
        #region Events
        private void setSize(object sender, EventArgs e)
        {
            if (img != null)
            {
                int crosslength = (int)(Math.Sqrt((img.Width * img.Width) + (img.Height * img.Height)));
                this.Size = new Size(crosslength, crosslength);
            }
        }

        private void Sprite_Paint(object sender, PaintEventArgs e)
        {
            if (Image != null)
            {
                Image = null;
            }
            if (image != null)
            {
                DrawImage(e);
            }
            else
            {
                e.Graphics.FillRectangle(new SolidBrush(BackColor), this.Location.X, this.Location.Y, Width, Height);
            }
        }
        #endregion

        //Contains internal functions for the class
        #region Private Functions
        private void DrawImage(PaintEventArgs e)
        {
            e.Graphics.TranslateTransform(this.Width / 2, this.Height / 2);
            e.Graphics.RotateTransform((float)(angle * 180 / Math.PI));
            e.Graphics.DrawImage(img, new Point(-img.Width / 2, -img.Height / 2));
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
        #endregion
    }
}

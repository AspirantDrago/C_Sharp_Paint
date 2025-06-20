using System;
using System.Drawing;
using System.Xml.Serialization;
using WindowsFormsApp4.Figures;

namespace WindowsFormsApp4
{
    [XmlInclude(typeof(Rect))]
    [Serializable]
    public abstract class Figure
    {
        public Color MainColor { get; set; } = Color.Red;
        public Color BorderColor { get; set; } = Color.Black;
        public uint BorderWidth { get; set; } = 1;
        public Size Size { get; set; }
        public Point Location { get; set; }

        public Figure() {}

        public Figure(Size size, Point location)
        {
            Size = size;
            Location = location;
        }

        public abstract void Draw(Graphics g);
    }
}

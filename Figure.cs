using System;
using System.Drawing;
using System.Xml.Serialization;
using WindowsFormsApp4.Figures;

namespace WindowsFormsApp4
{
    [XmlInclude(typeof(Rect))]
    [XmlInclude(typeof(Ellipse))]
    [Serializable]
    public abstract class Figure
    {
        [XmlIgnore]
        public Color MainColor { get; set; } = Color.Red;
        [XmlIgnore]
        public Color BorderColor { get; set; } = Color.Black;
        public uint BorderWidth { get; set; } = 1;
        public Size Size { get; set; }
        public Point Location { get; set; }

        [XmlElement("MainColor")]
        public int MainColorSerialized
        {
            get => MainColor.ToArgb();
            set => MainColor = Color.FromArgb(value);
        }
        [XmlElement("BorderColor")]
        public int BorderColorSerialized
        {
            get => BorderColor.ToArgb();
            set => BorderColor = Color.FromArgb(value);
        }

        public Figure() {}

        public Figure(Size size, Point location)
        {
            Size = size;
            Location = location;
        }

        public abstract void Draw(Graphics g);
    }
}

using System.Drawing;

namespace WindowsFormsApp4
{
    public abstract class Figure
    {
        public Color MainColor { get; set; } = Color.Red;
        public Color BorderColor { get; set; } = Color.Black;
        public uint BorderWidth { get; set; } = 1;
        public Size Size { get; set; }
        public Point Location { get; set; }

        public Figure(Size size, Point location)
        {
            Size = size;
            Location = location;
        }

        public abstract void Draw(Graphics g);
    }
}

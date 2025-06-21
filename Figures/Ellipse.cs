using System.Drawing;

namespace WindowsFormsApp4.Figures
{
    public class Ellipse : Figure
    {
        public Ellipse() : base() { }
        public Ellipse(Size size, Point location) : base(size, location) { }

        public override void Draw(Graphics g)
        {
            var rect = new Rectangle(Location, Size);
            g.FillEllipse(
                new SolidBrush(MainColor),
                rect
            );
            if (BorderWidth > 0)
                g.DrawEllipse(
                    new Pen(BorderColor, BorderWidth),
                    rect
                );
        }
    }
}
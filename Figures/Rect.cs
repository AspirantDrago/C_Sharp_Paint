using System.Drawing;

namespace WindowsFormsApp4.Figures
{
    public class Rect : Figure
    {
        public Rect(Size size, Point location) : base(size, location) {}

        public override void Draw(Graphics g)
        {
            var rect = new Rectangle(Location, Size);
            g.FillRectangle(
                new SolidBrush(MainColor),
                rect
            );
            g.DrawRectangle(
                new Pen(BorderColor, BorderWidth),
                rect
            );
        }
    }
}

using System;
using System.Drawing;

namespace WindowsFormsApp4.Figures
{
    [Serializable]
    public class Rect : Figure
    {
        public Rect() : base() {}
        public Rect(Size size, Point location) : base(size, location) {}

        public override void Draw(Graphics g)
        {
            var rect = new Rectangle(Location, Size);
            g.FillRectangle(
                new SolidBrush(MainColor),
                rect
            );
            if (BorderWidth > 0)
                g.DrawRectangle(
                    new Pen(BorderColor, BorderWidth),
                    rect
                );
        }
    }
}

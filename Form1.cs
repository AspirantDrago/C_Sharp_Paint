using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using WindowsFormsApp4.Figures;

namespace WindowsFormsApp4
{
    public partial class Form1 : Form
    {
        private List<Figure> Figures = new List<Figure>();
        private Figure TempFigure = null;
        private bool IsDrawing = false;

        public Form1()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            foreach (var f in Figures)
            {
                f.Draw(e.Graphics);
            }
            TempFigure?.Draw(e.Graphics);
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            IsDrawing = true;
            TempFigure = new Rect(new Size(0, 0), e.Location);
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!IsDrawing)
                return;
            TempFigure = new Rect(
                new Size(
                    e.X - TempFigure.Location.X,
                    e.Y - TempFigure.Location.Y
                ),
                TempFigure.Location
            );
            Refresh();
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            Figures.Add(TempFigure);
            TempFigure = null;
            IsDrawing = false;
        }
    }
}

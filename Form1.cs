using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using WindowsFormsApp4.Figures;

namespace WindowsFormsApp4
{
    public partial class Form1 : Form
    {
        private List<Figure> _figures = new List<Figure>();
        private List<Figure> _deletedFigures = new List<Figure>();
        private Figure TempFigure = null;
        private Point? StartPoint = null;
        private Color _backColor = Color.Red;
        private Color _borderColor = Color.Black;
        private uint borderW = 2;

        public Color BackColor
        {
            get
            {
                return _backColor;
            }
            set
            {
                _backColor = value;
                backColorBtn.BackColor = value;
                backColorDialog.Color = value;
            }
        }

        public Color BorderColor
        {
            get
            {
                return _borderColor;
            }
            set
            {
                _borderColor = value;
                borderBtn.BackColor = value;
                borderColorDialog.Color = value;
            }
        }



        void AddFigure(Figure f)
        {
            _figures.Add(f);
            _deletedFigures.Clear();
            undoMenuItem.Enabled = true;
            undoBtn.Enabled = undoMenuItem.Enabled;
            redoMenuItem.Enabled = false;
            redoBtn.Enabled = redoMenuItem.Enabled;
        }

        void Undo()
        {
            var n = _figures.Count - 1;
            _deletedFigures.Add(_figures[n]);
            _figures.RemoveAt(n);
            undoMenuItem.Enabled = _figures.Count > 0;
            undoBtn.Enabled = undoMenuItem.Enabled;
            redoMenuItem.Enabled = true;
            redoBtn.Enabled = redoMenuItem.Enabled;
            Refresh();
        }

        void Redo()
        {
            var n = _deletedFigures.Count - 1;
            _figures.Add(_deletedFigures[n]);
            _deletedFigures.RemoveAt(n);
            redoMenuItem.Enabled = _deletedFigures.Count > 0;
            redoBtn.Enabled = redoMenuItem.Enabled;
            undoMenuItem.Enabled = true;
            undoBtn.Enabled = undoMenuItem.Enabled;
            Refresh();
        } 

        public Form1()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            foreach (var f in _figures)
            {
                f.Draw(e.Graphics);
            }
            TempFigure?.Draw(e.Graphics);
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            StartPoint = e.Location;
            TempFigure = new Rect(new Size(0, 0), e.Location);
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (StartPoint is null)
                return;
            Point p = (Point)StartPoint;
            Point topLeft = new Point(
                Math.Min(p.X, e.X),
                Math.Min(p.Y, e.Y)
            );

            TempFigure = new Rect(
                new Size(
                    Math.Abs(e.X - p.X),
                    Math.Abs(e.Y - p.Y)
                ),
                topLeft
            );
            TempFigure.MainColor = BackColor;
            TempFigure.BorderColor = BorderColor;
            TempFigure.BorderWidth = borderW;
            Refresh();
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            AddFigure(TempFigure);
            TempFigure = null;
            StartPoint = null;
        }

        private void undoMenuItem_Click(object sender, EventArgs e)
        {
            Undo();
        }

        private void redoMenuItem_Click(object sender, EventArgs e)
        {
            Redo();
        }

        private void backColorBtn_Click(object sender, EventArgs e)
        {
            if (backColorDialog.ShowDialog() == DialogResult.OK)
            {
                BackColor = backColorDialog.Color;
            }
        }

        private void borderBtn_Click(object sender, EventArgs e)
        {
            if (borderColorDialog.ShowDialog() == DialogResult.OK)
            {
                BorderColor = borderColorDialog.Color;
            }
        }

        private void borderWidth_TextChanged(object sender, EventArgs e)
        {
            try
            {
                borderW = uint.Parse(
                    string.Join("", borderWidth.Text.Where(c => char.IsDigit(c))));
            }
            catch (Exception exception)
            {
                borderW = 0;
                Console.WriteLine(exception);
            }
            borderWidth.Text = borderW + " px";
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() != DialogResult.OK)
                return;
            Save(saveFileDialog.FileName);
        }

        void Save(string filename)
        {
            try
            {
                var ser = new XmlSerializer(typeof(List<Figure>));
                using (var ss = new StreamWriter(filename))
                {
                    ser.Serialize(ss, _figures);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(
                    e.ToString(),
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Asterisk
                );
            }
        }
    }
}

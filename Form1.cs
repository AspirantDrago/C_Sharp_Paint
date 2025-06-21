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
        private XmlSerializer serializer = new XmlSerializer(typeof(List<Figure>));

        private Type _typeOfFigure = typeof(Rect);

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

        void NewDocument()
        {
            _figures.Clear();
            _deletedFigures.Clear();
            TempFigure = null;
            Refresh();
        }

        void AddFigure(Figure f)
        {
            _figures.Add(f);
            _deletedFigures.Clear();
            undoMenuItem.Enabled = true;
            undoBtn.Enabled = undoMenuItem.Enabled;
            redoMenuItem.Enabled = false;
            redoBtn.Enabled = redoMenuItem.Enabled;
            mainFigurePanel.Refresh();
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
            mainFigurePanel.Refresh();
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
            mainFigurePanel.Refresh();
        } 

        public Form1()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.UserPaint |
                          ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.ResizeRedraw |
                          ControlStyles.ContainerControl |
                          ControlStyles.OptimizedDoubleBuffer |
                          ControlStyles.SupportsTransparentBackColor
                , true);
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            StartPoint = e.Location;
            TempFigure = (Figure)Activator.CreateInstance(_typeOfFigure,
                new Size(0, 0), e.Location
            );
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

            TempFigure = (Figure)Activator.CreateInstance(_typeOfFigure,
                new Size(
                    Math.Abs(e.X - p.X),
                    Math.Abs(e.Y - p.Y)
                ),
                topLeft
            );
            TempFigure.MainColor = BackColor;
            TempFigure.BorderColor = BorderColor;
            TempFigure.BorderWidth = borderW;
            tempFigurePanel.Refresh();
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
                
                using (var ss = new StreamWriter(filename))
                {
                    serializer.Serialize(ss, _figures);
                }
            }
            catch (Exception e)
            {
                ShowErrorMessage(e);
            }
        }

        private void новыйДокументToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewDocument();
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            try
            {
                using (var fs = new StreamReader(openFileDialog.FileName))
                {
                    NewDocument();
                    _figures = (List<Figure>)serializer.Deserialize(fs);
                    mainFigurePanel.Refresh();
                }
            }
            catch (Exception exception)
            {
                ShowErrorMessage(exception);
            }
        }

        void ShowErrorMessage(Exception e)
        {
            MessageBox.Show(
                e.ToString(),
                "Ошибка",
                MessageBoxButtons.OK,
                MessageBoxIcon.Asterisk
            );
        }

        private void selectFugureBtn(object btn)
        {
            foreach (ToolStripButton obj in figuresToolStrip.Items)
            {
                obj.Checked = false;
            }
            (btn as ToolStripButton).Checked = true;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            selectFugureBtn(sender);
            _typeOfFigure = typeof(Rect);
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            selectFugureBtn(sender);
            _typeOfFigure = typeof(Ellipse);
        }

        private void tempFigurePanel_Paint(object sender, PaintEventArgs e)
        {
            TempFigure?.Draw(e.Graphics);
        }

        private void toolStripContainer1_ContentPanel_Paint(object sender, PaintEventArgs e)
        {
            foreach (var f in _figures)
            {
                f.Draw(e.Graphics);
            }
        }
    }
}

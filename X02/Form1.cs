using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace X02
{
    public partial class Form1 : Form
    {
        Panel[,] tabla = new Panel[3, 3];
        char[,] mat = new char[3, 3];
        int gros = 5, turn = 0;
        Color cx = Color.Red;
        Color c0 = Color.Blue;

        public Form1()
        {
            InitializeComponent();
            this.Size = new Size(800, 800);
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            Form1_Resize(sender, e);
        }

        private void paintX(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen nigga = new Pen(cx, 5);
            Panel a = (Panel)sender;
            Point p = a.Location;
            Size sz = a.Size;
            int sp = 15;
            g.DrawLine(nigga, sp, sp, sz.Width-sp, sz.Height-sp);
            g.DrawLine(nigga, sz.Width - sp, sp, sp, sz.Height - sp);
            g.Dispose();
        }

        private void paint0(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen nigga = new Pen(c0, 5);
            Panel a = (Panel)sender;
            Point p = a.Location;
            Size sz = a.Size;
            int sp = 15;
            g.DrawEllipse(nigga, sp, sp, sz.Width - 2*sp, sz.Height - 2*sp);
            g.Dispose();
        }

        Point decode(string s)
        {
            string[] v = s.Split();
            int i = int.Parse(v[0]);
            int j = int.Parse(v[1]);
            return new Point(i, j);
        }

        void drawX(int i, int j)
        {
            tabla[i, j].Paint += new PaintEventHandler(paintX);
            tabla[i, j].Refresh();
            //this.Refresh();
            tabla[i, j].Paint -= paintX;
        }

        void draw0(int i, int j)
        {
            tabla[i, j].Paint += new PaintEventHandler(paint0);
            tabla[i, j].Refresh();
            tabla[i, j].Paint -= paint0;
        }

        private void CLICK(object sender, EventArgs e)
        {
            Point p = decode(((Panel)sender).Name);
            int i = p.X;
            int j = p.Y;
            //MessageBox m = new MessageBox();
            if (mat[i, j] != '\0') MessageBox.Show("Casuta plina!");
            else if (turn % 2 == 0)
            {
                drawX(i, j);
                mat[i, j] = 'x';
                if (haswon('x', i, j))
                {
                    MessageBox.Show("X a castigat!");
                    restart();
                }
                else if (turn == 8)
                {
                    MessageBox.Show("Egalitate");
                    restart();
                }
            }
            else
            {
                draw0(i, j);
                mat[i, j] = '0';
                if (haswon('0', i, j)) { MessageBox.Show("0 a castigat!"); restart(); }
                else if (turn == 8)
                {
                    MessageBox.Show("Egalitate");
                    restart();
                }
            }
            ++turn;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            this.Refresh();
            int w = this.Width;
            int h = this.Height;

            int x = w / 5;
            int y = h / 5;

            this.Controls.Clear();

            for (int i = 0; i < 3; ++i)
            {
                for (int j = 0; j < 3; ++j)
                {
                    tabla[i, j] = new Panel();
                    //tabla[i, j].BackColor = Color.White;
                    tabla[i, j].Location = new Point(x + gros, y + gros);
                    tabla[i, j].Size = new Size(w / 5 - 2 * gros, h / 5 - 2 * gros);
                    tabla[i, j].Name = i + " " + j;
                    tabla[i, j].Click += new EventHandler(CLICK);
                    this.Controls.Add(tabla[i, j]);
                    if (mat[i, j] == 'x') drawX(i, j);
                    else if (mat[i, j] == '0') draw0(i, j);
                    x += w / 5;
                }
                y += h / 5;
                x = w / 5;
            }
            Graphics g = this.CreateGraphics();
            g.FillRectangle(new SolidBrush(Color.Black), new Rectangle(w / 5 + gros, h / 5 + gros, 3 * w / 5 - 2 * gros - 3, 3 * h / 5 - 2 * gros - 3));
        }

        bool haswon(char theSeed, int currentRow, int currentCol)
        {
            return (mat[currentRow,0] == theSeed         // 3-in-the-row
                   && mat[currentRow,1] == theSeed
                   && mat[currentRow,2] == theSeed
              || mat[0,currentCol] == theSeed      // 3-in-the-column
                   && mat[1,currentCol] == theSeed
                   && mat[2,currentCol] == theSeed
              || currentRow == currentCol            // 3-in-the-diagonal
                   && mat[0,0] == theSeed
                   && mat[1,1] == theSeed
                   && mat[2,2] == theSeed
              || currentRow + currentCol == 2  // 3-in-the-opposite-diagonal
                   && mat[0,2] == theSeed
                   && mat[1,1] == theSeed
                   && mat[2,0] == theSeed);
        }

        private void stg(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.FillRectangle(Brushes.White, new Rectangle(0, 0, 2000, 2000));
        }

        void restart()
        {
            for(int i=0;i<3;++i)
                for(int j=0;j<3;++j)
                {
                    mat[i, j] = '\0';
                    tabla[i, j].Paint += stg;
                    tabla[i, j].Refresh();
                    tabla[i, j].Paint -= stg;
                }
            turn = -1;
        }

        void debug(Point p)
        {
            MessageBox.Show(p.X + " " + p.Y);
        }
    }
}

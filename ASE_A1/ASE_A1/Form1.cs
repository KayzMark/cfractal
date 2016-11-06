using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ASE_A1
{
    public partial class Form1 : Form
    {

        private const int MAX = 256;      // max iterations
        private const double SX = -2.025; // start value real
        private const double SY = -1.125; // start value imaginary
        private const double EX = 0.6;    // end value real
        private const double EY = 1.125;  // end value imaginary
        private static int x1, y1, xs, ys, xe, ye;
        private static double xstart, ystart, xende, yende, xzoom, yzoom;
        private static bool action, rectangle, finished;
        private static float xy;
        // PictureBox pictureBox = new PictureBox();
        private Image picture;
        private Graphics g1;
        private Cursor c1, c2;


        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (!rectangle) return;



            //e.consume();
            this.MouseMove += new MouseEventHandler(pictureBox_MouseMove);
            if (action)
            {
                // picture = new Bitmap(x1, y1);
                Graphics g1 = pictureBox.CreateGraphics();

                xe = e.Y;
                ye = e.Y;
                //Graphics g2.DrawImage(picture, 0, 0);
                if (rectangle)
                {
                    Pen p = new Pen(Color.White);
                    // Brush b = new Brush(Color.Orange);
                    if (xs < xe)
                    {
                        if (ys < ye) g1.DrawRectangle(p, xs, ys, (xe - xs), (ye - ys));
                        else g1.DrawRectangle(p, xs, ye, (xe - xs), (ys - ye));
                    }
                    else
                    {
                        if (ys < ye) g1.DrawRectangle(p, xe, ys, (xs - xe), (ye - ys));
                        else g1.DrawRectangle(p, xe, ye, (xs - xe), (ys - ye));
                    }
                }
                Refresh();
            }

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            // rectangle = false;
            int z, w;

            //e.consume();
            this.MouseUp += new MouseEventHandler(pictureBox_MouseUp);
            if (action)
            {
                xe = e.X;
                ye = e.Y;
                if (xs > xe)
                {
                    z = xs;
                    xs = xe;
                    xe = z;
                }
                if (ys > ye)
                {
                    z = ys;
                    ys = ye;
                    ye = z;
                }
                w = (xe - xs);
                z = (ye - ys);
                if ((w < 2) && (z < 2)) initvalues();
                else
                {
                    if (((float)w > (float)z * xy)) ye = (int)((float)ys + (float)w / xy);
                    else xe = (int)((float)xs + (float)z * xy);
                    xende = xstart + xzoom * (double)xe;
                    yende = ystart + yzoom * (double)ye;
                    xstart += xzoom * (double)xs;
                    ystart += yzoom * (double)ys;
                }
                xzoom = (xende - xstart) / (double)x1;
                yzoom = (yende - ystart) / (double)y1;
                mandelbrot();
                rectangle = false;
                // repaint();
                Refresh();
            }
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {

        }

        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            rectangle = true;
            // e.consume();
            this.MouseDown += new MouseEventHandler(pictureBox_MouseDown);
            if (action)
            {
                xs = e.X;
                ys = e.Y;
            }
        }

        //  private HSB HSBcolor = new HSBcolor();

        public Form1()
        {
            InitializeComponent();

            init();
            start();
            //mandelbrot();
        }

        public void init() // all instances will be prepared
        {
            //HSBcol = new HSB ();
            pictureBox.Size = new Size(640, 480);
            finished = false;
            //addMouseListener(this);
            // addMouseMotionListener(this);
            // c1 = new Cursor(Cursor.WAIT_CURSOR);
            //c2 = new Cursor(Cursor.CROSSHAIR_CURSOR);
            c1 = Cursors.WaitCursor;
            c2 = Cursors.Cross;
            x1 = pictureBox.Width;
            y1 = pictureBox.Height;
            xy = (float)x1 / (float)y1;
            picture = new Bitmap(x1, y1);
            g1 = Graphics.FromImage(picture);
            pictureBox.Image = picture;
            finished = true;
        }

        public void destroy() // delete all instances 
        {
            if (finished)
            {
                // removeMouseListener(this);
                // removeMouseMotionListener(this);
                picture = null;
                g1 = null;
                c1 = null;
                c2 = null;
                GC.Collect(); // garbage collection
            }
        }

        public void start()
        {
            action = false;
            rectangle = false;
            initvalues();
            xzoom = (xende - xstart) / (double)x1;
            yzoom = (yende - ystart) / (double)y1;
            mandelbrot();
        }

        public void stop()
        {
        }

        public void paint(Graphics g)
        {
            update(g);
        }

        public void update(Graphics g)
        {
            g.DrawImage(picture, 0, 0);
            if (rectangle)
            {
                //g.setColor(Color.white);
                Pen p = new Pen(Color.White);
                if (xs < xe)
                {
                    if (ys < ye) g.DrawRectangle(p, xs, ys, (xe - xs), (ye - ys));
                    else g.DrawRectangle(p, xs, ye, (xe - xs), (ys - ye));
                }
                else
                {
                    if (ys < ye) g.DrawRectangle(p, xe, ys, (xs - xe), (ye - ys));
                    else g.DrawRectangle(p, xe, ye, (xs - xe), (ys - ye));
                }
            }
        }

        public struct HSBColor
        {
            float h;
            float s;
            float b;
            int a;
            public HSBColor(float h, float s, float b)
            {
                this.a = 0xff;
                this.h = Math.Min(Math.Max(h, 0), 255);
                this.s = Math.Min(Math.Max(s, 0), 255);
                this.b = Math.Min(Math.Max(b, 0), 255);
            }
            public HSBColor(int a, float h, float s, float b)
            {
                this.a = a;
                this.h = Math.Min(Math.Max(h, 0), 255);
                this.s = Math.Min(Math.Max(s, 0), 255);
                this.b = Math.Min(Math.Max(b, 0), 255);
            }
            public float H
            {
                get { return h; }
            }
            public float S
            {
                get { return s; }
            }
            public float B
            {
                get { return b; }
            }
            public int A
            {
                get { return a; }
            }
            public Color Color
            {
                get
                {
                    return FromHSB(this);
                }
            }

            public static Color FromHSB(HSBColor hsbColor)
            {
                float r = hsbColor.b;
                float g = hsbColor.b;
                float b = hsbColor.b;
                if (hsbColor.s != 0)
                {
                    float max = hsbColor.b;
                    float dif = hsbColor.b * hsbColor.s / 255f;
                    float min = hsbColor.b - dif;
                    float h = hsbColor.h * 360f / 255f;
                    if (h < 60f)
                    {
                        r = max;
                        g = h * dif / 60f + min;
                        b = min;
                    }
                    else if (h < 120f)
                    {
                        r = -(h - 120f) * dif / 60f + min;
                        g = max;
                        b = min;
                    }
                    else if (h < 180f)
                    {
                        r = min;
                        g = max;
                        b = (h - 120f) * dif / 60f + min;
                    }
                    else if (h < 240f)
                    {
                        r = min;
                        g = -(h - 240f) * dif / 60f + min;
                        b = max;
                    }
                    else if (h < 300f)
                    {
                        r = (h - 240f) * dif / 60f + min;
                        g = min;
                        b = max;
                    }
                    else if (h <= 360f)
                    {
                        r = max;
                        g = min;
                        b = -(h - 360f) * dif / 60 + min;
                    }
                    else
                    {
                        r = 0;
                        g = 0;
                        b = 0;
                    }
                }
                return Color.FromArgb
                    (
                        hsbColor.a,
                        (int)Math.Round(Math.Min(Math.Max(r, 0f), 255)),
                        (int)Math.Round(Math.Min(Math.Max(g, 0), 255)),
                        (int)Math.Round(Math.Min(Math.Max(b, 0), 255))
                        );
            }

        }

        private void mandelbrot() // calculate all points
        {
            int x, y;
            float h, b, alt = 0.0f;

            Color col = Color.Black;

            action = false;
            //setCursor(c1);
            //Cursor.Current = c1;
            pictureBox.Cursor = c1;
            textBox.Text = "Mandelbrot-Set will be produced - please wait...";
            for (x = 0; x < x1; x += 2)
                for (y = 0; y < y1; y++)
                {
                    h = pointcolour(xstart + xzoom * (double)x, ystart + yzoom * (double)y); // color value
                    if (h != alt)
                    {
                        b = 1.0f - h * h; // brightnes
                                          ///djm added
                                          ///HSBcol.fromHSB(h,0.8f,b); //convert hsb to rgb then make a Java Color
                                          ///Color col = new Color(0,HSBcol.rChan,HSBcol.gChan,HSBcol.bChan);
                                          ///g1.setColor(col);
                        //djm end
                        //djm added to convert to RGB from HSB
                        // g1.setColor(Color.getHSBColor(h, 0.8f, b));
                        Brush b1 = new SolidBrush(HSBColor.FromHSB(new HSBColor(h * 255, 0.8f * 255, b * 255)));
                        //djm test
                        //  Color col = Color.getHSBColor(h, 0.8f, b);
                        col = HSBColor.FromHSB(new HSBColor(h * 255, 0.8f * 255, b * 255));

                        //g1.FillRectangle(b1, new System.Drawing.Rectangle(0, 0, pictureBox.Width, pictureBox.Height));
                        // int red = col.GetRed();
                        // int green = col.getGreen();
                        // int blue = col.getBlue();
                        //djm 
                        alt = h;
                        //Pen p1 = new Pen(col);
                        //g1.DrawLine(p1, x, y, x + 1, y);
                    }
                    Pen p1 = new Pen(col);
                    g1.DrawLine(p1, x, y, x + 1, y);
                }
            textBox.Text = "Mandelbrot-Set ready - please select zoom area with pressed mouse.";
            //setCursor(c2);
            // Cursor.Current = c2;
            pictureBox.Cursor = c2;
            action = true;
        }

        private float pointcolour(double xwert, double ywert) // color value from 0.0 to 1.0 by iterations
        {
            double r = 0.0, i = 0.0, m = 0.0;
            int j = 0;

            while ((j < MAX) && (m < 4.0))
            {
                j++;
                m = r * r - i * i;
                i = 2.0 * r * i + ywert;
                r = m + xwert;
            }
            return (float)j / (float)MAX;
        }

        private void initvalues() // reset start values
        {
            xstart = SX;
            ystart = SY;
            xende = EX;
            yende = EY;
            if ((float)((xende - xstart) / (yende - ystart)) != xy)
                xstart = xende - (yende - ystart) * (double)xy;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
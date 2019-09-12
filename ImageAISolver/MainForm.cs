using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace ImageAISolver
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void BtnOpen_Click(object sender, EventArgs e)
        {
            if( dlgOpen.ShowDialog() == DialogResult.OK     )
            {
                origin = new Bitmap(dlgOpen.FileName);
                pcbImage.Image = origin;
                pcbImage.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }
        Bitmap origin;
        Bitmap grayImage;
        byte[,] image;
        private void BtnGray_Click(object sender, EventArgs e)
        {
 

            grayImage = new Bitmap(origin.Width,origin.Height);
           // Graphics grayG = Graphics.FromImage(grayImage);
            image = new byte[origin.Height,origin.Width];

            for ( int r = 0; r < grayImage.Height; r++ )
                for( int c = 0; c < grayImage.Width; c++ )
                {
                    Color clr = origin.GetPixel(c, r);
                    int v = (clr.R + clr.G + clr.B) / 3;
                    image[r,c] = (byte)v;
                    grayImage.SetPixel(c, r, Color.FromArgb( v,v,v ));
                }
            pcbImage.Image = grayImage;

             double low = 0.3 * 255, up = 0.7 * 255;
            bool hit = false;
            int start = 0, end = 0;
            Graphics g = pcbImage.CreateGraphics();
            List<int> lines = new List<int>();
            float max = float.MinValue;
            float min = float.MaxValue;
            chart1.Series[0].Points.Clear();
            chart1.Series[1].Points.Clear();
            int last, current, delta = 0;
            for ( int r = 0; r < grayImage.Height; r++ )
            {
                float total = 0;
                last = image[r, 0];
                total = image[r, 0];
                delta = 0;
                for (int c = 1; c < grayImage.Width; c++)
                {
                    delta += Math.Abs(image[r, c] - last);
                    last = image[r, c];
                    total += image[r, c];
                }
                total /= grayImage.Width;
                chart1.Series[0].Points.AddY(total);
                chart1.Series[1].Points.AddY(delta);

                if (total > max) max = total;
                if (total < min) min = total;
                if (!hit)
                {
                    // get rising value
                    if (low < total && total < up)
                    {
                        hit = true;
                        start = r;
                    }
                }
                else
                {
                    // get lower or upper value
                    if( total < low || total > up )
                    {
                        hit = false;
                        end = r;
                        // draw a line at middle of start and end
                        g.DrawLine(Pens.Gold, 0, start, grayImage.Width, start);
                        g.DrawLine(Pens.Red, 0, start, grayImage.Width, ( start + end ) / 2 );
                        g.DrawLine(Pens.Gold, 0, start, grayImage.Width, end);
                        lines.Add((start + end) / 2);
                    }
                }
            }

            //
            DataPointCollection pts = chart1.Series[1].Points;
            int offset = 0;
            List<int> divisions = new List<int>();
            int ss = -1, ee;
            double smallest = double.MaxValue;
            double bound = 20;
            for (int i = pts.Count / 4; i < pts.Count * 3 / 4; i++)
            {
                if (pts[i].YValues[0] < smallest)
                {
                    smallest = pts[i].YValues[0];
                }
            }
            double factor = 1.5;
            bound = smallest *factor;
            int center=0;
            // 往右
            for (int i = pts.Count / 2; i < pts.Count; i++)
            {
                if (pts[i].YValues[0] <= bound) continue;
                else
                {
                    center = i+1;
                    break;
                }
            }
                // 往右
                for ( int i =center; i < pts.Count; i++ )
            {
                // 
                if( ss < 0 ) // find start of the smallest
                {
                    if(  pts[i].YValues[0] <= bound)
                    {
                        ss = i;
                    }
                    else
                    {

                    }
                }
                else // find end of the smallest 往右若碰到 0 表示邊界到了
                {
                    if( bound < pts[i].YValues[0] || pts[i].YValues[0] == 0 )
                    {
                        ee = i;
                        divisions.Add ( (ss + ee )  / 2 );
                        if (divisions.Count > 1) offset = divisions[1] - divisions[0];
                        ss = -1;

                        if (pts[i].YValues[0] == 0) break;//提早結束
                    }

                }
            }
            ss = -1;
            for (int i = center-1; i > 0; i--)
            {
                // 
                if (ss < 0) // find start of the smallest
                {
                    if ( pts[i].YValues[0] <= bound)
                    {
                        ss = i;
                    }
                    else
                    {

                    }
                }
                else // find end of the smallest
                {
                    if (bound < pts[i].YValues[0] || pts[i].YValues[0] == 0)
                    {
                        ee = i;
                        divisions.Add((ss + ee) / 2);
                        if (divisions.Count > 1) offset = divisions[1] - divisions[0];
                        ss = -1;
                    if (pts[i].YValues[0] == 0) break;//提早結束
                    }
                }
            }
            chart1.Series[2].Points.Clear();
            foreach (int i in divisions)
                chart1.Series[2].Points.AddXY(i, 0);
        }

        int hOff, hSize, hcount, offLimit;
        byte tol = 10;

        private void BtnColLine_Click(object sender, EventArgs e)
        {
            int trials = 20;
            int vSize = origin.Height / 20;
            int vStart = 3 * vSize / 2;
            byte vv;
            bool OK = false;
            for(  hcount = 5; hcount < 20; hcount++)
            {
                byte v;
                hSize = origin.Width / hcount;
                int start = hSize * 3 / 2;
                int end = start + hSize;
                int x =  start;
                int y = vStart;
               for( hOff = start; hOff < end; hOff ++ )
                {
                    OK = true;
                    v = grayImage.GetPixel(x, y).R;
                    for( int c = 0; c < hcount-1; c++ )
                    {
                         x = c * hSize + hOff;
                        for( int r = 0; r < 19; r ++ )
                        {
                             y = vStart + vSize * r;
                            vv = grayImage.GetPixel(x, y).R;
                            if( vv - v > tol && v - vv > tol )
                            {
                                OK = false;
                                break;
                            }
                        }
                        if (!OK) break;
                    }
                    if (OK)
                    {
                        // 成功
                        break;
                    }
                }
                if (OK) break;
            }

            labMessage.Text = $"hoff = {hOff} hsize = {hSize} hcount = {hcount}";

        }
    }
}

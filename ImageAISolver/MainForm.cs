using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using IronOcr;
using IronOcr.Languages;

namespace ImageAISolver
{

    public partial class MainForm : Form
    {
        AdvancedOcr ocrReader = new AdvancedOcr();

        //string OCRRead(int x1, int y1, int x2, int y2)
        //{
        //    ocrReader.Language = IronOcr.Languages.i;

        //    return "";
        //}

        Random rnd = new Random();
        public MainForm()
        {
            InitializeComponent();

        }

        private void BtnOpen_Click(object sender, EventArgs e)
        {
            if (dlgOpen.ShowDialog() == DialogResult.OK)
            {
                isDouble = false;
                Cursor = Cursors.WaitCursor;

                Text = "Character Recognizer + " + Path.GetFileName(dlgOpen.FileName);
                colorImage = new Bitmap(dlgOpen.FileName);
                pcbImage.Image = colorImage;
                pcbImage.SizeMode = PictureBoxSizeMode.StretchImage;
                isFramed = true; // 假設有框

                BtnGray_Click(null, null);
                Cursor = Cursors.Default;
                rowDifferences = new float[colorImage.Height];
                colDifferences = new float[colorImage.Width];
            }
        }


        void CenterBasedAdjustRectangle(ref Rectangle rect)
        {
            // 取得變化量
            float last = 0;
            float sum = (rect.Width * 255);
            int continuousLimit = 4;
            float verticalLimit = 0;

            if (rect.Height <= continuousLimit || rect.Width <= continuousLimit) return;

            for (int c = 0; c < rect.Width; c++) last += dataImage[rect.Y, rect.X + c];
            for (int r = 1; r < rect.Height - 1; r++)
            {
                float total = 0;
                for (int c = 0; c < rect.Width; c++) total += dataImage[rect.Y + r, rect.X + c];
                rowDifferences[r - 1] = Math.Abs(total - last) / sum;
                last = total;
            }

            sum = rect.Height * 255;
            last = 0;
            for (int r = 0; r < rect.Height; r++) last += dataImage[rect.Y + r, rect.X];
            for (int c = 1; c < rect.Width - 1; c++)
            {
                float total = 0;
                for (int r = 0; r < rect.Height; r++) total += dataImage[rect.Y + r, rect.X + c];
                colDifferences[c - 1] = Math.Abs(total - last) / sum;
                last = total;
            }


            for (int r = 0; r < continuousLimit; r++)
                verticalLimit += rowDifferences[r + (rect.Height - continuousLimit) / 2];
            verticalLimit /= continuousLimit * 10;
            float horizontalLimit = 0;
            for (int c = 0; c < continuousLimit; c++)
                horizontalLimit += colDifferences[c + (rect.Width - continuousLimit) / 2];
            horizontalLimit /= continuousLimit * 10;

            //verticalLimit =   0.00001f;
            //horizontalLimit = 0.00001f;
            verticalLimit = 0;
            horizontalLimit = 0;


            int continuousCount;
            int up = rect.Height / 2;
            int down = up + 1;
            int ll = rect.Width / 2;
            int rr = ll + 1;
            //top
            continuousCount = -1;
            for (int r = up; r >= 0; r--)
            {
                if (rowDifferences[r] <= verticalLimit)
                {
                    if (continuousCount < 0)
                    {
                        up = r;
                        continuousCount = 1;
                    }
                    else
                    {
                        continuousCount++;
                        if (continuousCount >= continuousLimit)
                            break;
                    }
                }
                else
                {
                    continuousCount = -1;
                    up = 0;
                }
            }
            //bottom
            continuousCount = -1;
            for (int r = down; r < rect.Height; r++)
            {
                if (rowDifferences[r] <= verticalLimit)
                {
                    if (continuousCount < 0)
                    {
                        down = r;
                        continuousCount = 1;
                    }
                    else
                    {
                        continuousCount++;
                        if (continuousCount >= continuousLimit)
                            break;
                    }
                }
                else
                {
                    continuousCount = -1;
                    down = rect.Height - 1;
                }
            }

            //left
            continuousCount = -1;
            for (int c = ll; c >= 0; c--)
            {
                if (colDifferences[c] <= horizontalLimit)
                {
                    if (continuousCount < 0)
                    {
                        ll = c;
                        continuousCount = 1;
                    }
                    else
                    {
                        continuousCount++;
                        if (continuousCount >= continuousLimit)
                            break;
                    }
                }
                else
                {
                    continuousCount = -1;
                    ll = 0;
                }
            }
            //right
            continuousCount = -1;
            for (int c = rr; c < rect.Width; c++)
            {
                if (colDifferences[c] <= horizontalLimit)
                {
                    if (continuousCount < 0)
                    {
                        rr = c;
                        continuousCount = 1;
                    }
                    else
                    {
                        continuousCount++;
                        if (continuousCount >= continuousLimit)
                            break;
                    }
                }
                else
                {
                    continuousCount = -1;
                    rr = rect.Width - 1;
                }
            }


            rect.X += ll;// (int)leftOff;
            rect.Width = rr - ll; // ll + rr;// (int)(leftOff + rightOff);
            rect.Y += up; // (int)topOff;
            rect.Height = down - up; // up + down; // (int)(topOff + bottomOff);

        }


        // Boundary zero out
        void ZeroBoundAdjustRectangle(ref Rectangle rect)
        {
            float thresh = 1e-12f;
            float zero = 0;

            // 取得變化量
            float last = 0;
            float sum = (rect.Width * 255);
            for (int c = 0; c < rect.Width; c++) last += dataImage[rect.Y, rect.X + c];
            for (int r = 1; r < rect.Height - 1; r++)
            {
                float total = 0;
                for (int c = 0; c < rect.Width; c++) total += dataImage[rect.Y + r, rect.X + c];
                rowDifferences[r - 1] = Math.Abs(total - last) / sum;
                last = total;
            }

            sum = rect.Height * 255;
            last = 0;
            for (int r = 0; r < rect.Height; r++) last += dataImage[rect.Y + r, rect.X];
            for (int c = 1; c < rect.Width - 1; c++)
            {
                float total = 0;
                for (int r = 0; r < rect.Height; r++) total += dataImage[rect.Y + r, rect.X + c];
                colDifferences[c - 1] = Math.Abs(total - last) / sum;
                last = total;
            }


            int zeroCount = 0;
            int zeroLimit = 3;
            bool enabled = false;
            int topOff = 0, bottomOff = 0, leftOff = 0, rightOff = 0;
            // top
            for (int r = 0; r < rect.Height - 1; r++)
            {
                if (rowDifferences[r] > thresh)
                {
                    if( r+2 < rect.Height - 1 )
                    {
                        if (rowDifferences[r + 1] <= zero && rowDifferences[r + 2] <= zero) continue;
                    }
                    topOff = r;
                    break;
                }
            }
            // bottom
            for (int r = rect.Height - 2; r >= 0; r--)
            {
                if (rowDifferences[r] > thresh)
                {
                    if( r - 2 >= 0 )
                    {
                        if (rowDifferences[r - 1] <= zero && rowDifferences[r - 2] <= zero) continue;
                    }
                    bottomOff = r;
                    break;
                }
            }
            // left
            for (int c = 0; c < rect.Width - 1; c++)
            {
                if (colDifferences[c] > thresh)
                {
                    if( c+ 2 < rect.Width - 1 )
                    {
                        if (colDifferences[c + 1] <= zero && colDifferences[c + 2] <= zero) continue;
                    }
                    leftOff = c;
                    break;
                }
            }
            // right
            for (int c = rect.Width - 2; c >= 0; c--)
            {
                if (colDifferences[c] > thresh)
                {
                    if( c - 2 >= 0 )
                    {
                        if (colDifferences[c - 1] <= zero && colDifferences[c - 2] <= zero) continue;
                    }
                    rightOff = c;
                    break;
                }
            }
            rect.X += (int)leftOff;
            rect.Width = rightOff - leftOff + 1;
            rect.Y += (int)topOff;
            rect.Height = bottomOff - topOff + 1;
        }



        void AdjustRectangle( ref Rectangle rect )
        {
            // 取得變化量
            float last = 0;
            float sum = (rect.Width * 255);

            for (int c = 0; c < rect.Width; c++) last += dataImage[rect.Y, rect.X + c];
            for (int r = 1; r < rect.Height - 1; r++)
            {
                float total = 0;
                for (int c = 0; c < rect.Width; c++) total += dataImage[rect.Y + r, rect.X + c];
                rowDifferences[r - 1] = Math.Abs(total - last) / sum;
                last = total;
            }

            sum = rect.Height * 255;
            last = 0;
            for (int r = 0; r < rect.Height; r++) last += dataImage[rect.Y + r, rect.X];
            for (int c = 1; c < rect.Width - 1; c++)
            {
                float total = 0;
                for (int r = 0; r < rect.Height; r++) total += dataImage[rect.Y + r, rect.X + c];
                colDifferences[c - 1] = Math.Abs(total - last) / sum;
                last = total;
            }
            float limit = 0.000001f; // 0.001f;
            int topOff = 0, bottomOff = 0, leftOff = 0, rightOff = 0;
            // top
            for (int r = 0; r < rect.Height - 1; r++)
            {
                if (rowDifferences[r] >= limit)
                {
                    topOff = r - 1;
                    break;
                }
            }
            // bottom
            for (int r = rect.Height - 2; r >= 0; r--)
            {
                if (rowDifferences[r] >= limit)
                {
                    bottomOff = rect.Height - (r + 1);
                    break;
                }
            }
            // left
            for (int c = 0; c < rect.Width - 1; c++)
            {
                if (colDifferences[c] >= limit)
                {
                    leftOff = c - 1;
                    break;
                }
            }
            // right
            for (int c = rect.Width - 2; c >= 0; c--)
            {
                if (colDifferences[c] >= limit)
                {
                    rightOff = rect.Width - (c + 1);
                    break;
                }
            }
            rect.X += (int)leftOff;
            rect.Width -= (int)(leftOff + rightOff);
            rect.Y += (int)topOff;
            rect.Height -= (int)(topOff + bottomOff);

        }


        bool isDouble = false;

        Bitmap colorImage;
        Bitmap grayImage;
        byte[,] dataImage;
        private void BtnGray_Click(object sender, EventArgs e)
        {

            grayImage = new Bitmap(colorImage.Width, colorImage.Height);
            // Graphics grayG = Graphics.FromImage(grayImage);
            dataImage = new byte[colorImage.Height, colorImage.Width];

            // 轉成灰階 或 黑白影像
            for (int r = 0; r < grayImage.Height; r++)
                for (int c = 0; c < grayImage.Width; c++)
                {
                    Color clr = colorImage.GetPixel(c, r);
                    int v = (clr.R + clr.G + clr.B) / 3;
                    if (ckbBinary.Checked)
                    {
                        if (v <= 127) v = 0;
                        else v = 255;
                    }
                    dataImage[r, c] = (byte)v;
                    grayImage.SetPixel(c, r, Color.FromArgb(v, v, v));
                }
            pcbImage.Image = grayImage;

            // 提取列統計資料
            GetGridInfo(true);
            // 提取行統計資料
            GetGridInfo(false);

            // 繪製網格
            Graphics g = pcbImage.CreateGraphics(); // Graphics.FromImage(grayImage);
            int y = verticalOffset;
            Point ps = Point.Empty;
            ps.X = horizontalGrids[0];// horizontalOffset;
            Point pe = Point.Empty;
            pe.X = horizontalGrids[horizontalGrids.Count - 1];// horizontalOffset + horizontalCount * horizontalInterval;
            int st = 0;
            if (horizontalHeader < 0 && horizontalHeader2 < 0)
            {
                st = 1;
                ps.X = horizontalGrids[1];
            }
            // ps.Y = pe.Y = y;
            // g.DrawLine(Pens.Red, ps, pe);
            for (int i = st; i < verticalGrids.Count; i++)// verticalCount; i++ )
            {
                //y += verticalInterval;
                ps.Y = pe.Y = verticalGrids[i];// y;
                g.DrawLine(Pens.Red, ps, pe);
            }

            st = 0;
            ps.Y = verticalGrids[0];// verticalOffset;
            pe.Y = verticalGrids[verticalGrids.Count - 1]; // verticalOffset + verticalInterval * verticalCount;
            int x = horizontalOffset;
            if (verticalHeader < 0 && verticalHeader2 < 0)
            {
                ps.Y = verticalGrids[1];
                st = 1;
            }
            // ps.X = pe.X = x;
            // g.DrawLine(Pens.Red, ps, pe);
            for (int i = st; i < horizontalGrids.Count; i++)// horizontalCount; i++ )
            {
                //x += horizontalInterval;
                ps.X = pe.X = horizontalGrids[i]; // x;
                g.DrawLine(Pens.Red, ps, pe);

            }

            // Draw row headers along vertical grid
            st = 0;
            rowTitleYOffset = 0;

            int headerLength = grayImage.Width / horizontalGrids.Count / 4;
            if (headerLength < 20) headerLength = 20; // at least 40 pixel
            if (isDouble) rowTitleYOffset = (verticalGrids[1] - verticalGrids[0]) / 2;
            int ys = verticalGrids[0] - rowTitleYOffset;
            int ye = verticalGrids[verticalGrids.Count - 1] - rowTitleYOffset;
            int xe;
            if (horizontalHeader < 0 && horizontalHeader2 < 0)
            {
                xe = (horizontalGrids[0] + horizontalGrids[1]) / 2;
                st = 1;
                ys = verticalGrids[1] - rowTitleYOffset;
            }
            else xe = horizontalHeader > 0 ? horizontalHeader : horizontalHeader2;
            if (xe <= headerLength) headerLength = (int)(xe * 0.8);
            xe = xe - headerLength;
            rowTitleXStart = xe;
            rowTitleWidth = headerLength + headerLength;

            g.DrawLine(Pens.Green, xe, ys, xe, ye); // vertical line
            int xs = xe;
            xe = xe + headerLength + headerLength;
            g.DrawLine(Pens.Green, xe, ys, xe, ye); // vertical line            
            for (int i = st; i < verticalGrids.Count; i++)
            {
                g.DrawLine(Pens.Green, xs, verticalGrids[i] - rowTitleYOffset, xe, verticalGrids[i] - rowTitleYOffset); // vertical line
            }

            st = 0;
            // Draw column headers along vertical grid

            xs = horizontalGrids[0];
            xe = horizontalGrids[horizontalGrids.Count - 1];
            if (verticalHeader < 0 && verticalHeader2 < 0)
            {
                ye = (verticalGrids[0] + verticalGrids[1]) / 2;
                st = 1;
                xs = horizontalGrids[1];
            }
            else ye = verticalHeader > 0 ? verticalHeader : verticalHeader2;
            headerLength = grayImage.Height / verticalGrids.Count / 4;
            if (ye <= headerLength) headerLength = (int)(ye * 0.8);
            if (headerLength < 15) headerLength = 15; // at least 40 pixel
            if (headerLength > 28) headerLength = 28;

            ye = ye - headerLength;
            colTitleYStart = ye;
            colTitleHeight = headerLength + headerLength;

            g.DrawLine(Pens.Green, xs, ye, xe, ye); // vertical line
            ys = ye;
            ye = ye + headerLength + headerLength;
            g.DrawLine(Pens.Green, xs, ye, xe, ye); // vertical line            
            for (int i = st; i < horizontalGrids.Count; i++)
            {
                g.DrawLine(Pens.Green, horizontalGrids[i], ys, horizontalGrids[i], ye); // vertical line
            }
            g.Dispose();


        }

        int leftBound, rightBound, topBound, bottonBound;
        bool isFramed; // 是否有框
        float[] rowDifferences, colDifferences;

        void GetGridInfo(bool alongHeight)
        {
            int seriesOffset = alongHeight ? 0 : 3;

            chart1.Series[0 + seriesOffset].Points.Clear();
            chart1.Series[1 + seriesOffset].Points.Clear();
            chart1.Series[2 + seriesOffset].Points.Clear();

            int delta = 0;
            //float min = float.MaxValue;
            float total = 0;

            if (alongHeight)
            {

                // 各列圖素統計
                for (int r = 0; r < grayImage.Height; r++)
                {
                    total = dataImage[r, 0];
                    delta = 0;
                    for (int c = 1; c < grayImage.Width; c++)
                    {
                        delta += Math.Abs(dataImage[r, c] - dataImage[r, c - 1]);
                        total += dataImage[r, c];
                    }
                    total /= grayImage.Width; // 此列圖素值平均
                    //if (total < min) min = total;

                    chart1.Series[0 + seriesOffset].Points.AddXY(r, total);
                    chart1.Series[1 + seriesOffset].Points.AddXY(r + 1, delta);
                }
            }
            else
            {
                for (int c = 0; c < grayImage.Width; c++)
                {
                    total = dataImage[0, c];
                    delta = 0;
                    for (int r = 1; r < grayImage.Height; r++)
                    {
                        delta += Math.Abs(dataImage[r, c] - dataImage[r - 1, c]);
                        total += dataImage[r, c];
                    }
                    total /= grayImage.Height;
                    chart1.Series[0 + seriesOffset].Points.AddXY(c, total);
                    chart1.Series[1 + seriesOffset].Points.AddXY(c + 1, delta);
                }
            }

            float limit = alongHeight ? grayImage.Height / 55.0f : grayImage.Width / 55.0f;
            float now = alongHeight ? grayImage.Height / (float)chart1.Series[1 + seriesOffset].Points.Count :
                grayImage.Width / (float)chart1.Series[1 + seriesOffset].Points.Count;


            if (now < limit)
            {
                // 有雜訊，重設 divisors 添加雜訊去除
                int dec = 10;
                int idx = 0;
                for (int i = dec; i < chart1.Series[1 + seriesOffset].Points.Count; i++)
                {
                    double y = chart1.Series[1 + seriesOffset].Points[i].YValues[0];
                    for (int j = 1; j < dec; j++)
                    {
                        y += chart1.Series[1 + seriesOffset].Points[i - j].YValues[0];
                    }
                    y /= dec;
                    chart1.Series[1 + seriesOffset].Points[idx].XValue =
                        chart1.Series[1 + seriesOffset].Points[i - dec / 2].XValue;
                    chart1.Series[1 + seriesOffset].Points[idx].YValues[0] = y;
                    idx++;
                }
                for (int j = 0; j < dec / 2; j++)
                    chart1.Series[1 + seriesOffset].Points.RemoveAt(chart1.Series[1 + seriesOffset].Points.Count - 1);
            }


            // 圖素差異線點數值
            DataPointCollection pts = chart1.Series[1 + seriesOffset].Points;

            //int offset = 0;
            List<int> divisors = new List<int>();
            double smallest = double.MaxValue;
            double largest = double.MinValue;
            double valleyBound = 20;

            // 檢視差異線1/4到3/4段內的最小值，確認分界線的數值，以便定義數值範圍
            for (int i = pts.Count / 4; i < pts.Count * 3 / 4; i++)
            {
                if (pts[i].YValues[0] < smallest)
                {
                    smallest = pts[i].YValues[0];
                }
                if (pts[i].YValues[0] > largest)
                {
                    largest = pts[i].YValues[0];
                }
            }

            int zeroBound = (int)largest / 50;
            // 找出標籤列或行，0開始0結尾，高度 largest/4 以上，寬 < 總寬 / 5，可能有兩個
            // 1: left, 2: left+right, 3: right, 4: interior
            int heightLimit = (int)(largest / 8);
            int widthLimit = (int)(alongHeight ? grayImage.Height / 6 : grayImage.Width / 6);
            int failedCount;
            double zs, ze, peeks, peeke;
            double peekValue = 0;
            bool verticalInMiddle = false;
            bool horizontalInMiddle = false;

            if (alongHeight)
            {
                verticalHeader = -1;
                verticalHeader2 = -1;
            }
            else
            {
                horizontalHeader = -1;
                horizontalHeader2 = -1;
            }
            failedCount = 0;
            zs = -1; ze = -1;
            peeke = -1;
            peeks = -1;
            peekValue = 0;
            for (int i = 0; i < pts.Count; i++)
            {
                if (zs < 0)
                {
                    if (peeks < 0)
                    {
                        // find first 0
                        if (pts[i].YValues[0] <= zeroBound)
                            zs = pts[i].XValue;
                    }
                }
                else
                {
                    if (peeks < 0)
                    {
                        // find first not 0
                        if (pts[i].YValues[0] > zeroBound)
                            peeks = pts[i].XValue;
                    }
                    else
                    {
                        // find second 0
                        if (pts[i].YValues[0] <= zeroBound)
                        {
                            peeke = pts[i].XValue;
                            // find next non zero to set ze;
                            for (int j = i + 1; j < pts.Count; j++)
                            {
                                if (pts[j].YValues[0] > zeroBound)
                                {
                                    ze = pts[j].XValue;
                                    i = j;
                                    break;
                                }
                            }
                            if (ze < 0) ze = pts[pts.Count - 1].XValue;
                            // Get a signal check validity
                            if (peekValue > heightLimit && ze - zs < widthLimit)
                            {
                                if (alongHeight)
                                {
                                    if (verticalHeader < 0)
                                        verticalHeader = (int)((peeke + peeks) / 2);
                                    else
                                        verticalHeader2 = (int)((peeke + peeks) / 2);
                                }
                                else
                                {
                                    if (horizontalHeader < 0)
                                        horizontalHeader = (int)((peeke + peeks) / 2);
                                    else
                                        horizontalHeader2 = (int)((peeke + peeks) / 2);
                                }
                            }
                            else
                                failedCount++;
                            // start next serach
                            zs = ze;
                            ze = -1; peeks = -1; peeke = -1;
                            peekValue = 0;
                        }
                    }
                }
                if (pts[i].YValues[0] > peekValue) peekValue = pts[i].YValues[0];

            }

            int voidS = pts.Count, voidE = 0;
            int level = (int)(largest / 5);
            if (failedCount > 3)
            {
                if (alongHeight) verticalInMiddle = true;
                else horizontalInMiddle = true;

                // 索引在內部，找出內部起始和結束
                bool startFound = false;
                for (int x = alongHeight ? verticalHeader : horizontalHeader; x > 0; x--)
                {
                    if (!startFound)
                    {
                        if (pts[x].YValues[0] < level) startFound = true;
                    }
                    else
                    {
                        if (pts[x].YValues[0] > level)
                        {
                            voidS = x;
                            break;
                        }
                    }
                }

                startFound = false;
                for (int x = alongHeight ? verticalHeader : horizontalHeader; x < pts.Count; x++)
                {
                    if (!startFound)
                    {
                        if (pts[x].YValues[0] < level) startFound = true;
                    }
                    else
                    {
                        if (pts[x].YValues[0] > level)
                        {
                            voidE = x;
                            break;
                        }
                    }

                }

                if (alongHeight && (verticalHeader < voidS || verticalHeader > voidE))
                    throw new Exception("Vertical Error");
                if (!alongHeight && (horizontalHeader < voidS || horizontalHeader > voidE))
                    throw new Exception("Horizontal Error");

                smallest = double.MaxValue;
                largest = double.MinValue;

                // update effect smallest 
                // 檢視差異線1/4到3/4段內的最小值，確認分界線的數值，以便定義數值範圍
                for (int i = pts.Count / 4; i < pts.Count * 3 / 4; i++)
                {
                    if (voidS < i && i < voidE) continue;
                    if (pts[i].YValues[0] < smallest)
                    {
                        smallest = pts[i].YValues[0];
                    }
                    if (pts[i].YValues[0] > largest)
                    {
                        largest = pts[i].YValues[0];
                    }
                }
            }
            else
            {
                largest = double.MinValue;
                smallest = double.MaxValue;

                // update effect smallest 
                // 檢視差異線1/4到3/4段內的最小值，確認分界線的數值，以便定義數值範圍
                for (int i = pts.Count / 4; i < pts.Count * 3 / 4; i++)
                {
                    if (pts[i].YValues[0] > largest)
                    {
                        largest = pts[i].YValues[0];
                    }
                    if (pts[i].YValues[0] < smallest)
                    {
                        smallest = pts[i].YValues[0];
                    }
                }
            }

            //double factor = 3;// 2.5; // 5; // 4; // 1.5; // 列的範圍固定，谷底較淺
            //if (!alongHeight) factor = factor * 2;// 因行的字數變化大，谷底放寬
            //valleyBound = smallest *factor;

            //int alt = (int)( smallest + (largest - smallest) / 10 );
            //if (valleyBound > alt)
            //    valleyBound = alt;

            valleyBound = (int)(smallest + (largest - smallest) / 12);

            //bool OK = true;
            //do
            //{
            int centerStart = 0;
            double localMin = 0; double localValue = 0;

            bool useLocal = true;

            // 由中間或 voidE 往右找到不能是谷底的起始點    
            int searchStart = voidE == 0 ? pts.Count / 2 : voidE;
            for (int i = searchStart; i < pts.Count; i++)
            {
                if (pts[i].YValues[0] <= valleyBound) continue;
                else
                {
                    centerStart = i + 1;
                    break;
                }
            }
            searchStart = centerStart; // 設定左邊開始搜尋的起點，避免遺漏中間的谷地

            double cliffDown = -1;
            zeroBound = (int)(valleyBound / 10);
            zeroBound = (int)(smallest / 10);
            // 開始往右尋找分隔線的谷底分布
            for (int i = centerStart; i < pts.Count; i++)
            {
                if (cliffDown < 0) // 尚未有懸崖下墜點，找尋中 find start of the smallest
                {
                    if (pts[i].YValues[0] <= valleyBound)
                    {
                        cliffDown = pts[i].XValue; // 找到下墜點
                        localValue = pts[i].YValues[0];
                        localMin = cliffDown;


                        DataPoint dp = new DataPoint();
                        dp.MarkerStyle = MarkerStyle.Cross;
                        dp.Color = Color.Black;
                        dp.XValue = pts[i].XValue;
                        dp.YValues = new double[] { valleyBound };
                        chart1.Series[2 + seriesOffset].Points.Add(dp);
                    }
                    else
                    {
                        if (pts[i].YValues[0] < localValue)
                        {
                            localValue = pts[i].YValues[0];
                            localMin = pts[i].XValue;
                        }
                    }
                }
                else // 已通過下墜點，在谷底中，尋找攀升點中 find end of the smallest 往右若碰到 0 表示邊界到了
                {
                    // 攀升點找到，或低迷到水平線永遠的谷底
                    if (pts[i].YValues[0] > valleyBound || pts[i].YValues[0] < zeroBound)
                    {
                        DataPoint dp = new DataPoint();
                        dp.MarkerStyle = MarkerStyle.Cross;
                        dp.Color = Color.Black;
                        dp.XValue = pts[i].XValue;
                        dp.YValues = new double[] { valleyBound };
                        chart1.Series[2 + seriesOffset].Points.Add(dp);

                        // 設定分隔點是谷底的中間
                        if (useLocal)
                            divisors.Add((int)localMin);
                        else divisors.Add((int)((cliffDown + pts[i].XValue) / 2));
                        //if (divisors.Count > 1) offset = divisors[1] - divisors[0];
                        // 繼續下一個谷底的搜尋
                        cliffDown = -1;
                        // 如果已碰到水平線，結束搜尋
                        if (pts[i].YValues[0] < zeroBound) break;//提早結束
                    }
                    else
                    {
                        if (pts[i].YValues[0] < localValue)
                        {
                            localValue = pts[i].YValues[0];
                            localMin = pts[i].XValue;
                        }
                    }
                }
            }
            // 右側找完，往左找
            // 
            // 由上次的起點(避免遺漏中間的谷地)或 voidS 往左找到不能是谷底的起始點   

            for (int i = voidS == pts.Count ? searchStart : voidS; i >= 0; i--)
            {
                if (pts[i].YValues[0] <= valleyBound) continue;
                else
                {
                    centerStart = i - 1;
                    break;
                }
            }

            cliffDown = -1;
            for (int i = centerStart; i > 0; i--)
            {
                if (cliffDown < 0) // find start of the smallest
                {
                    if (pts[i].YValues[0] <= valleyBound)
                    {
                        cliffDown = pts[i].XValue;

                        localValue = pts[i].YValues[0];
                        localMin = cliffDown;

                        DataPoint dp = new DataPoint();
                        dp.MarkerStyle = MarkerStyle.Cross;
                        dp.Color = Color.Black;
                        dp.XValue = pts[i].XValue;
                        dp.YValues = new double[] { valleyBound };
                        chart1.Series[2 + seriesOffset].Points.Add(dp);
                    }
                    else
                    {
                        if (pts[i].YValues[0] < localValue ||
                            (pts[i].YValues[0] == localValue && rnd.Next() % 2 == 0))
                        {
                            localValue = pts[i].YValues[0];
                            localMin = pts[i].XValue;
                        }
                    }
                }
                else // find end of the smallest
                {
                    if (valleyBound < pts[i].YValues[0] || pts[i].YValues[0] < zeroBound)
                    {
                        DataPoint dp = new DataPoint();
                        dp.MarkerStyle = MarkerStyle.Cross;
                        dp.Color = Color.Black;
                        dp.XValue = pts[i].XValue;
                        dp.YValues = new double[] { valleyBound };
                        chart1.Series[2 + seriesOffset].Points.Add(dp);

                        if (useLocal)
                            divisors.Add((int)(localMin));
                        else
                            divisors.Add((int)((cliffDown + pts[i].XValue) / 2));
                        cliffDown = -1;
                        if (pts[i].YValues[0] < zeroBound) break;//提早結束
                    }
                    else
                    {
                        if (pts[i].YValues[0] < localValue ||
                            (pts[i].YValues[0] == localValue && rnd.Next() % 2 == 0))
                        {

                            localValue = pts[i].YValues[0];
                            localMin = pts[i].XValue;
                        }
                    }
                }
            }

            // 找到的原始分隔點加入線圖，y值設為0



            #region reSampling
            //***** 再 sample 低點 方法
            // 找最大 和次大的Gap 存在時，提升 valleylvel 添加 divisors
            divisors.Sort();
            int gapidx = 0, bigGap = divisors[1] - divisors[0];
            int secondidx = 0, secondGap = bigGap;
            double sum = bigGap;
            for (int c = 2; c < divisors.Count; c++)
            {
                int d = divisors[c] - divisors[c - 1];
                if (d > secondGap)
                {
                    if (d > bigGap)
                    {
                        gapidx = c - 1;
                        bigGap = d;
                    }
                    else
                    {
                        secondGap = d;
                        secondidx = c - 1;
                    }
                }
                else sum += d;
            }
            sum /= divisors.Count - 3;
            if (bigGap > sum + sum)
            {
                // 添加 
                // 提高門檻已納入更多分隔點
                valleyBound = (int)(smallest + (largest - smallest) / 2);
                int xxss = divisors[gapidx], xxee = divisors[gapidx + 1];
                for (int i = xxss; i <= xxee; i++)
                    if (pts[i].YValues[0] <= valleyBound) continue;
                    else
                    {
                        xxss = i;
                        break;
                    }
                cliffDown = -1;
                for (int i = xxss; i <= xxee; i++)
                {
                    if (cliffDown < 0) // find start of the smallest
                    {
                        if (pts[i].YValues[0] <= valleyBound)
                        {
                            cliffDown = pts[i].XValue;

                            localValue = pts[i].YValues[0];
                            localMin = cliffDown;

                            DataPoint dp = new DataPoint();
                            dp.MarkerStyle = MarkerStyle.Cross;
                            dp.Color = Color.Black;
                            dp.XValue = pts[i].XValue;
                            dp.YValues = new double[] { valleyBound };
                            chart1.Series[2 + seriesOffset].Points.Add(dp);
                        }
                        else
                        {
                            if (pts[i].YValues[0] < localValue ||
                                (pts[i].YValues[0] == localValue && rnd.Next() % 2 == 0))
                            {

                                localValue = pts[i].YValues[0];
                                localMin = pts[i].XValue;
                            }
                        }
                    }
                    else // find end of the smallest
                    {
                        if (valleyBound < pts[i].YValues[0] || pts[i].YValues[0] < zeroBound)
                        {
                            DataPoint dp = new DataPoint();
                            dp.MarkerStyle = MarkerStyle.Cross;
                            dp.Color = Color.Black;
                            dp.XValue = pts[i].XValue;
                            dp.YValues = new double[] { valleyBound };
                            chart1.Series[2 + seriesOffset].Points.Add(dp);

                            if (useLocal)
                                divisors.Add((int)(localMin));
                            else
                                divisors.Add((int)((cliffDown + pts[i].XValue) / 2));

                            // divisors.Add((int)((cliffDown + pts[i].XValue) / 2));
                            cliffDown = -1;
                            if (pts[i].YValues[0] < zeroBound) break;//提早結束
                        }
                        else
                        {
                            if (pts[i].YValues[0] < localValue ||
                                (pts[i].YValues[0] == localValue && rnd.Next() % 2 == 0))
                            {

                                localValue = pts[i].YValues[0];
                                localMin = pts[i].XValue;
                            }
                        }
                    }


                }
            }
            if (secondGap > sum * 3 / 2)
            {
                // 提高門檻已納入更多分隔點
                valleyBound = (int)(smallest + (largest - smallest) / 2);
                // 添加 
                int xxss = divisors[secondidx], xxee = divisors[secondidx + 1];
                for (int i = xxss; i <= xxee; i++)
                    if (pts[i].YValues[0] <= valleyBound) continue;
                    else
                    {
                        xxss = i;
                        break;
                    }
                cliffDown = -1;
                for (int i = xxss; i <= xxee; i++)
                {
                    if (cliffDown < 0) // find start of the smallest
                    {
                        if (pts[i].YValues[0] <= valleyBound)
                        {
                            cliffDown = pts[i].XValue;

                            localValue = pts[i].YValues[0];
                            localMin = cliffDown;

                            DataPoint dp = new DataPoint();
                            dp.MarkerStyle = MarkerStyle.Cross;
                            dp.Color = Color.Black;
                            dp.XValue = pts[i].XValue;
                            dp.YValues = new double[] { valleyBound };
                            chart1.Series[2 + seriesOffset].Points.Add(dp);
                        }
                        else
                        {
                            if (pts[i].YValues[0] < localValue ||
                                (pts[i].YValues[0] == localValue && rnd.Next() % 2 == 0))
                            {

                                localValue = pts[i].YValues[0];
                                localMin = pts[i].XValue;
                            }
                        }
                    }
                    else // find end of the smallest
                    {
                        if (valleyBound < pts[i].YValues[0] || pts[i].YValues[0] < zeroBound)
                        {
                            DataPoint dp = new DataPoint();
                            dp.MarkerStyle = MarkerStyle.Cross;
                            dp.Color = Color.Black;
                            dp.XValue = pts[i].XValue;
                            dp.YValues = new double[] { valleyBound };
                            chart1.Series[2 + seriesOffset].Points.Add(dp);

                            if (useLocal)
                                divisors.Add((int)(localMin));
                            else
                                divisors.Add((int)((cliffDown + pts[i].XValue) / 2));

                            //divisors.Add((int)((cliffDown + pts[i].XValue) / 2));
                            cliffDown = -1;
                            if (pts[i].YValues[0] < zeroBound) break;//提早結束
                        }
                        else
                        {
                            if (pts[i].YValues[0] < localValue ||
                                (pts[i].YValues[0] == localValue && rnd.Next() % 2 == 0))
                            {

                                localValue = pts[i].YValues[0];
                                localMin = pts[i].XValue;
                            }
                        }
                    }


                }
            }
            #endregion



            #region addPreviousInterval

            // **** 訪前後間距 大區間添加法
            // 2019 
            // if the next interval is larger than 1.8 previous interval, add a divisor
            //divisors.Sort();
            //int keep = -1;
            //for (int i = 2; i < divisors.Count; i++)
            //{
            //    int pre = divisors[i - 1] - divisors[i - 2];
            //    if (keep > 0 && pre < keep / 2) continue;

            //    if (divisors[i] - divisors[i - 1] > 1.8 * pre)
            //    {
            //        keep = pre;

            //        if (divisors[i] - divisors[i - 1] < 2.2 * pre)
            //            divisors.Insert(i, (divisors[i - 1] + divisors[i]) / 2);
            //        else
            //            divisors.Insert(i, divisors[i - 1] + pre);
            //    }
            //}
            //keep = -1;
            //for (int i = divisors.Count - 3; i >= 0; i--)
            //{
            //    int pre = divisors[i + 2] - divisors[i + 1];
            //    if (keep > 0 && pre < keep / 2) continue;

            //    if (divisors[i + 1] - divisors[i] > 1.8 * pre)
            //    {
            //        keep = pre;
            //        if(divisors[i+1] - divisors[i] < 2.2 * pre)
            //            divisors.Insert(i + 1, (divisors[i + 1] + divisors[i]) / 2);
            //        else
            //            divisors.Insert(i + 1, divisors[i + 1] - pre);
            //        i++;
            //    }
            //}
            #endregion


            ////     if (!OK)
            ////     {
            ////         OK = true; // only add search once
            ////     }
            ////     else
            ////     {
            ////         // 估算 間隔平均差異量
            ////         double aagg = 0.0, bbb = divisors[0];
            ////         for (int b = 1; b < divisors.Count; b++)
            ////         {
            ////             aagg += divisors[b] - divisors[b - 1];
            ////             bbb += divisors[b];
            ////         }
            ////         aagg /= (divisors.Count - 1);
            ////         bbb /=  divisors.Count;
            ////         if (aagg > bbb / 3)
            ////         {
            ////             OK = false;
            ////             // 提升 valley bound 添加分隔點
            ////             valleyBound = (int)(smallest + (largest - smallest) / 2 );
            ////         }
            ////         else
            ////         {
            ////             // debug 強制
            ////             OK = false;
            ////             // 提升 valley bound 添加分隔點
            ////             valleyBound = (int)(smallest + (largest - smallest) / 2);
            ////         }
            ////     }
            ////} while (!OK);

            divisors.Sort();
            // 整理點資料調整出正確分隔點
            // 調整 divisions 
            // 取得間隔平均，若間隔小於平均的1/5視為需刪除調整的division
            float toosmall = divisors[1] - divisors[0];
            for (int i = 2; i < divisors.Count; i++)
                toosmall += divisors[i] - divisors[i - 1];
            toosmall /= divisors.Count - 1;
            toosmall /= 4;

            // 每個緯度最多有50組字體，因此若間隔小於 長 / 100 者視為雜訊
            int size = 100;
            toosmall = alongHeight ? grayImage.Height / size : grayImage.Width / size;

            int ccc = 0;
            for (int i = divisors.Count - 1; i > 0; i--)
            {
                if ((divisors[i] - divisors[i - 1]) < toosmall)
                {
                    int ii = i - 1;
                    ccc = 1;
                    while (ii - 1 >= 0 && divisors[ii] - divisors[ii - 1] < toosmall)
                    {
                        ccc++;
                        ii = ii - 1;
                    }
                    float newloc = divisors[i];
                    for (int j = 0; j < ccc; j++)
                        newloc += divisors[i - j - 1];
                    divisors[i] = (int)Math.Round(newloc / (ccc + 1));
                    //刪減雜訊分隔點
                    for (int j = 0; j < ccc; j++)
                        divisors.RemoveAt(i - j - 1);
                    i = i - ccc;
                }
            }



            // 大空隙添點
            //divisors.Sort();
            //int possibleInterval = -1;
            //List<int> temp = new List<int>();
            //for (int i = 1; i < divisors.Count; i++)
            //    temp.Add(divisors[i] - divisors[i-1]);
            //temp.Sort();
            //int zzz = temp[0];
            //List<int> vu = new List<int>();
            //List<int> tt = new List<int>();
            //vu.Add(zzz);
            //tt.Add(1);
            //int idd = 0;

            //for (int i = 1; i < temp.Count; i++)
            //{
            //    if( temp[i]-zzz < zzz /8.0 )
            //    {
            //        tt[idd]++;
            //        vu[idd] = (vu[idd]+temp[i])/ 2;
            //        zzz = vu[idd];
            //    }
            //    else
            //    {
            //         zzz = temp[i];
            //       vu.Add(zzz);
            //        tt.Add(1);
            //        idd++;
            //    }
            //}
            //int mid = 0;
            //int mmm = tt[0];
            //for( int i = 1; i < vu.Count; i++)
            //{
            //    if( tt[i] > mmm )
            //    {
            //        mid = i;
            //        mmm = tt[i];
            //    }
            //}
            //possibleInterval = vu[mid];


            //for (int i = 2; i < divisors.Count; i++)
            //{
            //    int pre = divisors[i - 1] - divisors[i - 2];
            //    if (possibleInterval > 0 && pre < possibleInterval / 2) continue;

            //    if (divisors[i] - divisors[i - 1] > 1.8 * pre)
            //    {
            //        possibleInterval = pre;

            //        if (divisors[i] - divisors[i - 1] < 2.2 * pre)
            //            divisors.Insert(i, (divisors[i - 1] + divisors[i]) / 2);
            //        else
            //            divisors.Insert(i, divisors[i - 1] + pre);
            //    }
            //}





            foreach (int i in divisors)
                chart1.Series[2 + seriesOffset].Points.AddXY(i, 0);

            if (divisors.Count < 3) return;



            // 找出間隔 及數量
            List<int> intervals = new List<int>();
            List<float> sections = new List<float>();
            List<int> counts = new List<int>();
            // int l = divisors[1];
            // 忽略前後兩個 bounds
            if (divisors.Count <= 2) return;
            for (int i = 2; i < divisors.Count - 1; i++)
                intervals.Add(divisors[i] - divisors[i - 1]);

            // 排除頭尾兩個，保留中間的 intervals
            intervals.Sort();

            sections.Add(intervals[0]);
            // l = intervals[0];
            float num = 4;
            float ttt = (float)intervals[0] / num; // tolerance
            counts.Add(1);
            int id = 0;
            for (int i = 1; i < intervals.Count - 1; i++)
            {
                // 接續的是否差異太大
                if (Math.Abs(intervals[i] - intervals[i - 1]) < ttt)
                {
                    // 歸屬在此interval，更新成平均值
                    sections[id] = (counts[id] * sections[id] + intervals[i]) / (counts[id] + 1);
                    ttt = (float)sections[id] / num;
                    counts[id]++;
                }
                else
                {
                    // 詫異大，新增一個
                    sections.Add(intervals[i]);
                    counts.Add(1);
                    // 變更餘裕
                    ttt = (float)intervals[i] / num;
                    id++;
                }
                // l = intervals[i];
            }
            while (sections.Count > 2)
            {
                // 刪掉個數最少者
                int small = counts[0];
                int idx = 0;
                for (int i = 1; i < counts.Count; i++)
                    if (counts[i] <= small)
                    {
                        idx = i;
                        small = counts[i];
                    }
                sections.RemoveAt(idx);
                counts.RemoveAt(idx);
            }

            //如果剩下兩個，個數懸殊，刪除個數小者
            if (sections.Count == 2)
            {
                if (counts[0] > counts[1])
                {
                    if (counts[0] - counts[1] > counts[1])
                    {
                        sections.RemoveAt(1);
                        counts.RemoveAt(1);
                    }
                }
                else
                {
                    if (counts[1] - counts[0] > counts[0])
                    {
                        sections.RemoveAt(0);
                        counts.RemoveAt(0);
                    }
                }
            }

            //isDouble = false;
            // 如果 section 最多有兩個
            int trueSection;
            int ssss;
            int tttt;
            if (sections.Count == 1)
            {
                trueSection = (int)Math.Round(sections[0]); // 單間格型
                ssss = divisors[1] - trueSection; // divisors[0] 會略有偏差
                tttt = divisors[divisors.Count - 2] + trueSection;
            }
            else
            {
                if (sections[1] > 5 * sections[0])
                {
                    trueSection = (int)sections[0]; // 分隔點資料有大漏洞
                    ssss = divisors[1] - trueSection; // divisors[0] 會略有偏差
                    tttt = divisors[divisors.Count - 2] + trueSection;
                }
                else
                {
                    trueSection = (int)Math.Round(sections[0] + sections[1]); // 左右有壕溝
                    ssss = (int)Math.Round((divisors[3] + divisors[2]) / 2.0 - trueSection);
                    tttt = divisors[divisors.Count - 1];
                }

                int lll = divisors[0];

                // 2019  ***
                for (int i = 1; i < divisors.Count - 1; i++)
                {

                    divisors[i] = (divisors[i] + lll) / 2;
                    lll = divisors[i + 1];
                    divisors.RemoveAt(i + 1);
                }
                isDouble = true;
                sections[0] = sections[0] + sections[1];
                sections.RemoveAt(1);


            }

            int startEnd;
            if (alongHeight) startEnd = verticalHeader >= 0 ? verticalHeader : 0;
            else startEnd = horizontalHeader >= 0 ? (!horizontalInMiddle ? horizontalHeader : 0) : 0;
            while (ssss < startEnd) ssss += trueSection;
            while ((ssss - trueSection) > startEnd) ssss = ssss - trueSection;


            //int ssss = sections.Count == 2 ?  (int)Math.Round( (divisors[2]+divisors[3])/2.0 - trueSection ): divisors[1]-trueSection;
            double thresh = largest / 100;
            if (alongHeight)
            {
                verticalOffset = ssss;

                verticalInterval = trueSection;
                verticalCount = -1;
                // ssss 之前是否有 索引

                // 上有索引，找出索引 
                int ps = -1;

                // Add header points
                DataPoint dp;

                if (verticalHeader >= 0)
                {
                    dp = new DataPoint();
                    dp.XValue = verticalHeader;
                    dp.YValues = new double[] { 25 };
                    dp.Color = Color.Red;
                    dp.MarkerStyle = MarkerStyle.Circle;
                    chart1.Series[2 + seriesOffset].Points.Add(dp);
                }
                if (verticalHeader2 >= 0)
                {
                    dp = new DataPoint();
                    dp.XValue = verticalHeader2;
                    dp.YValues = new double[] { 25 };
                    dp.Color = Color.Red;
                    dp.MarkerStyle = MarkerStyle.Circle;
                    chart1.Series[2 + seriesOffset].Points.Add(dp);
                }
            }
            else
            {
                horizontalOffset = ssss;
                horizontalInterval = trueSection;
                horizontalCount = -1;
                int ps = -1;

                DataPoint dp;
                if (horizontalHeader >= 0)
                {
                    dp = new DataPoint();
                    dp.XValue = horizontalHeader;
                    dp.YValues = new double[] { 25 };
                    dp.Color = Color.Red;
                    dp.MarkerStyle = MarkerStyle.Circle;
                    chart1.Series[2 + seriesOffset].Points.Add(dp);
                }

                if (horizontalHeader2 >= 0)
                {
                    dp = new DataPoint();
                    dp.XValue = horizontalHeader2;
                    dp.YValues = new double[] { 25 };
                    dp.Color = Color.Red;
                    dp.MarkerStyle = MarkerStyle.Circle;
                    chart1.Series[2 + seriesOffset].Points.Add(dp);
                }

            }

            // 微調 truesection: 頭尾兩 divisors 整除 接近的整數

            double dis = (double)(divisors[divisors.Count - 1] - divisors[0]);
            int n = (int)(dis / trueSection);
            int nin = (int)(Math.Round(dis / trueSection));
            if (nin == n)
            {
                int tsup = (int)(dis / n);
                if (Math.Abs(tsup - trueSection) < trueSection / 10)
                    trueSection = tsup;
            }



            int end = pts.Count;
            if (alongHeight)
            {
                verticalGrids.Clear();

                if (verticalHeader2 > 0) end = verticalHeader2;
                if (verticalInMiddle)
                {
                    for (int i = ssss; i < voidS + trueSection / 2;)
                    {
                        //DataPoint dp = new DataPoint();
                        //dp.XValue = i;
                        //dp.YValues = new double[] { largest / 4 };
                        //dp.Color = Color.Purple;
                        //dp.MarkerStyle = MarkerStyle.Triangle;
                        //chart1.Series[2 + seriesOffset].Points.Add(dp);
                        verticalGrids.Add(i);
                        i = i + trueSection;
                        verticalCount++;
                    }
                    for (int i = tttt; i > voidE - trueSection / 2;)
                    {
                        //DataPoint dp = new DataPoint();
                        //dp.XValue = i;
                        //dp.YValues = new double[] { largest / 4 };
                        //dp.Color = Color.Purple;
                        //dp.MarkerStyle = MarkerStyle.Triangle;
                        //chart1.Series[2 + seriesOffset].Points.Add(dp);
                        verticalGrids.Add(i);
                        i = i - trueSection;
                        verticalCount++;
                    }
                }
                else
                {
                    for (int i = ssss; i < end;)
                    {
                        //DataPoint dp = new DataPoint();
                        //dp.XValue = i;
                        //dp.YValues = new double[] { largest / 4 };
                        //dp.Color = Color.Purple;
                        //dp.MarkerStyle = MarkerStyle.Triangle;
                        //chart1.Series[2 + seriesOffset].Points.Add(dp);
                        verticalGrids.Add(i);
                        i = i + trueSection;
                        verticalCount++;
                    }
                }
                verticalGrids.Sort();

                // 微調，
                if (sections.Count == 1)
                {
                    for (int v = 0; v < verticalGrids.Count; v++)
                    {
                        int closePoint = -1, closeValue = int.MaxValue;
                        for (int a = 0; a < divisors.Count; a++)
                        {
                            int delt = Math.Abs(verticalGrids[v] - divisors[a]);
                            if (delt < closeValue)
                            {
                                closeValue = delt;
                                closePoint = divisors[a];
                            }
                        }
                        if (closeValue < trueSection * 0.6) // * 3.0 / 8.0)
                            verticalGrids[v] = closePoint;
                        else
                        {
                            if (v > 1 && verticalGrids[v] - verticalGrids[v - 1] < (verticalGrids[v - 1] - verticalGrids[v - 2]) * 0.9)
                            {
                                verticalGrids.RemoveAt(v);
                                v++;
                            }
                        }
                    }
                }
                else
                {
                    for (int v = 0; v < verticalGrids.Count; v++)
                    {
                        int closePoint = 0, closeValue = int.MaxValue;
                        for (int a = 1; a < divisors.Count; a++)
                        {
                            int pt = (divisors[a] + divisors[a - 1]) / 2;
                            int delt = Math.Abs(verticalGrids[v] - pt);
                            if (delt < closeValue)
                            {
                                closeValue = delt;
                                closePoint = pt;
                            }
                        }
                        if (closeValue < trueSection * 0.5 && v > 1 ? (closePoint - verticalGrids[v - 1] < 1.2 * (verticalGrids[v - 1] - verticalGrids[v - 2])) : true)
                            verticalGrids[v] = closePoint;
                    }
                }

                // add points
                foreach (int pt in verticalGrids)
                {
                    DataPoint dp = new DataPoint();
                    dp.XValue = pt;
                    dp.YValues = new double[] { largest / 4 };
                    dp.Color = Color.Purple;
                    dp.MarkerStyle = MarkerStyle.Triangle;
                    chart1.Series[2 + seriesOffset].Points.Add(dp);
                }


            }
            else
            {

                horizontalGrids.Clear();

                if (horizontalHeader2 > 0) end = horizontalHeader2;
                if (horizontalInMiddle)
                {
                    for (int i = ssss; i < voidS + trueSection / 2;)
                    {
                        //DataPoint dp = new DataPoint();
                        //dp.XValue = i;
                        //dp.YValues = new double[] { largest / 4 };
                        //dp.Color = Color.Purple;
                        //dp.MarkerStyle = MarkerStyle.Triangle;
                        //chart1.Series[2 + seriesOffset].Points.Add(dp);
                        horizontalGrids.Add(i);
                        i = i + trueSection;
                        horizontalCount++;
                    }
                    for (int i = tttt; i > voidE - trueSection / 2;)
                    {
                        //DataPoint dp = new DataPoint();
                        //dp.XValue = i;
                        //dp.YValues = new double[] { largest / 4 };
                        //dp.Color = Color.Purple;
                        //dp.MarkerStyle = MarkerStyle.Triangle;
                        //chart1.Series[2 + seriesOffset].Points.Add(dp);
                        horizontalGrids.Add(i);
                        i = i - trueSection;
                        horizontalCount++;
                    }

                }
                else
                {
                    for (int i = ssss; i < end;)
                    {
                        //DataPoint dp = new DataPoint();
                        //dp.XValue = i;
                        //dp.YValues = new double[] { largest / 4 };
                        //dp.Color = Color.Purple;
                        //dp.MarkerStyle = MarkerStyle.Triangle;
                        //chart1.Series[2 + seriesOffset].Points.Add(dp);
                        horizontalGrids.Add(i);
                        i = i + trueSection;
                        horizontalCount++;
                    }
                }

                horizontalGrids.Sort();
                // 微調，
                if (sections.Count == 1)
                {
                    for (int v = 0; v < horizontalGrids.Count; v++)
                    {
                        int closePoint = 0, closeValue = int.MaxValue;
                        for (int a = 0; a < divisors.Count; a++)
                        {
                            int delt = Math.Abs(horizontalGrids[v] - divisors[a]);
                            if (delt < closeValue)
                            {
                                closeValue = delt;
                                closePoint = divisors[a];
                            }
                        }
                        if (closeValue < trueSection * 0.6) // 3.0 / 8.0 )
                            horizontalGrids[v] = closePoint;
                        else
                        {
                            if (v > 1 && horizontalGrids[v] - horizontalGrids[v - 1] < (horizontalGrids[v - 1] - horizontalGrids[v - 2]) * 0.9)
                            {
                                horizontalGrids.RemoveAt(v);
                                v++;
                            }
                        }
                    }
                }
                else
                {
                    for (int v = 0; v < horizontalGrids.Count; v++)
                    {
                        int closePoint = 0, closeValue = int.MaxValue;
                        for (int a = 1; a < divisors.Count; a++)
                        {
                            int pt = (divisors[a] + divisors[a - 1]) / 2;
                            int delt = Math.Abs(horizontalGrids[v] - pt);
                            if (delt < closeValue)
                            {
                                closeValue = delt;
                                closePoint = pt;
                            }
                        }
                        if (closeValue < trueSection * 0.5 && v > 1 ? (closePoint - horizontalGrids[v - 1] < 1.2 * (horizontalGrids[v - 1] - horizontalGrids[v - 2])) : true)
                            horizontalGrids[v] = closePoint;
                    }
                }

                // add points
                foreach (int pt in horizontalGrids)
                {
                    DataPoint dp = new DataPoint();
                    dp.XValue = pt;
                    dp.YValues = new double[] { largest / 4 };
                    dp.Color = Color.Purple;
                    dp.MarkerStyle = MarkerStyle.Triangle;
                    chart1.Series[2 + seriesOffset].Points.Add(dp);
                }

            }


        }
        List<int> verticalGrids = new List<int>();
        List<int> horizontalGrids = new List<int>();

        int horizontalOffset, horizontalInterval, horizontalCount;
        int rowTitleYOffset = 0;
        int rowTitleWidth;
        int rowTitleXStart;
        int colTitleHeight;
        int colTitleYStart;

        private void BtnOCR_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            OcrResult result;
            Rectangle reducedRect;

            string[] rowTitles, colTitles;
            rowTitles = new string[verticalGrids.Count - 1];
            colTitles = new string[horizontalGrids.Count - 1];
            Rectangle rect = Rectangle.Empty;
            rect.Width = rowTitleWidth;
            rect.X = rowTitleXStart;
            Graphics g = Graphics.FromImage(grayImage);

            labMessage.Text = "row headers:  ";
            for (int r = 0; r < verticalGrids.Count - 1; r++)
            {
                rect.Y = verticalGrids[r] - rowTitleYOffset;
                rect.Height = verticalGrids[r + 1] - verticalGrids[r];

               // pictureBox2.Image = grayImage.Clone(rect, grayImage.PixelFormat);
                // AdjustRectangle(ref rect);
                //CenterBasedAdjustRectangle(ref rect);
                reducedRect = rect;
                    
                ZeroBoundAdjustRectangle(ref reducedRect);
                if (reducedRect.Left <= 1 || reducedRect.Height <= 1)
                {
                    rowTitles[r] = "";
                    continue;
                }
                //if (rect.Width != 0 && rect.Height != 0)
                //{
                //    Bitmap b = grayImage.Clone(rect, grayImage.PixelFormat);
                //    pictureBox1.Image = b;
                //}

                result = ocrReader.Read(grayImage, reducedRect);

                rowTitles[r] = result.Text;
                g.DrawRectangle(Pens.Red, rect);
                g.DrawRectangle(Pens.Blue, reducedRect);
                labMessage.Text += $" \"{rowTitles[r]}\"";

              // if( r== 4)  break;
            }

            labMessage.Text += "  column Headers:  ";

            rect.Height = colTitleHeight;
            rect.Y = colTitleYStart;
            for (int c = 0; c < horizontalGrids.Count - 1; c++)
            {
                rect.X = horizontalGrids[c];
                rect.Width = horizontalGrids[c + 1] - horizontalGrids[c];

                reducedRect = rect;
                // CenterBasedAdjustRectangle(ref rect);
                // AdjustRectangle(ref rect);
                ZeroBoundAdjustRectangle(ref reducedRect);
                if (reducedRect.Left <= 1 || reducedRect.Height <= 1)
                {
                    colTitles[c] = "";
                    continue;
                }

                result = ocrReader.Read(grayImage, reducedRect);

                string sss = result.Text.Trim().Replace( 'O','0');
                sss =  sss.Replace('o', '0');

                colTitles[c] = sss;
                bool OK = true;
                bool numberReset = false;
                int number = 0;
                try
                {
                    number = Convert.ToInt32(sss.Trim());
                    if( number.ToString().Length != sss.Trim().Length)
                    {
                        numberReset = true;
                    }
                }
                catch
                {
                    OK = false;
                }
                if (!OK)
                {
                    Bitmap bp = grayImage.Clone(reducedRect, grayImage.PixelFormat);
                    bp.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    result = ocrReader.Read(bp);
                    sss = result.Text.Trim().Replace('O', '0');
                    sss = sss.Replace('o', '0');
                    colTitles[c] = sss;
                    try
                    {
                        number = Convert.ToInt32(sss);
                        if (number.ToString().Length != sss.Length)
                        {
                            numberReset = true;
                        }
                    }
                    catch
                    {
                        bp.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        result = ocrReader.Read(bp);
                        number = -1;
                        numberReset = true;
                    }
                }
                if (numberReset)
                {
                    try
                    {
                        int lll = Convert.ToInt32(colTitles[c - 2]);
                        int nnn = Convert.ToInt32(colTitles[c - 1]);
                        colTitles[c] = (nnn + (nnn - lll)).ToString();
                    }
                    catch
                    {

                    }
                }
                    

                g.DrawRectangle(Pens.Red, rect);
                g.DrawRectangle(Pens.Blue, reducedRect);

                labMessage.Text += $" \"{colTitles[c]}\"";
            }
            pcbImage.Refresh();
            g.Dispose();
            Cursor = Cursors.Default;
        }

        int verticalOffset, verticalInterval, verticalCount;
        int verticalHeader, horizontalHeader, verticalHeader2, horizontalHeader2;

        int hOff, hSize, hcount, offLimit;
        byte tol = 10;

        private void BtnColLine_Click(object sender, EventArgs e)
        {
            int trials = 20;
            int vSize = colorImage.Height / 20;
            int vStart = 3 * vSize / 2;
            byte vv;
            bool OK = false;
            for (hcount = 5; hcount < 20; hcount++)
            {
                byte v;
                hSize = colorImage.Width / hcount;
                int start = hSize * 3 / 2;
                int end = start + hSize;
                int x = start;
                int y = vStart;
                for (hOff = start; hOff < end; hOff++)
                {
                    OK = true;
                    v = grayImage.GetPixel(x, y).R;
                    for (int c = 0; c < hcount - 1; c++)
                    {
                        x = c * hSize + hOff;
                        for (int r = 0; r < 19; r++)
                        {
                            y = vStart + vSize * r;
                            vv = grayImage.GetPixel(x, y).R;
                            if (vv - v > tol && v - vv > tol)
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

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
                Cursor = Cursors.WaitCursor;

                Text = "Character Recognizer + " + Path.GetFileName(dlgOpen.FileName);
                colorImage = new Bitmap(dlgOpen.FileName);
                pcbImage.Image = colorImage;
                pcbImage.SizeMode = PictureBoxSizeMode.StretchImage;
                isFramed = true; // 假設有框

                BtnGray_Click(null, null);
                Cursor = Cursors.Default;
            }
        }
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
                    if( ckbBinary.Checked )
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
            Graphics g = Graphics.FromImage(grayImage);
            int y = verticalOffset;
            Point ps = Point.Empty;
            ps.X = horizontalGrids[0];// horizontalOffset;
            Point pe = Point.Empty;
            pe.X = horizontalGrids[horizontalGrids.Count - 1];// horizontalOffset + horizontalCount * horizontalInterval;
            int st = 0;
            if( horizontalHeader < 0 && horizontalHeader2 < 0)
            {
                st = 1;
                ps.X = horizontalGrids[1];
            }
            // ps.Y = pe.Y = y;
            // g.DrawLine(Pens.Red, ps, pe);
            for ( int i = st; i < verticalGrids.Count; i++ )// verticalCount; i++ )
            {
                //y += verticalInterval;
                ps.Y = pe.Y = verticalGrids[i];// y;
                g.DrawLine(Pens.Red, ps, pe);
            }

            st = 0;
            ps.Y = verticalGrids[0];// verticalOffset;
            pe.Y = verticalGrids[verticalGrids.Count - 1]; // verticalOffset + verticalInterval * verticalCount;
            int x = horizontalOffset;
            if( verticalHeader < 0 && verticalHeader2 < 0 )
            {
                ps.Y = verticalGrids[1];
                st = 1;
            }
           // ps.X = pe.X = x;
           // g.DrawLine(Pens.Red, ps, pe);
            for ( int i = st; i < horizontalGrids.Count; i++ )// horizontalCount; i++ )
            {
                //x += horizontalInterval;
                ps.X = pe.X = horizontalGrids[i]; // x;
                g.DrawLine(Pens.Red, ps, pe);

            }

            // Draw row headers along vertical grid
            st = 0;
            int headerLength = grayImage.Width / horizontalGrids.Count / 4;
            if (headerLength < 20) headerLength = 20; // at least 40 pixel
            int ys = verticalGrids[0];
                int ye = verticalGrids[verticalGrids.Count - 1];
            int xe;
            if( horizontalHeader < 0 && horizontalHeader2 < 0 )
            {
                xe = ( horizontalGrids[0]+horizontalGrids[1])/2;
                st = 1;
                ys = verticalGrids[1];
            }
            else xe = horizontalHeader > 0 ? horizontalHeader : horizontalHeader2;
            if( xe <= headerLength ) headerLength = (int)(xe * 0.8 );
            xe = xe - headerLength;
            g.DrawLine(Pens.Green, xe, ys, xe, ye); // vertical line
            int xs = xe;
            xe = xe + headerLength + headerLength;
            g.DrawLine(Pens.Green, xe, ys, xe, ye); // vertical line            
            for( int i = st; i < verticalGrids.Count; i++ )
            {
                g.DrawLine(Pens.Green, xs, verticalGrids[i], xe, verticalGrids[i]); // vertical line
            }

            st = 0;
            // Draw column headers along vertical grid

            xs = horizontalGrids[0];
            xe = horizontalGrids[horizontalGrids.Count - 1];
            if (verticalHeader < 0 && verticalHeader2 < 0)
            {
                ye = (verticalGrids[0]+verticalGrids[1])/2;
                st = 1;
                xs = horizontalGrids[1];
            }
            else ye = verticalHeader > 0 ? verticalHeader : verticalHeader2;
            headerLength = grayImage.Height / verticalGrids.Count / 4;
            if (ye <= headerLength) headerLength = (int)(ye * 0.8);
            if (headerLength < 15) headerLength = 15; // at least 40 pixel
            if (headerLength > 28) headerLength = 28;

            ye = ye - headerLength;
            g.DrawLine(Pens.Green, xs, ye, xe, ye); // vertical line
            ys = ye;
            ye = ye + headerLength + headerLength;
            g.DrawLine(Pens.Green, xs, ye, xe, ye); // vertical line            
            for (int i = st; i < horizontalGrids.Count; i++)
            {
                g.DrawLine(Pens.Green, horizontalGrids[i],ys, horizontalGrids[i], ye); // vertical line
            }


        }

        int leftBound, rightBound, topBound, bottonBound;
        bool isFramed; // 是否有框

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
                        delta += Math.Abs(dataImage[r, c] - dataImage[r, c-1]);
                        total += dataImage[r, c];
                    }
                    total /= grayImage.Width; // 此列圖素值平均
                    //if (total < min) min = total;

                    chart1.Series[0+seriesOffset].Points.AddXY(r,total);
                    chart1.Series[1+seriesOffset].Points.AddXY(r+1,delta);
                }
            }
            else
            {
                for (int c = 0; c < grayImage.Width; c++)
                {
                    total = dataImage[ 0, c];
                    delta = 0;
                    for (int r = 1; r < grayImage.Height; r++)
                    {
                        delta += Math.Abs(dataImage[r,c ] - dataImage[r-1, c]);
                        total += dataImage[ r, c];
                    }
                    total /= grayImage.Height;
                    chart1.Series[0+seriesOffset].Points.AddXY(c,total);
                    chart1.Series[1+seriesOffset].Points.AddXY(c+1,delta);
                }
             }

            float limit = alongHeight ? grayImage.Height / 55.0f : grayImage.Width / 55.0f;
            float now = alongHeight ? grayImage.Height / (float)chart1.Series[1 + seriesOffset].Points.Count :
                grayImage.Width / (float)chart1.Series[1 + seriesOffset].Points.Count;


            if ( now < limit )
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
                        chart1.Series[1 + seriesOffset].Points[i - dec / 2].XValue ;
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

            int zeroBound = (int) largest / 50;
            // 找出標籤列或行，0開始0結尾，高度 largest/4 以上，寬 < 總寬 / 5，可能有兩個
            // 1: left, 2: left+right, 3: right, 4: interior
            int heightLimit = (int)( largest / 8 );
            int widthLimit = (int)(alongHeight ? grayImage.Height / 6 : grayImage.Width / 6);
            int failedCount;
            double zs, ze, peeks, peeke;
            double peekValue = 0;
            bool verticalInMiddle = false;
            bool horizontalInMiddle = false;

            if( alongHeight )
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
            for( int i = 0; i < pts.Count; i++)
            {
                if (zs < 0)
                {
                    if (peeks < 0)
                    {
                        // find first 0
                        if (pts[i].YValues[0] <= zeroBound )
                            zs =pts[i].XValue;
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
                        if (pts[i].YValues[0] <= zeroBound )
                        {
                            peeke = pts[i].XValue;
                            // find next non zero to set ze;
                            for (int j = i + 1; j < pts.Count; j++)
                            {
                                if (pts[j].YValues[0] > zeroBound )
                                {
                                    ze = pts[j].XValue;
                                    i = j;
                                    break;
                                }
                            }
                            if (ze < 0) ze = pts[ pts.Count - 1].XValue;
                            // Get a signal check validity
                            if (peekValue > heightLimit && ze - zs < widthLimit)
                            {
                                if (alongHeight)
                                {
                                    if (verticalHeader < 0)
                                        verticalHeader = (int)( (peeke + peeks) / 2);
                                    else
                                        verticalHeader2 = (int)( (peeke + peeks) / 2);
                                }
                                else
                                {
                                    if (horizontalHeader < 0)
                                        horizontalHeader = (int)( (peeke + peeks) / 2);
                                    else
                                        horizontalHeader2 = (int)( (peeke + peeks) / 2);
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
            int level = (int)( largest / 5 );
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

            valleyBound = (int)(smallest + (largest - smallest) /10);

            //bool OK = true;
            //do
            //{
                int centerStart = 0;

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
                            DataPoint dp = new DataPoint();
                            dp.MarkerStyle = MarkerStyle.Cross;
                            dp.Color = Color.Black;
                            dp.XValue = pts[i].XValue;
                            dp.YValues = new double[] { valleyBound };
                            chart1.Series[2 + seriesOffset].Points.Add(dp);
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
                            divisors.Add((int)((cliffDown + pts[i].XValue) / 2));
                            //if (divisors.Count > 1) offset = divisors[1] - divisors[0];
                            // 繼續下一個谷底的搜尋
                            cliffDown = -1;
                            // 如果已碰到水平線，結束搜尋
                            if (pts[i].YValues[0] < zeroBound) break;//提早結束
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
                            DataPoint dp = new DataPoint();
                            dp.MarkerStyle = MarkerStyle.Cross;
                            dp.Color = Color.Black;
                            dp.XValue = pts[i].XValue;
                            dp.YValues = new double[] { valleyBound };
                            chart1.Series[2 + seriesOffset].Points.Add(dp);
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

                            divisors.Add((int)((cliffDown + pts[i].XValue) / 2));
                            cliffDown = -1;
                            if (pts[i].YValues[0] < zeroBound) break;//提早結束
                        }
                    }
                }

                // 找到的原始分隔點加入線圖，y值設為0
                divisors.Sort();

            // 找最大 和次大的Gap 存在時，提升 valleylvel 添加 divisors
            int gapidx = 0, bigGap = divisors[1]-divisors[0];
            int secondidx = 0, secondGap = bigGap;
            double sum = bigGap;
            for( int c = 2; c < divisors.Count; c++)
            {
                int d = divisors[c] - divisors[c - 1];
                if (d > secondGap )
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
            if( bigGap > sum + sum )
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
                            DataPoint dp = new DataPoint();
                            dp.MarkerStyle = MarkerStyle.Cross;
                            dp.Color = Color.Black;
                            dp.XValue = pts[i].XValue;
                            dp.YValues = new double[] { valleyBound };
                            chart1.Series[2 + seriesOffset].Points.Add(dp);
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

                            divisors.Add((int)((cliffDown + pts[i].XValue) / 2));
                            cliffDown = -1;
                            if (pts[i].YValues[0] < zeroBound) break;//提早結束
                        }
                    }


                }
            }
            if (secondGap > sum * 3 /2)
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
                            DataPoint dp = new DataPoint();
                            dp.MarkerStyle = MarkerStyle.Cross;
                            dp.Color = Color.Black;
                            dp.XValue = pts[i].XValue;
                            dp.YValues = new double[] { valleyBound };
                            chart1.Series[2 + seriesOffset].Points.Add(dp);
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

                            divisors.Add((int)((cliffDown + pts[i].XValue) / 2));
                            cliffDown = -1;
                            if (pts[i].YValues[0] < zeroBound) break;//提早結束
                        }
                    }


                }
            }

            //     if (!OK)
            //     {
            //         OK = true; // only add search once
            //     }
            //     else
            //     {
            //         // 估算 間隔平均差異量
            //         double aagg = 0.0, bbb = divisors[0];
            //         for (int b = 1; b < divisors.Count; b++)
            //         {
            //             aagg += divisors[b] - divisors[b - 1];
            //             bbb += divisors[b];
            //         }
            //         aagg /= (divisors.Count - 1);
            //         bbb /=  divisors.Count;
            //         if (aagg > bbb / 3)
            //         {
            //             OK = false;
            //             // 提升 valley bound 添加分隔點
            //             valleyBound = (int)(smallest + (largest - smallest) / 2 );
            //         }
            //         else
            //         {
            //             // debug 強制
            //             OK = false;
            //             // 提升 valley bound 添加分隔點
            //             valleyBound = (int)(smallest + (largest - smallest) / 2);
            //         }
            //     }
            //} while (!OK);


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
            for( int i = divisors.Count -1; i > 0; i--)
            {
                if( ( divisors[i]-divisors[i-1] ) < toosmall )
                {
                    int ii = i-1;
                    ccc = 1;
                    while(  ii-1 >= 0 && divisors[ii] - divisors[ii-1] < toosmall )
                    {
                        ccc++;
                        ii = ii - 1;
                    }
                    float newloc = divisors[i];
                    for (int j = 0; j < ccc; j++)
                        newloc+= divisors[i - j - 1];
                    divisors[i] = (int)Math.Round( newloc / (ccc + 1));
                    //刪減雜訊分隔點
                    for (int j = 0; j < ccc; j++)
                        divisors.RemoveAt(i - j - 1);
                    i = i - ccc;
                }
            }


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
            for( int i = 2; i < divisors.Count-1;i++)
                intervals.Add(divisors[i] - divisors[i-1]);
 
            // 排除頭尾兩個，保留中間的 intervals
            intervals.Sort();

            sections.Add(intervals[0]);
            // l = intervals[0];
            float num = 4;
            float ttt = (float) intervals[0] / num; // tolerance
            counts.Add(1);
            int id = 0;
            for( int i = 1; i < intervals.Count-1; i++ )
            {
                // 接續的是否差異太大
                if( Math.Abs( intervals[i] - intervals[i-1]) < ttt )
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
            while( sections.Count > 2 )
            {
                // 刪掉個數最少者
                int small = counts[0];
                int idx = 0;
                for(int i = 1; i < counts.Count;i++)
                    if( counts[i] <= small )
                    {
                        idx = i;
                        small = counts[i];
                    }
                sections.RemoveAt(idx);
                counts.RemoveAt(idx);
            }

            //如果剩下兩個，個數懸殊，刪除個數小者
            if( sections.Count == 2)
            {
                if( counts[0] > counts[1]  )
                {
                    if( counts[0]-counts[1] > counts[1])
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
            }
            int startEnd;
            if (alongHeight) startEnd = verticalHeader >= 0 ? verticalHeader : 0;
            else startEnd = horizontalHeader >= 0 ? (  !horizontalInMiddle ? horizontalHeader : 0 ) : 0;
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
                int n = (int)( dis / trueSection);
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
                    for (int v = 0; v< verticalGrids.Count; v++)
                    {
                        int closePoint = -1, closeValue = int.MaxValue;
                        for(int a = 0; a < divisors.Count; a++)
                        {
                            int delt = Math.Abs(verticalGrids[ v] - divisors[ a] );
                            if(delt < closeValue)
                            {
                                closeValue = delt;
                                closePoint = divisors[a];
                            }
                        }
                        if (closeValue < trueSection / 4)
                            verticalGrids[v] = closePoint;
                    }
                }
                else
                {
                    for (int v = 0; v < verticalGrids.Count; v++)
                    {
                        int closePoint=0, closeValue = int.MaxValue;
                        for(int a = 1; a < divisors.Count; a++)
                        {
                            int pt = (divisors[a] + divisors[a - 1]) / 2;
                            int delt = Math.Abs(verticalGrids[v] - pt);
                            if (delt < closeValue)
                            {
                                closeValue = delt;
                                closePoint = pt;
                            }
                        }
                        if (closeValue < trueSection / 4)
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
                        for(int a =0; a < divisors.Count; a++ )
                        {
                            int delt = Math.Abs(horizontalGrids[v] - divisors[a]);
                            if (delt < closeValue)
                            {
                                closeValue = delt;
                                closePoint = divisors[a];
                            }
                        }
                        if (closeValue < trueSection / 4)
                            horizontalGrids[v] = closePoint;
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
                        if (closeValue < trueSection / 4)
                            horizontalGrids[v] = closePoint;
                    }
                }

                // add points
                foreach( int pt in horizontalGrids)
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
            for(  hcount = 5; hcount < 20; hcount++)
            {
                byte v;
                hSize = colorImage.Width / hcount;
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

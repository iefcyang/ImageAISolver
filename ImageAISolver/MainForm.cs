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
            ps.X = horizontalOffset;
            Point pe = Point.Empty;
            pe.X = horizontalOffset + horizontalCount * horizontalInterval;
            ps.Y = pe.Y = y;
            g.DrawLine(Pens.Red, ps, pe);
            for( int i = 0; i < verticalCount; i++ )
            {
                y += verticalInterval;
                ps.Y = pe.Y = y;
                g.DrawLine(Pens.Red, ps, pe);
            }

            ps.Y = verticalOffset;
            pe.Y = verticalOffset + verticalInterval * verticalCount;
            int x = horizontalOffset;
            ps.X = pe.X = x;
            g.DrawLine(Pens.Red, ps, pe);
            for ( int i = 0; i < horizontalCount; i++ )
            {
                x += horizontalInterval;
                ps.X = pe.X = x;
                g.DrawLine(Pens.Red, ps, pe);

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

                    chart1.Series[0+seriesOffset].Points.AddXY(r+1,total);
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
                    chart1.Series[0+seriesOffset].Points.AddXY(c+1,total);
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
                    chart1.Series[1 + seriesOffset].Points[idx].XValue = i - dec / 2;
                    chart1.Series[1 + seriesOffset].Points[idx].YValues[0] = y;
                    idx++;
                }
                for (int j = 0; j < dec / 2; j++)
                    chart1.Series[1 + seriesOffset].Points.RemoveAt(chart1.Series[1 + seriesOffset].Points.Count - 1);
            }
            
            /*
            Series s;
            int dec = 10;
            int sid = alongHeight ? 6 : 7;

            if (chart1.Series.Count > sid)
            {
                s = chart1.Series[sid];
                s.Points.Clear();
            }
            else
            {
                s = new Series();
                s.ChartType = SeriesChartType.Line;
                s.Color = Color.Black;
                s.BorderDashStyle = ChartDashStyle.Dot;
                chart1.Series.Add(s);
            }
 
            if (alongHeight) s.ChartArea = chart1.ChartAreas[0].Name;
            else s.ChartArea = chart1.ChartAreas[1].Name;
            s.YAxisType = AxisType.Secondary;

            for( int i = dec; i < chart1.Series[1+seriesOffset].Points.Count; i++ )
            {
                double y = chart1.Series[1+seriesOffset].Points[i].YValues[0];
                for( int j = 1; j < dec; j++)
                {
                    y+= chart1.Series[1 + seriesOffset].Points[i-j].YValues[0];
                }
                y /= dec;
                s.Points.AddXY(i-dec/2, y);
            }
            */


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
            int zs, ze, peeks, peeke;
            double peekValue = 0;
            if( alongHeight )
            {
                verticalHeader = -1;
                verticalHeader2 = -2;
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
                            zs = i;
                    }
                }
                else
                {
                    if (peeks < 0)
                    {
                        // find first not 0
                        if (pts[i].YValues[0] > zeroBound)
                            peeks = i;
                    }
                    else
                    {
                        // find second 0
                        if (pts[i].YValues[0] <= zeroBound )
                        {
                            peeke = i;
                            // find next non zero to set ze;
                            for (int j = i + 1; j < pts.Count; j++)
                            {
                                if (pts[j].YValues[0] > zeroBound )
                                {
                                    ze = j;
                                    i = j;
                                    break;
                                }
                            }
                            if (ze < 0) ze = pts.Count - 1;
                            // Get a signal check validity
                            if (peekValue > heightLimit && ze - zs < widthLimit)
                            {
                                if (alongHeight)
                                {
                                    if (verticalHeader < 0)
                                        verticalHeader = (peeke + peeks) / 2;
                                    else
                                        verticalHeader2 = (peeke + peeks) / 2;
                                }
                                else
                                {
                                    if (horizontalHeader < 0)
                                        horizontalHeader = (peeke + peeks) / 2;
                                    else
                                        horizontalHeader2 = (peeke + peeks) / 2;
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
                // update effect smallest 
                // 檢視差異線1/4到3/4段內的最小值，確認分界線的數值，以便定義數值範圍
                for (int i = pts.Count / 4; i < pts.Count * 3 / 4; i++)
                {
                    if (voidS < i && i < voidE) continue;
                    if (pts[i].YValues[0] < smallest)
                    {
                        smallest = pts[i].YValues[0];
                    }
                }
            }
            else
            {

                // update effect smallest 
                // 檢視差異線1/4到3/4段內的最小值，確認分界線的數值，以便定義數值範圍
                for (int i = pts.Count / 4; i < pts.Count * 3 / 4; i++)
                {
                    if (pts[i].YValues[0] < smallest)
                    {
                        smallest = pts[i].YValues[0];
                    }
                }
            }

            double factor = 3;// 2.5; // 5; // 4; // 1.5; // 列的範圍固定，谷底較淺
            if (!alongHeight) factor = factor * 2;// 因行的字數變化大，谷底放寬
            valleyBound = smallest *factor;
            int alt = (int)( smallest + (largest - smallest) / 10 );
            if (valleyBound > alt)
                valleyBound = alt;

            int centerStart = 0;

            // 由中間或 voidE 往右找到不能是谷底的起始點           
            for (int i = voidE == 0 ? pts.Count / 2 : voidE; i < pts.Count; i++)
            {
                if (pts[i].YValues[0] <= valleyBound) continue;
                else
                {
                    centerStart = i+1;
                    break;
                }
            }

            int cliffDown = -1;
            zeroBound = (int)( valleyBound / 10 );
            // 開始往右尋找分隔線的谷底分布
            for (int i = centerStart; i < pts.Count; i++)
            {
                if (cliffDown < 0) // 尚未有懸崖下墜點，找尋中 find start of the smallest
                {
                    if (  pts[i].YValues[0] <= valleyBound) cliffDown = i; // 找到下墜點
                }
                else // 已通過下墜點，在谷底中，尋找攀升點中 find end of the smallest 往右若碰到 0 表示邊界到了
                {
                    // 攀升點找到，或低迷到水平線永遠的谷底
                    if( pts[i].YValues[0] > valleyBound || pts[i].YValues[0] < zeroBound )
                    {
                        // 設定分隔點是谷底的中間
                        divisors.Add ( (cliffDown + i )  / 2 );
                        //if (divisors.Count > 1) offset = divisors[1] - divisors[0];
                        // 繼續下一個谷底的搜尋
                        cliffDown = -1;
                        // 如果已碰到水平線，結束搜尋
                        if (pts[i].YValues[0] < zeroBound ) break;//提早結束
                    }
                }
            }
            // 右側找完，往左找
            // 
            // 由中間或 voidS 往左找到不能是谷底的起始點           
            for (int i = voidS == pts.Count ? pts.Count / 2 : voidS; i >=0 ; i--)
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
                    if ( pts[i].YValues[0] <= valleyBound) cliffDown = i;
                }
                else // find end of the smallest
                {
                    if (valleyBound < pts[i].YValues[0] || pts[i].YValues[0] < zeroBound )
                    {
                        divisors.Add((cliffDown + i) / 2);
                        cliffDown = -1;
                        if (pts[i].YValues[0] < zeroBound ) break;//提早結束
                    }
                }
            }

            // 找到的原始分隔點加入線圖，y值設為0
            divisors.Sort();
            foreach (int i in divisors)
                chart1.Series[2+seriesOffset].Points.AddXY(i, 0);

            if (divisors.Count < 3) return;

            // 整理點資料調整出正確分隔點
             // 調整 divisions 
            // 取得間隔平均，若間隔小於平均的1/5視為需刪除調整的division
            float avg = divisors[1] - divisors[0];
            for (int i = 2; i < divisors.Count; i++)
                avg += divisors[i] - divisors[i - 1];
            avg /= divisors.Count - 1;
            avg /= 5;
            int ccc = 0;
            for( int i = divisors.Count -1; i > 0; i--)
            {
                if( ( divisors[i]-divisors[i-1] ) < avg )
                {
                    int ii = i-1;
                    ccc = 1;
                    while(  ii-1 >= 0 && divisors[ii] - divisors[ii-1] < avg )
                    {
                        ccc++;
                        ii = ii - 1;
                    }
                    float newloc = divisors[i];
                    for (int j = 0; j < ccc; j++)
                        newloc+= divisors[i - j - 1];
                    divisors[i] = (int)Math.Round( newloc / (ccc + 1));
                    for (int j = 0; j < ccc; j++)
                        divisors.RemoveAt(i - j - 1);
                    i = i - ccc;
                }
            }

            // 找出間隔 及數量
            List<int> intervals = new List<int>();
            List<float> sections = new List<float>();
            List<int> counts = new List<int>();
            int l = divisors[1];
            // 忽略前後兩個 bounds
            if (divisors.Count <= 2) return;
            for( int i = 2; i < divisors.Count-1;i++)
            {
                intervals.Add(divisors[i] - l);
                l = divisors[i];
            }
            // 排除頭尾兩個，保留中間的 intervals
            intervals.Sort();

            sections.Add(intervals[0]);
            l = intervals[0];
            float num = 8;
            float ttt = l / num; // tolerance
            counts.Add(1);
            int id = 0;
            for( int i = 1; i < intervals.Count-1; i++ )
            {
                // 接續的是否差異太大
                if( Math.Abs( intervals[i] -l ) < ttt )
                {
                    // 歸屬在此interval，更新成平均值
                    sections[id] = (counts[id] * sections[id] + intervals[i]) / (counts[id] + 1);
                    counts[id]++;
                }
                else
                {
                    // 詫異大，新增一個
                    sections.Add(intervals[i]);
                    counts.Add(1);
                    // 變更餘裕
                    ttt = intervals[i] / num;
                    id++;
                }
                l = intervals[i];
            }
            while( sections.Count > 2 )
            {
                // 刪掉個數最少者
                int small = counts[0];
                int idx = 0;
                for(int i = 1; i < counts.Count;i++)
                    if( counts[i] < small )
                    {
                        idx = i;
                        small = counts[i];
                    }
                sections.RemoveAt(idx);
                counts.RemoveAt(idx);
            }
            // 如果 section 只有一個
            int trueSection;
            if (sections.Count == 1) trueSection = (int)Math.Round( sections[0] );
            else
            {
                if (sections[1] > 5 * sections[0]) trueSection = (int)sections[0];
                else trueSection = (int)Math.Round( sections[0]  + sections[1] );
            }


            int ssss = sections.Count == 2 ?  (int)Math.Round( (divisors[2]+divisors[3])/2.0 - trueSection ): divisors[1]-trueSection;
            double thresh = largest / 100;
            if (alongHeight)
            {
                verticalInterval = trueSection;
                verticalCount = -1;
                // ssss 之前是否有 索引

                // 上有索引，找出索引 
                int ps = -1;
                // double largest = chart1.Series[1 + seriesOffset].Points[0].YValues[0];
                //for (int x = 1; x < ssss; x++)
                //{
                //    if (ps < 0)
                //    {
                //        if ( chart1.Series[1 + seriesOffset].Points[x-1].YValues[0] < thresh &&
                //            chart1.Series[1 + seriesOffset].Points[x].YValues[0] - chart1.Series[1 + seriesOffset].Points[x - 1].YValues[0] > thresh)
                //        {
                //            ps = x;
                //        }
                //    }
                //    else
                //    {
                //        if ( chart1.Series[1+seriesOffset].Points[x].YValues[0] < thresh )// && 
                //          //  chart1.Series[1 + seriesOffset].Points[x-1].YValues[0] - chart1.Series[1 + seriesOffset].Points[x].YValues[0] > thresh)
                //        {
                //            verticalHeader = (x + ps) / 2;
                //            break;
                //        }
                //    }
                //}


                //if ( ps >= 0 )
                //{
                //    verticalOffset = ssss;
                //}
                //else
                //{
                //    // 去除
                //    verticalOffset = ssss +  verticalInterval;
                //    verticalHeader = verticalOffset - verticalInterval / 2;
                //}

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
                //dp = new DataPoint();
                //dp.XValue = ps;
                //dp.YValues = new double[] { 25 };
                //dp.Color = Color.Black;
                //dp.MarkerStyle = MarkerStyle.Circle;
                //chart1.Series[2 + seriesOffset].Points.Add(dp);
            }
            else
            {
                horizontalInterval = trueSection;
                horizontalCount = -1;
                int ps = -1;
                //for (int x = 1; x < ssss; x++)
                //{
                //    if (ps < 0)
                //    {
                //        if ( chart1.Series[1 + seriesOffset].Points[x-1].YValues[0] < thresh &&
                //            chart1.Series[1 + seriesOffset].Points[x].YValues[0] - chart1.Series[1 + seriesOffset].Points[x - 1].YValues[0] > thresh  )
                //        {
                //            ps = x;
                //        }
                //    }
                //    else
                //    {
                //        if ( chart1.Series[1 + seriesOffset].Points[x].YValues[0] < thresh ) //&&
                //           // chart1.Series[1 + seriesOffset].Points[x-1].YValues[0] - chart1.Series[1 + seriesOffset].Points[x ].YValues[0] > thresh)
                //        {
                //            horizontalHeader = (x + ps) / 2;
                //            break;
                //        }
                //    }
                //}

                //if (ps >= 0)
                //{
                //    horizontalOffset = ssss;
                //}
                //else
                //{
                //    // 取除
                //    horizontalOffset = ssss + horizontalInterval;
                //    horizontalHeader = horizontalOffset - horizontalInterval / 2;
                //}
                // Add header points
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

                //dp = new DataPoint();
                //dp.XValue = ps;
                //dp.YValues = new double[] { 25 };
                //dp.Color = Color.Black;
                //dp.MarkerStyle = MarkerStyle.Circle;
                //chart1.Series[2 + seriesOffset].Points.Add(dp);
            }

            for (int i = ssss; i < (alongHeight ? colorImage.Height : colorImage.Width); )
            {
                DataPoint dp = new DataPoint();
                dp.XValue = i;
                dp.YValues = new double[] { 25 };
                dp.Color = Color.Purple;
                dp.MarkerStyle = MarkerStyle.Triangle;
                chart1.Series[2+seriesOffset].Points.Add(dp);
                i = i + trueSection;
                if (alongHeight) verticalCount++;
                else horizontalCount++;
            }





        }

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

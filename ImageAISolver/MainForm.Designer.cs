namespace ImageAISolver
{
    partial class MainForm
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea5 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea6 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend5 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Legend legend6 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series13 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series14 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series15 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series16 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series17 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series18 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title5 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.Windows.Forms.DataVisualization.Charting.Title title6 = new System.Windows.Forms.DataVisualization.Charting.Title();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.pcbImage = new System.Windows.Forms.PictureBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.ckbLog = new System.Windows.Forms.CheckBox();
            this.cbxSizeMode = new System.Windows.Forms.ComboBox();
            this.lsbContents = new System.Windows.Forms.ListBox();
            this.btnShowImage = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnOCR = new System.Windows.Forms.Button();
            this.ckbBinary = new System.Windows.Forms.CheckBox();
            this.btnColLine = new System.Windows.Forms.Button();
            this.btnBinary = new System.Windows.Forms.Button();
            this.btnGray = new System.Windows.Forms.Button();
            this.btnOpen = new System.Windows.Forms.Button();
            this.dlgOpen = new System.Windows.Forms.OpenFileDialog();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.labMessage = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pcbImage)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tabControl1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.ckbLog);
            this.splitContainer1.Panel2.Controls.Add(this.cbxSizeMode);
            this.splitContainer1.Panel2.Controls.Add(this.lsbContents);
            this.splitContainer1.Panel2.Controls.Add(this.btnShowImage);
            this.splitContainer1.Panel2.Controls.Add(this.pictureBox2);
            this.splitContainer1.Panel2.Controls.Add(this.pictureBox1);
            this.splitContainer1.Panel2.Controls.Add(this.btnOCR);
            this.splitContainer1.Panel2.Controls.Add(this.ckbBinary);
            this.splitContainer1.Panel2.Controls.Add(this.btnColLine);
            this.splitContainer1.Panel2.Controls.Add(this.btnBinary);
            this.splitContainer1.Panel2.Controls.Add(this.btnGray);
            this.splitContainer1.Panel2.Controls.Add(this.btnOpen);
            this.splitContainer1.Size = new System.Drawing.Size(1186, 714);
            this.splitContainer1.SplitterDistance = 1038;
            this.splitContainer1.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1038, 714);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.pcbImage);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1030, 685);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Image";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // pcbImage
            // 
            this.pcbImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pcbImage.Location = new System.Drawing.Point(3, 3);
            this.pcbImage.Name = "pcbImage";
            this.pcbImage.Size = new System.Drawing.Size(1024, 679);
            this.pcbImage.TabIndex = 0;
            this.pcbImage.TabStop = false;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.chart1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1030, 688);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Horizontal Line Feature";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // chart1
            // 
            chartArea5.AxisX.MajorGrid.LineColor = System.Drawing.Color.LightGray;
            chartArea5.AxisY.MajorGrid.LineColor = System.Drawing.Color.LightGray;
            chartArea5.AxisY2.MajorGrid.LineColor = System.Drawing.Color.LightGray;
            chartArea5.Name = "ChartArea1";
            chartArea6.AxisX.MajorGrid.LineColor = System.Drawing.Color.LightGray;
            chartArea6.AxisX2.MajorGrid.LineColor = System.Drawing.Color.LightGray;
            chartArea6.AxisY.MajorGrid.LineColor = System.Drawing.Color.LightGray;
            chartArea6.AxisY2.MajorGrid.LineColor = System.Drawing.Color.LightGray;
            chartArea6.Name = "ChartArea2";
            this.chart1.ChartAreas.Add(chartArea5);
            this.chart1.ChartAreas.Add(chartArea6);
            this.chart1.Dock = System.Windows.Forms.DockStyle.Fill;
            legend5.Alignment = System.Drawing.StringAlignment.Center;
            legend5.DockedToChartArea = "ChartArea1";
            legend5.IsDockedInsideChartArea = false;
            legend5.Name = "Legend1";
            legend6.Alignment = System.Drawing.StringAlignment.Center;
            legend6.DockedToChartArea = "ChartArea2";
            legend6.IsDockedInsideChartArea = false;
            legend6.Name = "Legend2";
            this.chart1.Legends.Add(legend5);
            this.chart1.Legends.Add(legend6);
            this.chart1.Location = new System.Drawing.Point(3, 3);
            this.chart1.Name = "chart1";
            series13.ChartArea = "ChartArea1";
            series13.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series13.Color = System.Drawing.Color.Red;
            series13.Legend = "Legend1";
            series13.LegendText = "Average";
            series13.Name = "Series1";
            series14.ChartArea = "ChartArea1";
            series14.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series14.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            series14.Legend = "Legend1";
            series14.LegendText = "Deviation Sum";
            series14.Name = "Series2";
            series14.YAxisType = System.Windows.Forms.DataVisualization.Charting.AxisType.Secondary;
            series15.ChartArea = "ChartArea1";
            series15.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            series15.Color = System.Drawing.Color.Blue;
            series15.Legend = "Legend1";
            series15.Name = "Series3";
            series15.YAxisType = System.Windows.Forms.DataVisualization.Charting.AxisType.Secondary;
            series16.ChartArea = "ChartArea2";
            series16.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series16.Color = System.Drawing.Color.Red;
            series16.Legend = "Legend2";
            series16.Name = "Series4";
            series17.ChartArea = "ChartArea2";
            series17.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series17.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            series17.Legend = "Legend2";
            series17.Name = "Series5";
            series17.YAxisType = System.Windows.Forms.DataVisualization.Charting.AxisType.Secondary;
            series18.ChartArea = "ChartArea2";
            series18.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            series18.Color = System.Drawing.Color.Blue;
            series18.Legend = "Legend2";
            series18.Name = "Series6";
            series18.YAxisType = System.Windows.Forms.DataVisualization.Charting.AxisType.Secondary;
            this.chart1.Series.Add(series13);
            this.chart1.Series.Add(series14);
            this.chart1.Series.Add(series15);
            this.chart1.Series.Add(series16);
            this.chart1.Series.Add(series17);
            this.chart1.Series.Add(series18);
            this.chart1.Size = new System.Drawing.Size(1024, 682);
            this.chart1.TabIndex = 0;
            this.chart1.Text = "chart1";
            title5.BackColor = System.Drawing.Color.White;
            title5.DockedToChartArea = "ChartArea1";
            title5.IsDockedInsideChartArea = false;
            title5.Name = "Row-wise (Vertical Division)";
            title5.Text = "Row-wise (Vertical Division)";
            title5.TextOrientation = System.Windows.Forms.DataVisualization.Charting.TextOrientation.Horizontal;
            title6.DockedToChartArea = "ChartArea2";
            title6.IsDockedInsideChartArea = false;
            title6.Name = "Colume-wise (Horizontal Division)";
            title6.Text = "Colume-wise (Horizontal Division)";
            this.chart1.Titles.Add(title5);
            this.chart1.Titles.Add(title6);
            // 
            // ckbLog
            // 
            this.ckbLog.AutoSize = true;
            this.ckbLog.Location = new System.Drawing.Point(12, 53);
            this.ckbLog.Name = "ckbLog";
            this.ckbLog.Size = new System.Drawing.Size(123, 20);
            this.ckbLog.TabIndex = 11;
            this.ckbLog.Text = "Exp Transformed";
            this.ckbLog.UseVisualStyleBackColor = true;
            // 
            // cbxSizeMode
            // 
            this.cbxSizeMode.FormattingEnabled = true;
            this.cbxSizeMode.Location = new System.Drawing.Point(11, 94);
            this.cbxSizeMode.Name = "cbxSizeMode";
            this.cbxSizeMode.Size = new System.Drawing.Size(121, 24);
            this.cbxSizeMode.TabIndex = 10;
            this.cbxSizeMode.SelectedValueChanged += new System.EventHandler(this.cbxSizeMode_SelectedValueChanged);
            // 
            // lsbContents
            // 
            this.lsbContents.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lsbContents.FormattingEnabled = true;
            this.lsbContents.ItemHeight = 16;
            this.lsbContents.Location = new System.Drawing.Point(5, 569);
            this.lsbContents.Name = "lsbContents";
            this.lsbContents.Size = new System.Drawing.Size(132, 116);
            this.lsbContents.TabIndex = 9;
            // 
            // btnShowImage
            // 
            this.btnShowImage.Location = new System.Drawing.Point(18, 435);
            this.btnShowImage.Name = "btnShowImage";
            this.btnShowImage.Size = new System.Drawing.Size(84, 24);
            this.btnShowImage.TabIndex = 8;
            this.btnShowImage.Text = "Next Image";
            this.btnShowImage.UseVisualStyleBackColor = true;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Red;
            this.pictureBox2.Location = new System.Drawing.Point(5, 462);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(101, 94);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 7;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Red;
            this.pictureBox1.Location = new System.Drawing.Point(5, 344);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(104, 85);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // btnOCR
            // 
            this.btnOCR.Location = new System.Drawing.Point(3, 304);
            this.btnOCR.Name = "btnOCR";
            this.btnOCR.Size = new System.Drawing.Size(125, 34);
            this.btnOCR.TabIndex = 5;
            this.btnOCR.Text = "Get Answer";
            this.btnOCR.UseVisualStyleBackColor = true;
            this.btnOCR.Click += new System.EventHandler(this.BtnOCR_Click);
            // 
            // ckbBinary
            // 
            this.ckbBinary.AutoSize = true;
            this.ckbBinary.Location = new System.Drawing.Point(12, 160);
            this.ckbBinary.Name = "ckbBinary";
            this.ckbBinary.Size = new System.Drawing.Size(94, 20);
            this.ckbBinary.TabIndex = 4;
            this.ckbBinary.Text = "Black/White";
            this.ckbBinary.UseVisualStyleBackColor = true;
            this.ckbBinary.Visible = false;
            // 
            // btnColLine
            // 
            this.btnColLine.Location = new System.Drawing.Point(7, 248);
            this.btnColLine.Name = "btnColLine";
            this.btnColLine.Size = new System.Drawing.Size(125, 30);
            this.btnColLine.TabIndex = 3;
            this.btnColLine.Text = "Column Separators";
            this.btnColLine.UseVisualStyleBackColor = true;
            this.btnColLine.Visible = false;
            this.btnColLine.Click += new System.EventHandler(this.BtnColLine_Click);
            // 
            // btnBinary
            // 
            this.btnBinary.Location = new System.Drawing.Point(7, 227);
            this.btnBinary.Name = "btnBinary";
            this.btnBinary.Size = new System.Drawing.Size(125, 25);
            this.btnBinary.TabIndex = 2;
            this.btnBinary.Text = "Binary B/W";
            this.btnBinary.UseVisualStyleBackColor = true;
            this.btnBinary.Visible = false;
            // 
            // btnGray
            // 
            this.btnGray.Location = new System.Drawing.Point(7, 186);
            this.btnGray.Name = "btnGray";
            this.btnGray.Size = new System.Drawing.Size(125, 35);
            this.btnGray.TabIndex = 1;
            this.btnGray.Text = "Gray B/W Image";
            this.btnGray.UseVisualStyleBackColor = true;
            this.btnGray.Visible = false;
            this.btnGray.Click += new System.EventHandler(this.BtnGray_Click);
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(12, 12);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(125, 35);
            this.btnOpen.TabIndex = 0;
            this.btnOpen.Text = "Open";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.BtnOpen_Click);
            // 
            // dlgOpen
            // 
            this.dlgOpen.DefaultExt = "jpg";
            this.dlgOpen.FileName = "openFileDialog1";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.labMessage});
            this.statusStrip1.Location = new System.Drawing.Point(0, 692);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1186, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // labMessage
            // 
            this.labMessage.IsLink = true;
            this.labMessage.Name = "labMessage";
            this.labMessage.Size = new System.Drawing.Size(1171, 17);
            this.labMessage.Spring = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1186, 714);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Character Recognizer";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pcbImage)).EndInit();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.PictureBox pcbImage;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.OpenFileDialog dlgOpen;
        private System.Windows.Forms.Button btnBinary;
        private System.Windows.Forms.Button btnGray;
        private System.Windows.Forms.Button btnColLine;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel labMessage;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.CheckBox ckbBinary;
        private System.Windows.Forms.Button btnOCR;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button btnShowImage;
        private System.Windows.Forms.ListBox lsbContents;
        private System.Windows.Forms.ComboBox cbxSizeMode;
        private System.Windows.Forms.CheckBox ckbLog;
    }
}


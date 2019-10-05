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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea7 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea8 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend7 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Legend legend8 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series19 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series20 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series21 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series22 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series23 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series24 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title7 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.Windows.Forms.DataVisualization.Charting.Title title8 = new System.Windows.Forms.DataVisualization.Charting.Title();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.pcbImage = new System.Windows.Forms.PictureBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
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
            this.btnShowImage = new System.Windows.Forms.Button();
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
            chartArea7.AxisX.MajorGrid.LineColor = System.Drawing.Color.LightGray;
            chartArea7.AxisY.MajorGrid.LineColor = System.Drawing.Color.LightGray;
            chartArea7.AxisY2.MajorGrid.LineColor = System.Drawing.Color.LightGray;
            chartArea7.Name = "ChartArea1";
            chartArea8.AxisX.MajorGrid.LineColor = System.Drawing.Color.LightGray;
            chartArea8.AxisX2.MajorGrid.LineColor = System.Drawing.Color.LightGray;
            chartArea8.AxisY.MajorGrid.LineColor = System.Drawing.Color.LightGray;
            chartArea8.AxisY2.MajorGrid.LineColor = System.Drawing.Color.LightGray;
            chartArea8.Name = "ChartArea2";
            this.chart1.ChartAreas.Add(chartArea7);
            this.chart1.ChartAreas.Add(chartArea8);
            this.chart1.Dock = System.Windows.Forms.DockStyle.Fill;
            legend7.Alignment = System.Drawing.StringAlignment.Center;
            legend7.DockedToChartArea = "ChartArea1";
            legend7.IsDockedInsideChartArea = false;
            legend7.Name = "Legend1";
            legend8.Alignment = System.Drawing.StringAlignment.Center;
            legend8.DockedToChartArea = "ChartArea2";
            legend8.IsDockedInsideChartArea = false;
            legend8.Name = "Legend2";
            this.chart1.Legends.Add(legend7);
            this.chart1.Legends.Add(legend8);
            this.chart1.Location = new System.Drawing.Point(3, 3);
            this.chart1.Name = "chart1";
            series19.ChartArea = "ChartArea1";
            series19.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series19.Color = System.Drawing.Color.Red;
            series19.Legend = "Legend1";
            series19.LegendText = "Average";
            series19.Name = "Series1";
            series20.ChartArea = "ChartArea1";
            series20.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series20.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            series20.Legend = "Legend1";
            series20.LegendText = "Deviation Sum";
            series20.Name = "Series2";
            series20.YAxisType = System.Windows.Forms.DataVisualization.Charting.AxisType.Secondary;
            series21.ChartArea = "ChartArea1";
            series21.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            series21.Color = System.Drawing.Color.Blue;
            series21.Legend = "Legend1";
            series21.Name = "Series3";
            series21.YAxisType = System.Windows.Forms.DataVisualization.Charting.AxisType.Secondary;
            series22.ChartArea = "ChartArea2";
            series22.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series22.Color = System.Drawing.Color.Red;
            series22.Legend = "Legend2";
            series22.Name = "Series4";
            series23.ChartArea = "ChartArea2";
            series23.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series23.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            series23.Legend = "Legend2";
            series23.Name = "Series5";
            series23.YAxisType = System.Windows.Forms.DataVisualization.Charting.AxisType.Secondary;
            series24.ChartArea = "ChartArea2";
            series24.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            series24.Color = System.Drawing.Color.Blue;
            series24.Legend = "Legend2";
            series24.Name = "Series6";
            series24.YAxisType = System.Windows.Forms.DataVisualization.Charting.AxisType.Secondary;
            this.chart1.Series.Add(series19);
            this.chart1.Series.Add(series20);
            this.chart1.Series.Add(series21);
            this.chart1.Series.Add(series22);
            this.chart1.Series.Add(series23);
            this.chart1.Series.Add(series24);
            this.chart1.Size = new System.Drawing.Size(1024, 682);
            this.chart1.TabIndex = 0;
            this.chart1.Text = "chart1";
            title7.BackColor = System.Drawing.Color.White;
            title7.DockedToChartArea = "ChartArea1";
            title7.IsDockedInsideChartArea = false;
            title7.Name = "Row-wise (Vertical Division)";
            title7.Text = "Row-wise (Vertical Division)";
            title7.TextOrientation = System.Windows.Forms.DataVisualization.Charting.TextOrientation.Horizontal;
            title8.DockedToChartArea = "ChartArea2";
            title8.IsDockedInsideChartArea = false;
            title8.Name = "Colume-wise (Horizontal Division)";
            title8.Text = "Colume-wise (Horizontal Division)";
            this.chart1.Titles.Add(title7);
            this.chart1.Titles.Add(title8);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Red;
            this.pictureBox2.Location = new System.Drawing.Point(6, 550);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(135, 129);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 7;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Red;
            this.pictureBox1.Location = new System.Drawing.Point(3, 383);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(138, 131);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // btnOCR
            // 
            this.btnOCR.Location = new System.Drawing.Point(16, 318);
            this.btnOCR.Name = "btnOCR";
            this.btnOCR.Size = new System.Drawing.Size(121, 59);
            this.btnOCR.TabIndex = 5;
            this.btnOCR.Text = "Get Answer";
            this.btnOCR.UseVisualStyleBackColor = true;
            this.btnOCR.Click += new System.EventHandler(this.BtnOCR_Click);
            // 
            // ckbBinary
            // 
            this.ckbBinary.AutoSize = true;
            this.ckbBinary.Location = new System.Drawing.Point(12, 105);
            this.ckbBinary.Name = "ckbBinary";
            this.ckbBinary.Size = new System.Drawing.Size(94, 20);
            this.ckbBinary.TabIndex = 4;
            this.ckbBinary.Text = "Black/White";
            this.ckbBinary.UseVisualStyleBackColor = true;
            // 
            // btnColLine
            // 
            this.btnColLine.Location = new System.Drawing.Point(12, 232);
            this.btnColLine.Name = "btnColLine";
            this.btnColLine.Size = new System.Drawing.Size(125, 49);
            this.btnColLine.TabIndex = 3;
            this.btnColLine.Text = "Column Separators";
            this.btnColLine.UseVisualStyleBackColor = true;
            this.btnColLine.Click += new System.EventHandler(this.BtnColLine_Click);
            // 
            // btnBinary
            // 
            this.btnBinary.Location = new System.Drawing.Point(12, 179);
            this.btnBinary.Name = "btnBinary";
            this.btnBinary.Size = new System.Drawing.Size(125, 35);
            this.btnBinary.TabIndex = 2;
            this.btnBinary.Text = "Binary B/W";
            this.btnBinary.UseVisualStyleBackColor = true;
            // 
            // btnGray
            // 
            this.btnGray.Location = new System.Drawing.Point(12, 53);
            this.btnGray.Name = "btnGray";
            this.btnGray.Size = new System.Drawing.Size(125, 35);
            this.btnGray.TabIndex = 1;
            this.btnGray.Text = "Gray B/W Image";
            this.btnGray.UseVisualStyleBackColor = true;
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
            // btnShowImage
            // 
            this.btnShowImage.Location = new System.Drawing.Point(16, 520);
            this.btnShowImage.Name = "btnShowImage";
            this.btnShowImage.Size = new System.Drawing.Size(84, 24);
            this.btnShowImage.TabIndex = 8;
            this.btnShowImage.Text = "Next Image";
            this.btnShowImage.UseVisualStyleBackColor = true;
           // this.btnShowImage.Click += new System.EventHandler(this.btnShowImage_Click);
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
    }
}


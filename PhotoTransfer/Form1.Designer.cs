namespace PhotoTransfer
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            BtnExcelSec = new Button();
            BtnKaynakKlasorSec = new Button();
            BtnHedefSec = new Button();
            BtnTasimaYap = new Button();
            TxtExcelPath = new TextBox();
            TxtHedefPath = new TextBox();
            TxtKaynakPath = new TextBox();
            ProgressStateBar = new ProgressBar();
            RtbLOG = new RichTextBox();
            dataGridView1 = new DataGridView();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            BtnGeriAl = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // BtnExcelSec
            // 
            BtnExcelSec.Location = new Point(12, 349);
            BtnExcelSec.Name = "BtnExcelSec";
            BtnExcelSec.Size = new Size(125, 24);
            BtnExcelSec.TabIndex = 0;
            BtnExcelSec.Text = "Excel Dosyası Seç";
            BtnExcelSec.UseVisualStyleBackColor = true;
            BtnExcelSec.Click += BtnExcelSec_Click;
            // 
            // BtnKaynakKlasorSec
            // 
            BtnKaynakKlasorSec.Location = new Point(12, 398);
            BtnKaynakKlasorSec.Name = "BtnKaynakKlasorSec";
            BtnKaynakKlasorSec.Size = new Size(125, 24);
            BtnKaynakKlasorSec.TabIndex = 1;
            BtnKaynakKlasorSec.Text = "Kaynak Klasör Seç";
            BtnKaynakKlasorSec.UseVisualStyleBackColor = true;
            BtnKaynakKlasorSec.Click += BtnKaynakKlasorSec_Click;
            // 
            // BtnHedefSec
            // 
            BtnHedefSec.Location = new Point(12, 446);
            BtnHedefSec.Name = "BtnHedefSec";
            BtnHedefSec.Size = new Size(125, 23);
            BtnHedefSec.TabIndex = 2;
            BtnHedefSec.Text = "Hedef Klasör Seç";
            BtnHedefSec.UseVisualStyleBackColor = true;
            BtnHedefSec.Click += BtnHedefSec_Click;
            // 
            // BtnTasimaYap
            // 
            BtnTasimaYap.Location = new Point(319, 505);
            BtnTasimaYap.Name = "BtnTasimaYap";
            BtnTasimaYap.Size = new Size(130, 32);
            BtnTasimaYap.TabIndex = 3;
            BtnTasimaYap.Text = "Taşıma Yap";
            BtnTasimaYap.UseVisualStyleBackColor = true;
            BtnTasimaYap.Click += BtnTasimaYap_Click;
            // 
            // TxtExcelPath
            // 
            TxtExcelPath.Location = new Point(163, 350);
            TxtExcelPath.Name = "TxtExcelPath";
            TxtExcelPath.ReadOnly = true;
            TxtExcelPath.Size = new Size(286, 23);
            TxtExcelPath.TabIndex = 4;
            TxtExcelPath.TextChanged += TxtExcelPath_TextChanged;
            // 
            // TxtHedefPath
            // 
            TxtHedefPath.Location = new Point(163, 447);
            TxtHedefPath.Name = "TxtHedefPath";
            TxtHedefPath.ReadOnly = true;
            TxtHedefPath.Size = new Size(286, 23);
            TxtHedefPath.TabIndex = 5;
            TxtHedefPath.TextChanged += TxtHedefPath_TextChanged;
            // 
            // TxtKaynakPath
            // 
            TxtKaynakPath.Location = new Point(163, 399);
            TxtKaynakPath.Name = "TxtKaynakPath";
            TxtKaynakPath.ReadOnly = true;
            TxtKaynakPath.Size = new Size(286, 23);
            TxtKaynakPath.TabIndex = 6;
            TxtKaynakPath.TextChanged += TxtKaynakPath_TextChanged;
            // 
            // ProgressStateBar
            // 
            ProgressStateBar.Location = new Point(571, 156);
            ProgressStateBar.Name = "ProgressStateBar";
            ProgressStateBar.Size = new Size(246, 23);
            ProgressStateBar.TabIndex = 8;
            // 
            // RtbLOG
            // 
            RtbLOG.Location = new Point(553, 340);
            RtbLOG.Name = "RtbLOG";
            RtbLOG.ReadOnly = true;
            RtbLOG.Size = new Size(289, 183);
            RtbLOG.TabIndex = 9;
            RtbLOG.Text = "";
            // 
            // dataGridView1
            // 
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.BackgroundColor = SystemColors.Highlight;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(12, 81);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowTemplate.Height = 25;
            dataGridView1.Size = new Size(437, 229);
            dataGridView1.TabIndex = 10;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.FromArgb(0, 192, 192);
            label1.Font = new Font("Calibri", 14.25F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(626, 94);
            label1.Name = "label1";
            label1.Size = new Size(130, 23);
            label1.TabIndex = 11;
            label1.Text = "İŞLEM ÇUBUĞU";
            label1.Click += label1_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Red;
            label2.Font = new Font("Calibri", 14.25F, FontStyle.Regular, GraphicsUnit.Point);
            label2.Location = new Point(642, 287);
            label2.Name = "label2";
            label2.Size = new Size(105, 23);
            label2.TabIndex = 12;
            label2.Text = "LOG EKRANI";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.Lime;
            label3.Font = new Font("Calibri", 14.25F, FontStyle.Regular, GraphicsUnit.Point);
            label3.Location = new Point(163, 29);
            label3.Name = "label3";
            label3.Size = new Size(141, 23);
            label3.TabIndex = 13;
            label3.Text = "DOSYA DURUMU";
            // 
            // BtnGeriAl
            // 
            BtnGeriAl.Location = new Point(163, 505);
            BtnGeriAl.Name = "BtnGeriAl";
            BtnGeriAl.Size = new Size(130, 32);
            BtnGeriAl.TabIndex = 14;
            BtnGeriAl.Text = "Geri Al";
            BtnGeriAl.UseVisualStyleBackColor = true;
            BtnGeriAl.Click += BtnGeriAl_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.MistyRose;
            ClientSize = new Size(903, 565);
            Controls.Add(BtnGeriAl);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(dataGridView1);
            Controls.Add(RtbLOG);
            Controls.Add(ProgressStateBar);
            Controls.Add(TxtKaynakPath);
            Controls.Add(TxtHedefPath);
            Controls.Add(TxtExcelPath);
            Controls.Add(BtnTasimaYap);
            Controls.Add(BtnHedefSec);
            Controls.Add(BtnKaynakKlasorSec);
            Controls.Add(BtnExcelSec);
            Name = "Form1";
            Text = "Fotoğraf Transfer Uygulaması";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button BtnExcelSec;
        private Button BtnKaynakKlasorSec;
        private Button BtnHedefSec;
        private Button BtnTasimaYap;
        private TextBox TxtExcelPath;
        private TextBox TxtHedefPath;
        private TextBox TxtKaynakPath;
        private ProgressBar ProgressStateBar;
        private RichTextBox RtbLOG;
        private DataGridView dataGridView1;
        private Label label1;
        private Label label2;
        private Label label3;
        private Button BtnGeriAl;
    }
}

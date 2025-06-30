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
            RtbLOG = new RichTextBox();
            label2 = new Label();
            label3 = new Label();
            lblOzetBilgi = new Label();
            dataGridView1 = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // BtnExcelSec
            // 
            BtnExcelSec.BackColor = Color.Lime;
            BtnExcelSec.Location = new Point(12, 326);
            BtnExcelSec.Name = "BtnExcelSec";
            BtnExcelSec.Size = new Size(145, 77);
            BtnExcelSec.TabIndex = 0;
            BtnExcelSec.Text = "Excel Dosyası Seç";
            BtnExcelSec.UseVisualStyleBackColor = false;
            BtnExcelSec.Click += BtnExcelSec_Click;
            // 
            // BtnKaynakKlasorSec
            // 
            BtnKaynakKlasorSec.BackColor = Color.Gold;
            BtnKaynakKlasorSec.FlatAppearance.BorderColor = Color.Gold;
            BtnKaynakKlasorSec.FlatAppearance.BorderSize = 0;
            BtnKaynakKlasorSec.Location = new Point(12, 410);
            BtnKaynakKlasorSec.Name = "BtnKaynakKlasorSec";
            BtnKaynakKlasorSec.Size = new Size(145, 79);
            BtnKaynakKlasorSec.TabIndex = 1;
            BtnKaynakKlasorSec.Text = "Kaynak Klasör Seç";
            BtnKaynakKlasorSec.UseVisualStyleBackColor = false;
            BtnKaynakKlasorSec.Click += BtnKaynakKlasorSec_Click;
            // 
            // BtnHedefSec
            // 
            BtnHedefSec.BackColor = Color.FromArgb(255, 128, 0);
            BtnHedefSec.Location = new Point(12, 495);
            BtnHedefSec.Name = "BtnHedefSec";
            BtnHedefSec.Size = new Size(145, 80);
            BtnHedefSec.TabIndex = 2;
            BtnHedefSec.Text = "Hedef Klasör Seç";
            BtnHedefSec.UseVisualStyleBackColor = false;
            BtnHedefSec.Click += BtnHedefSec_Click;
            // 
            // BtnTasimaYap
            // 
            BtnTasimaYap.BackColor = Color.FromArgb(0, 192, 192);
            BtnTasimaYap.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            BtnTasimaYap.ForeColor = SystemColors.ControlText;
            BtnTasimaYap.Location = new Point(200, 587);
            BtnTasimaYap.Name = "BtnTasimaYap";
            BtnTasimaYap.Size = new Size(145, 80);
            BtnTasimaYap.TabIndex = 3;
            BtnTasimaYap.Text = "Taşıma Yap";
            BtnTasimaYap.UseVisualStyleBackColor = false;
            BtnTasimaYap.Click += BtnTasimaYap_Click;
            // 
            // TxtExcelPath
            // 
            TxtExcelPath.BorderStyle = BorderStyle.FixedSingle;
            TxtExcelPath.Font = new Font("Calibri", 12F, FontStyle.Regular, GraphicsUnit.Point);
            TxtExcelPath.Location = new Point(163, 326);
            TxtExcelPath.Multiline = true;
            TxtExcelPath.Name = "TxtExcelPath";
            TxtExcelPath.ReadOnly = true;
            TxtExcelPath.Size = new Size(381, 77);
            TxtExcelPath.TabIndex = 4;
            TxtExcelPath.TextAlign = HorizontalAlignment.Center;
            TxtExcelPath.TextChanged += TxtExcelPath_TextChanged;
            // 
            // TxtHedefPath
            // 
            TxtHedefPath.BorderStyle = BorderStyle.FixedSingle;
            TxtHedefPath.Font = new Font("Calibri", 12F, FontStyle.Regular, GraphicsUnit.Point);
            TxtHedefPath.Location = new Point(163, 495);
            TxtHedefPath.Multiline = true;
            TxtHedefPath.Name = "TxtHedefPath";
            TxtHedefPath.ReadOnly = true;
            TxtHedefPath.Size = new Size(381, 80);
            TxtHedefPath.TabIndex = 5;
            TxtHedefPath.TextAlign = HorizontalAlignment.Center;
            TxtHedefPath.TextChanged += TxtHedefPath_TextChanged;
            // 
            // TxtKaynakPath
            // 
            TxtKaynakPath.BorderStyle = BorderStyle.FixedSingle;
            TxtKaynakPath.Font = new Font("Calibri", 12F, FontStyle.Regular, GraphicsUnit.Point);
            TxtKaynakPath.Location = new Point(163, 409);
            TxtKaynakPath.Multiline = true;
            TxtKaynakPath.Name = "TxtKaynakPath";
            TxtKaynakPath.ReadOnly = true;
            TxtKaynakPath.Size = new Size(381, 80);
            TxtKaynakPath.TabIndex = 6;
            TxtKaynakPath.TextAlign = HorizontalAlignment.Center;
            TxtKaynakPath.TextChanged += TxtKaynakPath_TextChanged;
            // 
            // RtbLOG
            // 
            RtbLOG.BorderStyle = BorderStyle.FixedSingle;
            RtbLOG.Location = new Point(663, 81);
            RtbLOG.Name = "RtbLOG";
            RtbLOG.ReadOnly = true;
            RtbLOG.Size = new Size(409, 556);
            RtbLOG.TabIndex = 9;
            RtbLOG.Text = "";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.FromArgb(255, 128, 128);
            label2.Font = new Font("Calibri", 14.25F, FontStyle.Regular, GraphicsUnit.Point);
            label2.Location = new Point(831, 28);
            label2.Name = "label2";
            label2.Size = new Size(105, 23);
            label2.TabIndex = 12;
            label2.Text = "LOG EKRANI";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.FromArgb(224, 224, 224);
            label3.Font = new Font("Calibri", 14.25F, FontStyle.Regular, GraphicsUnit.Point);
            label3.Location = new Point(214, 28);
            label3.Name = "label3";
            label3.Size = new Size(114, 23);
            label3.TabIndex = 13;
            label3.Text = "BİLGİ EKRANI";
            // 
            // lblOzetBilgi
            // 
            lblOzetBilgi.AutoSize = true;
            lblOzetBilgi.BackColor = Color.White;
            lblOzetBilgi.Font = new Font("Calibri", 20.25F, FontStyle.Bold, GraphicsUnit.Point);
            lblOzetBilgi.ForeColor = SystemColors.ControlText;
            lblOzetBilgi.Location = new Point(24, 96);
            lblOzetBilgi.Name = "lblOzetBilgi";
            lblOzetBilgi.Size = new Size(63, 33);
            lblOzetBilgi.TabIndex = 16;
            lblOzetBilgi.Text = "özet";
            lblOzetBilgi.Click += lblOzetBilgi_Click;
            // 
            // dataGridView1
            // 
            dataGridView1.BackgroundColor = SystemColors.ButtonHighlight;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(12, 81);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowTemplate.Height = 25;
            dataGridView1.Size = new Size(532, 192);
            dataGridView1.TabIndex = 17;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(1084, 679);
            Controls.Add(lblOzetBilgi);
            Controls.Add(dataGridView1);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(RtbLOG);
            Controls.Add(TxtKaynakPath);
            Controls.Add(TxtHedefPath);
            Controls.Add(TxtExcelPath);
            Controls.Add(BtnTasimaYap);
            Controls.Add(BtnHedefSec);
            Controls.Add(BtnKaynakKlasorSec);
            Controls.Add(BtnExcelSec);
            MaximizeBox = false;
            Name = "Form1";
            Text = "KMO Fotoğraf Transfer Uygulaması";
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
        private RichTextBox RtbLOG;
        private Label label2;
        private Label label3;
        private Label lblOzetBilgi;
        private DataGridView dataGridView1;
    }
}

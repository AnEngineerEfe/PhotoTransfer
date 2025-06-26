using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace PhotoTransfer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // Sadece "Durum" sütununu renklendirmek için
            dataGridView1.CellFormatting += DataGridView1_CellFormatting;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        // Excel’den tüm hücreleri oku
        private List<List<string>> ExceldenTumVeriyiOku(string excelDosyaYolu)
        {
            var tumVeri = new List<List<string>>();

            if (!File.Exists(excelDosyaYolu))
            {
                MessageBox.Show("Excel dosyasý bulunamadý!");
                return tumVeri;
            }

            try
            {
                using (var workbook = new XLWorkbook(excelDosyaYolu))
                {
                    var sheet = workbook.Worksheet(1);
                    var range = sheet.RangeUsed();

                    if (range == null)
                    {
                        MessageBox.Show("Excel dosyasý boþ.");
                        return tumVeri;
                    }

                    foreach (var row in range.Rows())
                    {
                        var satirVeri = new List<string>();
                        foreach (var cell in row.Cells())
                        {
                            satirVeri.Add(cell.GetString().Trim());
                        }
                        tumVeri.Add(satirVeri);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Excel okuma hatasý: " + ex.Message);
            }

            return tumVeri;
        }

        private void BtnExcelSec_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Excel Dosyalarý|*.xlsx;*.xls";
                ofd.Title = "Excel Dosyasý Seçin";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    TxtExcelPath.Text = ofd.FileName;
                }
            }
        }

        private void BtnKaynakKlasorSec_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                fbd.Description = "Kaynak klasörü seçin";
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    TxtKaynakPath.Text = fbd.SelectedPath;
                }
            }
        }

        private void BtnHedefSec_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                fbd.Description = "Hedef klasörü seçin";
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    TxtHedefPath.Text = fbd.SelectedPath;
                }
            }
        }

        private void BtnTasimaYap_Click(object sender, EventArgs e)
        {
            string excelYolu = TxtExcelPath.Text;
            string kaynakKlasor = TxtKaynakPath.Text;
            string hedefKlasor = TxtHedefPath.Text;

            if (string.IsNullOrWhiteSpace(excelYolu) ||
                string.IsNullOrWhiteSpace(kaynakKlasor) ||
                string.IsNullOrWhiteSpace(hedefKlasor))
            {
                MessageBox.Show("Lütfen tüm yollarý seçin!");
                return;
            }

            var tumExcelVerisi = ExceldenTumVeriyiOku(excelYolu);
            if (tumExcelVerisi.Count == 0)
            {
                MessageBox.Show("Excel dosyasý boþ veya okunamadý.");
                return;
            }

            List<string> dosyaAdlari = new List<string>();
            foreach (var satir in tumExcelVerisi)
            {
                foreach (var hucre in satir)
                {
                    if (!string.IsNullOrWhiteSpace(hucre))
                        dosyaAdlari.Add(hucre);
                }
            }

            RtbLOG.Clear();
            ProgressStateBar.Value = 0;
            ProgressStateBar.Maximum = dosyaAdlari.Count;

            int tasinanDosyaSayisi = 0;
            int hataSayisi = 0;
            string[] uzantilar = { ".jpg", ".jpeg", ".png" };

            List<AktarimSonucu> aktarimListesi = new List<AktarimSonucu>();

            foreach (var dosyaAdi in dosyaAdlari)
            {
                string kaynakDosyaYolu = null;

                foreach (var uzanti in uzantilar)
                {
                    string denemeYolu = Path.Combine(kaynakKlasor, dosyaAdi + uzanti);
                    if (File.Exists(denemeYolu))
                    {
                        kaynakDosyaYolu = denemeYolu;
                        break;
                    }
                }

                if (kaynakDosyaYolu == null)
                {
                    RtbLOG.AppendText($"[Uyarý] {dosyaAdi} bulunamadý veya geçersiz uzantý.\n");
                    aktarimListesi.Add(new AktarimSonucu { DosyaAdi = dosyaAdi, Durum = "Dosya Bulunamadý" });
                    hataSayisi++;
                    ProgressStateBar.Value++;
                    continue;
                }

                try
                {
                    string hedefDosyaYolu = Path.Combine(hedefKlasor, Path.GetFileName(kaynakDosyaYolu));

                    if (File.Exists(hedefDosyaYolu))
                        File.Delete(hedefDosyaYolu);

                    File.Move(kaynakDosyaYolu, hedefDosyaYolu);

                    RtbLOG.AppendText($"[Baþarýlý] {dosyaAdi} TAÞINDI.\n");
                    aktarimListesi.Add(new AktarimSonucu { DosyaAdi = dosyaAdi, Durum = "TAÞINDI" });
                    tasinanDosyaSayisi++;
                }
                catch (Exception ex)
                {
                    RtbLOG.AppendText($"[Hata] {dosyaAdi} taþýma hatasý: {ex.Message}\n");
                    aktarimListesi.Add(new AktarimSonucu { DosyaAdi = dosyaAdi, Durum = "Taþýma Hatasý" });
                    hataSayisi++;
                }

                ProgressStateBar.Value++;
            }

            MessageBox.Show($"Ýþlem tamamlandý.\nBaþarýlý: {tasinanDosyaSayisi}\nHatalý: {hataSayisi}");
            dataGridView1.DataSource = aktarimListesi;
        }

        // Sadece "Durum" hücresini renklendir
        private void DataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == "Durum")
            {
                string durum = e.Value?.ToString();
                if (durum == "TAÞINDI")
                {
                    e.CellStyle.BackColor = Color.LightGreen;
                }
                else if (durum == "Dosya Bulunamadý" || durum == "Taþýma Hatasý")
                {
                    e.CellStyle.BackColor = Color.LightCoral;
                }
            }
        }
    }

    public class AktarimSonucu
    {
        public string DosyaAdi { get; set; }
        public string Durum { get; set; }
    }
}

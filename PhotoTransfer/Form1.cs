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

            // Sadece "Durum" s�tununu renklendirmek i�in
            dataGridView1.CellFormatting += DataGridView1_CellFormatting;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        // Excel�den t�m h�creleri oku
        private List<List<string>> ExceldenTumVeriyiOku(string excelDosyaYolu)
        {
            var tumVeri = new List<List<string>>();

            if (!File.Exists(excelDosyaYolu))
            {
                MessageBox.Show("Excel dosyas� bulunamad�!");
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
                        MessageBox.Show("Excel dosyas� bo�.");
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
                MessageBox.Show("Excel okuma hatas�: " + ex.Message);
            }

            return tumVeri;
        }

        private void BtnExcelSec_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Excel Dosyalar�|*.xlsx;*.xls";
                ofd.Title = "Excel Dosyas� Se�in";
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
                fbd.Description = "Kaynak klas�r� se�in";
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
                fbd.Description = "Hedef klas�r� se�in";
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
                MessageBox.Show("L�tfen t�m yollar� se�in!");
                return;
            }

            var tumExcelVerisi = ExceldenTumVeriyiOku(excelYolu);
            if (tumExcelVerisi.Count == 0)
            {
                MessageBox.Show("Excel dosyas� bo� veya okunamad�.");
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
                    RtbLOG.AppendText($"[Uyar�] {dosyaAdi} bulunamad� veya ge�ersiz uzant�.\n");
                    aktarimListesi.Add(new AktarimSonucu { DosyaAdi = dosyaAdi, Durum = "Dosya Bulunamad�" });
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

                    RtbLOG.AppendText($"[Ba�ar�l�] {dosyaAdi} TA�INDI.\n");
                    aktarimListesi.Add(new AktarimSonucu { DosyaAdi = dosyaAdi, Durum = "TA�INDI" });
                    tasinanDosyaSayisi++;
                }
                catch (Exception ex)
                {
                    RtbLOG.AppendText($"[Hata] {dosyaAdi} ta��ma hatas�: {ex.Message}\n");
                    aktarimListesi.Add(new AktarimSonucu { DosyaAdi = dosyaAdi, Durum = "Ta��ma Hatas�" });
                    hataSayisi++;
                }

                ProgressStateBar.Value++;
            }

            MessageBox.Show($"��lem tamamland�.\nBa�ar�l�: {tasinanDosyaSayisi}\nHatal�: {hataSayisi}");
            dataGridView1.DataSource = aktarimListesi;
        }

        // Sadece "Durum" h�cresini renklendir
        private void DataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == "Durum")
            {
                string durum = e.Value?.ToString();
                if (durum == "TA�INDI")
                {
                    e.CellStyle.BackColor = Color.LightGreen;
                }
                else if (durum == "Dosya Bulunamad�" || durum == "Ta��ma Hatas�")
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

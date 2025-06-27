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
        private List<AktarimSonucu> sonAktarimListesi = new List<AktarimSonucu>();
        private string sonKaynakKlasor = "";
        private string sonHedefKlasor = "";

        public Form1()
        {
            InitializeComponent();

            // Sadece "Durum" s�tununu renklendiriyoruz
            dataGridView1.CellFormatting += DataGridView1_CellFormatting;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        // Excel�den t�m h�creleri okuyoruz
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
                ofd.InitialDirectory = "C:\\Users\\efe.erdogan\\Desktop";
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
                fbd.InitialDirectory = "C:\\Users\\efe.erdogan\\Desktop";
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
                fbd.InitialDirectory = "C:\\Users\\efe.erdogan\\Desktop";
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

            var excelDosyaAdi = Path.GetFileName(excelYolu);
            var kaynakKlasorAdi = Path.GetFileName(kaynakKlasor.TrimEnd('\\'));
            var hedefKlasorAdi = Path.GetFileName(hedefKlasor.TrimEnd('\\'));

            string mesaj = $"Se�ti�iniz Excel Dosyas�: {excelDosyaAdi}\n" +
                           $"Se�ti�iniz Kaynak Klas�r: {kaynakKlasorAdi}\n" +
                           $"Se�ti�iniz Hedef Klas�r: {hedefKlasorAdi}\n\n" +
                           "Ta��ma i�lemini yapmak istedi�inizden emin misiniz?";

            var cevap = MessageBox.Show(mesaj, "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (cevap != DialogResult.Yes)
                return;


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
                    // Uzant�lar� dosya ad�n�n yan�nda g�steriyoruz
                    string dosyaAdiUzantili = dosyaAdi + "(" + string.Join("/", uzantilar) + ")";

                    RtbLOG.AppendText($"[Uyar�] {dosyaAdiUzantili} bulunamad� veya ge�ersiz uzant�.\n");
                    aktarimListesi.Add(new AktarimSonucu { DosyaAdi = dosyaAdiUzantili, Durum = "DOSYA BULUNAMADI" });
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
                    aktarimListesi.Add(new AktarimSonucu { DosyaAdi = Path.GetFileName(kaynakDosyaYolu), Durum = "TA�INDI" });

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


            sonAktarimListesi = new List<AktarimSonucu>(aktarimListesi);
            sonKaynakKlasor = kaynakKlasor;
            sonHedefKlasor = hedefKlasor;


            TxtExcelPath.Text = "";
            TxtKaynakPath.Text = "";
            TxtHedefPath.Text = "";


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
                else if (durum == "DOSYA BULUNAMADI" || durum == "Ta��ma Hatas�")
                {
                    e.CellStyle.BackColor = Color.LightCoral;
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void TxtExcelPath_TextChanged(object sender, EventArgs e)
        {

        }

        private void TxtKaynakPath_TextChanged(object sender, EventArgs e)
        {

        }

        private void TxtHedefPath_TextChanged(object sender, EventArgs e)
        {

        }

        public class AktarimSonucu
        {
            public string DosyaAdi { get; set; }
            public string Durum { get; set; }
        }

        private void BtnGeriAl_Click(object sender, EventArgs e)
        {
            var onay = MessageBox.Show("Son i�lemi geri almak istedi�inizden emin misiniz?",
                           "Geri Al Onay�",
                           MessageBoxButtons.YesNo,
                           MessageBoxIcon.Question);

            if (onay != DialogResult.Yes)
                return;

            if (sonAktarimListesi == null || sonAktarimListesi.Count == 0)
            {
                MessageBox.Show("Geri al�nacak ta��ma i�lemi bulunamad�.");
                return;
            }

            int geriAlinanDosyaSayisi = 0;
            int hataSayisi = 0;

            // Geri alma s�ras�nda durumlar� g�ncellemek i�in yeni liste
            List<AktarimSonucu> geriAlmaSonuclari = new List<AktarimSonucu>();

            foreach (var sonuc in sonAktarimListesi)
            {
                if (sonuc.Durum == "TA�INDI")
                {
                    string hedefDosyaYolu = Path.Combine(sonHedefKlasor, sonuc.DosyaAdi);
                    string kaynakDosyaYolu = Path.Combine(sonKaynakKlasor, sonuc.DosyaAdi);

                    try
                    {
                        if (File.Exists(hedefDosyaYolu))
                        {
                            if (File.Exists(kaynakDosyaYolu))
                                File.Delete(kaynakDosyaYolu);

                            File.Move(hedefDosyaYolu, kaynakDosyaYolu);
                            geriAlinanDosyaSayisi++;
                            geriAlmaSonuclari.Add(new AktarimSonucu { DosyaAdi = sonuc.DosyaAdi, Durum = "GER� ALINDI" });
                            RtbLOG.AppendText($"[Ba�ar�l�] {sonuc.DosyaAdi} geri al�nd�.\n");
                        }
                        else
                        {
                            geriAlmaSonuclari.Add(new AktarimSonucu { DosyaAdi = sonuc.DosyaAdi, Durum = "GER� ALMA HATASI (Dosya bulunamad�)" });
                            hataSayisi++;
                            RtbLOG.AppendText($"[Hata] {sonuc.DosyaAdi} geri alma hatas�: Hedef dosya bulunamad�.\n");
                        }
                    }
                    catch (Exception ex)
                    {
                        geriAlmaSonuclari.Add(new AktarimSonucu { DosyaAdi = sonuc.DosyaAdi, Durum = "GER� ALMA HATASI" });
                        hataSayisi++;
                        RtbLOG.AppendText($"[Hata] {sonuc.DosyaAdi} geri alma hatas�: {ex.Message}\n");
                    }
                }
                else
                {
                    // Ta��nmam�� dosyalar i�in durum de�i�mez, ekleyelim
                    geriAlmaSonuclari.Add(sonuc);
                }
            }

            // DataGridView g�ncelle
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = geriAlmaSonuclari;

            MessageBox.Show($"Geri alma i�lemi tamamland�.\nBa�ar�l�: {geriAlinanDosyaSayisi}\nHatal�: {hataSayisi}");

            // sonAktarimListesi temizlenebilir veya yeni listeyle g�ncellenebilir
            sonAktarimListesi = geriAlmaSonuclari;
        }
    }
}

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
        private bool renklendirmeAktif = true;

        private List<AktarimSonucu> sonAktarimListesi = new List<AktarimSonucu>();
        private string sonKaynakKlasor = "";
        private string sonHedefKlasor = "";
       

        public Form1()
        {
            InitializeComponent();

            // Sadece "Durum" sütununu renklendiriyoruz
            dataGridView1.CellFormatting += DataGridView1_CellFormatting;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        // Excel’den tüm hücreleri okuyoruz
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
                ofd.InitialDirectory = "C:\\Users\\efe.erdogan\\Desktop";
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
                fbd.Description = "Hedef klasörü seçin";
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
                MessageBox.Show("Lütfen tüm yollarý seçin!");
                return;
            }

            var excelDosyaAdi = Path.GetFileName(excelYolu);
            var kaynakKlasorAdi = Path.GetFileName(kaynakKlasor.TrimEnd('\\'));
            var hedefKlasorAdi = Path.GetFileName(hedefKlasor.TrimEnd('\\'));

            string mesaj = $"Seçtiðiniz Excel Dosyasý: {excelDosyaAdi}\n" +
                           $"Seçtiðiniz Kaynak Klasör: {kaynakKlasorAdi}\n" +
                           $"Seçtiðiniz Hedef Klasör: {hedefKlasorAdi}\n\n" +
                           "Taþýma iþlemini yapmak istediðinizden emin misiniz?";

            var cevap = MessageBox.Show(mesaj, "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (cevap != DialogResult.Yes)
                return;


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
                    // Uzantýlarý dosya adýnýn yanýnda gösteriyoruz
                    string dosyaAdiUzantili = dosyaAdi + "(" + string.Join("/", uzantilar) + ")";

                    RtbLOG.AppendText($"[Uyarý] {dosyaAdiUzantili} bulunamadý veya geçersiz uzantý.\n");
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

                    RtbLOG.AppendText($"[Baþarýlý] {dosyaAdi} TAÞINDI.\n");
                    aktarimListesi.Add(new AktarimSonucu { DosyaAdi = Path.GetFileName(kaynakDosyaYolu), Durum = "TAÞINDI" });

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
            renklendirmeAktif = true;
            dataGridView1.DataSource = aktarimListesi;

            try
            {
                string logDosyaAdi = "tasima_log_" + DateTime.Now.ToString("dd_MM_yyyy-HH_mm_ss") + ".txt";
                string logDosyaYolu = Path.Combine(hedefKlasor, logDosyaAdi);

                using (StreamWriter sw = new StreamWriter(logDosyaYolu, false))
                {
                    sw.WriteLine("TAÞIMA ÝÞLEMÝ RAPORU");
                    sw.WriteLine("=====================");
                    sw.WriteLine("Tarih: " + DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss"));
                    sw.WriteLine("Excel Dosyasý: " + excelYolu);
                    sw.WriteLine("Kaynak Klasör: " + kaynakKlasor);
                    sw.WriteLine("Hedef Klasör: " + hedefKlasor);
                    sw.WriteLine();

                    foreach (var sonuc in aktarimListesi)
                    {
                        sw.WriteLine($"{sonuc.DosyaAdi} => {sonuc.Durum}");
                    }
                }

                RtbLOG.AppendText($"\n[Log] Taþýma raporu oluþturuldu: {logDosyaAdi}\n");

                // Log dosyasýný otomatik açýyoruz
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                {
                    FileName = logDosyaYolu,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                RtbLOG.AppendText($"\n[Hata] Log dosyasý oluþturulamadý: {ex.Message}\n");
            }



            sonAktarimListesi = new List<AktarimSonucu>(aktarimListesi);
            sonKaynakKlasor = kaynakKlasor;
            sonHedefKlasor = hedefKlasor;


            TxtExcelPath.Text = "";
            TxtKaynakPath.Text = "";
            TxtHedefPath.Text = "";


        }

        // Sadece "Durum" hücresini renklendir
        private void DataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (!renklendirmeAktif) return;

            if (dataGridView1.Columns[e.ColumnIndex].Name == "Durum")
            {
                string durum = e.Value?.ToString();
                if (durum == "TAÞINDI")
                {
                    e.CellStyle.BackColor = Color.LightGreen;
                }
                else if (durum == "DOSYA BULUNAMADI" || durum == "Taþýma Hatasý")
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
            var onay = MessageBox.Show("Son iþlemi geri almak istediðinizden emin misiniz?",
                           "Geri Al Onayý",
                           MessageBoxButtons.YesNo,
                           MessageBoxIcon.Question);

            if (onay != DialogResult.Yes)
                return;

            if (sonAktarimListesi == null || sonAktarimListesi.Count == 0)
            {
                MessageBox.Show("Geri alýnacak taþýma iþlemi bulunamadý.");
                return;
            }

            int geriAlinanDosyaSayisi = 0;
            int hataSayisi = 0;

            // Geri alma sýrasýnda durumlarý güncellemek için yeni liste
            List<AktarimSonucu> geriAlmaSonuclari = new List<AktarimSonucu>();

            foreach (var sonuc in sonAktarimListesi)
            {
                if (sonuc.Durum == "TAÞINDI")
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
                            geriAlmaSonuclari.Add(new AktarimSonucu { DosyaAdi = sonuc.DosyaAdi, Durum = "GERÝ ALINDI" });
                            RtbLOG.AppendText($"[Baþarýlý] {sonuc.DosyaAdi} geri alýndý.\n");
                        }
                        else
                        {
                            geriAlmaSonuclari.Add(new AktarimSonucu { DosyaAdi = sonuc.DosyaAdi, Durum = "GERÝ ALMA HATASI (Dosya bulunamadý)" });
                            hataSayisi++;
                            RtbLOG.AppendText($"[Hata] {sonuc.DosyaAdi} geri alma hatasý: Hedef dosya bulunamadý.\n");
                        }
                    }
                    catch (Exception ex)
                    {
                        geriAlmaSonuclari.Add(new AktarimSonucu { DosyaAdi = sonuc.DosyaAdi, Durum = "GERÝ ALMA HATASI" });
                        hataSayisi++;
                        RtbLOG.AppendText($"[Hata] {sonuc.DosyaAdi} geri alma hatasý: {ex.Message}\n");
                    }
                }
                else
                {
                    // Taþýnmamýþ dosyalar için durum deðiþmez, ekleyelim
                    geriAlmaSonuclari.Add(sonuc);
                }
            }

            // DataGridView güncelle
            renklendirmeAktif = false;

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = geriAlmaSonuclari;

            MessageBox.Show($"Geri alma iþlemi tamamlandý.\nBaþarýlý: {geriAlinanDosyaSayisi}\nHatalý: {hataSayisi}");

            // sonAktarimListesi temizlenebilir veya yeni listeyle güncellenebilir
            sonAktarimListesi = geriAlmaSonuclari;
        }

        private void BtnIleriAl_Click(object sender, EventArgs e)
        {
            var onay = MessageBox.Show("Geri alýnan dosyalarý tekrar taþýmak istiyor musunuz?",
        "Ýleri Al Onayý", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (onay != DialogResult.Yes) return;

            if (sonAktarimListesi == null || sonAktarimListesi.Count == 0)
            {
                MessageBox.Show("Ýleri alýnacak iþlem bulunamadý.");
                return;
            }

            int ileriAlinan = 0;
            int hata = 0;
            List<AktarimSonucu> ileriAlSonuclar = new List<AktarimSonucu>();

            foreach (var item in sonAktarimListesi)
            {
                if (item.Durum == "GERÝ ALINDI")
                {
                    string kaynak = Path.Combine(sonKaynakKlasor, item.DosyaAdi);
                    string hedef = Path.Combine(sonHedefKlasor, item.DosyaAdi);

                    try
                    {
                        if (File.Exists(kaynak))
                        {
                            if (File.Exists(hedef))
                                File.Delete(hedef);

                            File.Move(kaynak, hedef);
                            ileriAlinan++;
                            ileriAlSonuclar.Add(new AktarimSonucu { DosyaAdi = item.DosyaAdi, Durum = "TAÞINDI" });
                            RtbLOG.AppendText($"[Ýleri Al] {item.DosyaAdi} tekrar taþýndý.\n");
                        }
                        else
                        {
                            ileriAlSonuclar.Add(new AktarimSonucu { DosyaAdi = item.DosyaAdi, Durum = "ÝLERÝ AL HATASI (Kaynak yok)" });
                            hata++;
                            RtbLOG.AppendText($"[Hata] {item.DosyaAdi} ileri al hatasý: Kaynak dosya yok.\n");
                        }
                    }
                    catch (Exception ex)
                    {
                        ileriAlSonuclar.Add(new AktarimSonucu { DosyaAdi = item.DosyaAdi, Durum = "ÝLERÝ AL HATASI" });
                        hata++;
                        RtbLOG.AppendText($"[Hata] {item.DosyaAdi} ileri al hatasý: {ex.Message}\n");
                    }
                }
                else
                {
                    ileriAlSonuclar.Add(item);
                }
            }

            renklendirmeAktif = false;
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = ileriAlSonuclar;
            sonAktarimListesi = ileriAlSonuclar;

            MessageBox.Show($"Ýleri alma tamamlandý.\nBaþarýlý: {ileriAlinan}\nHatalý: {hata}");
        }
    }
}

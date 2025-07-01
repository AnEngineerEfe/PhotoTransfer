using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2016.Drawing.Charts;
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
        // private bool renklendirmeAktif = true;

        // private List<AktarimSonucu> sonAktarimListesi = new List<AktarimSonucu>();
        //  private string sonKaynakKlasor = "";
        // private string sonHedefKlasor = "";

        
        public Form1()
        {
            InitializeComponent();

            // Sadece "Durum" sütununu renklendiriyoruz
            // dataGridView1.CellFormatting += DataGridView1_CellFormatting;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lblOzetBilgi.Text = "";
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
                        var ilkHucre = row.Cell(1).GetString().Trim(); // Sadece 1. sütun (A)
                        satirVeri.Add(ilkHucre);
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

        string masaustuYolu = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        private void BtnExcelSec_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.InitialDirectory = masaustuYolu;
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
                fbd.InitialDirectory = masaustuYolu;
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
                fbd.InitialDirectory = masaustuYolu;
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    TxtHedefPath.Text = fbd.SelectedPath;
                }
            }
        }

        private async void BtnTasimaYap_Click(object sender, EventArgs e)
        {
            HashSet<string> tasinanDosyalar = new HashSet<string>();

            RtbLOG.Clear();

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

            // LoadingForm göster
            LoadingForm loadingForm = new LoadingForm();
            loadingForm.Show();
            this.Enabled = false;
            Application.DoEvents();

            List<AktarimSonucu> aktarimListesi = null;
            int tasinanDosyaSayisi = 0;
            int hataSayisi = 0;
            
            

           


            await Task.Run(() =>
            {
                
                var tumExcelVerisi = ExceldenTumVeriyiOku(excelYolu);
                if (tumExcelVerisi.Count == 0)
                    return;

                List<string> dosyaAdlari = new List<string>();
                foreach (var satir in tumExcelVerisi)
                {
                    foreach (var hucre in satir)
                    {
                        if (!string.IsNullOrWhiteSpace(hucre))
                        {
                            dosyaAdlari.Add(hucre);
                            
                        }
                    }
                }

                tasinanDosyaSayisi = 0;
                hataSayisi = 0;
                string[] uzantilar = { ".jpg", ".jpeg", ".png" };

                aktarimListesi = new List<AktarimSonucu>();
                int toplamDosyaSayisi = dosyaAdlari.Count;
                int sayac = 0;              

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
                        string dosyaAdiUzantili = dosyaAdi + "(" + string.Join("/", uzantilar) + ")";

                        var ayniIsimliDosya = tasinanDosyalar.FirstOrDefault(d => Path.GetFileNameWithoutExtension(d).Equals(dosyaAdi, StringComparison.OrdinalIgnoreCase));

                        if (!string.IsNullOrEmpty(ayniIsimliDosya))
                        {
                            aktarimListesi.Add(new AktarimSonucu { DosyaAdi = ayniIsimliDosya, Durum = "MÜKERRER KAYIT" });
                        }
                        else
                        {
                            aktarimListesi.Add(new AktarimSonucu { DosyaAdi = dosyaAdiUzantili, Durum = "DOSYA BULUNAMADI" });
                        }



                        hataSayisi++;
                        sayac++;
                        int progressValue = (int)((sayac / (double)toplamDosyaSayisi) * 100);
                        loadingForm.SetProgress(progressValue);
                        continue;
                    }

                    try
                    {
                        string hedefDosyaYolu = Path.Combine(hedefKlasor, Path.GetFileName(kaynakDosyaYolu));

                        if (File.Exists(hedefDosyaYolu))
                        {
                            aktarimListesi.Add(new AktarimSonucu { DosyaAdi = Path.GetFileName(kaynakDosyaYolu), Durum = "DOSYA ZATEN MEVCUT" });

                            
                            hataSayisi++;
                            sayac++;
                            int progressValue = (int)((sayac / (double)toplamDosyaSayisi) * 100);
                            loadingForm.SetProgress(progressValue);
                            continue; // Taþýma yapmadan geç
                        }


                        File.Move(kaynakDosyaYolu, hedefDosyaYolu);
                        tasinanDosyalar.Add(Path.GetFileName(kaynakDosyaYolu));



                        aktarimListesi.Add(new AktarimSonucu { DosyaAdi = Path.GetFileName(kaynakDosyaYolu), Durum = "TAÞINDI" });
                        tasinanDosyaSayisi++;
                    }
                    catch
                    {
                        aktarimListesi.Add(new AktarimSonucu { DosyaAdi = dosyaAdi, Durum = "Taþýma Hatasý" });
                        hataSayisi++;
                    }

                    sayac++;
                    int progress = (int)((sayac / (double)toplamDosyaSayisi) * 100);
                    loadingForm.SetProgress(progress);
                }
            });

            loadingForm.Close();
            this.Enabled = true;

            if (aktarimListesi == null || aktarimListesi.Count == 0)
            {
                MessageBox.Show("Excel dosyasý boþ veya okunamadý.");
                return;
            }

            // Sonuçlarý DataGridView'a aktar
            // renklendirmeAktif = true;
            // dataGridView1.DataSource = aktarimListesi;

            // Log dosyasýný oluþtur - sadece taþýnamayanlarý yaz
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
                        if (sonuc.Durum != "TAÞINDI")
                        {
                            string logSatiri = $"{sonuc.DosyaAdi} => {sonuc.Durum}";
                            sw.WriteLine(logSatiri);
                            this.Invoke((MethodInvoker)(() =>
                            {
                                RtbLOG.AppendText(logSatiri + "\n");
                            }));
                        }
                    }
                }

                RtbLOG.AppendText($"\n[Log] Taþýma raporu oluþturuldu: {Path.GetFileName(logDosyaYolu)}\n");

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

            // sonAktarimListesi = new List<AktarimSonucu>(aktarimListesi);
            //  sonKaynakKlasor = kaynakKlasor;
            //   sonHedefKlasor = hedefKlasor;

            TxtExcelPath.Text = "";
            TxtKaynakPath.Text = "";
            TxtHedefPath.Text = "";

            lblOzetBilgi.Text = $"Dosya Sayýsý: {tasinanDosyaSayisi + hataSayisi} " +
                    $"\n{tasinanDosyaSayisi} dosya taþýndý." +
                 
                    $"\n{hataSayisi} dosya taþýnamadý.";

            MessageBox.Show($"Ýþlem tamamlandý.\nBaþarýlý: {tasinanDosyaSayisi}\nHatalý: {hataSayisi}");
        }

        // Sadece "Durum" hücresini renklendir
        /* private void DataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
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
         } */

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

        private void lblOzetBilgi_Click(object sender, EventArgs e)
        {

        }
    }
}

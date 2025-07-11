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
        string masaustuYolu = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lblOzetBilgi.Text = "";
        }

        // Excel�den t�m h�creleri okuyoruz

        private List<string> ExceldenTumVeriyiOku(string excelDosyaYolu)
        {
            var tumVeri = new List<string>();

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
                        var hucre = row.Cell(1).GetString().Trim();
                        if (!string.IsNullOrWhiteSpace(hucre))
                            tumVeri.Add(hucre);
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
                ofd.InitialDirectory = masaustuYolu;
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
                fbd.Description = "Hedef klas�r� se�in";
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

            // LoadingForm g�ster
            LoadingForm loadingForm = new LoadingForm();
            loadingForm.Show();
            this.Enabled = false;
            Application.DoEvents();

            List<AktarimSonucu> aktarimListesi = null;
            int tasinanDosyaSayisi = 0;
            int hataSayisi = 0;

            await Task.Run(() =>
            {

                var dosyaAdlari = ExceldenTumVeriyiOku(excelYolu);


                tasinanDosyaSayisi = 0;
                hataSayisi = 0;
                string[] uzantilar = { ".jpg", ".jpeg", ".png" ,".mov",".mp4"};


                aktarimListesi = new List<AktarimSonucu>();
                int toplamDosyaSayisi = dosyaAdlari.Count;
                int sayac = 0;              

                foreach (var dosyaAdi in dosyaAdlari)
                {
                    string kaynakDosyaYolu = null;

                    foreach (var uzanti in uzantilar)
                    {
                       var tumDosyalar = Directory.GetFiles(kaynakKlasor, "*" + uzanti, SearchOption.TopDirectoryOnly);

                        foreach (var dosya in tumDosyalar)
                        {
                            string dosyaAdiKaynak = Path.GetFileNameWithoutExtension(dosya);
                            string dosyaAdiKisa = dosyaAdiKaynak.Length > 32 ? dosyaAdiKaynak.Substring(0, 32) : dosyaAdiKaynak;

                            if (dosyaAdi == dosyaAdiKisa)
                            {
                                kaynakDosyaYolu = dosya;
                                break;
                            }
                        }

                        if (kaynakDosyaYolu != null)
                            break;   
                    }


                    if (kaynakDosyaYolu == null)
                    {
                        string dosyaAdiUzantili = dosyaAdi + "(" + string.Join("/", uzantilar) + ")";

                        var ayniIsimliDosya = tasinanDosyalar.FirstOrDefault(d => Path.GetFileNameWithoutExtension(d).Equals(dosyaAdi, StringComparison.OrdinalIgnoreCase));

                        if (!string.IsNullOrEmpty(ayniIsimliDosya))
                        {
                            aktarimListesi.Add(new AktarimSonucu { DosyaAdi = ayniIsimliDosya, Durum = "M�KERRER DOSYA" });
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
                            continue; // Ta��ma yapmadan ge�
                        }


                        File.Move(kaynakDosyaYolu, hedefDosyaYolu);
                        tasinanDosyalar.Add(Path.GetFileName(kaynakDosyaYolu));

                        aktarimListesi.Add(new AktarimSonucu { DosyaAdi = Path.GetFileName(kaynakDosyaYolu), Durum = "TA�INDI" });
                        tasinanDosyaSayisi++;
                    }
                    catch
                    {
                        aktarimListesi.Add(new AktarimSonucu { DosyaAdi = dosyaAdi, Durum = "Ta��ma Hatas�" });
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
                MessageBox.Show("Excel dosyas� bo� veya okunamad�.");
                return;
            }

            // Log dosyas�n� olu�tur - sadece ta��namayanlar� yaz
            try
            {
                string logDosyaAdi = "Hatal� Kay�tlar  " + DateTime.Now.ToString("dd-MMMM-yyyy HH.mm.ss") + ".txt";
                string logDosyaYolu = Path.Combine(hedefKlasor, logDosyaAdi);

                using (StreamWriter sw = new StreamWriter(logDosyaYolu, false))
                {
                    sw.WriteLine("TA�IMA ��LEM� RAPORU");
                    sw.WriteLine("=====================");
                    sw.WriteLine("Tarih: " + DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss"));
                    sw.WriteLine("Excel Dosyas�: " + excelYolu);
                    sw.WriteLine("Kaynak Klas�r: " + kaynakKlasor);
                    sw.WriteLine("Hedef Klas�r: " + hedefKlasor);
                    sw.WriteLine();

                    foreach (var sonuc in aktarimListesi)
                    {
                        if (sonuc.Durum != "TA�INDI")
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

                RtbLOG.AppendText($"\n[Log] Ta��ma raporu olu�turuldu: {Path.GetFileName(logDosyaYolu)}\n");

                /*System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                {
                    FileName = logDosyaYolu,
                    UseShellExecute = true
                });*/
            }
            catch (Exception ex)
            {
                RtbLOG.AppendText($"\n[Hata] Log dosyas� olu�turulamad�: {ex.Message}\n");
            }

            TxtExcelPath.Text = "";
            TxtKaynakPath.Text = "";
            TxtHedefPath.Text = "";

            lblOzetBilgi.Text = $"Dosya Say�s�: {tasinanDosyaSayisi + hataSayisi} " +
                    $"\n{tasinanDosyaSayisi} dosya ta��nd�." +
                    $"\n{hataSayisi} dosya ta��namad�.";

            //MessageBox.Show($"��lem tamamland�.\nBa�ar�l�: {tasinanDosyaSayisi}\nHatal�: {hataSayisi}");
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

        private void lblOzetBilgi_Click(object sender, EventArgs e)
        {

        }
    }
}

using System;
using System.Windows.Forms;

namespace PhotoTransfer
{
    public partial class LoadingForm : Form
    {
        public LoadingForm()
        {
            InitializeComponent();

            // ProgressBar ayarları
            progressBar1.Minimum = 0;
            progressBar1.Maximum = 100;
            progressBar1.Value = 0;
            progressBar1.Style = ProgressBarStyle.Continuous;
        }

        private void label1_Click_1(object sender, EventArgs e)
        {
            // Buraya tıklama olayında ne yapılacaksa yazabilirsin.
            // Şimdilik boş bırakabilirsin.
        }


        // ProgressBar maksimum değerini ayarlamak için
        public void SetMaxProgress(int max)
        {
            if (progressBar1.InvokeRequired)
            {
                progressBar1.Invoke(new Action(() => progressBar1.Maximum = max));
            }
            else
            {
                progressBar1.Maximum = max;
            }
        }

        // ProgressBar değerini 1 arttırmak için
        public void IncrementProgress()
        {
            if (progressBar1.InvokeRequired)
            {
                progressBar1.Invoke(new Action(() =>
                {
                    if (progressBar1.Value < progressBar1.Maximum)
                        progressBar1.Value += 1;
                }));
            }
            else
            {
                if (progressBar1.Value < progressBar1.Maximum)
                    progressBar1.Value += 1;
            }
        }

        public void SetProgress(int value)
        {
            if (progressBar1.InvokeRequired)
            {
                progressBar1.Invoke(new Action(() => progressBar1.Value = value));
            }
            else
            {
                progressBar1.Value = value;
            }
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }
    }
}

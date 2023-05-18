
using System.Windows.Forms.DataVisualization.Charting;

namespace PrInf_lab5
{
    public partial class Form1 : Form
    {
        private GaloisLFSR generator;
        private Bitmap originalImage;
        private Bitmap encryptedImage;
        Chart chart = new Chart();
        public Form1()
        {
            InitializeComponent();

            chart.Size = new Size(400, 300); 
            chart.Location = new Point(20, 20);

            this.Controls.Add(chart);

            // Начальное значение регистра и обратную связь для генератора
            uint initialRegister = 0b1000;
            uint feedbackMask = 0b1001;
            generator = new GaloisLFSR(initialRegister, feedbackMask);

            originalImage = new Bitmap("C:\\VSProjects\\PrInf_lab5\\PrInf_lab5\\Resources\\tux.png");

            pictureBox.Image = originalImage;

            // Точечная диаграмма генерируемой последовательности
            DisplaySequenceChart();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            EncryptImage();
            MessageBox.Show("Изображение зашифровано.", "Шифрование", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void DisplaySequenceChart()
        {
            chart.Series.Clear();

            Series series = new Series("Bit Sequence");
            series.ChartType = SeriesChartType.Point;
            chart.SuppressExceptions = false;

            // Генерируем последовательность битов и добавляем их в серию
            for (int i = 0; i < originalImage.Width * originalImage.Height; i++)
            {
                bool bit = generator.GetNextBit();
                series.Points.AddXY(i, bit ? 1 : 0);
            }

            chart.Series.Add(series);
        }

        private void EncryptImage()
        {
            encryptedImage = new Bitmap(originalImage);

            // Гаммирование к каждому пикселю изображения
            for (int x = 0; x < originalImage.Width; x++)
            {
                for (int y = 0; y < originalImage.Height; y++)
                {
                    Color originalColor = originalImage.GetPixel(x, y);
                    Color encryptedColor = EncryptColor(originalColor);
                    encryptedImage.SetPixel(x, y, encryptedColor);
                }
            }
            pictureBox.Image = encryptedImage;
        }

        private Color EncryptColor(Color color)
        {
            byte r = color.R;
            byte g = color.G;
            byte b = color.B;

            // XOR к каждому компоненту цвета с битом из генератора
            r ^= generator.GetNextBit() ? (byte)0xFF : (byte)0x00;
            g ^= generator.GetNextBit() ? (byte)0xFF : (byte)0x00;
            b ^= generator.GetNextBit() ? (byte)0xFF : (byte)0x00;

            return Color.FromArgb(r, g, b);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            encryptedImage.Save("C:\\VSProjects\\PrInf_lab5\\PrInf_lab5\\Resources\\encrypted_tux.png");
            MessageBox.Show("Зашифрованное изображение сохранено.", "Сохранение", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
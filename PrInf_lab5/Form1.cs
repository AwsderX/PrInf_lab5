
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


            // ��������� �������� �������� � �������� ����� ��� ����������
            uint initialRegister = 0b1000;
            uint feedbackMask = 0b1001;
            generator = new GaloisLFSR(initialRegister, feedbackMask);

            originalImage = new Bitmap("C:\\VSProjects\\PrInf_lab5\\PrInf_lab5\\Resources\\tux.png");

            pictureBox.Image = originalImage;

            // ���������� ������������������ �����
            List<bool> bitSequence = GenerateBitSequence(originalImage.Width * originalImage.Height);

            // �������� ��������� ������������ ������������������
            DisplaySequenceChart(bitSequence);

            // ������ �������� ������������������ � ������� �������� ��-�������
            double chiSquare = ChiSquareTest(bitSequence);
            MessageBox.Show($"�������� ��-�������: {chiSquare}", "������ ��������", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            EncryptImage();
            MessageBox.Show("����������� �����������.", "����������", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void DisplaySequenceChart(List<bool> bitSequence)
        {
            chart.Series.Clear();

            Series series = new Series("Bit Sequence");
            series.ChartType = SeriesChartType.Point;

            // ��������� ���� � �����
            for (int i = 0; i < bitSequence.Count; i++)
            {
                bool bit = bitSequence[i];
                series.Points.AddXY(i, bit ? 1 : 0);
            }

            // �������� ����� ������� ���������
            ChartArea chartArea = new ChartArea();
            // ��������� �������� ��� ��� Y
            chartArea.AxisY.Minimum = -0.5;
            // ���������� ������� ��������� � ��������� ChartAreas
            chart.ChartAreas.Add(chartArea);
            chart.Series.Add(series);
        }

        private List<bool> GenerateBitSequence(int length)
        {
            List<bool> bitSequence = new List<bool>();

            for (int i = 0; i < length; i++)
            {
                bool bit = generator.GetNextBit();
                bitSequence.Add(bit);
            }

            return bitSequence;
        }

        private void EncryptImage()
        {
            encryptedImage = new Bitmap(originalImage);

            // ������������ � ������� ������� �����������
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

            // XOR � ������� ���������� ����� � ����� �� ����������
            r ^= generator.GetNextBit() ? (byte)0xFF : (byte)0x00;
            g ^= generator.GetNextBit() ? (byte)0xFF : (byte)0x00;
            b ^= generator.GetNextBit() ? (byte)0xFF : (byte)0x00;

            return Color.FromArgb(r, g, b);
        }

        private double ChiSquareTest(List<bool> bitSequence)
        {
            const int IntervalSize = 8; // ������ ��������� ��� ������ (� �����)
            int totalIntervals = bitSequence.Count / IntervalSize;

            int[] observedCounts = new int[totalIntervals];
            int[] expectedCounts = new int[totalIntervals];

            // ������� ����������� � ��������� �������
            for (int i = 0; i < bitSequence.Count; i++)
            {
                bool bit = bitSequence[i];
                int intervalIndex = i / IntervalSize;
                observedCounts[intervalIndex] += bit ? 1 : 0;
                expectedCounts[intervalIndex]++;
            }

            // ��������� ��-�������
            double chiSquare = 0.0;
            for (int i = 0; i < totalIntervals; i++)
            {
                double diff = observedCounts[i] - expectedCounts[i];
                chiSquare += diff * diff / expectedCounts[i];
            }

            return chiSquare;
        }
    }
}
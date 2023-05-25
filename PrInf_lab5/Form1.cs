using System.Drawing.Imaging;
using System.Windows.Forms.DataVisualization.Charting;

namespace PrInf_lab5
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            textBox1.Text = "1, 0, 0, 1, 1";
            textBox2.Text = "4, 3, 2";
            //originalImage = new Bitmap("C:\\VSProjects\\PrInf_lab5\\PrInf_lab5\\Resources\\tux.png");

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int header = 110;
            int block_s = 8;
            int[] seed = Array.ConvertAll(textBox1.Text.Split(','), Convert.ToInt32); // начальное значение регистра
            int[] taps = Array.ConvertAll(textBox2.Text.Split(','), Convert.ToInt32); // образующий многочлен для конфигурации Галуа x^4 + x^3 + x^2 + 1
            GaloisLFSR lfsr = new GaloisLFSR(seed, taps);
            int[] sequence = lfsr.Generate((int)Math.Pow(2, taps.Max()) - 1); //максимальное количество различных состояний
            textBoxOut.Text+=(string.Join(", ", sequence) + Environment.NewLine);

            int numBins = 2; // 0,1 
            double[] observedFrequencies = new double[numBins];
            for (int i = 0; i < sequence.Length; i++)
            {
                observedFrequencies[sequence[i]]++;
            }
            double expectedFrequency = sequence.Length / (double)numBins;
            double hi_sq = 0;
            for (int i = 0; i < numBins; i++)
            {
                hi_sq += Math.Pow(observedFrequencies[i] - expectedFrequency, 2) / expectedFrequency;
            }

            textBoxOut.Text += $"Хи-квадрат: {hi_sq}";

            // Загрузка изображения в память
            byte[] imageData = File.ReadAllBytes("C:\\VSProjects\\PrInf_lab5\\PrInf_lab5\\Resources\\tux.bmp");

            // Гаммирование каждого блока изображения
            int blocksCount = (imageData.Length - header) / block_s;
            for (int i = 0; i < blocksCount; i++)
            {
                // Получение следующего 8-битного блока для шифрования
                byte[] block = new byte[block_s];
                Array.Copy(imageData, header + i * block_s, block, 0, block_s);

                // Гаммирование блока и сохранение результата
                byte[] cipherBlock = new byte[block_s];
                byte[] keyStream = ToByteArray(lfsr.Generate(block_s * 8));
                for (int j = 0; j < block_s; j++)
                {
                    cipherBlock[j] = (byte)(block[j] ^ keyStream[j]);
                }
                Array.Copy(cipherBlock, 0, imageData, header + i * block_s, block_s);
            }

            // Сохранение зашифрованного изображения на диск
            File.WriteAllBytes("C:\\VSProjects\\PrInf_lab5\\PrInf_lab5\\Resources\\encrypted_tux.bmp", imageData);


            LFSRDiagram diagram = new LFSRDiagram(sequence);

            // Создаем окно и добавляем в него диаграмму
            Form form = new Form();
            form.FormClosing += (sender, e) => { Application.Exit(); };
            form.Controls.Add(diagram);
            form.Size = new Size(500, 300);
            form.ShowDialog();
        }
        private byte[] ToByteArray(int[] bits)
        {
            byte[] bytes = new byte[bits.Length / 8];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = 0;
                for (int j = 0; j < 8; j++)
                {
                    bytes[i] |= (byte)(bits[i * 8 + j] << j);
                }
            }
            return bytes;
        }
    }

}
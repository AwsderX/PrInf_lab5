using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrInf_lab5
{
    public class GaloisLFSR
    {
        private int[] seed; // Содержит значения битов регистра
        private int[] taps; // Содержит номера разрядов, используемых для обратной связи

        public GaloisLFSR(int[] seed, int[] taps)
        {
            this.seed = seed;
            this.taps = taps;
        }

        public void Shift()
        {
            int feedback = 0;
            // Вычисляем обратную связь
            foreach (int tap in taps)
            {
                feedback ^= seed[tap];
            }
            // Сдвигаем все биты регистра вправо на 1
            for (int i = seed.Length - 1; i > 0; i--)
            {
                seed[i] = seed[i - 1];
            }
            // Вставляем обратную связь в первый бит регистра
            seed[0] = feedback;
        }

        public int[] Generate(int length)
        {
            int[] bits = new int[length];

            for (int i = 0; i < length; i++)
            {
                // Генерируем следующий бит и записываем его в массив
                bits[i] = seed[0]; // Получаем младший бит регистра

                // Сдвигаем регистр на один бит вправо
                Shift();
            }

            return bits;
        }
    }
}

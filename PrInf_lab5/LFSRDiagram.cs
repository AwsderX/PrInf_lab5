using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrInf_lab5
{
    public class LFSRDiagram : Panel
    {
        private int[] sequence;

        public LFSRDiagram(int[] sequence)
        {
            this.sequence = sequence;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            int width = Width;
            int height = Height;
            int barWidth = Math.Max(1, width / sequence.Length);

            // Рисуем оси координат
            using (Pen pen = new Pen(Color.Black))
            {
                e.Graphics.DrawLine(pen, 0, height / 2, width, height / 2);
                e.Graphics.DrawLine(pen, width / 2, 0, width / 2, height);
            }

            // Рисуем точки
            using (Brush brush = new SolidBrush(Color.Red))
            {
                for (int i = 0; i < sequence.Length; i++)
                {
                    int x = i * barWidth;
                    int y = height / 2 - 10 * sequence[i];
                    e.Graphics.FillEllipse(brush, x, y, barWidth / 2, barWidth / 2);
                }
            }
        }
    }
}

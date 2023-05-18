using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrInf_lab5
{
    public class GaloisLFSR
    {
        private uint register;
        private uint feedbackMask;

        public GaloisLFSR(uint initialRegister, uint feedbackMask)
        {
            this.register = initialRegister;
            this.feedbackMask = feedbackMask;
        }

        public bool GetNextBit()
        {
            bool outputBit = (register & 0x01) == 1;
            bool feedbackBit = (register & feedbackMask) != 0;

            register >>= 1;

            if (feedbackBit)
            {
                register |= 0x80000000;
            }

            return outputBit;
        }
    }
}

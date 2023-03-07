using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleProgram
{
    class Pixel
    {
        private byte r;
        private byte g;
        private byte b;


        public Pixel(byte red, byte green, byte blue)
        {
            this.r = red;
            this.g = green;
            this.b = blue;
        }
        public byte[] toByteArray()
        {
            byte[] array = new byte[3] {
                (byte)this.r,
                (byte)this.g,
                (byte)this.b };

            return array;
        }
        public static bool IsEqual(Pixel a, Pixel b)
        {
            if (a.GetR == b.GetR && a.GetG == b.GetG && a.GetB == b.GetB)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public byte GetR
        {
            get { return this.r; }
            set { this.r = value; }
        }
        public byte GetG
        {
            get { return this.g; }
            set { this.g = value; }
        }
        public byte GetB
        {
            get { return this.b; }
            set { this.b = value; }
        }
        public string toString()
        {
            string retu = this.r + " " + this.g + " " + this.b;
            return retu;
        }
    }
}

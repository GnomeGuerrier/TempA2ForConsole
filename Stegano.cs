using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleProgram
{
    /// <summary>
    /// Class used to encode or decode Least Significant Bit steganography
    /// </summary>
    class Steganography : MyImage
    {



        public MyImage imageAEncoder;
        public MyImage encodeImage;
        /// <summary>
        /// Constructueur de image  pour la stéganographie, 
        /// </summary>
        /// <param name="originalim">Image originale</param>
        public Steganography(MyImage originalim) 
        {
            this.imageAEncoder = originalim;

            this.hauteur = imageAEncoder.hauteur;
            this.largeur = imageAEncoder.largeur;
            this.offset = imageAEncoder.offset;
            this.tailleFichier = imageAEncoder.tailleFichier;
            this.type = imageAEncoder.type;
            this.bpc = imageAEncoder.bpc;
            this.myfile = originalim.myfile;
            this.Path = originalim.Path;

        }

        
       
        /// <summary>
        /// Encode une image dans une autre, avec comme image de base l'image elle meme
        /// </summary>
        /// <param name="hiddenim">l'image a cacher</param>
        /// <param name="hiddenbits">Sur quel bits encoder l'image</param>
                       
        public void Encode(MyImage hiddenim, int hiddenbits)
        {
            encodeImage = this.imageAEncoder;
            for (int x = 0; x < this.imageAEncoder.image.GetLength(0); x++)
            {
                for (int y = 0; y < this.imageAEncoder.image.GetLength(1); y++)
                {
                    if (hiddenim.image.GetLength(0) > x && hiddenim.image.GetLength(1) > y)
                    {
                        string RedBin = ToBin(this.imageAEncoder.image[x, y].GetR, this.bpc / 3);
                        string GreenBin = ToBin(this.imageAEncoder.image[x, y].GetG, this.bpc / 3);
                        string BlueBin = ToBin(this.imageAEncoder.image[x, y].GetB, this.bpc / 3);

                        string HiddenRedBin = ToBin(hiddenim.image[x, y].GetR, this.bpc / 3);
                        string HiddenGreenBin = ToBin(hiddenim.image[x, y].GetG, this.bpc / 3);
                        string HiddenBlueBin = ToBin(hiddenim.image[x, y].GetB, this.bpc / 3);

                        string NewRed = RedBin.Substring(0, this.bpc / 3 - hiddenbits) + HiddenRedBin.Substring(this.bpc / 3 - hiddenbits);
                        string NewGreen = GreenBin.Substring(0, this.bpc / 3 - hiddenbits) + HiddenGreenBin.Substring(this.bpc / 3 - hiddenbits);
                        string NewBlue = BlueBin.Substring(0, this.bpc / 3 - hiddenbits) + HiddenBlueBin.Substring(this.bpc / 3 - hiddenbits);

                        encodeImage.image[x, y].GetR = (byte)Convert.ToInt32(NewRed, 2);
                        encodeImage.image[x, y].GetG = (byte)Convert.ToInt32(NewGreen, 2);
                        encodeImage.image[x, y].GetB = (byte)Convert.ToInt32(NewBlue, 2);
                    }
                }
            }
            //AdjustBrightnessAndContrast(encodeImage,50,1.2);
            encodeImage.From_Image_To_File("TestHugoFinal");
        }



        public static void AdjustBrightnessAndContrast(MyImage image, double brightnessScale, double contrastScale)
        {
            for (int x = 0; x < image.image.GetLength(0); x++)
            {
                for (int y = 0; y < image.image.GetLength(1); y++)
                {
                    int newR = (int)Math.Round((image.image[x, y].GetR - 128) * contrastScale + 128 + brightnessScale);
                    int newG = (int)Math.Round((image.image[x, y].GetG - 128) * contrastScale + 128 + brightnessScale);
                    int newB = (int)Math.Round((image.image[x, y].GetB - 128) * contrastScale + 128 + brightnessScale);

                    image.image[x, y].GetR = (byte)Math.Min(Math.Max(newR, 0), 255);
                    image.image[x, y].GetG = (byte)Math.Min(Math.Max(newG, 0), 255);
                    image.image[x, y].GetB = (byte)Math.Min(Math.Max(newB, 0), 255);
                }
            }
        }



        public static string ToBin(int value, int len)
        {
            string temp = "";
            if (len > 1)
            {
                temp = ToBin(value >> 1, len - 1);
            }
            else
            {
                temp = "";
            }
            temp += "01"[value & 1];
            return temp;
        }

        public void Decode()
        {

            for (int bits = 1; bits < this.bpc / 3; bits++)
            {
                MyImage decodedim = this.imageAEncoder;
                decodedim.Path = imageAEncoder.Path;                                                    //Same properties, only the matrix is going to change
                for (int x = 0; x < this.imageAEncoder.image.GetLength(0); x++)
                {
                    for (int y = 0; y < this.imageAEncoder.image.GetLength(1); y++)
                    {

                        //string of binary conversion of the value of each decimal color value of the x,y pixel
                        string RedBin = ToBin(this.imageAEncoder.image[x, y].GetR, this.bpc / 3);
                        string GreenBin = ToBin(this.imageAEncoder.image[x, y].GetG, this.bpc / 3);
                        string BlueBin = ToBin(this.imageAEncoder.image[x, y].GetB, this.bpc / 3);

                        string NewRed = "";
                        string NewGreen = "";
                        string NewBlue = "";

                        for (int digit = 0; digit < this.bpc / 3; digit++)   //counter from 0 to 7, to access each binary completly
                        {

                            //If the bits is 2, we want want to take all the bits from the (8-2)=6th bit
                            //if the digit considered is lower than the significant bits that we are treated right now, we don't take it into account because it belongs to the original image
                            if (digit >= (8 - bits)) NewRed += RedBin[digit];
                        }
                        for (int digit = 0; digit < this.bpc / 3; digit++)
                        {
                            if (digit >= (8 - bits)) NewGreen += GreenBin[digit];
                        }
                        for (int digit = 0; digit < this.bpc / 3; digit++)
                        {
                            if (digit >= (8 - bits)) NewBlue += BlueBin[digit];
                        }
                        //Since we only took [bits] digits to create the new pixel, we need to add [8-bits] 0 to the end of the value:
                        for (int i = 0; i < (8 - bits); i++)
                        {
                            NewRed += "0";
                            NewGreen += "0";
                            NewBlue += "0";
                        }

                        if (RedBin != "00000000" || GreenBin != "00000000" || BlueBin != "00000000")
                        {
                            /*Console.WriteLine("Pixel (" + x + "," + y + ")");
                            Console.WriteLine("Original R: " + RedBin + ", G: " + GreenBin + ", B: " + BlueBin);
                            Console.WriteLine("Encoded R: " + NewRed + ", G: " + NewGreen + ", B: " + NewBlue);*/
                        }





                        //Remap new values
                        decodedim.image[x, y].GetR = (byte)Convert.ToInt32(Convert.ToString(Convert.ToInt32(NewRed, 2), 10));
                        decodedim.image[x, y].GetG = (byte)Convert.ToInt32(Convert.ToString(Convert.ToInt32(NewGreen, 2), 10));
                        decodedim.image[x, y].GetB = (byte)Convert.ToInt32(Convert.ToString(Convert.ToInt32(NewBlue, 2), 10));
                        //Console.WriteLine(decodedim.myfile[x, y].toString());

                    }
                }

                decodedim.From_Image_To_File("Stegano_Decoded_SB=" + bits);
                Console.WriteLine("Done Saving Stegano_Decoded_SB=" + bits);

            }
        }



    }
}
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
                        string NouveauVert = GreenBin.Substring(0, this.bpc / 3 - hiddenbits) + HiddenGreenBin.Substring(this.bpc / 3 - hiddenbits);
                        string NouveauBleue = BlueBin.Substring(0, this.bpc / 3 - hiddenbits) + HiddenBlueBin.Substring(this.bpc / 3 - hiddenbits);

                        encodeImage.image[x, y].GetR = (byte)Convert.ToInt32(NewRed, 2);
                        encodeImage.image[x, y].GetG = (byte)Convert.ToInt32(NouveauVert, 2);
                        encodeImage.image[x, y].GetB = (byte)Convert.ToInt32(NouveauBleue, 2);
                    }
                }
            }
            //AdjustBrightnessAndContrast(encodeImage,50,1.2);
            encodeImage.From_Image_To_File("TestHugoFinal");
        }


        /// <summary>
        /// Permet si besoin d'augmenter la luminosité d'une image
        /// </summary>
        /// <param name="image">l'image de base</param>
        /// <param name="brightnessScale">La quatité à augmenter</param>
        /// <param name="contrastScale">le contraste a appliquer</param>
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


        /// <summary>
        /// Permet de transformer en binaire le code
        /// </summary>
        /// <param name="value">la valeur</param>
        /// <param name="len">la taille</param>
        /// <returns></returns>
        public static string ToBin(int value, int longueur)
        {
            string temporaire = "";
            if (longueur > 1)
            {
                temporaire = ToBin(value >> 1, longueur - 1);
            }
            else
            {
                temporaire = "";
            }
            temporaire += "01"[longueur & 1];
            return temporaire;
        }
        /// <summary>
        /// permet de décoder l'image sélectionnée
        /// </summary>
        public void Decode()
        {

            for (int bits = 1; bits < this.bpc / 3; bits++)
            {
                MyImage decodedim = this.imageAEncoder;
                decodedim.Path = imageAEncoder.Path;                                                    
                for (int x = 0; x < this.imageAEncoder.image.GetLength(0); x++)
                {
                    for (int y = 0; y < this.imageAEncoder.image.GetLength(1); y++)
                    {

                        //Permet de chopper les bits de fin (LSB)
                        string RedBin = ToBin(this.imageAEncoder.image[x, y].GetR, this.bpc / 3);
                        string GreenBin = ToBin(this.imageAEncoder.image[x, y].GetG, this.bpc / 3);
                        string BlueBin = ToBin(this.imageAEncoder.image[x, y].GetB, this.bpc / 3);

                        string NouveauR = "";
                        string NouveauVert = "";
                        string NouveauBleue = "";

                        for (int digit = 0; digit < this.bpc / 3; digit++)   //permet d'acceder au 7 bits completement
                        {

                            if (digit >= (8 - bits)) NouveauR += RedBin[digit];
                        }
                        for (int digit = 0; digit < this.bpc / 3; digit++)
                        {
                            if (digit >= (8 - bits)) NouveauVert += GreenBin[digit];
                        }
                        for (int digit = 0; digit < this.bpc / 3; digit++)
                        {
                            if (digit >= (8 - bits)) NouveauBleue += BlueBin[digit];
                        }
                        //Since we only took [bits] digits to create the new pixel, we need to add [8-bits] 0 to the end of the value:
                        for (int i = 0; i < (8 - bits); i++)
                        {
                            NouveauR += "0";
                            NouveauVert += "0";
                            NouveauBleue += "0";
                        }

                        if (RedBin != "00000000" || GreenBin != "00000000" || BlueBin != "00000000")
                        {
                            /*Console.WriteLine("Pixel (" + x + "," + y + ")");
                            Console.WriteLine("Original R: " + RedBin + ", G: " + GreenBin + ", B: " + BlueBin);
                            Console.WriteLine("Encoded R: " + NewRed + ", G: " + NouveauVert + ", B: " + NouveauBleue);*/
                        }





                        //Remap new values
                        decodedim.image[x, y].GetR = (byte)Convert.ToInt32(Convert.ToString(Convert.ToInt32(NouveauR, 2), 10));
                        decodedim.image[x, y].GetG = (byte)Convert.ToInt32(Convert.ToString(Convert.ToInt32(NouveauVert, 2), 10));
                        decodedim.image[x, y].GetB = (byte)Convert.ToInt32(Convert.ToString(Convert.ToInt32(NouveauBleue, 2), 10));
                        //Console.WriteLine(decodedim.myfile[x, y].toString());

                    }
                }
                // Sauvegarde l'image décodée
                decodedim.From_Image_To_File("Stegano_Decoded_SB=" + bits);
                Console.WriteLine("Done Saving Stegano_Decoded_SB=" + bits);

            }
        }



    }
}
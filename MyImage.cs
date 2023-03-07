using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Drawing.Drawing2D;

namespace ConsoleProgram
{
    class MyImage

    {
        string basicPath = "C:/Users/eliot/OneDrive/Documents/Cours ESILV/A2/S4/Algo/ConsoleProgram/bin/Debug/images/";
        private byte[] myfile;
        private string name;
        private string type;
        private int tailleFichier;
        private int offset;
        private int largeur;
        private int hauteur;
        private int bpc;
        private int hres;
        private int vres;

        private Pixel[,] image;
        public MyImage(string path)
        {

            this.myfile = File.ReadAllBytes(path);
            this.name = path.Split('.', '/')[path.Split('.', '/').Length - 2]; //récupère le nom du fichier
            this.type = Encoding.ASCII.GetString(myfile, 0, 2);
            this.tailleFichier = Convertir_Endian_To_Int(getA(myfile, 2, 5));
            this.largeur = Convertir_Endian_To_Int(getA(myfile, 18, 21));
            this.hauteur = Convertir_Endian_To_Int(getA(myfile, 22, 25));
            this.offset = Convertir_Endian_To_Int(getA(myfile, 10, 13));
            this.bpc = Convertir_Endian_To_Int(getA(myfile, 28, 29));
            this.hres = Convertir_Endian_To_Int(getA(myfile, 38, 41));
            this.vres = Convertir_Endian_To_Int(getA(myfile, 42, 45));
            this.image = MatricePixel(this.myfile);

            //"C:/Users/eliot/OneDrive/Documents/Cours ESILV/A2/S4/Algo/ProjetA2S4Informatique/bin/Debug/images/sortie2.bmp"

        }

        public void From_Image_To_File(string filename)
        {
            string path = basicPath + filename + ".bmp";
            int padding = (4 - ((this.largeur * 3) % 4)) % 4;
            byte[] bytes = new byte[this.tailleFichier + padding * this.hauteur];
            ArrayAddBytes(bytes, Encoding.ASCII.GetBytes(this.type), 0);
            ArrayAddInt(bytes, this.tailleFichier + padding * this.hauteur, 2, 4);
            ArrayAddInt(bytes, this.largeur, 18, 4);
            ArrayAddInt(bytes, this.hauteur, 22, 4);
            ArrayAddInt(bytes, this.offset, 10, 4);
            ArrayAddInt(bytes, this.bpc, 28, 2);
            ArrayAddInt(bytes, this.hres, 38, 4);
            ArrayAddInt(bytes, this.vres, 42, 4);

            ArrayAddBytes(bytes, new byte[4] { 40, 0, 0, 0 }, 14);
            ArrayAddBytes(bytes, new byte[2] { 1, 0 }, 26);
            ArrayAddInt(bytes, (this.tailleFichier - this.offset), 34, 4);
            ArrayAddInt(bytes, this.hres, 34, 4);

            ArrayAddPixels(bytes, this.image, this.offset, padding);

            File.WriteAllBytes(path, bytes);

        }




        //fonction utiles


        public Pixel[,] MatricePixel(byte[] bytes)
        {
            Pixel[,] result = new Pixel[this.hauteur, this.largeur];
            int padding = (4 - ((this.largeur * 3) % 4)) % 4;
            int index = this.offset;
            for (int i = result.GetLength(0) - 1; i >= 0; i--)
            {
                for (int j = 0; j < result.GetLength(1); j++)
                {
                    result[i, j] = new Pixel(bytes[index], bytes[index + 1], bytes[index + 2]);
                    index += 3;
                }
                if (padding != 0)
                {
                    for (int k = 0; k < padding; k++)
                    {
                        index += 1;
                    }
                }
            }
            return result;
        }

        public void ImageToString()
        {
            foreach (Pixel a in this.image)
            {
                Console.WriteLine(a.toString());
            }
        }
        public byte[] Getmyfile
        {
            get { return this.myfile; }
            set { this.myfile = value; }
        }
        public Pixel[,] GetImage
        {
            get { return this.image; }
            set { this.image = value; }
        }
        public static int Convertir_Endian_To_Int(byte[] array)
        {
            int result = 0;

            for (int i = array.Length - 1; i >= 0; i--)
            {

                result |= array[i] << i * 8;
            }
            return result;
        }
        static public byte[] Convertir_Int_To_Endian(int number, int arraylength)
        {
            byte[] result = new byte[arraylength];
            for (int i = 0; i < arraylength; i++)
            {
                result[i] = (byte)(((uint)number >> i * 8) & 0xFF);   //Shift right i*8 bits and add a 0 if necessary
            }
            return result;
        }
        public static byte[] getA(byte[] bytes, int start, int end)
        {
            byte[] b = new byte[(end - start + 1)];
            for (int i = start; i <= end; i++)
            {
                b[i - start] = bytes[i];
                //Console.WriteLine("i=" + i + " and bytes[i]=" + bytes[i]);
            }
            return b;
        }
        public static void ArrayAddBytes(byte[] array, byte[] arraytoadd, int offset)
        {
            for (int i = 0; i < arraytoadd.Length; i++)
            {
                array[i + offset] = arraytoadd[i];
            }
        }
        public static void ArrayAddInt(byte[] array, int number, int offset, int length)
        {
            byte[] numberasendian = Convertir_Int_To_Endian(number, length);
            for (int i = 0; i < length; i++)
            {
                array[i + offset] = numberasendian[i];
            }
        }

        public static void ArrayAddPixels(byte[] array, Pixel[,] mat, int offs, int padding)
        {
            int count = 0;
            for (int i = mat.GetLength(0) - 1; i >= 0; i--)
            {
                for (int j = 0; j < mat.GetLength(1); j++)
                {
                    ArrayAddBytes(array, mat[i, j].toByteArray(), (offs + count));

                    count += 3;

                }
                if (padding != 0)
                {
                    for (int k = 0; k < padding; k++)
                    {

                        ArrayAddBytes(array, new byte[] { 0 }, (offs + count));
                        count += 1;
                    }
                }


            }
        }


        //Effect image
        //image en gris
        /*
        public void ImageEnGris()
        {

            MyImage imGris = new MyImage(basicPath+this.name+".bmp");
            for (int i = 0; i < imGris.image.GetLength(0); i++)
            {
                for (int j = 0; j < imGris.image.GetLength(1); j++)
                {
                    //To have different level of grey we do the average on each pixel
                    int grey = imGris.image[i, j].GetR / 3 + imGris.image[i, j].GetG / 3 + imGris.image[i, j].GetB / 3;
                    imGris.image[i, j].GetR =(byte) grey;
                    imGris.image[i, j].GetG = (byte)grey;
                    imGris.image[i, j].GetB = (byte)grey;
                }
            }
            imGris.From_Image_To_File(this.name + "_ImageEnGris");
        }
        */


        public void Agrandissement(double zoom)
        {

            MyImage imAgrandissement = new MyImage(basicPath + this.name + ".bmp");
            if (zoom >= 1)
            {
                Console.WriteLine("extend");
                int zoomE = Convert.ToInt32(zoom);

                imAgrandissement.tailleFichier = 54 + ((int)((double)hauteur * zoomE) * (int)((double)largeur * zoomE) * 3);

                imAgrandissement.largeur = largeur * zoomE;
                imAgrandissement.hauteur = hauteur * zoomE;
                Pixel[,] Extend = new Pixel[image.GetLength(0) * zoomE, image.GetLength(1) * zoomE];

                int line = 0;
                int column = 0;
                for (int i = 0; i < image.GetLength(0); i++)
                {
                    column = 0;
                    for (int j = 0; j < image.GetLength(1); j++)
                    {
                        for (int index1 = line; index1 < line + zoomE; index1++)
                        {
                            for (int index2 = column; index2 < column + zoomE; index2++)
                            {
                                Extend[index1, index2] = image[i, j];
                            }
                        }

                        column += zoomE;
                    }
                    line += zoomE;
                }
                Console.WriteLine("begin save");

                imAgrandissement.image = Extend;
                for (int i = 0; i < Extend.GetLength(0); i++)
                {
                    for (int j = 0; j < Extend.GetLength(1); j++)
                    {
                        imAgrandissement.image[i, j] = Extend[i, j];
                    }
                }

                imAgrandissement.From_Image_To_File(this.name + "_Resize_" + zoom);
                Console.WriteLine("Done");
            }

            if (zoom > 0 && zoom < 1)
            {
                zoom = 1 - zoom;
                int zoomR = Convert.ToInt32(zoom * 10);

                imAgrandissement.tailleFichier = 54 + (hauteur / zoomR * largeur / zoomR * 3);
                imAgrandissement.largeur = largeur / zoomR;
                imAgrandissement.hauteur = hauteur / zoomR;
                Pixel[,] Reduce = new Pixel[(image.GetLength(0) / zoomR), (image.GetLength(1) / zoomR)];

                int line = 0;
                int column = 0;
                for (int i = 0; i < Reduce.GetLength(0); i++)
                {
                    column = 0;
                    for (int j = 0; j < Reduce.GetLength(1); j++)
                    {
                        int Moyenne_Rouge = 0;
                        int Moyenne_Vert = 0;
                        int Moyenne_Bleu = 0;

                        for (int index1 = line; index1 < line + zoomR; index1++)
                        {
                            for (int index2 = column; index2 < column + zoomR; index2++)
                            {

                                Moyenne_Rouge += image[index1, index2].GetB;
                                Moyenne_Vert += image[index1, index2].GetG;
                                Moyenne_Bleu += image[index1, index2].GetR;
                            }
                        }
                        Moyenne_Rouge = (byte)(Moyenne_Rouge / (zoomR * zoomR));
                        Moyenne_Vert = (byte)(Moyenne_Vert / (zoomR * zoomR));
                        Moyenne_Bleu = (byte)(Moyenne_Bleu / (zoomR * zoomR));
                        Reduce[i, j] = new Pixel((byte)Moyenne_Bleu, (byte)Moyenne_Vert, (byte)Moyenne_Rouge);
                        column += zoomR;
                    }
                    line += zoomR;
                }
                imAgrandissement.image = Reduce;
                for (int i = 0; i < Reduce.GetLength(0); i++)
                {
                    for (int j = 0; j < Reduce.GetLength(1); j++)
                    {
                        imAgrandissement.image[i, j] = Reduce[i, j];
                    }
                }

                imAgrandissement.From_Image_To_File(this.name + "_Resize_" + (1 - zoom));
            }
        }
    }
}

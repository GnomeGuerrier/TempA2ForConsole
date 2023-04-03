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
        string basicPath = "./images/";
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
        public byte[] GetFile
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
                result[i] = (byte)(((uint)number >> i * 8) & 0xFF);   
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

        #region Agrandissement
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
        #endregion
        public Pixel ReturnPixel(int x, int y)
        {
            Pixel black = new Pixel(0,0,0);
            if (this.hauteur<=x || this.largeur<=y)return black;
            else{
                Pixel ret = image[x,y].GetPixels();
                return ret;
            }
            
            
            
        }
        public void PlacePixel(int x, int y, Pixel pixel)
        {
            this.image[x,y] = pixel;
        }

        public void Rotation(double angles)
        {
            int angle = Convert.ToInt32(angles);
            Console.WriteLine("Begin Rotation");
            double radians = angle * Math.PI / 180.0;
            double sin = Math.Sin(radians);
            double cos = Math.Cos(radians);
            MyImage result = new MyImage(basicPath + this.name + ".bmp");

            int newL=(int) (this.largeur*Math.Abs(cos)+this.hauteur*Math.Abs(sin));
            int newH = (int)(this.hauteur * Math.Abs(cos) + this.largeur * Math.Abs(sin));

            result.tailleFichier = (54 + newH * newL * 3);
            result.hauteur = newH;
            result.largeur = newL;
            int centerX = this.largeur / 2;
            int centerY = this.hauteur / 2;
            Console.WriteLine("new hauteur" + newH);
            Console.WriteLine("new largeur" + newL);
            Console.WriteLine("hauteur classique" + this.hauteur);
            Console.WriteLine("largeur classique" + this.largeur);
            for (int y = 0; y < this.hauteur; y++)
            {
                for (int x = 0; x < this.largeur; x++)
                {
                    
                    int transX = x - centerX;
                    int transY = y - centerY;

                    double rotatedX = (transX * cos - transY * sin);
                    double rotatedY = (transX * sin + transY * cos);

                    int finalX = (int)Math.Abs(rotatedX + centerX);
                    int finalY = (int)Math.Abs(rotatedY + centerY);

                    
                    if (finalX >= 0 && finalX < newL && finalY >= 0 && finalY < newH)
                    {
                        Console.WriteLine("-----");
                        Console.WriteLine("x=" + x);
                        Console.WriteLine("y=" + y);
                        Console.WriteLine("finalx" + finalX);
                        Console.WriteLine("finaly" + finalY);
                        
                        
                        Pixel originalPixel = this.ReturnPixel(x, y);
                        result.PlacePixel(finalX, finalY, originalPixel);
                    }
                    
                }
            }
            Console.WriteLine("Begin Save");
            result.From_Image_To_File(this.name + "_Rotation_" + angle);
            Console.WriteLine("done");
        }

        public void Emboss(int[,] embossKernel)
        {
           System.Console.WriteLine("Start Emboss");
            Pixel[,] inputImage = this.image;
            int width = inputImage.GetLength(0);
            int height = inputImage.GetLength(1);
            Pixel[,] outputImage = new Pixel[width, height];

            

            for (int x = 1; x < width - 1; x++)
            {
                for (int y = 1; y < height - 1; y++)
                {
                    int rSum = 0, gSum = 0, bSum = 0;

                    for (int i = -1; i <= 1; i++)
                    {
                        for (int j = -1; j <= 1; j++)
                        {
                            int offsetX = x + i;
                            int offsetY = y + j;
                            int kernelX = i + 1;
                            int kernelY = j + 1;

                            rSum += inputImage[offsetX, offsetY].GetR * embossKernel[kernelX, kernelY];
                            gSum += inputImage[offsetX, offsetY].GetG * embossKernel[kernelX, kernelY];
                            bSum += inputImage[offsetX, offsetY].GetB * embossKernel[kernelX, kernelY];
                        }
                    }

                    int r = rSum + 128;
                    int g = gSum + 128;
                    int b = bSum + 128;
                    r = Math.Min(Math.Max(r, 0), 255);
                    g = Math.Min(Math.Max(g, 0), 255);
                    b = Math.Min(Math.Max(b, 0), 255);

                    //Console.WriteLine(new Pixel((byte)r, (byte)g, (byte)b).toString());

                    this.image[x, y] = new Pixel((byte)r, (byte)g, (byte)b);
                    //Console.WriteLine(outputImage[x,y].toString());
                }
            }
            
            //this.image = outputImage;
            /*for(int x = 0; x < outputImage.GetLength(1); x++)
            {
                for(int y = 0; y < outputImage.GetLength(1); y++)
                {
                    this.image[x,y] = outputImage[x, y];
                }
            }*/
            //
            System.Console.WriteLine("start Save");
            this.From_Image_To_File(this.name + "_embossGPT");
            System.Console.WriteLine("Done Save");
        }
        public void Flou()
        {
            Console.WriteLine("begin");
            Pixel[,] inputImage = this.image;
            int width = inputImage.GetLength(0);
            int height = inputImage.GetLength(1);
            Pixel[,] outputImage = new Pixel[width, height];

            double[,] embossKernel = new double[,] { { 1/9, 1/9, 1/9 }, { 1 / 9, 1 / 9, 1 / 9 }, { 1 / 9, 1 / 9, 1 / 9 } };

            for (int x = 1; x < width - 1; x++)
            {
                for (int y = 1; y < height - 1; y++)
                {
                    int rSum = 0, gSum = 0, bSum = 0;

                    for (int i = -1; i <= 1; i++)
                    {
                        for (int j = -1; j <= 1; j++)
                        {
                            int offsetX = x + i;
                            int offsetY = y + j;
                            int kernelX = i + 1;
                            int kernelY = j + 1;

                            rSum += inputImage[offsetX, offsetY].GetR *(int) embossKernel[kernelX, kernelY];
                            gSum += inputImage[offsetX, offsetY].GetG * (int)embossKernel[kernelX, kernelY];
                            bSum += inputImage[offsetX, offsetY].GetB * (int)embossKernel[kernelX, kernelY];
                        }
                    }

                    
                    //Console.WriteLine(new Pixel((byte)r, (byte)g, (byte)b).toString());

                    this.image[x, y] = new Pixel((byte)rSum, (byte)gSum, (byte)bSum);
                    //Console.WriteLine(outputImage[x,y].toString());
                }
            }

            //this.image = outputImage;
            /*for(int x = 0; x < outputImage.GetLength(1); x++)
            {
                for(int y = 0; y < outputImage.GetLength(1); y++)
                {
                    this.image[x,y] = outputImage[x, y];
                }
            }*/
            //
            Console.WriteLine("begin save");
            this.From_Image_To_File(this.name + "_Flou");
            Console.WriteLine("begin done");
        }
    }
}

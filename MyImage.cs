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
        public string Path = "";
        public byte[] myfile;
        public string name;
        public string type;
        public int tailleFichier;
        public int offset;
        public int largeur;
        public int hauteur;
        public int bpc;
        public int hres;
        public int vres;

        public Pixel[,] image;
        public MyImage(string path)
        {
            this.Path = path;
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
        public MyImage(){}
        /// <summary>
        /// Permet d'enregistrer une image, faite de la classe 
        /// </summary>
        /// <param name="filename">nom qui va être rajouter </param>
        public void From_Image_To_File(string filename)
        {

            string path = this.Path.Substring(0, this.Path.Length - 4)+"_"+filename+".bmp";
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

        public void HuffmanString(){
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
            
        }


        //fonction utiles

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes">La matrice de byte à transformer</param>
        /// <returns></returns>
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
        
        public void ImageEnGris(string FilePath)
        {

            MyImage imGris = new MyImage(FilePath);
            for (int i = 0; i < imGris.image.GetLength(0); i++)
            {
                for (int j = 0; j < imGris.image.GetLength(1); j++)
                {
                    //On fait la moyenne sur chaque pixel
                    int grey = imGris.image[i, j].GetR / 3 + imGris.image[i, j].GetG / 3 + imGris.image[i, j].GetB / 3;
                    imGris.image[i, j].GetR =(byte) grey;
                    imGris.image[i, j].GetG = (byte)grey;
                    imGris.image[i, j].GetB = (byte)grey;
                }
            }
            imGris.From_Image_To_File("_ImageEnGris");
        }
        

        #region Agrandissement
        public void Agrandissement(double zoom)
        {

            MyImage imAgrandissement = new MyImage(this.Path);
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


       
         public void MandelBrot(string FilePath)
        {
            
            //Creation of the image
            MyImage Fractal = new MyImage();
            Fractal.name = "Mandelbrot";
            Fractal.type = "BM";
            Fractal.largeur = 1000;
            Fractal.hauteur = 1000;
            Fractal.offset = 54;
            Fractal.tailleFichier = Fractal.largeur * Fractal.hauteur * 3 + Fractal.offset;
            Fractal.bpc = 24;
            Fractal.hres = 0;
            Fractal.vres = 0;
            Fractal.image = new Pixel[Fractal.hauteur, Fractal.largeur];
            Fractal.Path = FilePath;
            int[][] Couleurs = new int[3][] { new int[] { 0, 0, 0 }, new int[] { 232, 26, 46 }, new int[] { 255, 255, 255 } };
            //The Mandelbrot set is the set of complex numbers c
            Complexe c = new Complexe();
            int count = 0;
            for (int i = 0; i < Fractal.largeur; i++) // real part of the complex number is represented by a displacement along the x-axis
            {
                c.Re = (double)(i - Fractal.largeur / 2) / (0.25 * Fractal.largeur);
                for (int j = 0; j < Fractal.hauteur; j++) //imaginary part of the complex number is represented by a displacement along the y-axis
                {
                    Complexe z = new Complexe(0, 0);
                    c.Im = (double)(j - Fractal.hauteur / 2) / (0.25 * Fractal.hauteur);
                    //z(n+1)=Zn²+c
                    do
                    {
                        z = Complexe.ComplexMult(z, z);
                        z = Complexe.ComplexAdd(z, c);
                        count++;
                        if (z.Module() > 2) break; //The sequence zn is not bounded if the modulus of one of its terms is greater than 2
                    }
                    while (count < 255);
                    //The number of iterations to reach a modulus greater than 2 is used to determine the color to use

                    int[] colour = new int[] { 0, 0, 0 };
                    if (count != 254) { colour = new int[] { count * 255 / 50, 0, 0 }; }
                    Fractal.image[j, i] = new Pixel(colour);
                    count = 0;
                }
            }
            Fractal.From_Image_To_File("MandelBrot");
        }

    }
}

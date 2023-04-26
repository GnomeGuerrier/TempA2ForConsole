using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleProgram
{
    /// <summary>
    /// Class convolution, qui hérite de MyImage
    /// </summary>
    class Convolution : MyImage
    {


        
        int[,] filtre;
        int facteur; 
        int bias; 



        
        public Convolution(MyImage im, string filtre) 
        {
            this.filtre = GetFilter(filtre);  
            this.facteur = GetFactor(filtre); 
            this.hauteur = im.hauteur;
            this.largeur = im.largeur;
            this.offset = im.offset;
            this.tailleFichier = im.tailleFichier;
            this.type = im.type;
            this.bpc = im.bpc;
            this.image = new Pixel[im.image.GetLength(0), im.image.GetLength(1)];
            this.ConstructMat(im);
        }

       

        public void ConstructMat(MyImage im)
        {

            for (int x = 0; x < im.image.GetLength(0); x++)
            {
                for (int y = 0; y < im.image.GetLength(1); y++)
                {
                    double red = 0;
                    double blue = 0;
                    double green = 0;

                    for (int filtrex = 0; filtrex < filtre.GetLength(0); filtrex++) 
                    {
                        for (int filtrey = 0; filtrey < filtre.GetLength(1); filtrey++)
                        {
                            int i = (x - filtre.GetLength(0) / 2 + filtrex + im.hauteur) % im.hauteur;  
                            int j = (y - filtre.GetLength(1) / 2 + filtrey + im.largeur) % im.largeur;
                            double filtrevalue = filtre[filtrex, filtrey];
                            red += im.image[i, j].GetR * filtrevalue / this.facteur;
                            green += im.image[i, j].GetG * filtrevalue / this.facteur;
                            blue += im.image[i, j].GetB * filtrevalue / this.facteur;

                        }
                    }

                    this.image[x, y] = new Pixel();
                    this.image[x, y].GetR = (byte)Math.Min(Math.Max(Convert.ToInt32(red + bias), 0), 255);
                    this.image[x, y].GetG = (byte)Math.Min(Math.Max(Convert.ToInt32(green + bias), 0), 255);
                    this.image[x, y].GetB = (byte)Math.Min(Math.Max(Convert.ToInt32(blue + bias), 0), 255);
                }
            }
        }



        /// <summary>
        /// Listes de tout les filtres
        /// </summary>
        /// <param name="filtrename">nom des filtres</param>
        static int[,] GetFilter(string filtrename)
        {
            switch (filtrename)
            {
                case "Edge_detect":
                    return new int[,] { { 0, 1, 0 }, { 1, -4, 1 }, { 0, 1, 0 } };
                case "Edge_enhance":
                    return new int[,] { { 0, 0, 0 }, { -1, 1, 0 }, { 0, 0, 0 } };
                case "Emboss":
                    return new int[,] { { -2, -1, 0 }, { -1, 1, 1 }, { 0, 1, 2 } };
                case "BoxBlur":
                    return new int[,] { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 } };
                case "GaussianBlur":
                    return new int[,] { { 1, 2, 1 }, { 2, 4, 2 }, { 1, 2, 1 } };
                case "Sharpen":
                    return new int[,] { { 0, -1, 0 }, { -1, 5, -1 }, { 0, -1, 0 } };
            }
            return new int[,] { { 0, 0, 0 }, { 0, 1, 0 }, { 0, 0, 0 } };


        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="filtrename">string of the current filer</param>
        /// <returns>the facteur as an int</returns>
        static int GetFactor(string filtrename)
        {
            switch (filtrename)
            {
                case "BoxBlur":
                    return 9;
                case "GaussianBlur":
                    return 16;
                default:
                    return 1;
            }
        }



   
       
        
    }
}
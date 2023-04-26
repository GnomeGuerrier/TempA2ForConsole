using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleProgram
{
    /// <summary>
    /// Convolution class, daughter of the MyImage class, used to add convolution filters to an MyImage
    /// </summary>
    class Convolution : MyImage
    {

        //==================================================================================================================================================================================================================================================
        // CONSTRUCTOR
        //==================================================================================================================================================================================================================================================

        #region Constructor

        /// <summary>
        /// Characteristics of a convolution filter
        /// </summary>
        int[,] filter; //filter matrix
        int factor;  //Used to multiply the result
        int bias; //Used to add brightness, to be added after the factor multiplication



        /// <summary>
        /// simple constructor
        /// </summary>
        /// <param name="im">MyImage to apply the convolution filter to</param>
        /// <param name="filter">the filter that need to be applied, as a string</param>
        public Convolution(MyImage im, string filter)  //Add type of convolution, and search in the according db? db inside this function?
        {
            this.filter = GetFilter(filter);  //get the matrix filter
            this.factor = GetFactor(filter);  //get the multiplicative factor
               //Get the bias, not use for the moment but often use in some filters, to modify overall visibility by lighting up or darkening the MyImage
            this.hauteur = im.hauteur;
            this.largeur = im.largeur;
            this.offset = im.offset;
            this.tailleFichier = im.tailleFichier;
            this.type = im.type;
            this.bpc = im.bpc;
            this.image = new Pixel[im.image.GetLength(0), im.image.GetLength(1)];
            this.ConstructMat(im);
        }

        #endregion

        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        #region Constructor helper

        /// <summary>
        /// Construct the MyImage pixel matrix with the addition of the convolution filter
        /// </summary>
        /// <param name="im">the MyImage on which we apply the filter</param>
        public void ConstructMat(MyImage im)
        {

            for (int x = 0; x < im.image.GetLength(0); x++)
            {
                for (int y = 0; y < im.image.GetLength(1); y++)
                {
                    double red = 0;
                    double blue = 0;
                    double green = 0;

                    for (int filterx = 0; filterx < filter.GetLength(0); filterx++)  //If bug, change filterx/y and getlengths
                    {
                        for (int filtery = 0; filtery < filter.GetLength(1); filtery++)
                        {
                            int i = (x - filter.GetLength(0) / 2 + filterx + im.hauteur) % im.hauteur;   //Minus the half length of the filter matrix
                            int j = (y - filter.GetLength(1) / 2 + filtery + im.largeur) % im.largeur;
                            double filtervalue = filter[filterx, filtery];
                            //Console.WriteLine("im.largeur=" + im.largeur + "/" + im.Matrice.GetLength(1) + " im.Heigh" + im.hauteur + "/" + im.Matrice.GetLength(0));
                            //Console.WriteLine("i=" + i + " j=" + j);
                            red += im.image[i, j].GetR * filtervalue / this.factor;
                            green += im.image[i, j].GetG * filtervalue / this.factor;
                            blue += im.image[i, j].GetB * filtervalue / this.factor;

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
        /// databases of all filters
        /// </summary>
        /// <param name="filtername">name of the filter</param>
        /// <returns>returns the filter string into its corresponding matrix</returns>
        static int[,] GetFilter(string filtername)
        {
            switch (filtername)
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
        /// Returns the factor (if any) required by the matrix
        /// </summary>
        /// <param name="filtername">string of the current filer</param>
        /// <returns>the factor as an int</returns>
        static int GetFactor(string filtername)
        {
            switch (filtername)
            {
                case "BoxBlur":
                    return 9;
                case "GaussianBlur":
                    return 16;
                default:
                    return 1;
            }
        }



        /// <summary>
        /// Return the bias based on the filter, not used for the filters we have in our database in the moment
        /// </summary>
        /// <param name="filtername">string of the current filer</param>
        /// <returns>the bias number as an int</returns>
       
        #endregion
    }
}
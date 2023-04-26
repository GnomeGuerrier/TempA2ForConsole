using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 
/// 
/// Change to be a simple method and not class
/// 
/// </summary>

namespace ConsoleProgram
{
    class Rotate : MyImage
    {

        #region Constructor

        public Rotate() { }
        /// <summary>
        /// Constructeur pour la classe rotation
        /// </summary>
        /// <param name="angle">L'angle de rotation</param>
        /// <param name="original">L'image de base</param>
        public Rotate(int angle, MyImage original)
        {

            this.offset = original.offset;
            this.type = original.type;
            this.bpc = original.bpc;
            this.name ="_Rotate";
            this.GetDimensions(angle, original);
            this.image = new Pixel[this.hauteur, this.largeur];
            this.GetMatrice(angle, original);
            this.Path = original.Path;

        
        }

  
        /// <summary>
        /// crée la nouvelle matrice de pixel pour la rotation
        /// </summary>
        /// <param name="angle">L'angle de rotation</param>
        /// <param name="original">l'image de base</param>
        void GetMatrice(int angle, MyImage original)
        {
            float rad = (2 * (float)Math.PI * angle) / 360;  

            float cos = (float)Math.Cos(rad);
            float sin = (float)Math.Sin(rad);
           


            for (int x = 0; x < this.largeur; x++)   
            {
                for (int y = 0; y < this.hauteur; y++) 
                {
                    
                    int deltax = x - this.largeur / 2;
                    int deltay = y - this.hauteur / 2;
                 
                    int xoriginal = (int)(cos * deltax - sin * deltay) + original.largeur / 2;
                    int yoriginal = (int)(sin * deltax + cos * deltay) + original.hauteur / 2;


                    if (xoriginal >= 0 && xoriginal < original.image.GetLength(1) && yoriginal >= 0 && yoriginal < original.image.GetLength(0))
                    {
                        this.image[y, x] = original.image[yoriginal, xoriginal];
                    }
                    else
                    {
                        this.image[y, x] = new Pixel(0, 0, 0);
                    }
                }
            }
        }
        /// <summary>
        /// Permet d'avoir les nouvelles dimensiosn de la matrice (post rotation)
        /// </summary>
        /// <param name="angle"></param>
        /// <param name="original"></param>
        void GetDimensions(int angle, MyImage original)
        {

            double rad = (2 * Math.PI * angle) / 360;   
            double cos = Math.Cos(rad);
            double sin = Math.Sin(rad);

             if (angle <= 180)
            {
                if (angle <= 90) //Angle is [0,90]
                {
                    
                    this.hauteur = 1 + (int)(2 * (cos * original.hauteur / 2 - sin * -original.largeur / 2)); 
                    this.largeur = 1 + (int)(2 * (cos * original.largeur / 2 + sin * original.hauteur / 2));   
                    
                }
                else // ]90,180]
                {
                    this.hauteur = 1 + (int)(2 * (cos * -original.hauteur / 2 + (-sin * -original.largeur / 2)));   
                    this.largeur = 1 + (int)(2 * (cos * -original.largeur / 2 + (sin * original.hauteur / 2)));   
                }
            }
            else
            {
                if (angle <= 270) //]180;270]
                {
                    this.hauteur = 1 + (int)(2 * (cos * (-original.hauteur / 2 - 1) + (-sin * (original.largeur / 2 - 1))));  
                    this.largeur = 1 + (int)(2 * (cos * (-original.largeur / 2 - 1) + (sin * (-original.hauteur / 2 - 1)))); 
                }
                else //]270;360]
                {
                    this.hauteur = 1 + (int)(2 * (cos * (original.hauteur / 2 - 1) + (-sin * (original.largeur / 2 - 1)))); 
                    this.largeur = 1 + (int)(2 * (cos * (original.largeur / 2 - 1) + (sin * (-original.hauteur / 2 - 1)))); 
                }
            }

            int padding = 4 - (this.largeur % 4);
            this.tailleFichier = 3 * (this.largeur + padding) * this.hauteur;

            //Console.WriteLine("new largeur and hauteur= " + this.largeur + "x" + this.hauteur);
        }

        #endregion

    }
}
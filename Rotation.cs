using System;
using System.Collections.Generic;
using System.Linq;
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

        //==================================================================================================================================================================================================================================================
        // CONSTRUCTOR
        //==================================================================================================================================================================================================================================================

        #region Constructor

        public Rotate() { }
        public Rotate(int angle, MyImage original)
        {

            this.offset = original.offset;
            this.type = original.type;
            this.bpc = original.bpc;
            this.name = original.name + "_Rotate";
            this.GetDimensions(angle, original);
            this.image = new Pixel[this.hauteur, this.largeur];
            this.GetMatrice(angle, original);

        
        }

        #endregion

        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        #region Constructor helper

        void GetMatrice(int angle, MyImage original)
        {
            float rad = (2 * (float)Math.PI * angle) / 360;   //Convert in rad because cos and sin dont work with degrees

            float cos = (float)Math.Cos(rad);
            float sin = (float)Math.Sin(rad);
            //Console.WriteLine("cos and sin=" + cos + " " + sin);


            for (int x = 0; x < this.largeur; x++)   //getlength1
            {
                for (int y = 0; y < this.hauteur; y++) //
                {
                    //Translations
                    int deltax = x - this.largeur / 2;
                    int deltay = y - this.hauteur / 2;
                    //Compute Sigma^-1*(x',y') to get (x,y)
                    int xoriginal = (int)(cos * deltax - sin * deltay) + original.largeur / 2;
                    int yoriginal = (int)(sin * deltax + cos * deltay) + original.hauteur / 2;


                    //Console.WriteLine("x'=" + x + " y'=" + y + " [x,y]=["+xoriginal+","+yoriginal+"]");
                    //Console.WriteLine("new bounds: " + this.Matrice.GetLength(0)+this.Matrice.GetLength(1) + " old bounds: " + original.Matrice.GetLength(0)+ original.Matrice.GetLength(1));
                    if (xoriginal >= 0 && xoriginal < original.image.GetLength(1) && yoriginal >= 0 && yoriginal < original.image.GetLength(0))
                    {
                        //Console.WriteLine(this.toString());
                        //Console.WriteLine("Trying to append "+ original.Matrice[yoriginal, xoriginal].toString()+"at y,x");
                        //Console.WriteLine("New Matrice is " + this.MatricetoString() + "\nOld is " + original.MatricetoString());

                        this.image[y, x] = original.image[yoriginal, xoriginal];
                    }
                    else
                    {
                        this.image[y, x] = new Pixel(0, 0, 0);
                    }
                }
            }
        }

        void GetDimensions(int angle, MyImage original)
        {

            double rad = (2 * Math.PI * angle) / 360;   //Convert in rad because cos and sin dont work with degrees
            double cos = Math.Cos(rad);
            double sin = Math.Sin(rad);

            //if (angle > 180) { angle -= 360; }
            //Each four quarter have different magic points, each have 2, one which x' coordinate is half of the new largeur, one which y' coordinate is half of the new Heigth
            //We just have to find these 2 magic points, each are 2 corners, in the 4 cases, compute their x' and y', and replace the largeur and Heigth with those
            if (angle <= 180)
            {
                if (angle <= 90) //Angle is [0,90]
                {
                    //For example:
                    this.hauteur = 1 + (int)(2 * (cos * original.hauteur / 2 - sin * -original.largeur / 2));  //Bottom left corner's new y
                    this.largeur = 1 + (int)(2 * (cos * original.largeur / 2 + sin * original.hauteur / 2));    //Bottom right corner's new x
                    //With new x and y computed with the sigma*x=x' with sigma the rotation matrix
                }
                else // ]90,180]
                {
                    this.hauteur = 1 + (int)(2 * (cos * -original.hauteur / 2 + (-sin * -original.largeur / 2)));   //Top left corner's new y
                    this.largeur = 1 + (int)(2 * (cos * -original.largeur / 2 + (sin * original.hauteur / 2)));    //Bottom left corner's new x
                }
            }
            else
            {
                if (angle <= 270) //]180;270]
                {
                    this.hauteur = 1 + (int)(2 * (cos * (-original.hauteur / 2 - 1) + (-sin * (original.largeur / 2 - 1))));  //Top right corner's new y
                    this.largeur = 1 + (int)(2 * (cos * (-original.largeur / 2 - 1) + (sin * (-original.hauteur / 2 - 1))));  //Top left corner's new x
                }
                else //]270;360]
                {
                    this.hauteur = 1 + (int)(2 * (cos * (original.hauteur / 2 - 1) + (-sin * (original.largeur / 2 - 1))));  //Bottom right corner's new y
                    this.largeur = 1 + (int)(2 * (cos * (original.largeur / 2 - 1) + (sin * (-original.hauteur / 2 - 1)))); //Top tight corner's new x
                }
            }

            int padding = 4 - (this.largeur % 4);
            this.tailleFichier = 3 * (this.largeur + padding) * this.hauteur;

            //Console.WriteLine("new largeur and hauteur= " + this.largeur + "x" + this.hauteur);
        }

        #endregion

    }
}
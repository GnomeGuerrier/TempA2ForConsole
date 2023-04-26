using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleProgram
{
    /// <summary>
    /// Class qui permet de faire des opérations sur les complexes
    /// </summary>
    class Complexe
    {
        
      

        //Parties imaginaires et reélles
        private double re;
        private double im;



      
        public Complexe() { }



        /// <summary>
        /// Constructor simple
        /// </summary>
        /// <param name="reel">real part of the number</param>
        /// <param name="immaginaire">imaginary part of the number</param>
        public Complexe(double reel, double immaginaire)
        {
            this.re = reel;
            this.im = immaginaire;
        }



        /// <summary>
        /// Permet de copier un complexe
        /// </summary>
        /// <param name="x">nombre complexe a copier</param>
        public Complexe(Complexe x)
        {
            this.re = x.re;
            this.im = x.im;
        }

       
        public double Re
        {
            get { return this.re; }
            set { this.re = value; }
        }
        public double Im
        {
            get { return this.im; }
            set
            {
                this.im = value;
            }
        }
       

       

        /// <summary>
        /// Multiplie deux complexes entre eux
        /// </summary>
        /// <param name="x1">le premier complexe</param>
        /// <param name="x2">le deuxieme complex</param>
        public static Complexe ComplexMult(Complexe x1, Complexe x2)
        {
            return new Complexe(x1.Re * x2.Re - x1.Im * x2.Im, x1.Re * x2.Im + x2.Re * x1.Im);
        }



        /// <summary>
        /// Additionne 2 complexe
        /// </summary>
        /// <param name="x1">le premier cmplexe</param>
        /// <param name="x2">le deuxieme complexe</param>
        public static Complexe ComplexAdd(Complexe x1, Complexe x2)
        {
            return new Complexe(x1.Re + x2.Re, x1.Im + x2.Im);
        }



        /// <summary>
        /// Calcule le module d'un complexe
        /// </summary>
        /// <returns>the module of the complex number</returns>
        public double Module()
        {
            return Math.Sqrt(this.Re * this.Re + this.Im * this.Im);
        }



        /// <summary>
        /// Affiche un complexe
        /// </summary>
        public string toString()
        {
            return "(" + this.Re.ToString() + "," + this.Im.ToString() + "i)";
        }

       

    }
}
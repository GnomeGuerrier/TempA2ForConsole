using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.ComponentModel;
using System.Media;
using System.Diagnostics;



//test

namespace Programme1
{
    class Program
    {
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Valider la compatibilité de la plateforme", Justification = "<En attente>")]
        static void Main(string[] args)
        {
            string basicPath = "C:/Users/eliot/OneDrive/Documents/Cours ESILV/A2/S4/Algo/ConsoleProgram/bin/Debug/images/";
            MyImage image = new MyImage(basicPath + "lac.bmp");
            //image.From_Image_To_File("cocosortie");
            // image.ImageEnGris();
            image.Agrandissement(20);
            Console.ReadLine();




        }

    }
}

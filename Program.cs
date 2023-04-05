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





namespace ConsoleProgram
{
    class Program
    {
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Valider la compatibilité de la plateforme", Justification = "<En attente>")]
        static void Main(string[] args)
        {
            string basicPath = "./images/";
            MyImage imageBase = new MyImage(basicPath + "TestHugo1.bmp");
            MyImage imageCode = new MyImage(basicPath + "TestHugo2.bmp");
            Steganography stega = new Steganography(image);
            stega.Encode(imageCode);
            //image.From_Image_To_File("cocosortie");
            // image.ImageEnGris();
            //image.Agrandissement(20);
            //image.Rotation(45);
            //image.Emboss();
            // image.Flou();
            //int[,] embossKernel = new int[,] { { -1, -1, 0 }, { -1, 0, 1 }, { 0, 1, 1 } };
           // image.Emboss(embossKernel);
            Console.ReadLine();




        }

    }
}

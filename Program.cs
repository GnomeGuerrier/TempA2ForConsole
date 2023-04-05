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
            string basicPath = "./bin/Debug/net7.0/images/";
            MyImage image = new MyImage(basicPath + "lena.bmp");
            MyImage fractal = new MyImage();
            //image.From_Image_To_File("cocosortie");
            // image.ImageEnGris();
            //image.Agrandissement(20);
            //image.Rotation(90);
            //image.Emboss();
            // image.Flou();
            //double[,] embossKernel = new double[,] { { 1 / 9, 1 / 9, 1 / 9 }, { 1 / 9, 1 / 9, 1 / 9 }, { 1 / 9, 1 / 9, 1 / 9 } };
            //image.Embossing(embossKernel);
           // fractal.MandelBrot();
            //MyImage imageBase = new MyImage(basicPath + "TestHugo1.bmp");
            //MyImage imageCode = new MyImage(basicPath + "TestHugo2.bmp");
            //Steganography stega = new Steganography(imageBase);
            //stega.Encode(imageCode,4);
            //MyImage decode = new MyImage(basicPath+ "TestHugoFinal.bmp");
            //Steganography ddec= new Steganography(decode);
            //ddec.Decode();
            Rotate rotate =new Rotate(45,image);
            rotate.From_Image_To_File()
            Console.ReadLine();

          

        
        }

    }
}

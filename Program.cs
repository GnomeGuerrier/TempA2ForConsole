﻿using System;
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
            MyImage image = new MyImage(basicPath + "coco.bmp");
            //image.From_Image_To_File("cocosortie");
            // image.ImageEnGris();
            //image.Agrandissement(20);
            //image.Rotation(45);
            //image.Emboss();
            // image.Flou();
            //double[,] embossKernel = new double[,] { { 1 / 9, 1 / 9, 1 / 9 }, { 1 / 9, 1 / 9, 1 / 9 }, { 1 / 9, 1 / 9, 1 / 9 } };
            //image.Embossing(embossKernel);
            Rotate rotate = new Rotate(45,image);
            rotate.From_Image_To_File("Test_Rotation");
            Console.ReadLine();

          

        
        }

    }
}

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
using System.Collections;
using MLModel1_ConsoleApp1;
using System.IO;



namespace ConsoleProgram
{
    class Program
    {
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Valider la compatibilité de la plateforme", Justification = "<En attente>")]
        static void Main(string[] args)
        {
            string basicPath = "./bin/Debug/net7.0/images/";
            MyImage image = new MyImage(basicPath + "coco.bmp");
            //image.From_Image_To_File("cocosortie");
            // image.ImageEnGris();
            //image.Agrandissement(20);
            //image.Rotation(45);
            //image.Emboss();
            // image.Flou();
            //double[,] embossKernel = new double[,] { { 1 / 9, 1 / 9, 1 / 9 }, { 1 / 9, 1 / 9, 1 / 9 }, { 1 / 9, 1 / 9, 1 / 9 } };
            //image.Embossing(embossKernel);
           // Rotate rotate = new Rotate(78,image);
            //rotate.From_Image_To_File("Test_Rotation78");
            //Convolution convo = new Convolution(image,"Edge_enhance");
            //convo.From_Image_To_File("Emboss82852");
           /* image.MandelBrot();
            MyImage imageBase = new MyImage(basicPath+"TestHugo1.bmp");
            MyImage imageCode = new MyImage(basicPath+"TestHugo2.bmp");
            Steganography stega = new Steganography(imageBase);
            stega.Encode(imageCode,4);
            stega.Decode();*/
           // image.MandelBrot();
          /* Console.WriteLine("Please enter the string:");
            string input = Console.ReadLine();
            ArbreHuffman huffmanTree = new ArbreHuffman();

            // Build the Huffman tree
            huffmanTree.Build(input);

            // Encode
            BitArray encoded = huffmanTree.Encode(input);

            Console.Write("Encoded: ");
            foreach (bool bit in encoded)
            {
                Console.Write((bit ? 1 : 0) + "");
            }
            Console.WriteLine();

            // Decode
            string decoded = huffmanTree.Decode(encoded);

            Console.WriteLine("Decoded: s" + decoded);

            */


          #region ML
          /*
            var imageBytes = File.ReadAllBytes(@"C:\Users\eliot\OneDrive\Bureau\téléchargé.jpg");
            MLModel1.ModelInput sampleData = new MLModel1.ModelInput()
            {
                ImageSource = imageBytes,
            };

            // Make a single prediction on the sample data and print results.
            var predictionResult = MLModel1.Predict(sampleData);
            Console.WriteLine($"\n\nPredicted Label value: {predictionResult.PredictedLabel} \nPredicted Label scores: [{String.Join(",", predictionResult.Score)}]\n\n");
          */
          #endregion  

        
        }
        public void ML(string chemin){
            var imageBytes = File.ReadAllBytes(chemin);
            MLModel1.ModelInput sampleData = new MLModel1.ModelInput()
            {
                ImageSource = imageBytes,
            };

            // Make a single prediction on the sample data and print results.
            var predictionResult = MLModel1.Predict(sampleData);
            Console.WriteLine($"\n\nPredicted Label value: {predictionResult.PredictedLabel} \nPredicted Label scores: [{String.Join(",", predictionResult.Score)}]\n\n");
          
        }

    }
}

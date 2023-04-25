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
using System.Windows.Forms;
using MLModel1_ConsoleApp1;
using System.IO;


namespace ConsoleProgram
{
    class Program
    {
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Valider la compatibilité de la plateforme", Justification = "<En attente>")]

        [STAThread]
        
        static void Main(string[] args)
        {

            //string basicPath = "./bin/Debug/net7.0/images/";
            //MyImage image = new MyImage(basicPath + "coco.bmp");
            WelcomePage();
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

        #region Machine Learning

        // Machine Learning
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

        #endregion


        #region Interface
        static void WelcomePage()
        {

            //Page lancement

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("---- PROBLEME SCIENTIFIQUE & INFORMATIQUE ----");

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("     Projet  PSI     \n      ESILV  A2\n\n\n");

            Console.WriteLine("Par:\nCOUTAZ Eliott\t|\tTD A\nBONNELL Hugo\t|\tTD A\n\n\n\n\n\n");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Pour lancer l'application, appuyez sur une touche");
            Console.ForegroundColor = ConsoleColor.White;
            Console.ReadKey();



            //Choix photo à importer

            ChoixFichier:
            Console.Clear();
            Console.WriteLine("Veuillez choisir une photo à modifier, appuyez sur une touche pour ouvrir votre navigateur de fichiers.");
            Console.ReadKey();
            string FilePath = SelectFile();
            if (FilePath == "Erreur")
            {
                goto ChoixFichier;
            }
            FilePath = FilePath.Replace('\\','/');
            Console.WriteLine("Nom fichier: " + FilePath);
            Console.ReadKey();
            MainMenu(FilePath);

        }

        private static string SelectFile()
        {
            var dlg = new OpenFileDialog()
            {
                InitialDirectory = "",
                Filter = "BMP Files (*.bmp) | *.bmp | All Files (*.*) | *.*",
                RestoreDirectory = true
            };

            // Pas de fichier
            if (dlg.ShowDialog() != DialogResult.OK)
                return "Erreur";
   
            //Return le fichier si choisi
            return dlg.FileName;

        }

        static void MainMenu(string FilePath)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("---- Menu Principal ----\n\n");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("[1] Passer l'image en nuances de gris\n[2] Rotation\n[3] Agrandissement\n[4] Convolution\n[5] Créer une fractale\n[6] Compression Huffman\n[7] Machine Learning - Chien ou Chat ?\n\nEntrez le numéro de votre choix:\n");
            Console.ForegroundColor = ConsoleColor.Blue;
            string choix = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.White;
            switch (choix)
            {
                case "1":
                    PageGris(FilePath);
                    break;
                case "2":
                    PageRotation(FilePath);
                    break;
                case "3":
                    PageAgrandissment(FilePath);
                    break;
                case "4":
                    PageConvolution(FilePath);
                    break;
                case "5":
                    PageFractale(FilePath);
                    break;
                case "6":
                    PageHuffman(FilePath);
                    break;
                case "7":
                    PageML(FilePath);
                    break;
                default:
                    MainMenu(FilePath);
                    break;
            }
        }


        static void PageGris(string FilePath)
        {
            MyImage image = new MyImage(FilePath);
            image.ImageEnGris(FilePath);
        }

        static void PageRotation(string FilePath)
        {

        }

        static void PageAgrandissment(string FilePath)
        { 
        
        }

        static void PageConvolution(string FilePath)
        {

        }
        
        static void PageFractale(string FilePath)
        {

        }

        static void PageHuffman(string FilePath)
        {
            
        }

        static void PageML(string FilePath)
        {

        }

        #endregion
        
        public static string AskForExitName()
        {
            Console.WriteLine("\nNommez le fichier à sauvegader :\n");
            Console.ForegroundColor = ConsoleColor.Blue;
            string ExitName = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("");
            return ExitName;

        }

    }
}

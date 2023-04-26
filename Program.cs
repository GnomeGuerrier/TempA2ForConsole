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

            Console.WriteLine("Par:\nCOUTAZ Eliott\t|\tTD A\nBONNELL Hugo\t|\tTD A\nMONCET Grégoire\t|\tTD A\n\n\n\n\n\n");
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
                Filter = "JPG Files (*.jpg) | *.jpg | BMP Files (*.bmp) | *.bmp | All Files (*.*) | *.*",
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
            Console.WriteLine("[1] Passer l'image en nuances de gris\n[2] Rotation\n[3] Agrandissement\n[4] Convolution\n[5] Créer une fractale\n[6] Compression Huffman\n[7] Steganographie\n[8] Machine Learning - Chien ou Chat ?\n\nEntrez le numéro de votre choix:\n");
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
                    PageStegano(FilePath);
                    break;
                case "8":
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
            MyImage image = new MyImage(FilePath);
            Console.WriteLine("Vous allez faire tourner votre image, de quel angle souhaitez-vous la faire tourner (en °) ?\n");
            Console.ForegroundColor = ConsoleColor.Blue;
            int angle = Convert.ToInt32(Console.ReadLine());
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("");
            Rotate rotate = new Rotate(angle, image);
            rotate.From_Image_To_File("Rotation_" + angle );
            MainMenu(FilePath);
        }

        static void PageAgrandissment(string FilePath)
        {
            MyImage image = new MyImage(FilePath);
            Console.WriteLine("Vous allez agrandir votre image, quel multiplicateur souhaitez vous appliquer ?\n0<x<1 => Rétrécir \n1<x => Agrandir\nDonnez votre multiplicateur :\n");
            Console.ForegroundColor = ConsoleColor.Blue;
            double zoom = Convert.ToDouble(Console.ReadLine());
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("");
            image.Agrandissement(zoom);
            MainMenu(FilePath);
        }

        static void PageConvolution(string FilePath)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Vous êtes dans le menu convolution, choisissez votre filtre :\n[1] Edge_detect\n[2] Edge_enhance\n[3] Emboss\n[4] BoxBlur\n[5] GaussianBlur\n[6] Sharpen\n[7] Retour au menu");
            MyImage image = new MyImage(FilePath);
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Blue;
            int choix = Convert.ToInt32(Console.ReadLine());
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("");
            Convolution convo;
            switch (choix)
            {
                case 1:
                    convo = new Convolution(image, "Edge_detect");
                    convo.Path = image.Path;
                    convo.From_Image_To_File("Edge_detect");
                    break;
                case 2:
                    convo = new Convolution(image, "Edge_enhance");
                    convo.Path = image.Path;
                    convo.From_Image_To_File("Edge_enhance");
                    break;
                case 3:
                    convo = new Convolution(image, "Emboss");
                    convo.Path = image.Path;
                    convo.From_Image_To_File("Emboss");
                    break;
                case 4:
                    convo = new Convolution(image, "BoxBlur");
                    convo.Path = image.Path;
                    convo.From_Image_To_File("BoxBlur");
                    break;
                case 5:
                    convo = new Convolution(image, "GaussianBlur");
                    convo.Path = image.Path;
                    convo.From_Image_To_File("GaussianBlur");
                    break;
                case 6:
                    convo = new Convolution(image, "Sharpen");
                    convo.Path = image.Path;
                    convo.From_Image_To_File("Sharpen");
                    break;
                case 7:
                    MainMenu(FilePath);
                    break;
                default:
                    PageConvolution(FilePath);
                    break;

            }

            
        }

        static void PageFractale(string FilePath)
        {
            MyImage image = new MyImage(FilePath);
            Console.WriteLine("Création de la franctale...");
            image.MandelBrot(FilePath);
            MainMenu(FilePath);
        }

        static void PageHuffman(string FilePath)
        {
            //Eliott tkt
        }

        static void PageStegano(string FilePath)
        {
            MyImage imBase = new MyImage(FilePath);
            Console.WriteLine("Voulez-vous encoder ou décoder une image ?\n[1] Encoder\n[2] Décoder\n");
            Console.ForegroundColor = ConsoleColor.Blue;
            string choix = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.White;
            Steganography stega = new Steganography(imBase);

            if (choix == "1")
            {
                Console.WriteLine("Choisissez une image à cacher dans votre image\nAppuyez sur une touche pour ouvrir l'explorateur de fichiers");
                Console.ReadKey();
                string FilePath2 = SelectFile();
                MyImage imCache = new MyImage(FilePath2);
                stega.Encode(imCache, 4);
                Console.WriteLine("L'image a été encodée");
                Console.ReadKey();
            }
            else if (choix == "2")
            {
                stega.Decode();
                Console.WriteLine("Appuyez sur une touche pour continuer ...");
                Console.ReadKey();
            }
            else if (choix == "3")
            {
                Console.WriteLine("Choisissez une image à cacher dans votre image\nAppuyez sur une touche pour ouvrir l'explorateur de fichiers");
                Console.ReadKey();
                string FilePath2 = SelectFile();
                MyImage imCache = new MyImage(FilePath2);
                stega.Encode(imCache, 4);
                stega.Decode();
                Console.WriteLine("L'image a été encodée puis décodée");
                Console.ReadKey();
            }
            else
            {
                PageStegano(FilePath);
            }

            MainMenu(FilePath);
            
        }

        static void PageML(string FilePath)
        {
            var imageBytes = File.ReadAllBytes(FilePath);
            MLModel1.ModelInput sampleData = new MLModel1.ModelInput()
            {
                ImageSource = imageBytes,
            };

            // Make a single prediction on the sample data and print results.
            var predictionResult = MLModel1.Predict(sampleData);
            Console.WriteLine($"\n\nPredicted Label value: {predictionResult.PredictedLabel} \nPredicted Label scores: [{String.Join(",", predictionResult.Score)}]\n\n");
            Console.Clear();
            if (predictionResult.Score[1] > 0.2 && predictionResult.Score[1]<0.8)
            {
                Console.WriteLine("Votre image n'est à priori ni un chien, ni un chat...");
            }
            else
            {
                if(predictionResult.PredictedLabel == "Cat")
                {
                    Console.WriteLine("Votre image est reconnu comme un chat avec un score de " + (1-predictionResult.Score[1]) * 100 + "%");
                }
                else
                {
                    Console.WriteLine("Votre image est reconnu comme un chien avec un score de " + predictionResult.Score[1] * 100 + "%");
                }
                
            }


            Console.ReadLine();
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

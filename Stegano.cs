﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleProgram
{
    /// <summary>
    /// Class used to encode or decode Least Significant Bit steganography
    /// </summary>
    class Steganography
    {
        //==================================================================================================================================================================================================================================================
        // CONSTRUCTOR
        //==================================================================================================================================================================================================================================================

        #region Constructor

        private MyImage im;   //Original image (used as based image for encoding, and trying to decrypt

        /// <summary>
        /// basic constructor, this takes the original image, to decode it, or encode another image inside it
        /// </summary>
        /// <param name="originalim"></param>
        public Steganography(MyImage originalim)  //This is for encoding and decoding, it only acts as a setter for the base picture
        {
            this.im = originalim;

            this.hauteur = im.hauteur;
            this.largeur = im.largeur;
            this.offset = im.offset;
            this.tailleFichier = im.tailleFichier;
            this.type = im.type;
            this.bpc = im.bpc;
            this.myfile = originalim.myfile; ;

        }

        #endregion

        //==================================================================================================================================================================================================================================================
        // FUNCTIONS
        //==================================================================================================================================================================================================================================================

        #region Functions

        /// <summary>
        /// Encode an image inside our image
        /// </summary>
        /// <param name="hiddenim">the image to be hidden</param>
        /// <param name="hiddenbits">the numbers of significant bit to be used</param>
        public void Encode(MyImage hiddenim, int hiddenbits)   //Change 8 to bpc/3?
        {


            MyImage encodedim = new MyImage(this.im);
            for (int x = 0; x < this.im.myfile.GetLength(0); x++)
            {
                for (int y = 0; y < this.im.myfile.GetLength(1); y++)
                {

                    if (hiddenim.myfile.GetLength(0) > x && hiddenim.myfile.GetLength(1) > y) //in case hiddenim is smaller, it gets cropped
                    {
                        string RedBin = Utilities.ToBin(this.im.myfile[x, y].R, this.bpc / 3);
                        string GreenBin = Utilities.ToBin(this.im.myfile[x, y].G, this.bpc / 3);
                        string BlueBin = Utilities.ToBin(this.im.myfile[x, y].B, this.bpc / 3);

                        string HiddenRedBin = Utilities.ToBin(hiddenim.myfile[x, y].R, this.bpc / 3);
                        string HiddenGreenBin = Utilities.ToBin(hiddenim.myfile[x, y].G, this.bpc / 3);
                        string HiddenBlueBin = Utilities.ToBin(hiddenim.myfile[x, y].B, this.bpc / 3);

                        string NewRed = "";
                        string NewGreen = "";
                        string NewBlue = "";
                        //We want to add hiddenbits number of digits of the original RedBin binary, so when digit is strictly higher than hiddenbits(-1 for index) we use the 
                        //Hidden bit with digit-(8-hiddenbits) so for example if hiddenbits is 4, when we get to digit=4 we access the 4-(8-4)=0th bit of the hidden image
                        for (int digit = 0; digit < this.bpc / 3; digit++)
                        {
                            NewRed += digit > hiddenbits - 1 ? HiddenRedBin[digit - (this.bpc / 3 - hiddenbits)] : RedBin[digit];
                        }
                        for (int digit = 0; digit < this.bpc / 3; digit++)
                        {
                            NewGreen += digit > hiddenbits - 1 ? HiddenGreenBin[digit - (this.bpc / 3 - hiddenbits)] : GreenBin[digit];
                        }
                        for (int digit = 0; digit < this.bpc / 3; digit++)
                        {
                            NewBlue += digit > hiddenbits - 1 ? HiddenBlueBin[digit - (this.bpc / 3 - hiddenbits)] : BlueBin[digit];
                        }



                        encodedim.myfile[x, y].R = Convert.ToInt32(Convert.ToString(Convert.ToInt32(NewRed, 2), 10));
                        encodedim.myfile[x, y].G = Convert.ToInt32(Convert.ToString(Convert.ToInt32(NewGreen, 2), 10));
                        encodedim.myfile[x, y].B = Convert.ToInt32(Convert.ToString(Convert.ToInt32(NewBlue, 2), 10));

                    }
                    else //If hiddenim is smaller than actual im, we don't alter the pixel
                    {
                        encodedim.myfile[x, y] = new Pixel(im.myfile[x, y]);
                    }


                }
            }
            encodedim.From_Image_To_File("TestHugoFinal");
        }





        /// <summary>
        /// A bruteforce way to decode an LSB hidden image, it tries for all bits possible to recover the hidden image from the original image
        /// </summary>
        public void Decode()
        {

            for (int bits = 1; bits < this.bpc / 3; bits++)
            {
                MyImage decodedim = new MyImage(this.im);  //Same properties, only the matrix is going to change
                for (int x = 0; x < this.im.myfile.GetLength(0); x++)
                {
                    for (int y = 0; y < this.im.myfile.GetLength(1); y++)
                    {

                        //string of binary conversion of the value of each decimal color value of the x,y pixel
                        string RedBin = Utilities.ToBin(this.im.myfile[x, y].R, this.bpc / 3);
                        string GreenBin = Utilities.ToBin(this.im.myfile[x, y].G, this.bpc / 3);
                        string BlueBin = Utilities.ToBin(this.im.myfile[x, y].B, this.bpc / 3);

                        string NewRed = "";
                        string NewGreen = "";
                        string NewBlue = "";

                        for (int digit = 0; digit < this.bpc / 3; digit++)   //counter from 0 to 7, to access each binary completly
                        {

                            //If the bits is 2, we want want to take all the bits from the (8-2)=6th bit
                            //if the digit considered is lower than the significant bits that we are treated right now, we don't take it into account because it belongs to the original image
                            if (digit >= (8 - bits)) NewRed += RedBin[digit];
                        }
                        for (int digit = 0; digit < this.bpc / 3; digit++)
                        {
                            if (digit >= (8 - bits)) NewGreen += GreenBin[digit];
                        }
                        for (int digit = 0; digit < this.bpc / 3; digit++)
                        {
                            if (digit >= (8 - bits)) NewBlue += BlueBin[digit];
                        }
                        //Since we only took [bits] digits to create the new pixel, we need to add [8-bits] 0 to the end of the value:
                        for (int i = 0; i < (8 - bits); i++)
                        {
                            NewRed += "0";
                            NewGreen += "0";
                            NewBlue += "0";
                        }


                        //Remap new values
                        decodedim.myfile[x, y].R = Convert.ToInt32(Convert.ToString(Convert.ToInt32(NewRed, 2), 10));
                        decodedim.myfile[x, y].G = Convert.ToInt32(Convert.ToString(Convert.ToInt32(NewGreen, 2), 10));
                        decodedim.myfile[x, y].B = Convert.ToInt32(Convert.ToString(Convert.ToInt32(NewBlue, 2), 10));
                        //Console.WriteLine(decodedim.myfile[x, y].toString());

                    }
                }

                decodedim.From_Image_To_File("Stegano_Decoded_SB=" + bits, false);
                Console.WriteLine("Done Saving Stegano_Decoded_SB=" + bits);

            }
        }

        #endregion

    }
}



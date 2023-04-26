using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace ConsoleProgram
{
    public class ArbreHuffman
    {
        public List<Noeud> noeuds = new List<Noeud>();
        public Noeud Racine { get; set; }
        public Dictionary<Pixel, int> Frequences = new Dictionary<Pixel, int>();
        public MyImage image;
        #region 
        #region
        public Dictionary<char, int> Frequencies = new Dictionary<char, int>();
        string input = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aenean fringilla elit vel felis accumsan, eget mollis arcu rutrum. Ut aliquam odio nec sem tristique molestie. Vivamus justo justo, rhoncus id imperdiet sed, luctus sodales lacus. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Cras malesuada scelerisque ligula sed gravida. Maecenas vel iaculis tellus. Vivamus interdum dictum erat pulvinar imperdiet. Nullam nulla dui, consectetur eget egestas ac, tincidunt vel dui.\r\n\r\nVivamus vehicula ex at ligula sagittis, quis semper lorem accumsan. Etiam imperdiet mattis orci, tempus gravida augue tristique vitae. Fusce lobortis sapien sit amet ultrices viverra. Sed a convallis arcu. Etiam tincidunt viverra mi, ac faucibus neque eleifend nec. Sed at felis libero. In sodales tortor sed euismod mattis. Integer lectus nisl, posuere ut posuere in, commodo sed risus.\r\n\r\nMaecenas in enim faucibus, hendrerit nibh ac, pulvinar arcu. Phasellus ut tellus rhoncus ante pretium efficitur in vitae sem. Nullam rutrum id dui eget ornare. Maecenas nec scelerisque arcu. Fusce ornare tempus ipsum, sed commodo tellus rutrum a. Curabitur id massa ac turpis placerat suscipit eget ultricies mi. Pellentesque eleifend elit diam, ac bibendum elit vehicula vehicula. Integer semper eu arcu et mattis. Nullam interdum velit nisl, id sodales augue eleifend nec. Quisque non hendrerit tellus. Maecenas ligula ante, commodo sed molestie quis, suscipit volutpat enim. Praesent dictum lectus ipsum, aliquet malesuada libero congue eu.\r\n\r\nMorbi bibendum risus at felis tempus, eget dapibus risus facilisis. Pellentesque eget auctor magna, quis tristique felis. Sed ornare sagittis arcu, id gravida mauris auctor eu. Aliquam ullamcorper sit amet erat in volutpat. Morbi at consequat sem, in volutpat arcu. Suspendisse vulputate non magna id convallis. Etiam odio tellus, interdum a facilisis sit amet, pretium eget nisl. Aliquam sodales ex tempus, varius dolor eu, tincidunt diam. Praesent posuere ipsum ipsum, ut tempus felis sollicitudin vel. Vestibulum ut maximus ante, vel tincidunt nulla. Sed tortor massa, fringilla in pretium vel, posuere vulputate erat. Quisque maximus erat bibendum odio hendrerit, in viverra elit volutpat.\r\n\r\nDonec tincidunt ante justo, a porta massa sollicitudin eget. Phasellus finibus elementum facilisis. Donec viverra neque urna, ut convallis arcu euismod ut. Curabitur ut nunc eget ipsum elementum mollis. Sed magna dolor, cursus nec odio faucibus, sagittis varius lacus. Maecenas maximus efficitur viverra. Donec eget neque id elit tempus lacinia. Nam elementum blandit sapien, quis eleifend metus malesuada vel. Phasellus a quam et magna sagittis lacinia. Nullam ut faucibus sem. Donec in dignissim sem, eu vulputate ante. Interdum et malesuada fames ac ante ipsum primis in faucibus. Donec malesuada, ipsum id ullamcorper volutpat, dolor purus malesuada leo, id mattis diam magna ut ligula. Quisque fringilla, ipsum nec sagittis rutrum, leo orci condimentum diam, id hendrerit nunc justo et magna.";
        #endregion
        #endregion
        public void Build(MyImage  innput)
        {
            this.image = innput;
            for (int i = 0; i < input.Length; i++)
            {
                if (!Frequencies.ContainsKey(input[i]))
                {
                    Frequencies.Add(input[i], 0);
                }

                Frequencies[input[i]]++;
            }

            foreach (KeyValuePair<char, int> symbol in Frequencies)
            {
                noeuds.Add(new Noeud() { Symbol = symbol.Key, Fréquence = symbol.Value });
            }

            while (noeuds.Count > 1)
            {
                List<Noeud> orderedNodes = noeuds.OrderBy(node => node.Fréquence).ToList<Noeud>();

                if (orderedNodes.Count >= 2)
                {
                    // Take first two items
                    List<Noeud> taken = orderedNodes.Take(2).ToList<Noeud>();

                    // Create a parent node by combining the frequencies
                    Noeud parent = new Noeud()
                    {
                        Symbol = '*',
                        Fréquence = taken[0].Fréquence + taken[1].Fréquence,
                        Gauche = taken[0],
                        Droite = taken[1]
                    };

                    noeuds.Remove(taken[0]);
                    noeuds.Remove(taken[1]);
                    noeuds.Add(parent);
                }

                this.Racine = noeuds.FirstOrDefault();

            }

        }

        public  BitArray Encode(MyImage innput)
        {
            List<bool> encodedSource = new List<bool>();

            for (int i = 0; i < this.input.Length; i++)
            {
                List<bool> encodedSymbol = this.Racine.Traverse(this.input[i], new List<bool>());
                encodedSource.AddRange(encodedSymbol);
            }

            BitArray bits = new BitArray(encodedSource.ToArray());

            return bits;
        }

        public void Decode(BitArray bits)
        {
            Noeud current = this.Racine;
            string decoded = "";

            foreach (bool bit in bits)
            {
                if (bit)
                {
                    if (current.Droite != null)
                    {
                        current = current.Droite;
                    }
                }
                else
                {
                    if (current.Gauche != null)
                    {
                        current = current.Gauche;
                    }
                }

                if (IsLeaf(current))
                {
                    decoded += current.Symbol;
                    current = this.Racine;
                }
            }
            this.image.From_Image_To_File("_Huffman");
            
        }

        public bool IsLeaf(Noeud node)
        {
            return (node.Gauche == null && node.Droite == null);
        }








































    }
}

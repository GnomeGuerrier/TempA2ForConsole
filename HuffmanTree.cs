using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace ConsoleProgram
{
    public class HuffmanTree
    {
        private List<Noeud> noeuds = new List<Noeud>();
        public Noeud Racine { get; set; }
        public Dictionary<char, int> Frequencies = new Dictionary<char, int>();

        public void Build(string source)
        {
            for (int i = 0; i < source.Length; i++)
            {
                if (!Frequencies.ContainsKey(source[i]))
                {
                    Frequencies.Add(source[i], 0);
                }

                Frequencies[source[i]]++;
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

        public BitArray Encode(string source)
        {
            List<bool> encodedSource = new List<bool>();

            for (int i = 0; i < source.Length; i++)
            {
                List<bool> encodedSymbol = this.Racine.Traverse(source[i], new List<bool>());
                encodedSource.AddRange(encodedSymbol);
            }

            BitArray bits = new BitArray(encodedSource.ToArray());

            return bits;
        }

        public string Decode(BitArray bits)
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

            return decoded;
        }

        public bool IsLeaf(Noeud node)
        {
            return (node.Gauche == null && node.Droite == null);
        }

    }
}

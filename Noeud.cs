using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleProgram
{
    public class Noeud
    {
        public char Symbol { get; set; }
        public int Fréquence { get; set; }
        public Noeud Droite { get; set; }
        public Noeud Gauche { get; set; }

        public List<bool> Traverse(char symbol, List<bool> data)
        {
            // Leaf
            if (Droite == null && Gauche == null)
            {
                if (symbol.Equals(this.Symbol))
                {
                    return data;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                List<bool> gauche = null;
                List<bool> droite = null;

                if (Gauche != null)
                {
                    List<bool> cheminGauche = new List<bool>();
                    cheminGauche.AddRange(data);
                    cheminGauche.Add(false);

                    gauche = Gauche.Traverse(symbol, cheminGauche);
                }

                if (Droite != null)
                {
                    List<bool> cheminDroite = new List<bool>();
                    cheminDroite.AddRange(data);
                    cheminDroite.Add(true);
                    droite = Droite.Traverse(symbol, cheminDroite);
                }

                if (gauche != null)
                {
                    return gauche;
                }
                else
                {
                    return droite;
                }
            }
        }
    }
}
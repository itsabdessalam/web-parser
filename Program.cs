/**
 * Created by Abdessalam BENHARIRA
 * Project Name : Web Parser
 */
 
using System;

namespace WebParser {
    class Program {
        static void Main (string[] args) {
            // Enlevez les commentaires pour lancer le programme
            Parser p = new Parser ();
            p.RecupererData ();
            p.ChangerTitre ();
            p.SupprimerScripts ();
            p.ChangerSrcImg ();
            p.TexteEnGras ();
            p.NettoyageHtml ();
            p.MotFrequent ();
        }
    }
}

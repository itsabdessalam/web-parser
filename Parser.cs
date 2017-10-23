/**
 * Created by Abdessalam BENHARIRA
 * Project Name : Web Parser
 */

using System;
//List
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
//Regex
using System.Text.RegularExpressions;

namespace WebParser {
    class Parser {
        public string htmlCode;
        //Le chemin
        string path = Environment.GetFolderPath (Environment.SpecialFolder.Desktop);
        // Methode pour recuperer le code html de la page
        public void RecupererData () {
            // https://msdn.microsoft.com/fr-fr/library/system.net.webclient(v=vs.110).aspx
            using (WebClient client = new WebClient ()) {
                htmlCode = client.DownloadString ("http://abdessalam-benharira.me");
                using (StreamWriter swriter = new StreamWriter (path + "/maPage.txt", true)) {
                    swriter.WriteLine (htmlCode);
                }
            }
        }
        // Methode pour changer la balise <title> de la page
        public void ChangerTitre () {

            string htmlTextT;
            using (StreamReader reader = new StreamReader (path + "/maPage.txt")) {
                while ((htmlTextT = reader.ReadLine ()) != null) {
                    using (StreamWriter swriter = File.AppendText (path + "/test.txt")) {
                        htmlTextT = Regex.Replace (htmlTextT, @"<title>\s*(.+?)\s*</title>", "<title>Projet Web Parser CSharp</title>");
                        swriter.WriteLine (htmlTextT);
                    }
                }
            }
        }
        // Methode pour supprimer toutes les balises <script> de la page
        public void SupprimerScripts () {
            string htmlTextS;
            using (StreamReader reader = new StreamReader (path + "/test.txt")) {
                while ((htmlTextS = reader.ReadLine ()) != null) {
                    using (StreamWriter swriter = File.AppendText (path + "/test1.txt")) {
                        htmlTextS = Regex.Replace (htmlTextS, @"<script[^>]*>[\s\S]*?</script>", String.Empty);
                        swriter.WriteLine (htmlTextS);
                    }
                }
            }
            // je supprime le fichier passé en lecture
            if (File.Exists (path + "/test.txt")) {
                File.Delete (path + "/test.txt");
            }

        }
        // Methode pour changer la src de <img>
        public void ChangerSrcImg () {
            string htmlTextI;
            using (StreamReader reader = new StreamReader (path + "/test1.txt")) {
                while ((htmlTextI = reader.ReadLine ()) != null) {
                    using (StreamWriter swriter = File.AppendText (path + "/test2.txt")) {
                        // Soit on remplace toute la balise
                        htmlTextI = Regex.Replace (htmlTextI, "<img.+?src=[\"'](.+?)[\"'].*?>", " <img src=\"http://abdessalam-benharira.me/blog/wp-content/uploads/2017/09/logo.png\">");
                        // soit on remplace seulement le src vu que la balise <script> contient src et qu'on les a déja supprimé ou on utilise
                        // htmlTextI = Regex.Replace (htmlTextI, "src=\"([^\"]*)\"" ,"src=\"http://abdessalam-benharira.me/blog/wp-content/uploads/2017/09/logo.png\"");
                        swriter.WriteLine (htmlTextI);
                    }
                }
            }
            // je supprime le fichier passé en lecture
            if (File.Exists (path + "/test1.txt")) {
                File.Delete (path + "/test1.txt");
            }
        }
        public void TexteEnGras () {
            string htmlTextB;
            using (StreamReader reader = new StreamReader (path + "/test2.txt")) {
                while ((htmlTextB = reader.ReadLine ()) != null) {
                    using (StreamWriter swriter = File.AppendText (path + "/maPageFinale.txt")) {
                        /*en mettant <style> * { font-weight:bold !important; } </style> avant la fermeture du head
                         et en important (ecrase le style de font appliqué avant) tout le texte sera en gras 
                        */
                        htmlTextB = Regex.Replace (htmlTextB, "</head>", "\t" + "<style> * { font-weight:bold !important; } </style>" + "\n" + "</head>");
                        swriter.WriteLine (htmlTextB);
                    }
                }
            }
            // je supprime le fichier passé en lecture
            if (File.Exists (path + "/test2.txt")) {
                File.Delete (path + "/test2.txt");
            }
        }
        // Cette methode permet de recuperer le texte html sans balise
        public void NettoyageHtml () {
            string htmlTextC;
            List<string> SplitText = new List<string> ();
            using (StreamReader reader = new StreamReader (path + "/maPageFinale.txt")) {
                while ((htmlTextC = reader.ReadLine ()) != null) {
                    using (StreamWriter swriter = File.AppendText (path + "/texteSplit.txt")) {
                        htmlTextC = Regex.Replace (htmlTextC, "<[^>].+?>", string.Empty);
                        //htmlTextC = Regex.Replace (htmlText, @"[^\w\s]", string.Empty);
                        // supprimer les espaces
                        htmlTextC = Regex.Replace (htmlTextC, @"\s+", " ");
                        htmlTextC = htmlTextC.Replace (Environment.NewLine + Environment.NewLine, Environment.NewLine);
                        // je mets tous les separateurs dans un tableau
                        char[] separateurs = { ',', '.', '!', '?', ';', ':', '/', '\'', '’', '(', ')', ' ' };
                        string[] words = htmlTextC.Split (separateurs);
                        // je mets le tableau dans une liste
                        SplitText = words.ToList<string> ();
                        // je supprime tous les element vides de la liste
                        SplitText = SplitText.Where (s => !string.IsNullOrWhiteSpace (s)).Distinct ().ToList ();
                        foreach (string item in SplitText) {
                            swriter.WriteLine (item);
                        }
                    }
                }
            }
        }
        // Cette methode permet de recuperer le mot le plus repeté
        public void MotFrequent () {
            List<string> lignes = File.ReadLines (path + "/texteSplit.txt").ToList ();
            /*https://docs.microsoft.com/fr-fr/dotnet/csharp/programming-guide/classes-and-structs/anonymous-types
             https://docs.microsoft.com/fr-fr/dotnet/framework/data/adonet/ef/language-reference/initialization-expressions
             */

            string motFrequent = lignes.GroupBy (s => s).OrderByDescending (s => s.Count ()).First ().Key;
            using (StreamWriter swriter = File.AppendText (path + "/MotFrequent.txt")) {

                swriter.WriteLine (motFrequent);
            }
        }
    }
}
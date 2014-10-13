﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace SikkerDigitalPost.Klient.Utilities
{
    public class FileUtility
    {
        enum Bruker
        {
            Aleksander,
            Marius
        }

        private static Bruker CurrentBruker
        {
            get
            {
                if (Environment.MachineName.Contains("ALEKS"))
                {
                    return Bruker.Aleksander;
                }

                if (Environment.MachineName.Contains("LOK"))
                {
                    return Bruker.Marius;
                }

                throw new ArgumentException("Kunne ikke finne ut hvilken bruker du er! Legg deg selv til i listen.");
            }
        }

        /// <summary>
        /// Returnerer base-sti basert på nåværede bruker.
        /// </summary>
        public static string BasePath 
        {
            get
            {
                if (CurrentBruker == Bruker.Aleksander)
                {
                    return @"C:\sdp";
                }

                if (CurrentBruker == Bruker.Marius)
                {
                    return @"C:\Prosjekt\DigiPost\Temp\";
                }

                throw new Exception("Kunne ikke finne ut hvilken bruker du er! Legg deg selv til i listen.");
            }
        }

        /// <summary>
        /// Hvis din basesti er "C:\base" og du sender inn "mappe\hei.txt", så vil filen lagres
        /// på "C:\base\mappe\hei.txt".
        /// </summary>
        /// <param name="pathRelativeToBase">The relative part of the path. The base path is set in your settings. </param>
        /// <param name="data">The data to write.</param>
        public static void WriteToFileInBasePath(string pathRelativeToBase, string data)
        {
            File.WriteAllText(Path.Combine(BasePath,pathRelativeToBase),data);
        }

        public static void AppendToFileInBasePath(string pathRelativeToBase, string data)
        {
            File.AppendAllText(Path.Combine(BasePath, pathRelativeToBase),data);
        }

        public static void WriteXmlToFileInBasePath(string pathRelativeToBase, string xml)
        {
            XDocument doc = XDocument.Parse(xml);
            WriteToFileInBasePath(pathRelativeToBase, doc.ToString());
        }
    }
}

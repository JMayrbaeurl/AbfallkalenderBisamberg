using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace AbfallkalenderBisamberg2011WP7
{
    public class Abfallart
    {
        public static Abfallart Restmüll = new Abfallart { Name = "Restmüll" };
        public static Abfallart Restmüll14tg = new Abfallart { Name = "Restmüll 14tg" };
        public static Abfallart Biomüll = new Abfallart { Name = "Biomüll" };
        public static Abfallart GelberSack = new Abfallart { Name = "Gelber Sack" };
        public static Abfallart Altpapier = new Abfallart { Name = "Altpapier" };
        public static Abfallart Sperrmüll = new Abfallart { Name = "Sperrmüll" };

        private string bezeichnung;

        public string Name
        {
            get { return bezeichnung; }
            set { bezeichnung = value; }
        }

        private static IDictionary<string, Abfallart> verzeichnis;
        public static IDictionary<string, Abfallart> Abfallartenverzeichnis { get { return Abfallart.verzeichnis; } }

        static Abfallart()
        {
            Abfallart.verzeichnis = new Dictionary<string, Abfallart>();
            Abfallart.verzeichnis.Add(Restmüll.Name, Restmüll);
            Abfallart.verzeichnis.Add(Restmüll14tg.Name, Restmüll14tg);
            Abfallart.verzeichnis.Add(Biomüll.Name, Biomüll);
            Abfallart.verzeichnis.Add(GelberSack.Name, GelberSack);
            Abfallart.verzeichnis.Add(Altpapier.Name, Altpapier);
            Abfallart.verzeichnis.Add(Sperrmüll.Name, Sperrmüll);
        }
    }
}

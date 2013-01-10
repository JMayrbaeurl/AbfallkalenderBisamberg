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
    public class Zone
    {
        private static Zone zoneA = new Zone { Kennzeichen = "A" };
        public static Zone A { get { return Zone.zoneA; } }

        private static Zone zoneB = new Zone { Kennzeichen = "B" };
        public static Zone B { get { return Zone.zoneB; } }

        private static Zone zoneC = new Zone { Kennzeichen = "C" };
        public static Zone C { get { return Zone.zoneC; } }

        private static Zone zoneAlle = new Zone { Kennzeichen = "", IstFuerAlle = true};
        public static Zone Alle { get { return Zone.zoneAlle; } }

        private string zonenkennzeichen;

        public string Kennzeichen
        {
            get { return zonenkennzeichen; }
            set { zonenkennzeichen = value; }
        }

        public bool IstFuerAlle { get; private set; }

        private static IDictionary<string, Zone> verzeichnis;
        public static IDictionary<string, Zone> Zonenverzeichnis { get { return Zone.verzeichnis; } }

        static Zone()
        {
            Zone.verzeichnis = new Dictionary<string, Zone>();
            Zone.verzeichnis.Add(zoneA.Kennzeichen, zoneA);
            Zone.verzeichnis.Add(zoneB.Kennzeichen, zoneB);
            Zone.verzeichnis.Add(zoneC.Kennzeichen, zoneC);

            Zone.sortierteListe = new List<Zone> { zoneA, zoneB, zoneC, zoneAlle};
        }

        private static List<Zone> sortierteListe;
        public static List<Zone> Zonenliste { get { return sortierteListe; } }
    }
}

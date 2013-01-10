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
using System.Xml.Serialization;

namespace AbfallkalenderBisamberg2011WP7
{
    public class Entsorgung
    {
        private List<Zone> fuerZonen;

        private static List<Zone> fuerAlleZonen = new List<Zone>() { Zone.Alle };

        public List<Zone> Entsorgungszonen { 
            get {
                if (this.fuerZonen != null)
                    return this.fuerZonen;
                else
                {
                    return Entsorgung.fuerAlleZonen;
                }
            } 
        }

        [XmlAttribute]
        public string Zonen
        {
            get 
            {
                if (fuerZonen == null || fuerZonen.Count < 1)
                    return Zone.Alle.Kennzeichen;
                else
                {
                    string result = "";

                    for (int i = 0; i < this.fuerZonen.Count; i++)
                    {
                        result += this.fuerZonen[i].Kennzeichen;
                        if (i < (this.fuerZonen.Count - 1))
                            result += "/";
                    }
                     
                    return result;
                }
            }

            set {
                if (value != null)
                {
                    string[] zonenKzs = value.Split(',');
                    if (this.fuerZonen == null)
                        this.fuerZonen = new List<Zone>();
                    else
                        this.fuerZonen.Clear();
                    foreach (var item in zonenKzs)
                    {
                        this.fuerZonen.Add(Zone.Zonenverzeichnis[item]);
                    }
                }
                else
                    this.fuerZonen = null;
            }
        }

        private DateTime tagesdatum;
        [XmlAttribute]
        public DateTime Datum
        {
            get { return tagesdatum; }
            set { tagesdatum = value; }
        }

        private Abfallart abfallart;
        [XmlAttribute]
        public string Abfall
        {
            get
            {
                if (this.abfallart != null)
                    return this.abfallart.Name;
                else
                    return "";
            }
            set
            {
                if (value != null)
                {
                    this.abfallart = Abfallart.Abfallartenverzeichnis[value];
                }
            }
        }

        public bool istAbfallart(Abfallart abfallart)
        {
            bool result = false;

            if (this.abfallart != null && abfallart != null)
            {
                result = this.abfallart == abfallart;
            }

            return result;
        }

        public Abfallart Art { get { return this.abfallart; } }

        public bool istFuerZone(Zone zone)
        {
            if (zone.IstFuerAlle)
                return this.Entsorgungszonen.Contains(Zone.Alle);
            else
            {
                return this.Entsorgungszonen.Contains(zone) || this.Entsorgungszonen.Contains(Zone.Alle);
            }
        }
    }
}

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
using System.Xml.Serialization;
using System.Collections.Generic;

namespace AbfallkalenderBisamberg2011WP7
{
    
    public class Abfallkalender
    {
        private Abfallzentrum abfallsammelzentrum;

        [XmlElement]
        public Abfallzentrum Abfallsammelzentrum
        {
            get { return abfallsammelzentrum; }
            set { abfallsammelzentrum = value; }
        }

        private List<Feiertag> feiertage;
        [XmlElement]
        public List<Feiertag> Feiertag
        {
            get { return feiertage; }
            set { feiertage = value; }
        }

        private List<Entsorgung> entsorgungen;
        [XmlElement]
        public List<Entsorgung> Entsorgung
        {
            get { return entsorgungen; }
            set { entsorgungen = value; }
        }
        
    }

    public class Abfallzentrum
    {
        private List<AbfallzentrumOeffnungszeit> oeffnungszeiten;

        [XmlElement]
        public List<AbfallzentrumOeffnungszeit> Oeffnungszeiten
        {
            get { return oeffnungszeiten; }
            set { oeffnungszeiten = value; }
        }
        
    }

    public class AbfallzentrumOeffnungszeit
    {
        private DateTime tagesDatum;

        [XmlAttribute]
        public DateTime GeoeffnetAm
        {
            get { return tagesDatum; }
            set { tagesDatum = value; }
        }
        
    }

    public class Feiertag
    {
        private DateTime datum;

        [XmlAttribute]
        public DateTime Datum
        {
            get { return datum; }
            set { datum = value; }
        }

        private string bezeichnung;

        [XmlAttribute]
        public string Name
        {
            get { return bezeichnung; }
            set { bezeichnung = value; }
        }

        private bool istArbeitsfrei = true;

        [XmlAttribute]
        public bool Arbeitsfrei
        {
            get { return istArbeitsfrei; }
            set { istArbeitsfrei = value; }
        }

    }
}

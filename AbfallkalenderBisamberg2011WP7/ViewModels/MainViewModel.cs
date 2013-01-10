using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using System.IO;

namespace AbfallkalenderBisamberg2011WP7
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private DateTime startZeitpunkt;

        private List<string> monatsnamen = new List<string> { "Jänner", "Februar", "März", "April", "Mai", "Juni", "Juli", "August", "September", "Oktober", "November", "Dezember" };

        public MainViewModel()
        {
            this.startZeitpunkt = DateTime.Now;

            this.Items = new ObservableCollection<ItemViewModel>();

            this.SetupKalender(2013);
        }

        private void SetupKalender(int jahr)
        {
            this.monatsEintrage = new List<ObservableCollection<TageseintraegeViewModel>>();

            DateTime start = new DateTime(jahr, 1, 1);
            while (start.Year == jahr)
            {
                ObservableCollection<TageseintraegeViewModel> eintraege = this.monatsEintrage.Count == start.Month ? this.monatsEintrage[start.Month-1] : null;
                if (eintraege == null)
                {
                    eintraege = new ObservableCollection<TageseintraegeViewModel>();
                    this.monatsEintrage.Add(eintraege);
                }

                eintraege.Add(new TageseintraegeViewModel(start));

                start = start.AddDays(1);
            }

            this.sortierteEintraege = new List<ObservableCollection<TageseintraegeViewModel>>();
            for (int i = 1; i <= 12; i++ )
            {
                this.sortierteEintraege.Add(this.monatsEintrage[this.IndexForMonat(i)]);
            }
        }

        private List<ObservableCollection<TageseintraegeViewModel>> monatsEintrage;
        public List<ObservableCollection<TageseintraegeViewModel>> Eintraege { get { return this.monatsEintrage; } }

        private List<ObservableCollection<TageseintraegeViewModel>> sortierteEintraege;
        public List<ObservableCollection<TageseintraegeViewModel>> Monate { get { return this.sortierteEintraege; } }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<ItemViewModel> Items { get; private set; }

        public bool IsDataLoaded
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public void LoadData()
        {
            // Sample data; replace with real data
            this.Items.Add(new ItemViewModel() { Abfall = Abfallart.Restmüll, LineTwo = "Maecenas praesent accumsan bibendum" });
            this.Items.Add(new ItemViewModel() { Abfall = Abfallart.Restmüll14tg, LineTwo = "Dictumst eleifend facilisi faucibus" });
            this.Items.Add(new ItemViewModel() { Abfall = Abfallart.Biomüll, LineTwo = "Habitant inceptos interdum lobortis" });
            this.Items.Add(new ItemViewModel() { Abfall = Abfallart.GelberSack, LineTwo = "Nascetur pharetra placerat pulvinar" });
            this.Items.Add(new ItemViewModel() { Abfall = Abfallart.Altpapier, LineTwo = "Maecenas praesent accumsan bibendum" });
            this.Items.Add(new ItemViewModel() { Abfall = Abfallart.Sperrmüll, LineTwo = "Dictumst eleifend facilisi faucibus" });

            Stream file = App.GetResourceStream(new Uri("Data/AbfallkalenderBisamberg2013.xml", UriKind.Relative)).Stream;
            XmlSerializer serializer = new XmlSerializer(typeof(Abfallkalender));
            Abfallkalender kalender = serializer.Deserialize(file) as Abfallkalender;

            if (kalender != null)
            {
                foreach (AbfallzentrumOeffnungszeit oeffn in kalender.Abfallsammelzentrum.Oeffnungszeiten)
                {
                    TageseintraegeViewModel eintrag = this.Tageseintrag(oeffn.GeoeffnetAm);
                    if (eintrag != null)
                        eintrag.Tagesinfo = "asz";
                }

                foreach (Feiertag feiertag in kalender.Feiertag)
                {
                    TageseintraegeViewModel eintrag = this.Tageseintrag(feiertag.Datum);
                    if (eintrag != null)
                        eintrag.Feiertag = feiertag;
                }

                foreach (Entsorgung entsorgung in kalender.Entsorgung)
                {
                    TageseintraegeViewModel eintrag = this.Tageseintrag(entsorgung.Datum);
                    if (eintrag != null)
                        eintrag.Entsorgungen.Add(entsorgung);
                }
            }

            this.FindeNaechsteEntsorgungen();
            this.AnzeigeFuerNaechsteEntsorgungenAktualisieren();

            this.IsDataLoaded = true;
        }

        private void AnzeigeFuerNaechsteEntsorgungenAktualisieren()
        {
            this.NotifyPropertyChanged("NaechsterRestmuell");
            this.NotifyPropertyChanged("NaechsterRestmuell14tg");
            this.NotifyPropertyChanged("NaechsterBiomuell");
            this.NotifyPropertyChanged("NaechsterGelberSack");
            this.NotifyPropertyChanged("NaechsterAltpapier");
            this.NotifyPropertyChanged("NaechsterSperrmuell");
        }

        private void AnzeigenFuerEntsorgungenAktualisieren()
        {
            foreach (ObservableCollection<TageseintraegeViewModel> monat in this.monatsEintrage)
            {
                foreach (TageseintraegeViewModel tagesVM in monat)
                {
                    tagesVM.ErzwingeInhaltsaktualisierung();
                }
            }
        }

        public TageseintraegeViewModel Tageseintrag(DateTime fuerDatum)
        {
            if (fuerDatum != null)
                return this.monatsEintrage[fuerDatum.Month - 1][fuerDatum.Day - 1];
            else
                return null;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public string ErsterMonatsname { get { return this.monatsnamen[this.IndexForMonat(1)]; } }
        public string ZweiterMonatsname { get { return this.monatsnamen[this.IndexForMonat(2)]; } }
        public string DritterMonatsname { get { return this.monatsnamen[this.IndexForMonat(3)]; } }
        public string VierterMonatsname { get { return this.monatsnamen[this.IndexForMonat(4)]; } }
        public string FuenfterMonatsname { get { return this.monatsnamen[this.IndexForMonat(5)]; } }
        public string SechsterMonatsname { get { return this.monatsnamen[this.IndexForMonat(6)]; } }
        public string SiebterMonatsname { get { return this.monatsnamen[this.IndexForMonat(7)]; } }
        public string AchterMonatsname { get { return this.monatsnamen[this.IndexForMonat(8)]; } }
        public string NeunterMonatsname { get { return this.monatsnamen[this.IndexForMonat(9)]; } }
        public string ZehnterMonatsname { get { return this.monatsnamen[this.IndexForMonat(10)]; } }
        public string ElfterMonatsname { get { return this.monatsnamen[this.IndexForMonat(11)]; } }
        public string ZwoelfterMonatsname { get { return this.monatsnamen[this.IndexForMonat(12)]; } }

        private int IndexForMonat(int anzeigeIndex)
        {
            if (this.startZeitpunkt.Year != 2012)
            {
                return anzeigeIndex - 1;
            }
            else
            {
                return (anzeigeIndex-1 + this.startZeitpunkt.Month-1) % 12;
            }
        }

        private Dictionary<Abfallart, Dictionary<Zone, DateTime>> naechsteEntsorgungen;

        private void FindeNaechsteEntsorgungen()
        {
            this.naechsteEntsorgungen = new Dictionary<Abfallart, Dictionary<Zone, DateTime>>();

            foreach (Abfallart abfall in Abfallart.Abfallartenverzeichnis.Values)
            {
                this.naechsteEntsorgungen.Add(abfall, new Dictionary<Zone,DateTime>());
            }

            DateTime heute = DateTime.Now;

            if (heute.Year == 2013)
            {
                DateTime fuerTag = heute;
                while (fuerTag.Year == 2013)
                {
                    TageseintraegeViewModel eintraege = this.Tageseintrag(fuerTag);
                    foreach (Entsorgung entsorgung in eintraege.Entsorgungen)
                    {
                        Dictionary<Zone, DateTime> zonenDaten = this.naechsteEntsorgungen[entsorgung.Art];
                        List<Zone> entsorgungszonen = entsorgung.Entsorgungszonen;
                        foreach (Zone zone in entsorgungszonen)
                        {
                            if (!zonenDaten.ContainsKey(zone))
                            {
                                zonenDaten.Add(zone, entsorgung.Datum);
                            }

                            if (zone.IstFuerAlle)
                            {
                                foreach (Zone zonenlisteneintrag in Zone.Zonenliste)
                                {
                                    if (!zonenlisteneintrag.IstFuerAlle)
                                    {
                                        if (!zonenDaten.ContainsKey(zonenlisteneintrag))
                                        {
                                            zonenDaten.Add(zonenlisteneintrag, entsorgung.Datum);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    fuerTag = fuerTag.AddDays(1.0);
                }
            }
        }

        public string NaechsterRestmuell { get { return this.NaechsteEntsorgung(Abfallart.Restmüll); } }
        public string NaechsterRestmuell14tg { get { return this.NaechsteEntsorgung(Abfallart.Restmüll14tg); } }
        public string NaechsterBiomuell { get { return this.NaechsteEntsorgung(Abfallart.Biomüll); } }
        public string NaechsterGelberSack { get { return this.NaechsteEntsorgung(Abfallart.GelberSack); } }
        public string NaechsterAltpapier { get { return this.NaechsteEntsorgung(Abfallart.Altpapier); } }
        public string NaechsterSperrmuell { get { return this.NaechsteEntsorgung(Abfallart.Sperrmüll); } }

        private string NaechsteEntsorgung(Abfallart fuerAbfallart)
        {
            string result = null;

            if (this.naechsteEntsorgungen != null && this.naechsteEntsorgungen.ContainsKey(fuerAbfallart))
            {
                Dictionary<Zone, DateTime> zonenDaten = this.FindeZonedatenNaechsteEntsorgung(fuerAbfallart);

                if (!this.NaechsteEntsorgungFuerAlleZonen(zonenDaten))
                {
                    foreach (Zone zonenlisteneintrag in Zone.Zonenliste)
                    {
                        if (!zonenlisteneintrag.IstFuerAlle && zonenDaten.ContainsKey(zonenlisteneintrag))
                        {
                            if (result != null && result.Length > 0)
                                result += " ";

                            result += zonenlisteneintrag.Kennzeichen + ": " + String.Format("{0:d}", zonenDaten[zonenlisteneintrag]);
                        }
                    }
                }
                else
                {
                    var enumer = zonenDaten.Values.GetEnumerator();
                    enumer.MoveNext();

                    result = String.Format("{0:d}", enumer.Current);
                }
            }

            return result;
        }

        private Dictionary<Zone, DateTime> FindeZonedatenNaechsteEntsorgung(Abfallart fuerAbfallart)
        {
            Dictionary<Zone, DateTime> zonenDaten = this.naechsteEntsorgungen[fuerAbfallart];

            if (zonenDaten != null && !ZonenEinschraenkung.IstFuerAlle)
            {
                Dictionary<Zone, DateTime> zuzeigendeZoneDaten = new Dictionary<Zone, DateTime>();
                if (zonenDaten.ContainsKey(ZonenEinschraenkung))
                    zuzeigendeZoneDaten.Add(ZonenEinschraenkung, zonenDaten[ZonenEinschraenkung]);

                zonenDaten = zuzeigendeZoneDaten;
            }

            return zonenDaten;
        }

        private bool NaechsteEntsorgungFuerAlleZonen(Dictionary<Zone, DateTime> zonenDaten)
        {
            bool result = false;

            if (zonenDaten != null && zonenDaten.Count > 0)
            {
                if (zonenDaten.Count == 1)
                    result = true;
                else
                {
                    DateTime lastValue = DateTime.MinValue;
                    bool isFirst = true;
                    result = true;

                    foreach (KeyValuePair<Zone, DateTime> kvp in zonenDaten)
                    {
                        if (isFirst)
                        {
                            lastValue = kvp.Value;
                            isFirst = false;
                        }
                        else
                        {
                            if (kvp.Value.CompareTo(lastValue) != 0)
                            {
                                result = false;
                                break;
                            }
                        }
                    }
                }
            }

            return result;
        }

        public static Zone ZonenEinschraenkung = Zone.Alle;

        public static void EinschraenkenAufZone(Zone neueZone)
        {
            if (neueZone != ZonenEinschraenkung)
            {
                ZonenEinschraenkung = neueZone;

                App.ViewModel.AnzeigeFuerNaechsteEntsorgungenAktualisieren();
                App.ViewModel.AnzeigenFuerEntsorgungenAktualisieren();
            }
        }
    }
}
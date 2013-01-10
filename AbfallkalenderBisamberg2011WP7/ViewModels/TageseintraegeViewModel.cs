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
using System.ComponentModel;
using System.Collections.Generic;

namespace AbfallkalenderBisamberg2011WP7
{
    public class TageseintraegeViewModel : INotifyPropertyChanged
    {
        private DateTime tagesdatum;

        private static Brush sonntagsfarbe = new SolidColorBrush(Colors.DarkGray);
        private static Brush feiertagsfarbe = new SolidColorBrush(Colors.Blue);
        private static Brush restmuellfarbe = new SolidColorBrush(Colors.DarkGray);

        private static Brush schwarzeSchriftfarbe = new SolidColorBrush(Colors.Black);
        private static Brush weisseSchriftfarbe = new SolidColorBrush(Colors.White);

        private static Thickness noPadding = new Thickness();
        private static Thickness stdPadding = new Thickness(4.0, 0.0, 4.0, 0.0);

        public TageseintraegeViewModel(DateTime fuerDatum)
        {
            this.tagesdatum = fuerDatum;
        }

        public DateTime Datum
        {
            get { return tagesdatum; }
            set { tagesdatum = value; }
        }

        public int Tag
        {
            get { return this.tagesdatum.Day; }
        }

        public string WochentagKurzform
        {
            get { return String.Format("{0:ddd}", this.tagesdatum); }
        }

        public Brush Hintergrundfarbe
        {
            get
            {
                if (this.tagesdatum.DayOfWeek == DayOfWeek.Sunday)
                    return sonntagsfarbe;
                else if (this.Feiertag != null && this.Feiertag.Arbeitsfrei)
                {
                    return feiertagsfarbe;
                } else
                    return null;
            }
        }

        public Brush HintergrundfarbeFuerErstenBlock
        {
            get
            {
                Brush result = this.Hintergrundfarbe;
                if (result == null)
                    result = restmuellfarbe;

                return result;
            }
        }

        public Brush VordergrundfarbeFuerErstenBlock
        {
            get
            {
                if (this.entsorgungen.Count > 0)
                    return schwarzeSchriftfarbe;
                else
                {
                    return weisseSchriftfarbe;
                }
            }
        }

        public Thickness PaddingFuerErstenBlock
        {
            get
            {
                if (this.Feiertag == null && this.HatRestmuelleintrag())
                    return stdPadding;
                else
                    return noPadding;
            }
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

        private string kommentar;

        public string Tagesinfo
        {
            get { return kommentar; }
            set { kommentar = value; }
        }
        
        public Brush Tagesinfofarbe
        {
            get
            {
                if (this.Tagesinfo != null)
                    return new SolidColorBrush(Colors.Red);
                else
                    return null;
            }
        }

        private Feiertag feiertag;

        public Feiertag Feiertag
        {
            get { return feiertag; }
            set { feiertag = value; }
        }
        
        public string TextFuerErstenBlock
        {
            get
            {
                if (this.Feiertag == null)
                {
                    return this.Restmuell();
                }
                else
                    return this.Feiertag.Name;
            }
        }

        private string Restmuell()
        {
            Entsorgung restmuellEntsorgung = this.findeErsteEntsorgung(Abfallart.Restmüll);
            if (restmuellEntsorgung != null)
            {
                string zonen = restmuellEntsorgung.Zonen;
                return Abfallart.Restmüll.Name + this.ErzeugeZonenstring(zonen);
            } else
                return null;
        }

        private string ErzeugeZonenstring(string zonen)
        {
            return ((zonen.Length > 0 && MainViewModel.ZonenEinschraenkung.IstFuerAlle) ? " " + zonen : "");
        }

        private bool HatRestmuelleintrag()
        {
            return this.findeErsteEntsorgung(Abfallart.Restmüll) != null;
        }

        private Entsorgung findeErsteEntsorgung(Abfallart abfallart)
        {
            Entsorgung result = null;

            foreach (var einzelEntsorgung in this.entsorgungen)
            {
                if (einzelEntsorgung.istAbfallart(abfallart))
                {
                    if (MainViewModel.ZonenEinschraenkung.IstFuerAlle)
                    {
                        result = einzelEntsorgung;
                        break;
                    }
                    else
                    {
                        if (einzelEntsorgung.istFuerZone(MainViewModel.ZonenEinschraenkung))
                        {
                            result = einzelEntsorgung;
                            break;
                        }
                    }
                }
            }

            return result;
        }

        public string TextFuerZweitenBlock
        {
            get
            {
                if (this.Feiertag == null)
                {
                    return this.Restmuell14tg();
                }
                else
                    return null;
            }
        }

        private string Restmuell14tg()
        {
            Entsorgung restmuellEntsorgung = this.findeErsteEntsorgung(Abfallart.Restmüll14tg);
            if (restmuellEntsorgung != null)
            {
                string zonen = restmuellEntsorgung.Zonen;
                return Abfallart.Restmüll14tg.Name + this.ErzeugeZonenstring(zonen);
            }
            else
                return null;
        }

        public Thickness PaddingFuerZweitenBlock
        {
            get
            {
                if (this.Feiertag == null && this.HatRestmuell14tgeintrag())
                    return stdPadding;
                else
                    return noPadding;
            }
        }

        private bool HatRestmuell14tgeintrag()
        {
            return this.findeErsteEntsorgung(Abfallart.Restmüll14tg) != null;
        }

        public string TextFuerDrittenBlock
        {
            get
            {
                if (this.Feiertag == null)
                {
                    return this.Biomuell();
                }
                else
                    return null;
            }
        }

        private string Biomuell()
        {
            Entsorgung biomuellEntsorgung = this.findeErsteEntsorgung(Abfallart.Biomüll);
            if (biomuellEntsorgung != null)
            {
                string zonen = biomuellEntsorgung.Zonen;
                return Abfallart.Biomüll.Name + this.ErzeugeZonenstring(zonen);
            }
            else
                return null;
        }

        public Thickness PaddingFuerDrittenBlock
        {
            get
            {
                if (this.Feiertag == null && this.HatBiomuelleintrag())
                    return stdPadding;
                else
                    return noPadding;
            }
        }

        private bool HatBiomuelleintrag()
        {
            return this.findeErsteEntsorgung(Abfallart.Biomüll) != null;
        }

        public string TextFuerViertenBlock
        {
            get
            {
                if (this.Feiertag == null)
                {
                    return this.GelberSack();
                }
                else
                    return null;
            }
        }

        private string GelberSack()
        {
            Entsorgung gelbeSackEntsorgung = this.findeErsteEntsorgung(Abfallart.GelberSack);
            if (gelbeSackEntsorgung != null)
            {
                string zonen = gelbeSackEntsorgung.Zonen;
                return Abfallart.GelberSack.Name + this.ErzeugeZonenstring(zonen);
            }
            else
                return null;
        }

        public Thickness PaddingFuerViertenBlock
        {
            get
            {
                if (this.Feiertag == null && this.HatGelberSackeintrag())
                    return stdPadding;
                else
                    return noPadding;
            }
        }

        private bool HatGelberSackeintrag()
        {
            return this.findeErsteEntsorgung(Abfallart.GelberSack) != null;
        }

        public string TextFuerFuenftenBlock
        {
            get
            {
                if (this.Feiertag == null)
                {
                    return this.Altpapier();
                }
                else
                    return null;
            }
        }

        private string Altpapier()
        {
            Entsorgung altpapierEntsorgung = this.findeErsteEntsorgung(Abfallart.Altpapier);
            if (altpapierEntsorgung != null)
            {
                string zonen = altpapierEntsorgung.Zonen;
                return Abfallart.Altpapier.Name + this.ErzeugeZonenstring(zonen);
            }
            else
                return null;
        }

        public Thickness PaddingFuerFuenftenBlock
        {
            get
            {
                if (this.Feiertag == null && this.HatAltpapiereintrag())
                    return stdPadding;
                else
                    return noPadding;
            }
        }

        private bool HatAltpapiereintrag()
        {
            return this.findeErsteEntsorgung(Abfallart.Altpapier) != null;
        }

        public string TextFuerFuerSechstenBlock
        {
            get
            {
                if (this.Feiertag == null)
                {
                    return this.Sperrmuell();
                }
                else
                    return null;
            }
        }

        private string Sperrmuell()
        {
            Entsorgung sperrmuellEntsorgung = this.findeErsteEntsorgung(Abfallart.Sperrmüll);
            if (sperrmuellEntsorgung != null)
            {
                string zonen = sperrmuellEntsorgung.Zonen;
                return Abfallart.Sperrmüll.Name + this.ErzeugeZonenstring(zonen);
            }
            else
                return null;
        }

        public Thickness PaddingFuerSechstenBlock
        {
            get
            {
                if (this.Feiertag == null && this.HatSperrmuelleintrag())
                    return stdPadding;
                else
                    return noPadding;
            }
        }

        private bool HatSperrmuelleintrag()
        {
            return this.findeErsteEntsorgung(Abfallart.Sperrmüll) != null;
        }

        private List<Entsorgung> entsorgungen = new List<Entsorgung>();

        public List<Entsorgung> Entsorgungen { get { return this.entsorgungen; } }

        public Thickness MarginFuerEintraege()
        {
            if (this.entsorgungen.Count > 0)
                return new Thickness() { Left = 6, Top = 0, Right = 0, Bottom = 2 };
            else
                return new Thickness();
        }

        public void ErzwingeInhaltsaktualisierung()
        {
            this.NotifyPropertyChanged("TextFuerErstenBlock");
            this.NotifyPropertyChanged("PaddingFuerErstenBlock");
            this.NotifyPropertyChanged("TextFuerZweitenBlock");
            this.NotifyPropertyChanged("PaddingFuerZweitenBlock");
            this.NotifyPropertyChanged("TextFuerDrittenBlock");
            this.NotifyPropertyChanged("PaddingFuerDrittenBlock");
            this.NotifyPropertyChanged("TextFuerViertenBlock");
            this.NotifyPropertyChanged("PaddingFuerViertenBlock");
            this.NotifyPropertyChanged("TextFuerFuenftenBlock");
            this.NotifyPropertyChanged("PaddingFuerFuenftenBlock");
            this.NotifyPropertyChanged("TextFuerSechstenBlock");
            this.NotifyPropertyChanged("PaddingFuerSechstenBlock");
        }
    }
}

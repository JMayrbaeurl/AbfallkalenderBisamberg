using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace AbfallkalenderBisamberg2011WP7
{
    public partial class SettingsPage : PhoneApplicationPage
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (MainViewModel.ZonenEinschraenkung == Zone.Alle)
                this.rbZoneAlle.IsChecked = true;
            else if (MainViewModel.ZonenEinschraenkung == Zone.A)
                this.rbZoneA.IsChecked = true;
            else if (MainViewModel.ZonenEinschraenkung == Zone.B)
                this.rbZoneB.IsChecked = true;
            else if (MainViewModel.ZonenEinschraenkung == Zone.C)
                this.rbZoneC.IsChecked = true;
        }

        private void OnRBZoneAlleClicked(object sender, RoutedEventArgs e)
        {
            MainViewModel.EinschraenkenAufZone(Zone.Alle);
        }

        private void OnRBZoneAClicked(object sender, RoutedEventArgs e)
        {
            MainViewModel.EinschraenkenAufZone(Zone.A);
        }

        private void OnRBZoneBClicked(object sender, RoutedEventArgs e)
        {
            MainViewModel.EinschraenkenAufZone(Zone.B);
        }

        private void OnRBZoneCClicked(object sender, RoutedEventArgs e)
        {
            MainViewModel.EinschraenkenAufZone(Zone.C);
        }
    }
}
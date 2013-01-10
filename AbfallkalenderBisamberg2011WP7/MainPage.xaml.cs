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
using Microsoft.Phone.Shell;
using System.Windows.Navigation;

namespace AbfallkalenderBisamberg2011WP7
{
    public partial class MainPage : PhoneApplicationPage
    {
        bool newPageInstance = false;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // If the constructor has been called, this is not a page that was already in memory.
            newPageInstance = true;

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        // Load data for the ViewModel Items
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs args)
        {
            PhoneApplicationService.Current.State["selectedPivot"] = this.KalenderPivot.SelectedIndex;

            base.OnNavigatedFrom(args);

            // Set newPageInstance back to false. It will be set back to true if the constructor is called again.
            newPageInstance = false;
        }

        // Fixing bug of Pivot control
        // See http://www.wintellect.com/CS/blogs/jprosise/archive/2011/02/02/tombstoning-pivot-controls-in-silverlight-for-windows-phone.aspx
        private void OnPivotControlLoaded(object sender, RoutedEventArgs e)
        {
            if (this.newPageInstance && PhoneApplicationService.Current.State.ContainsKey("selectedPivot"))
            {
                this.KalenderPivot.SelectedIndex = (int)PhoneApplicationService.Current.State["selectedPivot"];
            }
        }

        private void OnInfoAppbarButtonClick(object sender, EventArgs e)
        {
            // Navigate to the new page
            NavigationService.Navigate(new Uri("/InfoPage.xaml", UriKind.Relative));
        }

        private void OnSettingsAppbarButtonClick(object sender, EventArgs e)
        {
            // Navigate to the new page
            NavigationService.Navigate(new Uri("/SettingsPage.xaml", UriKind.Relative));
        }
    }
}
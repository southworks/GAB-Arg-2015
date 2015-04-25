namespace MeetPub.WP8
{
    using System;
    using System.Linq;

    using BarMeetUp.Models;

    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Navigation;

    using MeetPub.WP8.Common;

    public sealed partial class MainPage : Page
    {
        private readonly NavigationHelper navigationHelper;

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
            this.Loaded += this.MainPageLoaded;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        private async void MainPageLoaded(object sender, RoutedEventArgs e)
        {
            var pubs = await App.MobileService.GetTable<Pub>().OrderBy(p => p.Name).ToListAsync();

            this.DataContext = new { Pubs = pubs };
            this.TodayText.Text = string.Format(
                "Hoy {0}/{1}/{2}",
                DateTime.Today.Day,
                DateTime.Today.Month,
                DateTime.Today.Year);
        }

        private void ListViewBase_OnItemClick(object sender, ItemClickEventArgs e)
        {
            this.Frame.Navigate(typeof(DetailsPage));
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
            {
                return;
            }

            this.Frame.Navigate(typeof(DetailsPage), e.AddedItems.FirstOrDefault());
        }

        private async void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            // TODO: Save the unique state of the page here.
        }
    }
}

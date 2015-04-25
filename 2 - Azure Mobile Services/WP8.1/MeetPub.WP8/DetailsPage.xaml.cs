namespace MeetPub.WP8
{
    using System;
    using System.Threading.Tasks;
    using BarMeetUp.Models;
    using Common;
    using Windows.Phone.UI.Input;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Navigation;

    using MeetPub.WP8.Models;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DetailsPage : Page
    {
        private readonly NavigationHelper navigationHelper;

        public DetailsPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var pub = e.Parameter as Pub;
            if (pub == null)
            {
                return;
            }

            this.DataContext = pub;
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        private async void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            // TODO: Save the unique state of the page here.
        }

        private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                await App.MobileService.GetTable<Assistance>().InsertAsync(new Assistance()
                {
                    Date = DateTime.Today,
                    PubID = ((Pub)this.DataContext).Id,
                });

                Frame.Navigate(typeof(MainPage));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

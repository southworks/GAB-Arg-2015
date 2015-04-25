namespace MeetPub.WP8
{
    using System;
    using System.Diagnostics;

    using MeetPub.WP8.Common;

    using Microsoft.WindowsAzure.MobileServices;

    using Windows.UI.Popups;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Navigation;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Login
    {
        private readonly NavigationHelper navigationHelper;

        private readonly ObservableDictionary defaultViewModel = new ObservableDictionary();

        public Login()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Gets the view model for this <see cref="Page"/>.
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        private async void LoginFacebookClick(object sender, RoutedEventArgs e)
        {
            var error = false;
            this.LoginFacebookButton.IsEnabled = false;
            this.LoginProgress.IsActive = true;
            try
            {
                var user = await App.MobileService.LoginAsync(MobileServiceAuthenticationProvider.WindowsAzureActiveDirectory);
                if (user == null)
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                error = true;
                Debug.WriteLine(ex);
            }

            if (error)
            {
                var dialog =
                    new MessageDialog(
                        "An error has occurred while trying to login with Facebook. Please retry.",
                        "Authentication Failed");
                await dialog.ShowAsync();
                this.LoginFacebookButton.IsEnabled = true;
            }
            else
            {
                App.RefreshSettings();
                this.Frame.Navigate(typeof(MainPage));
            }

            this.LoginProgress.IsActive = false;
        }
    }
}

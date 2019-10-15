using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VrpdScanner
{
    public partial class App : Application
    {
        public App()
        {
            //InitializeComponent();

            Button b = new Button() { Text = "Open scan page" };
            b.Clicked += async (sender, e) =>
            {
                await Current.MainPage.Navigation.PushAsync(new ScanPage());
            };
            // The root page of your application
            ContentPage content = new ContentPage
            {
                Title = "TestZXing",
                Content = new StackLayout
                {
                    VerticalOptions = LayoutOptions.Center,
                    Children = {
                        new Label {
                            HorizontalTextAlignment = TextAlignment.Center,
                            Text = "Welcome to Xamarin Forms!"
                        }, b
                    }
                }
            };
            MainPage = new NavigationPage(content);
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }
    }
}


using Xamarin.Forms;

namespace WordGame
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new LoadingPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            MainPage = new LoadingPage();
        }
    }
}

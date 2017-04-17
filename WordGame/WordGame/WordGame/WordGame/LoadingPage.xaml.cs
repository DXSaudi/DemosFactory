using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WordGame
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoadingPage : ContentPage
    {
        public LoadingPage()
        {
            InitializeComponent();
            
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (!CrossConnectivity.Current.IsConnected)
            {
                mainLabel.Text = ":( There is no connection";
            }
            else
            {
                await GetCountries();
            }
        }

        protected async Task GetCountries()
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync("http://services.groupkt.com/country/get/all"))
                {
                    using (HttpContent content = response.Content)
                    {
                        string stringContent = await content.ReadAsStringAsync();
                        var jsonResponse = JObject.Parse(stringContent);
                        var restResponse = JObject.Parse(jsonResponse["RestResponse"].ToString());
                        List<Country> countries = JsonConvert.DeserializeObject<List<Country>>(restResponse["result"].ToString());
                        List<String> countriesNames = new List<string>();
                        foreach (Country c in countries)
                        {
                            countriesNames.Add(c.name.ToLower());
                        }
                        var mainPage = new NavigationPage(new MainPage());
                        NavigationPage.SetHasNavigationBar(mainPage, false);
                        App.Current.MainPage = new MainPage(countriesNames);
                    }
                }
            }
        }



    }
}

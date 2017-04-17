using System;
using Xamarin.Forms;
using System.Net.Http;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WordGame
{
    public partial class MainPage : ContentPage
    {
        static int score=0;
        List<string> countrynames;
        List<string> suggestedCorrections;
        List<Answer> answers=new List<Answer>();

        public MainPage()
        {
            InitializeComponent();
            nextBtn.Clicked += NextBtn_Clicked;
        }

        public MainPage(List<string> retrievedCountries)
        {
            InitializeComponent();
            nextBtn.Clicked += NextBtn_Clicked;
            countrynames = retrievedCountries;
        }

        protected override async void OnAppearing()
        {
            int randomNumber = GenerateRandom(0, countrynames.Count());
            string selected = countrynames.ElementAt(randomNumber);
            letterLabel.Text = selected.ToCharArray().ElementAt(0).ToString().ToUpper();
            await progress.ProgressTo(1, 60000, Easing.Linear);
            App.Current.MainPage = new ResultPage(answers,score);
        }

        private async void NextBtn_Clicked(object sender, EventArgs e)
        {
            int index;

            if (userEntry.Text == null) { 
                System.Diagnostics.Debug.WriteLine("Empty answer");
            }else if ( !userEntry.Text.ToLower().StartsWith(letterLabel.Text.ToLower()) ){
                answers.Add(new Answer { answer = userEntry.Text, result = "Incorrect" });
            }else {
                // 1.1- check if it is one of countrynames elements, add score, store the Answers, remove element from list
                string userInput = userEntry.Text.Trim().ToLower();
                index = countrynames.IndexOf(userInput);
                if (index != -1){
                    score += 2;
                    answers.Add(new Answer { answer = userEntry.Text, result = "Correct" });
                    countrynames.RemoveAt(index);
                }else{
                    // 1.2- check if it is misspelled, add score, add to Answers, remove element from list
                    await MakeRequest(userInput);
                    bool misspelled = false;
                    foreach (string suggestion in suggestedCorrections) {
                        index = countrynames.IndexOf(suggestion);
                        if (index != -1){
                            misspelled = true;
                            score++;
                            answers.Add(new Answer { answer = userEntry.Text, result = "Misspelled"});
                            countrynames.RemoveAt(index);
                            break;
                        }
                    }
                    if (!misspelled){
                        // 1.3- Same first letter but does not match any country name
                        answers.Add(new Answer { answer = userEntry.Text, result = "Incorrect" });
                    }
                }
            }

            int randomNumber = GenerateRandom(0, countrynames.Count());
            string selected = countrynames.ElementAt(randomNumber);
            letterLabel.Text = selected.ToCharArray().ElementAt(0).ToString().ToUpper();
            System.Diagnostics.Debug.WriteLine("Score: "+score);
            userEntry.Text = "";
        }
        
        protected async Task MakeRequest(string userEntryText)
        {
            
            // Request parameters
            IEnumerable<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>()
            {
            new KeyValuePair<string,string>("mode","spell"),
            new KeyValuePair<string,string>("text",userEntryText)
            };

            HttpContent httpContent = new FormUrlEncodedContent(parameters);
            using (HttpClient client = new HttpClient())
            {
                
                // Request headers
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "{Type your key here}");
                var uri = "https://api.cognitive.microsoft.com/bing/v5.0/spellcheck/?";
                using (HttpResponseMessage response = await client.PostAsync(uri, httpContent))
                {
                    using (HttpContent content = response.Content)
                    {
                        string mycontent = await content.ReadAsStringAsync();
                        var jsonResponse = JObject.Parse(mycontent);
                        List<FlaggedTokens> RTVs = JsonConvert.DeserializeObject<List<FlaggedTokens>>(jsonResponse["flaggedTokens"].ToString());
                        suggestedCorrections = new List<string>();
                        foreach (FlaggedTokens obj in RTVs)
                        {
                            foreach (Suggestions sugg in obj.suggestions)
                            {
                                suggestedCorrections.Add(userEntryText.Replace(obj.token, sugg.suggestion));
                            }
                        }
                    }
                }
            }
        }

        public int GenerateRandom(int min, int max)
        {
            var seed = Convert.ToInt32(Regex.Match(Guid.NewGuid().ToString(), @"\d+").Value);
            return new Random(seed).Next(min, max);
        }

        public class FlaggedTokens
        {
            public string offset { get; set; }
            public string token { get; set; }
            public string type { get; set; }
            public List<Suggestions> suggestions { get; set; }
        }

        public class Suggestions
        {
            public string suggestion { get; set; }
            public string score { get; set; }
        }

    }
}

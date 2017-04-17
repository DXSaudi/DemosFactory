using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WordGame
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResultPage : ContentPage
    {
        List<Answer> globalAnswers;
        public ResultPage()
        {
            InitializeComponent();
        }

        public ResultPage(List<Answer> answers, int score)
        {
            InitializeComponent();
            scoreLabel.Text = "Your score is "+score+" pts";
            globalAnswers = answers;
            answersList.ItemsSource = globalAnswers;
        }

    }


    
}

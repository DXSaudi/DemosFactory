
using ServiceHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Email;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace IntelligentKioskSample.Areas
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EmotionsGamePage : Page
    {

        public EmotionsGamePage()
        {
            this.InitializeComponent();

            this.cameraControl.ImageCaptured += CameraControl_ImageCaptured;
            this.cameraControl.CameraRestarted += CameraControl_CameraRestarted;
            imageFromCameraWithFaces.OnFailure += ImageFromCameraWithFaces_OnFailure;
            imageFromCameraWithFaces.OnSuccess += ImageFromCameraWithFaces_OnSuccess;
        }



        private bool IsPlaying = false;

        private DispatcherTimer facesTimer;
        private DispatcherTimer restartTimer;
        private DispatcherTimer gameTimer;

        private Random RandomProvider;
        private int NumOfFaces = 2;


        private async void CameraControl_CameraRestarted(object sender, EventArgs e)
        {
            // We induce a delay here to give the camera some time to start rendering before we hide the last captured photo.
            // This avoids a black flash.
            await Task.Delay(500);

            this.imageFromCameraWithFaces.Visibility = Visibility.Collapsed;
        }

        private async void CameraControl_ImageCaptured(object sender, ImageAnalyzer e)
        {
            this.imageFromCameraWithFaces.DataContext = e;
            this.imageFromCameraWithFaces.Visibility = Visibility.Visible;

            await this.cameraControl.StopStreamAsync();
        }

        private void ImageFromCameraWithFaces_OnSuccess(object sender, bool e)
        {
            IsPlaying = false;
            RestartButton.Visibility = Visibility.Visible;
            EmailButton.Visibility = Visibility.Visible;

            restartTimer = new DispatcherTimer { Interval = new TimeSpan(0, 1, 0) };
            restartTimer.Tick += (w, x) =>
            {

                if (IsPlaying) return;

                restartTimer?.Stop();
                Restart("RestartTimer Tick event - Subed by OnSuccess");

            };
            restartTimer.Start();
        }

        private void ImageFromCameraWithFaces_OnFailure(object sender, bool e)
        {
            IsPlaying = false;

            restartTimer?.Stop();

            RestartButton.Visibility = Visibility.Visible;
            EmailButton.Visibility = Visibility.Collapsed;

            Restart("OnFailure event - Direct Call");

        }

        protected override async void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            await this.cameraControl.StopStreamAsync();
            base.OnNavigatingFrom(e);
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await this.StartWebCameraAsync();
            base.OnNavigatedTo(e);

        }

        private async void OnWebCamButtonClicked(object sender, RoutedEventArgs e)
        {
            await StartWebCameraAsync();
        }

        private async Task StartWebCameraAsync(bool restarted = false)
        {
            if (!IsPlaying)
            {


                try
                {
                    webCamHostGrid.Visibility = Visibility.Visible;
                    imageWithFacesControl.Visibility = Visibility.Collapsed;
                    RestartButton.Visibility = Visibility.Collapsed;

                    await this.cameraControl.StartStreamAsync(restarted);
                    await Task.Delay(250);
                    this.imageFromCameraWithFaces.Visibility = Visibility.Collapsed;

                    UpdateWebCamHostGridSize();

                    facesTimer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 2) };

                    facesTimer.Tick += FacesListener;
                    facesTimer.Start();

                }
                catch (Exception x)
                {
                    Debug.WriteLine($"Exception in 'StartWebCameraAsync' {x.Message}");
                }
            }
        }

        private void FacesListener(object sender, object e)
        {
            if (!IsPlaying)
            {
                Debug.WriteLine("FacesListener Invoked!");

                try
                {

                    if (cameraControl == null) { return; }

                    if (cameraControl.NumFacesOnLastFrame == NumOfFaces)
                    {
                        facesTimer.Stop();
                        gameTimer?.Stop();
                        restartTimer?.Stop();

                        StartNewGame();
                    }
                    else if (cameraControl.NumFacesOnLastFrame == (NumOfFaces - 1))
                    {
                        progressNote.Visibility = Visibility.Visible;
                        progressNote.Text = "Waiting for the secound player to join...";

                        //  dTimer.Stop();
                        //  StartNewGame();
                    }
                    else if (cameraControl.NumFacesOnLastFrame < (NumOfFaces - 1) && cameraControl.NumFacesOnLastFrame > NumOfFaces)
                    {
                        progressNote.Visibility = Visibility.Visible;
                        progressNote.Text = "You can play this game with two players only!";

                    }


                }
                catch (Exception x)
                {
                    Debug.WriteLine($"Exception in 'FacesListener' {x.Message}");

                }
            }

        }

        private void StartNewGame()
        {
            if (IsPlaying) { return; }

            IsPlaying = true;
            Debug.WriteLine("StartNewGame Invoked!");

            facesTimer?.Stop();
            restartTimer?.Stop();
            gameTimer?.Stop();

            RandomProvider = new Random((int)DateTime.Now.Ticks);
            progressNote.Visibility = Visibility.Collapsed;
            RestartButton.Visibility = Visibility.Collapsed;
            EmailButton.Visibility = Visibility.Collapsed;

            string[] emotions = { "Happy", "Sad", "Neutral", "Surprised", "Angry" };
            string[] emojis = { "😃", "🙁", "😐", "😯", "😤" };

            int rEmotion = RandomProvider.Next(0, 5);
            rEmotion = rEmotion == 5 ? 4 : rEmotion;

            ExpectedEmotion randomEmotion = (ExpectedEmotion)Enum.Parse(typeof(ExpectedEmotion), emotions[rEmotion]);

            App.RequiredEmotion = randomEmotion;

            cameraControl.ExpectedEmotion = randomEmotion;

            emotionRequired.Text = randomEmotion.ToString().ToUpper();
            emojiRequired.Text = emojis[rEmotion];

            gameMsgContainer.Visibility = Visibility.Visible;


            gameTimer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 1) };

            int timerIndex = 7;
            timer.Visibility = Visibility.Visible;
            timer.FontSize = 100;

            gameTimer.Tick += async (a, b) =>
             {
                 timer.Text = timerIndex.ToString();
                 timerIndex--;
                 Debug.WriteLine($"gameTimer Ticked @ {timerIndex + 1}");


                 if (timerIndex == -1)
                 {
                     gameTimer.Stop();

                     Debug.WriteLine("Block G Reached");
                     //  gameTimer = new DispatcherTimer();
                     timer.Text = "";
                     //  timerIndex = 7;
                     timer.Visibility = Visibility.Collapsed;


                     if (cameraControl.NumFacesOnLastFrame == NumOfFaces)
                     {
                         Debug.WriteLine("Calling the Camera to take the pic");

                         cameraControl.SimulateButtonClick();
                     }
                     else if (cameraControl.NumFacesOnLastFrame < NumOfFaces)
                     {
                         timer.Visibility = Visibility.Visible;
                         timer.FontSize = 40;
                         timer.Text = "WAITING FOR ANOTHER PLAYER!";

                         while (cameraControl.NumFacesOnLastFrame != NumOfFaces)
                         {

                             await Task.Delay(300);

                             System.Diagnostics.Debug.WriteLine("Waiting for the second face!");
                         }

                         timer.Visibility = Visibility.Collapsed;
                         timer.Text = "";

                         timer.FontSize = 100;
                         Debug.WriteLine("Calling the Camera to take the pic");

                         cameraControl.SimulateButtonClick();

                     }

                 }
             };

            gameTimer.Start();

            Debug.WriteLine($"gameTimer Started - StartNewGame reached to end!");


            // 

            //while (timerIndex != 0)
            //{
            //    timer.Text = timerIndex.ToString();
            //    timerIndex--;
            //    await Task.Delay(1000);
            //}
            ////  gameMsg.Visibility = Visibility.Collapsed;
            //await Task.Delay(1000);

            //  progressNote.Text = "Taking picture...";

            //todo: show the timer in the middle in the screen
            //todo: always show the challenge
            //todo: add how-to play -use- (all screens)
            //todo: remove processing



            //  RestartButton.Visibility = Visibility.Visible;

            //var e = await cameraControl.TakeAutoCapturePhoto();
            //CameraControl_ImageCaptured(null, e);

            //            progressNote.Text = "Processing...";

            //"anger": 9.075572e-13,
            //"contempt": 7.048959e-9,
            //"disgust": 1.02152783e-11,
            //"fear": 1.778957e-14,
            //"happiness": 0.9999999,
            //"neutral": 1.31694478e-7,
            //"sadness": 6.04054263e-12,
            //"surprise": 3.92249462e-11
        }

        private void OnPageSizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateWebCamHostGridSize();
        }

        private void UpdateWebCamHostGridSize()
        {
            this.webCamHostGrid.Width = this.webCamHostGrid.ActualHeight * (this.cameraControl.CameraAspectRatio != 0 ? this.cameraControl.CameraAspectRatio : 1.777777777777);
        }

        private void RestartGame(object sender, RoutedEventArgs e)
        {

            Restart("button");
        }

        private async void SendEmail(object sender, RoutedEventArgs e)
        {

            var dialog1 = new InputDialog();
            var result = await dialog1.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
              

                string email_address = dialog1.Text;


                try
                {
                    var grid = imageFromCameraWithFaces.HostGrid;

                    Grid headCointainer = new Grid
                    {
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        Background = root.Background,
                        Height = 60,
                        VerticalAlignment = VerticalAlignment.Bottom
                    };

                    TextBlock resultText = new TextBlock
                    {
                        Text = $"The result of showing {App.RequiredEmotion} face!",
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        FontWeight = FontWeights.Light,
                        FontSize = 24
                    };

                    headCointainer.Children.Add(resultText);
                    //var head = gameMsgContainer;
                    //head.Background = root.Background;
                    //head.HorizontalAlignment = HorizontalAlignment.Stretch;
                    //head.Height = 60;

                    grid.Children.Add(headCointainer);

                    var success = await PromoteResultForSending(grid, email_address);

                    grid.Children.Remove(headCointainer);

                    if (success)
                    {
                        await new MessageDialog("Please check your inbox or spam folder for the email", "We've sent you the result,").ShowAsync();
                    }

                  
                }
                catch (Exception x)
                {
                    Debug.WriteLine(x.Message);
                }

                cameraContainer.Visibility = Visibility.Visible;
                progressRing.Visibility = Visibility.Collapsed;
                progressRing.IsActive = false;

            }
        }

        private async Task<bool> PromoteResultForSending(UIElement element, string emailAddress)
        {
            try
            {

                RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap();
                await renderTargetBitmap.RenderAsync(element);

                //progress
                cameraContainer.Visibility = Visibility.Collapsed;
                progressRing.Visibility = Visibility.Visible;
                progressRing.IsActive = true;
                //

                var pixelBuffer = await renderTargetBitmap.GetPixelsAsync();
                StorageFile imgFile = await ApplicationData.Current.LocalFolder.CreateFileAsync("result.png", CreationCollisionOption.ReplaceExisting);

                // Encode the image to the selected file on disk
                using (var fileStream = await imgFile.OpenAsync(FileAccessMode.ReadWrite))
                {
                    var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, fileStream);

                    encoder.SetPixelData(
                        BitmapPixelFormat.Bgra8,
                        BitmapAlphaMode.Ignore,
                        (uint)renderTargetBitmap.PixelWidth,
                        (uint)renderTargetBitmap.PixelHeight,
                        DisplayInformation.GetForCurrentView().LogicalDpi,
                        DisplayInformation.GetForCurrentView().LogicalDpi,
                        pixelBuffer.ToArray());

                    await encoder.FlushAsync();
                }

                await new E().SendEmail(new Areas.EmailMessage
                {
                    ToEmail = emailAddress,
                    To = emailAddress,
                    Subject = "Your Emotionizer Result!",
                    Body = "Hi,\nThanks for taking the time and trying Emotionizer game at Microsoft Arabia office. Emotionizer game is built using Cognitive Services, an API - based service that allow you to bring the power of AI and Machine Learning into your App. Learn more here: https://www.microsoft.com/cognitive-services.\n\n Attached you will find your result.\nRegards,",
                    From = "Microsoft",
                    FromEmail = "no-reply@mydomain.com",
                    Attachments = new ReadOnlyCollection<StorageFile>(new List<StorageFile> { { imgFile } })
                });

                return true;

            }
            catch (Exception x)
            {
                Debug.WriteLine(x.Message);
                return false;
            }
        }


        private async void Restart(string caller)
        {
            if (!IsPlaying)
            {
                Debug.WriteLine($"RestartGame Invoked! Caller is [{caller}]");

                restartTimer?.Stop();
                facesTimer?.Stop();
                gameTimer?.Stop();

                restartTimer = null;
                facesTimer = null;
                gameTimer = null;

                progressNote.Visibility = Visibility.Collapsed;
                gameMsgContainer.Visibility = Visibility.Collapsed;
                timer.Visibility = Visibility.Collapsed;
                EmailButton.Visibility = Visibility.Collapsed;
                await this.StartWebCameraAsync(true);
            }

        }
    }


    public class E
    {
        public async Task<string> SendEmail(EmailMessage message)
        {
            HttpClient httpClient = null;
            string responseData = string.Empty;
            try
            {
                using (var postData = new MultipartFormDataContent())
                {
                    var subjectContent = new StringContent(message.Subject);
                    subjectContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                    {
                        Name = "subject"
                    };
                    postData.Add(subjectContent);
                    var bodyContent = new StringContent(message.Body);
                    bodyContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                    {
                        Name = "html"
                    };
                    postData.Add(bodyContent);
                    //setup api key and api user param
                    var apiUserContent = new StringContent("<<API USER>>");
                    apiUserContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                    {
                        Name = "api_user"
                    };
                    postData.Add(apiUserContent);
                    var apiKeyContent = new StringContent("<<API Password>>");
                    apiKeyContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                    {
                        Name = "api_key"
                    };
                    postData.Add(apiKeyContent);
                    var fromNameContent = new StringContent(message.From);
                    fromNameContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                    {
                        Name = "from_name"
                    };
                    postData.Add(fromNameContent);
                    var fromContent = new StringContent(message.FromEmail);
                    fromContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                    {
                        Name = "from"
                    };
                    postData.Add(fromContent);
                    StringContent toContent = new StringContent(message.ToEmail);
                    toContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                    {
                        Name = "to_name"
                    };
                    postData.Add(toContent);
                    var toNameContent = new StringContent(message.To);
                    toNameContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                    {
                        Name = "to"
                    };
                    postData.Add(toNameContent);
                    if (message.Attachments.Count() > 0)
                    {
                        foreach (var attachment in message.Attachments)
                        {
                            Stream fs = await attachment.OpenStreamForReadAsync();
                            postData.Add(AddFile(fs, "files", attachment.Name));
                        }
                    }
                    string fullUrl = string.Format("https://{0}{1}", "api.sendgrid.com", "/api/mail.send.json");
                    httpClient = new HttpClient();
                    HttpResponseMessage response = await httpClient.PostAsync(new Uri(fullUrl), postData);
                    using (HttpContent content = response.Content)
                    {
                        responseData = await content.ReadAsStringAsync();
                    }
                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                        throw new Exception(responseData);
                }
                return responseData;
            }
            finally
            {
                if (httpClient != null)
                    ((IDisposable)httpClient).Dispose();
            }
        }

        private StreamContent AddFile(Stream stream, string name, string fileName)
        {
            var fileContent = new StreamContent(stream);
            fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = string.Format("{0}[{1}]", name, fileName),
                FileName = fileName
            };
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            return fileContent;
        }


    }

    public enum ExpectedEmotion
    {
        Happy,
        Sad,
        Angry,
        Surprised,
        Neutral,
        NaN
    }
    public class EmailMessage
    {
        public string From { get; set; }
        public string FromEmail { get; set; }
        public string To { get; set; }
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public IReadOnlyList<StorageFile> Attachments { get; set; }
    }

    interface IEmailService
    {
        string ApiUser { get; }
        string ApiKey { get; }

        Task<String> SendEmail(EmailMessage message);
    }



}

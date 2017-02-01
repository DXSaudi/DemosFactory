using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.UI;
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
    public sealed partial class StartPage : Page
    {
        public StartPage()
        {
            this.InitializeComponent();
        }
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var r = await UpdateSyncedSettingsAsync();
            await checkSettingsVersion(r);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Areas.FaceDetectionPage));
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Areas.EmotionsGamePage));
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Areas.ShoppingGamePage));
        }

        private async Task checkSettingsVersion(bool success)
        {
            var syncedSettings = await ApplicationData.Current.LocalFolder.GetFileAsync("SyncedSettings.xml");
            var properties = await syncedSettings.GetBasicPropertiesAsync();
             
            string r = success ? "Success" : "Failed";
            updateNote.Text = $"Last update: {properties.DateModified.ToString("dd-MM-yyyy hh:mm tt")}, Last attempt: {r}."; //syncedSettings.DateCreated.ToString("dd-MM-yyyy hh:mm: ss tt");

        }

        private async Task<bool> UpdateSyncedSettingsAsync()
        {
            try
            {

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, new Uri("<<URL TO Your SyncedSettings.xml>>"));

                var ca = new System.Net.Http.Headers.CacheControlHeaderValue();
                ca.NoCache = true;
                ca.MustRevalidate = true;

                request.Headers.CacheControl = ca;

                HttpClient client = new HttpClient();

                var data = await client.SendAsync(request);
                string settingsXml = await data.Content.ReadAsStringAsync();

                ////yet-to-be-tested what if replacing will change the creation date or not
                //StorageFile syncedSettings = await ApplicationData.Current.LocalFolder.GetFileAsync("SyncedSettings.xml");
                //await syncedSettings.DeleteAsync();
                //syncedSettings = null;

                var syncedSettings = await ApplicationData.Current.LocalFolder.CreateFileAsync("SyncedSettings.xml", CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(syncedSettings, settingsXml);

                return true;

            }
            catch (Exception x)
            {
                Debug.WriteLine(x.Message);
                return false;
            }

        }



        private async Task<bool> PromoteResultForSending(UIElement element)
        {
            try
            {

                RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap();
                await renderTargetBitmap.RenderAsync(element);
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
                    ToEmail = "v-mualg@microsoft.com",
                    To = "Muhamed ALGHAZAWI",
                    Subject = "Kiosk Sample Result",
                    Body = "Thank you for trying Kisok, Please find your result in the attachments!",
                    From = "Microsoft",
                    FromEmail = "",
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

    }
}

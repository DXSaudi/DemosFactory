using System;
using UIKit;
using Foundation;
using JSQMessagesViewController;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;
using System.Collections;
using CoreGraphics;

namespace XamarinChat
{
    public class User
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
    }

    public class ChatPageViewController : MessagesViewController
    {
		
        MessagesBubbleImage outgoingBubbleImageData, incomingBubbleImageData;
		MessagesAvatarImage incomingAvatar;
        List<Message> messages = new List<Message>();
        int messageCount = 0;
        private HttpClient _client;
        private Conversation _lastConversation;
        string DirectLineKey = "Your direct line key";
        UIColor lightGray, darkBlue;

		//Tracking of which user said what
        User sender = new User { Id = "2CC8343", DisplayName = "You" };
        User friend = new User { Id = "Test1993", DisplayName = "Xamarin Bot" };

		//Holds the entire message history for a given session
        MessageSet ms = new MessageSet();


        public override async void ViewDidLoad()
        {
            base.ViewDidLoad();
            Title = "متحدث رمضان الآلي";

            // initialize colors
            lightGray = new UIColor(red: 0.85f, green: 0.85f, blue: 0.85f, alpha: 1.0f);
            darkBlue = new UIColor(red: 0.33f, green: 0.41f, blue: 0.51f, alpha: 1.0f);

            // instantiate an HTTPClient, and set properties to our DirectLine bot
            _client = new HttpClient();
            _client.BaseAddress = new Uri("https://directline.botframework.com/api/conversations/");
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("BotConnector",
                DirectLineKey);
            var response = await _client.GetAsync("/api/tokens/");
            if (response.IsSuccessStatusCode)
            {
                var conversation = new Conversation();
                HttpContent contentPost = new StringContent(JsonConvert.SerializeObject(conversation), Encoding.UTF8,
                    "application/json");
                response = await _client.PostAsync("/api/conversations/", contentPost);
                if (response.IsSuccessStatusCode)
                {
                    var conversationInfo = await response.Content.ReadAsStringAsync();
                    _lastConversation = JsonConvert.DeserializeObject<Conversation>(conversationInfo);
                }
            }

            // You must set your senderId and display name
            SenderId = sender.Id;
            SenderDisplayName = sender.DisplayName;
            
            // These MessagesBubbleImages will be used in the GetMessageBubbleImageData override
            var bubbleFactory = new MessagesBubbleImageFactory();
            outgoingBubbleImageData = bubbleFactory.CreateOutgoingMessagesBubbleImage(lightGray);
            CollectionView.CollectionViewLayout.OutgoingAvatarViewSize = CoreGraphics.CGSize.Empty;

            // Incoming from Bot
            incomingBubbleImageData = bubbleFactory.CreateIncomingMessagesBubbleImage(darkBlue);
            UIImage av = FromUrl("http://norashare.com/wp-content/uploads/2017/06/RamadanLogo.png");
            incomingAvatar = new MessagesAvatarImage(av, av, av);

            // Load some messagees to start
            string initialMessage = "أهلا بك في متحدث رمضان الآلي.\nتستطيع أن تسألني عن: وقت الفطور، وقت الإمساك، الخيمات الرمضانية.";
            messages.Add(new Message(friend.Id, friend.DisplayName, NSDate.DistantPast, initialMessage));
            FinishReceivingMessage(true);
        }

        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var cell = base.GetCell(collectionView, indexPath) as MessagesCollectionViewCell;
        
            // Override GetCell to make modifications to the cell
            // In this case darken the text for the sender
            var message = messages[indexPath.Row];
            if (message.SenderId == SenderId)
                cell.TextView.TextColor = darkBlue;
            
            return cell;
        }

        static UIImage FromUrl(string uri)
        {
            var img = new UIImage();
            using (var url = new NSUrl(uri))
            using (var data = NSData.FromUrl(url))
                img = UIImage.LoadFromData(data);
            return img;
        }

        public override nint GetItemsCount(UICollectionView collectionView, nint section)
        {
            return messages.Count;
        }

        public override IMessageData GetMessageData(MessagesCollectionView collectionView, NSIndexPath indexPath)
        {
            return messages[indexPath.Row];
        }

        public override IMessageBubbleImageDataSource GetMessageBubbleImageData(MessagesCollectionView collectionView, NSIndexPath indexPath)
        {
            var message = messages[indexPath.Row];
            if (message.SenderId == SenderId)
                return outgoingBubbleImageData;
            return incomingBubbleImageData;
        }

        public override IMessageAvatarImageDataSource GetAvatarImageData(MessagesCollectionView collectionView, NSIndexPath indexPath)
        {
            var message = messages[indexPath.Row];
			return incomingAvatar;
        }

        public override async void PressedSendButton(UIButton button, string text, string senderId, string senderDisplayName, NSDate date)
        {
			//Clear the text and play a send sound
            InputToolbar.ContentView.TextView.Text = "";
            InputToolbar.ContentView.RightBarButtonItem.Enabled = false;
            SystemSoundPlayer.PlayMessageSentSound();

			//set message details and add to the message queue
            var message = new Message("2CC8343", "You", NSDate.Now, text);
            messages.Add(message);
            FinishReceivingMessage(true);

			//Show typing indicator to add to the natual feel of the bot
            ShowTypingIndicator = true;

			//send message to bot and await the message set
            ms = await SendMessage(text);

			//iterate through our message set, and print new messasges from the bot
            while (ms.messages.Length > messageCount)
            {
                if (ms.messages[messageCount].from == "Test1993")
                {

                    ScrollToBottom(true);

                    SystemSoundPlayer.PlayMessageReceivedSound();

                    Message messageBot;

                    if (ms.messages[messageCount].text != null)
                    {
                        messageBot = new Message(friend.Id, friend.DisplayName, NSDate.Now, ms.messages[messageCount].text);
                        messages.Add(messageBot);
                    }
                    
                    string[] msg_images = ms.messages[messageCount].images;
                    if (msg_images.Length > 0) {
                            string image_url = msg_images[0];
                            UIImage img = FromUrl(image_url);
                            PhotoMediaItem item = new PhotoMediaItem(img);
                            messageBot = new Message(friend.Id, friend.DisplayName, NSDate.Now, item);
                            messages.Add(messageBot);    
                    }
                    
                    FinishReceivingMessage(true);
                    InputToolbar.ContentView.RightBarButtonItem.Enabled = true;
                }

                messageCount++;

            }
        }


        public async Task<MessageSet> SendMessage(string messageText)
        {
            try
            {
                var messageToSend = new BotMessage() { text = messageText, conversationId = _lastConversation.conversationId };
                var contentPost = new StringContent(JsonConvert.SerializeObject(messageToSend), Encoding.UTF8, "application/json");
                var conversationUrl = "https://directline.botframework.com/api/conversations/" + _lastConversation.conversationId + "/messages/";
                
                var response = await _client.PostAsync(conversationUrl, contentPost);
                var messageInfo = await response.Content.ReadAsStringAsync();

                var messagesReceived = await _client.GetAsync(conversationUrl);
                var messagesReceivedData = await messagesReceived.Content.ReadAsStringAsync();

                Console.WriteLine("messagesReceivedData: "+ messagesReceivedData);

                var messages = JsonConvert.DeserializeObject<MessageSet>(messagesReceivedData);

                return messages;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

    }
    public class MessageSet
    {

        public BotMessage[] messages { get; set; }
        public string watermark { get; set; }
        public string eTag { get; set; }

    }

    public class BotMessage
    {
        public string id { get; set; }
        public string conversationId { get; set; }
        public DateTime created { get; set; }
        public string from { get; set; }
        public string text { get; set; }
        public string[] images { get; set; }
        public Attachment[] attachments { get; set; }
        public string eTag { get; set; }
    }

    public class Attachment
    {
        public string contentType { get; set; }
        public Content content { get; set; }
    }

    public class Content
    {
        public string title { get; set; }
        public string text { get; set; }
        public image[] images { get; set; }
        public button[] buttons { get; set; }
    }

    public class image
    {
        public string url { get; set; }
    }

    public class button
    {
        public string type { get; set; }
        public string title { get; set; }
        public string value { get; set; }
    }

    public class Conversation
    {
        public string conversationId { get; set; }
        public string token { get; set; }
        public string eTag { get; set; }
    }

}


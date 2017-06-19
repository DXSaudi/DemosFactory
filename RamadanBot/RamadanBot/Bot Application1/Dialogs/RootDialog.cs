using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Collections.Generic;
using System.Linq;
using AdaptiveCards;
using System.Collections;

namespace Bot_Application1.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        List<string> time = new List<string> { "وقت", "متى"};
        List<string> place = new List<string> { "مكان", "أين", "اين", "وين", "اماكن", "أماكن" };
        List<string> iftar = new List<string>{"فطور","إفطار","المغرب","فطر","أفطر","يفطر","نفطر","افطار" };
        List<string> imsak = new List<string> { "فجر", "امساك", "إمساك","أمسك","يمسك","نمسك" };
        List<string> tents = new List<string> { "خيمات", "خيم", "خيمه", "خيمة" };
        
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            // #1
            var activity = await result as Activity;
            string receivedMSG = (activity.Text).ToLower();

            // #2
            IMessageActivity reply = context.MakeMessage();
            reply.Recipient = reply.Recipient;
            reply.Type = "message";

            // #3
            PrayTime p = new PrayTime();
            string[] s = p.getDatePrayerTimes(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 24.7136, 46.6753, 3);

            // #4.1
            if (iftar.Any(w => receivedMSG.Contains(w)))
            {
                if (time.Any(w => receivedMSG.Contains(w)))
                {
                    reply.Text = ($"{s[5]} وقت أذان المغرب");
                    await context.PostAsync(reply);
                }
                else if (place.Any(w => receivedMSG.Contains(w)))
                {
                    reply.AttachmentLayout = "carousel";
                    Attachment[] attachments = getTenants();
                    for (int i = 0; i < attachments.Length; i++)
                        reply.Attachments.Add(attachments[i]);
                    await context.PostAsync(reply);
                }
                else
                {
                    reply.Text = ($"وضح لي أكثر. هل تسأل عن وقت الإفطار، أو مكان للإفطار؟");
                    await context.PostAsync(reply);
                }
            }
            // #4.2
            else if (imsak.Any(w => receivedMSG.Contains(w)))
            {
                reply.Text = ($"{s[0]} وقت أذان الفجر");
                await context.PostAsync(reply);
            }
            // #4.3
            else if (tents.Any(w => receivedMSG.Contains(w)))
            {
                reply.AttachmentLayout = "carousel";
                Attachment[] attachments = getTenants();
                for (int i = 0; i < attachments.Length; i++)
                    reply.Attachments.Add(attachments[i]);
                await context.PostAsync(reply);
            }
            // #4.4
            else
            {
                CardImage cimage = new CardImage()
                {
                    Url = "http://ar.assabile.com/media/category/310x242/date-ramadan-debut-ramadan-min.png"
                };
                HeroCard card = new HeroCard()
                {
                    Title = "أهلا بك في متحدث رمضان الآلي.",
                    Subtitle = "تستطيع أن تسألني عن: وقت الفطور، وقت الإمساك، الخيمات الرمضانية.",
                    Images = { cimage }
                };

                // Add the card as an attachment of the reply             
                Attachment plAttachment = card.ToAttachment();
                reply.Attachments.Add(plAttachment);
                
                await context.PostAsync(reply);
            }
            // #5
            context.Wait(MessageReceivedAsync);
        }

        Attachment[] getTenants()
        {
            Attachment[] attachments= new Attachment[6];
            
            Attachment attachment = HeroCardAttachment("http://www.hiamag.com/sites/default/files/article/07/06/2016/fay.jpg",
                    "https://goo.gl/maps/kbmP3AnZJc12",
                    "خيمة فوانيس الفيصلية",
                    "6:30-9:00 PM \n350 SR",
                    "+966112732222");
            attachments[0] = attachment;

            attachment = HeroCardAttachment("http://www.hiamag.com/sites/default/files/article/07/06/2016/%20%D8%B3%D9%8A%D8%B2%D9%88%D9%86%D8%B2.jpg",
                "https://goo.gl/maps/N9FVF9YivTL2",
                "الخيمة الرمضانية في قاعة المملكة",
                "6:30-8:30 PM \n395 SR",
                "+9660112115544");
            attachments[1]=attachment;

            attachment = HeroCardAttachment("http://cdn.akhbaar24.com/7b9ed77d-30af-47eb-9bb4-07b55b6bd9a2.jpg",
                "https://goo.gl/maps/WbDZ69vt9W52",
                "الليالي الرمضانية في الريتزكارلتون",
                "6:30-8:30 PM \n375 SR",
                "+966118028333");
            attachments[2]=attachment;

            attachment = HeroCardAttachment("http://www.hiamag.com/sites/default/files/article/07/06/2016/_0.JPG",
                "https://goo.gl/maps/m9DSiG1YL8K2",
                "ملتقى الشرق والغرب في نارسيس",
                "6:30-9:00 PM \n370 SR",
                "+966112946300");
            attachments[3]=attachment;
            
            attachment = HeroCardAttachment("http://www.hiamag.com/sites/default/files/article/07/06/2016/_1.jpg",
                "https://goo.gl/maps/NBP6Vi9ALWy",
                "خيمة انتركونتيننتال الرمضانية",
                "6:30-8:30 PM \n315 SR",
                "+966114655000");
            attachments[4] = attachment;
            
            attachment = HeroCardAttachment("http://www.alsharq.net.sa/wp-content/uploads/2017/05/a2-513x340.jpg",
               "https://goo.gl/maps/J7E1Pfn8w5o",
               "خيمة كمبينسكي الرمضانية",
                "6:30-8:30 PM \n345 SR",
                "+966115117885");

            attachments[5] = attachment;
            return attachments;
        }

        Attachment HeroCardAttachment(string imageURL, string locationURL, string title, string subtitle, string text)
        {
            CardImage cimage = new CardImage()
            {
                Url = imageURL
            };

            List<CardAction> Buttons = new List<CardAction>();
            CardAction locationButton = new CardAction()
             {
                 Type = "openUrl",
                 Title = "الموقع",
                 Value = locationURL
             };
            Buttons.Add(locationButton);

            HeroCard card = new HeroCard()
            {
                Title = title,
                Subtitle = subtitle,
                Text = text,
                Buttons = Buttons,
                Images = { cimage }
            };

            return card.ToAttachment();
        }

    }

}

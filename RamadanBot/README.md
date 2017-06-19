Ramadan Bot will tell you the time for Iftar and Imsak in Riyadh. He will also suggest Ramadan Tents for Iftar.

The project consist of a bot application and an iOS application built by Xamarin.

![alt text](https://github.com/DXSaudi/DemosFactory/blob/master/WordGame/Screenshot.png "screenshot")

# Install
  - [Visual Studio 2015][df1] with [Xamarin][df2].
  - [Bot Application Template][df2]. Save the template in the Visual Studio templates directory.
  - [Bot Framework Emulator][df3] for testing.
  
# Usage
1. After creating a new bot from [Bot Framework portal][df4], copy your App ID and password in `RamadanBot\Bot Application1\Web`.
```
<add key="MicrosoftAppId" value="Your app Id" />
<add key="MicrosoftAppPassword" value="Your bot pass" />
```
2. If you want to use the bot in the mobile application, you have to create a Direct Line channel in your bot page in [Bot Framework portal][df4].

3. Copy the direct line key in `RamadanBot_iOS\RamadanBot\ChatPageViewController`.
```
string DirectLineKey = "Your direct line key";
```
# Acknowledgment
  - [Xamarin Shopping Bot Sample project][df5] and [JSQMessagesViewController][df6] library.
  - Bot icon and application colors are inspired by [Taah Ramadan App][df7].

# Team
  - Nasser AlNasser - Senior Technical Evanglist @Microsoft 
  - Nora AlNashwan - Apps Development Specialist @Microsoft

# License
MIT

[df1]: <https://www.microsoft.com/en-us/download/details.aspx?id=48146>
[df2]: <https://msdn.microsoft.com/en-us/library/mt613162.aspx>
[df3]: <https://www.microsoft.com/cognitive-services/>
[df4]: <https://dev.botframework.com/>
[df5]: <https://github.com/hahaysh/XamarinShoppingBotSample>
[df6]: <https://github.com/jessesquires/JSQMessagesViewController>
[df7]: <https://www.behance.net/gallery/38296703/Taah-Ramadan-android-app>

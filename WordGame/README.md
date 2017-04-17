You have 60 seconds to find as many countries as possible with a given letter. The app is developed using Xamarin and Visual Studio, and take advantage of Bing spell check API to correct user slips!


# Install
  - [Visual Studio 2015][df1] with [Xamarin][df2].
  
# Usage
1. **Get started for free** from [cognitive services page][df3].
2. After you select an account to sign up with, verify your email by requesting verification from the top bar.
![alt text](https://github.com/DXSaudi/DemosFactory/blob/master/WordGame/Images/emailVerification.PNG "email verification")

3. Click on Subscribe to new free trials and check Bing Spell Check then Subscribe. 
![alt text](https://github.com/DXSaudi/DemosFactory/blob/master/WordGame/Images/subscribe.PNG "subscribe")

4. Now you can copy your API key and paste it in the code:
```
client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "{Type your key here}");
```

In this game, we send user input and spell as values for `text` and `mode` parameter, consequently.
```
IEnumerable<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>()
            {
            new KeyValuePair<string,string>("mode","spell"),
            new KeyValuePair<string,string>("text",userEntryText)
            };
```
And the result we revieve contains suggestions for some words/tokens. Thus, we replace each token with the suggestion, then check again if it match one of the countries in `countrynames` list.
```
suggestedCorrections.Add(userEntryText.Replace(obj.token, sugg.suggestion));
```
Look at [API Reference][df4] for more details.

# Team
  - Nasser AlNasser - Senior Technical Evanglist @Microsoft 
  - Nora AlNashwan - Apps Development Specialist @Microsoft

# License
MIT

[df1]: <https://www.microsoft.com/en-us/download/details.aspx?id=48146>
[df2]: <https://msdn.microsoft.com/en-us/library/mt613162.aspx>
[df3]: <https://www.microsoft.com/cognitive-services/>
[df4]: <https://dev.cognitive.microsoft.com/docs/services/56e73033cf5ff80c2008c679/operations/56e73036cf5ff81048ee6727>

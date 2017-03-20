Kid Gift Bot is a project that outline a simple gift ordering form. In this project, we demonestrate using FormFlow to handle a guided conversation.

# Install  
  - [Visual Studio 2015][df1] 
  - [Bot Application Template][df2]. Save the template in the Visual Studio templates directory.
  - [Bot Framework Emulator][df3] for testing.
  
# Usage
In `GiftOrder` class, we define the form we want the user to fill in. 
The form consists of gift options and quantity, which are represented by enum and int data types, respectively. See [documentation][df4] to see other data types for form fields.
```
public GiftOptions? Gift;
public int Quantity;
```
**Describe** attribute is used to add an image to each gift.
```
[Describe(Image = @"http://nord.imgix.net/Zoom/17/_100368897.jpg")]
CastleToy
```
**Prompt** attribute changes the default bot prompt "Please select a {&} {||}", where {&} is the variable name and {||} are the options.
```
[Prompt("What kind of {&} would you like? {||}")]
public GiftOptions? Gift;
```
**Numeric** attribute here limits the accepted values of the quantity to be between 1 and 5.
```
[Numeric(1, 5)]
public int Quantity;
```
You can try other [attributes][df4] to improve the bot.
When the user send a message, conversation will be initiated by connecting our form to the bot framework
```
internal static IDialog<GiftOrder> MakeRootDialog()
        {
            return Chain.From(() => FormDialog.FromForm(GiftOrder.BuildForm));
        }
```


# Team
  - Nasser AlNasser - Senior Technical Evanglist @Microsoft 
  - Nora AlNashwan - Apps Development Specialist @Microsoft

# License
MIT

[df1]: <https://www.microsoft.com/en-us/download/details.aspx?id=48146>
[df2]: <http://aka.ms/bf-bc-vstemplate>
[df3]: <https://aka.ms/edx-DAT211x-bot1>
[df4]: <https://docs.botframework.com/en-us/csharp/builder/sdkreference/forms.html>

﻿#pragma checksum "C:\Kiosk\Kiosk\Areas\EmotionsGamePage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "58AEFE97B763EFC87BDA718651D4C764"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IntelligentKioskSample.Areas
{
    partial class EmotionsGamePage : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                {
                    this.root = (global::Windows.UI.Xaml.Controls.Grid)(target);
                }
                break;
            case 2:
                {
                    this.cameraContainer = (global::Windows.UI.Xaml.Controls.Grid)(target);
                }
                break;
            case 3:
                {
                    this.progressRing = (global::Windows.UI.Xaml.Controls.ProgressRing)(target);
                }
                break;
            case 4:
                {
                    this.RestartButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 83 "..\..\..\Areas\EmotionsGamePage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.RestartButton).Click += this.RestartGame;
                    #line default
                }
                break;
            case 5:
                {
                    this.EmailButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 85 "..\..\..\Areas\EmotionsGamePage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.EmailButton).Click += this.SendEmail;
                    #line default
                }
                break;
            case 6:
                {
                    this.webCamHostGrid = (global::Windows.UI.Xaml.Controls.Grid)(target);
                }
                break;
            case 7:
                {
                    this.imageWithFacesControl = (global::IntelligentKioskSample.Controls.ImageWithFaceBorderUserControl)(target);
                }
                break;
            case 8:
                {
                    this.progressNote = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 9:
                {
                    this.gameMsgContainer = (global::Windows.UI.Xaml.Controls.StackPanel)(target);
                }
                break;
            case 10:
                {
                    this.timer = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 11:
                {
                    this.emotionRequired = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 12:
                {
                    this.emojiRequired = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 13:
                {
                    this.imageFromCameraWithFaces = (global::IntelligentKioskSample.Controls.ImageWithFaceBorderUserControl)(target);
                }
                break;
            case 14:
                {
                    this.cameraControl = (global::IntelligentKioskSample.Controls.emotionCameraControl)(target);
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}


﻿#pragma checksum "C:\Kiosk\Kiosk\Controls\ImageWithFaceBorderUserControl.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "3B2253E810D6150170A404E1E6BBC44B"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IntelligentKioskSample.Controls
{
    partial class ImageWithFaceBorderUserControl : 
        global::Windows.UI.Xaml.Controls.UserControl, 
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
                    this.userControl = (global::Windows.UI.Xaml.Controls.UserControl)(target);
                    #line 11 "..\..\..\Controls\ImageWithFaceBorderUserControl.xaml"
                    ((global::Windows.UI.Xaml.Controls.UserControl)this.userControl).DataContextChanged += this.OnDataContextChanged;
                    #line default
                }
                break;
            case 2:
                {
                    this.hostGrid = (global::Windows.UI.Xaml.Controls.Grid)(target);
                    #line 15 "..\..\..\Controls\ImageWithFaceBorderUserControl.xaml"
                    ((global::Windows.UI.Xaml.Controls.Grid)this.hostGrid).SizeChanged += this.OnImageSizeChanged;
                    #line default
                }
                break;
            case 3:
                {
                    this.progressIndicator = (global::Windows.UI.Xaml.Controls.ProgressRing)(target);
                }
                break;
            case 4:
                {
                    this.imageControl = (global::Windows.UI.Xaml.Controls.Image)(target);
                    #line 16 "..\..\..\Controls\ImageWithFaceBorderUserControl.xaml"
                    ((global::Windows.UI.Xaml.Controls.Image)this.imageControl).SizeChanged += this.OnImageSizeChanged;
                    #line default
                }
                break;
            case 5:
                {
                    this.bitmapImage = (global::Windows.UI.Xaml.Media.Imaging.BitmapImage)(target);
                    #line 18 "..\..\..\Controls\ImageWithFaceBorderUserControl.xaml"
                    ((global::Windows.UI.Xaml.Media.Imaging.BitmapImage)this.bitmapImage).ImageOpened += this.OnBitmapImageOpened;
                    #line default
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


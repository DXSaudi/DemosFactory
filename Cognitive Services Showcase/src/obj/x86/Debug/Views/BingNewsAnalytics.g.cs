﻿#pragma checksum "C:\Kiosk\Kiosk\Views\BingNewsAnalytics.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "9BE87D479286FC93DB2822A4EB487889"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IntelligentKioskSample.Views
{
    partial class BingNewsAnalyticsPage : 
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
                    this.page = (global::Windows.UI.Xaml.Controls.Page)(target);
                }
                break;
            case 2:
                {
                    this.MainGrid = (global::Windows.UI.Xaml.Controls.Grid)(target);
                }
                break;
            case 3:
                {
                    this.progressRing = (global::Windows.UI.Xaml.Controls.ProgressRing)(target);
                }
                break;
            case 4:
                {
                    this.sentimentDistributionControl = (global::IntelligentKioskSample.Controls.SentimentDistributionControl)(target);
                }
                break;
            case 5:
                {
                    this.languageComboBox = (global::Windows.UI.Xaml.Controls.ComboBox)(target);
                    #line 58 "..\..\..\Views\BingNewsAnalytics.xaml"
                    ((global::Windows.UI.Xaml.Controls.ComboBox)this.languageComboBox).SelectionChanged += this.LanguageSelectionChanged;
                    #line default
                }
                break;
            case 6:
                {
                    this.searchBox = (global::Windows.UI.Xaml.Controls.AutoSuggestBox)(target);
                    #line 51 "..\..\..\Views\BingNewsAnalytics.xaml"
                    ((global::Windows.UI.Xaml.Controls.AutoSuggestBox)this.searchBox).QuerySubmitted += this.OnSearchQuerySubmitted;
                    #line 51 "..\..\..\Views\BingNewsAnalytics.xaml"
                    ((global::Windows.UI.Xaml.Controls.AutoSuggestBox)this.searchBox).TextChanged += this.OnSearchTextChanged;
                    #line default
                }
                break;
            case 7:
                {
                    global::Windows.UI.Xaml.Controls.Button element7 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 52 "..\..\..\Views\BingNewsAnalytics.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)element7).Click += this.SearchClicked;
                    #line default
                }
                break;
            case 8:
                {
                    this.sortBySentimentToggle = (global::Windows.UI.Xaml.Controls.ToggleSwitch)(target);
                    #line 32 "..\..\..\Views\BingNewsAnalytics.xaml"
                    ((global::Windows.UI.Xaml.Controls.ToggleSwitch)this.sortBySentimentToggle).Toggled += this.SortBySentimentToggleChanged;
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


﻿

#pragma checksum "C:\Users\anichopr\Documents\YoutubeDownloader\Shared\public\Apps\YoutubeDownloader\YoutubeDownloader\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "0D13434844EAEFD2932B88FD1ADF5E9C"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace YoutubeDownloader
{
    partial class MainPage : global::Windows.UI.Xaml.Controls.Page, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 17 "..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.WebView)(target)).NavigationCompleted += this.Browser_NavigationCompleted;
                 #line default
                 #line hidden
                #line 17 "..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.WebView)(target)).FrameContentLoading += this.WebViewControl_FrameContentLoading;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 29 "..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.ForwardAppBarButton_Click;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 56 "..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.DownloadVideoButton_Click;
                 #line default
                 #line hidden
                break;
            case 4:
                #line 61 "..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.DownloadVideoButton_Click;
                 #line default
                 #line hidden
                break;
            case 5:
                #line 66 "..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.DownloadHistoryButton_Click;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}



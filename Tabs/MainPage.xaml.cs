using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using muxc = Microsoft.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;
using Windows.Storage.Pickers;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Tabs
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        public static FileOpenPicker openPicker = new FileOpenPicker();

        private void initFilePicker()
        {
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            //
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            //
            openPicker.FileTypeFilter.Add(".txt");
            //
            openPicker.FileTypeFilter.Add(".sql");
            //
            openPicker.FileTypeFilter.Add(".js");
        }
        public void themeConfig()
        {
            object theme = localSettings.Values["theme"];
            if (theme != null)
            {
                int value = (int)theme;
                this.RequestedTheme = (ElementTheme)value;

            }

        }
        public MainPage()
        {
            this.InitializeComponent();
            initFilePicker();
            themeConfig();
        }

        private void createTab(string tabTitle = "New Document", object parameters = null)
        {

            var newTab = new muxc.TabViewItem();
            newTab.IconSource = new muxc.SymbolIconSource() { Symbol = Symbol.Document };
            newTab.Header = tabTitle;

            // The Content of a TabViewItem is often a frame which hosts a page.
            Frame frame = new Frame();
            newTab.Content = frame;
            frame.Navigate(typeof(FilePage),parameters);

            MyTabView.TabItems.Add(newTab);
        }
        private void TabView_AddTabButtonClick(muxc.TabView sender, object args)
        {
            createTab();
        }

        private void TabView_TabCloseRequested(muxc.TabView sender, muxc.TabViewTabCloseRequestedEventArgs args)
        {
            sender.TabItems.Remove(args.Tab);
        }

       

     

        private  void RadioClaro_Click(object sender, RoutedEventArgs e)
        {
            RequestedTheme = ElementTheme.Light;
            localSettings.Values["theme"] = (int)ElementTheme.Light;
        }

        private void RadioOscuro_Click(object sender, RoutedEventArgs e)
        {

            RequestedTheme = ElementTheme.Dark;
            localSettings.Values["theme"] = (int)ElementTheme.Dark;
        }

        private async void OpenFileItem_Click(object sender, RoutedEventArgs e)
        {
            StorageFile file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                
                createTab(file.Name, file);
                // Application now has read/write access to the picked file
                //OutputTextBlock.Text = "Picked photo: " + file.Name;
            }
            
        }
        private void SaveFileItem_Click(object sender, RoutedEventArgs e)
        {
            var item = (muxc.TabViewItem)MyTabView.SelectedItem;
            Frame f = (Frame)item.Content;
            FilePage p = (FilePage)f.Content;
            p.SaveFile() ;


        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Text;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Tabs
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FilePage : Page
    {
        private double height { get; set; }
        public  string TextAGuardar;
        public  StorageFile file;
        public  FileSavePicker savePicker = new FileSavePicker();

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
             file = (StorageFile)e.Parameter;
            if (file != null)
            {
            string text = await FileIO.ReadTextAsync(file);
                //TextContent.Document.SetText(TextSetOptions.None,text);
                TextContent.Text = text;

            }
        }
        private void initPicker()
        {
            savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            savePicker.FileTypeChoices.TryAdd("Plain Text",new List<string>() { ".txt"});

            savePicker.SuggestedFileName = "Nuevo archivo";
        }
        public FilePage()
        {
            this.InitializeComponent();
            var bounds = ApplicationView.GetForCurrentView().VisibleBounds;
            height = bounds.Height*0.95;
            initPicker();
        }
        public async  void SaveFile()
        {
            string name = "";
            try
            {
                name = TextAGuardar.Substring(0, 10);
            }
            catch (Exception)
            {
                name = "Nuevo Archivo";
            }
            
            savePicker.SuggestedFileName = name;

            if (file != null)
            {
                await FileIO.WriteTextAsync(file, TextAGuardar);
                return;
            }
            file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                await FileIO.WriteTextAsync(file, TextAGuardar);

            }


        }
        private void RebText_TextChanged(object sender, RoutedEventArgs e)
        {
            //Clear line numbers
            LineNumbers.Text = "";
            int i = 1;

            //Get all the thext
            //ITextRange text = TextContent.Document.GetRange(0, TextConstants.MaxUnitCount);
            //string s = text.Text;
            string s = TextContent.Text;
            TextAGuardar = s;
            if (s != "\r")
            {
                //Replace return char with some char that will be never used (I hope...)
                string[] tmp = s.Replace("\r", "§").Split('§');
                foreach (string st in tmp)
                {
                    //String, adding new line
                        LineNumbers.Text += $"{i++}\r";
                    
                }
            }
        }
    }
}

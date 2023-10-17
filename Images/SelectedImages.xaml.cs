using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using HtmlAgilityPack;

namespace SimpleSearchGooglePhoto.Images
{
    /// <summary>
    /// Interaction logic for SelectedImages.xaml
    /// </summary>
    public partial class SelectedImages : Window
    {
        public SelectedImages(MainWindow main, ImageControl imageControl)
        {
            this.main = main;
            this.imageControl = imageControl;
            InitializeComponent();
            Load();
        }
        ImageControl imageControl;
        MainWindow main;
        public void Load()
        {

            //string searchQuery = SearchTextBox.Text;
            //string googleUrl = $"https://www.google.com/search?q=" + searchQuery + $"{nextPage}";

            //HtmlWeb web = new HtmlWeb();
            //HtmlDocument doc = web.Load(googleUrl);

            //List<string> imageUrls = new List<string>();
            //HtmlNodeCollection imgNodes = doc.DocumentNode.SelectNodes("//img[@data-src]");

            var imageControls = new List<SelectImage>();

            foreach (var imgUrl in Helper.ImageUrls)
            {

                var imageControl = new SelectImage(main, this,this.imageControl);
                imageControl.ImageUrl = imgUrl;
                imageControls.Add(imageControl);
            }

            ImageSelectControl.ItemsSource = imageControls;
        }
    }
}

using HtmlAgilityPack;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Input;

namespace SimpleSearchGooglePhoto;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        SearchTextBox.Focus();
    }

    int count = 0;
    private string nextPage = "&tbm=isch&start=0";
    private void SearchButton_Click(object sender, RoutedEventArgs e)
    {
        string searchQuery = SearchTextBox.Text;
        string googleUrl = "https://www.google.com/search?q=" + WebUtility.UrlEncode(searchQuery) + $"{nextPage}";

        HtmlWeb web = new HtmlWeb();
        HtmlDocument doc = web.Load(googleUrl);

        // Find and extract image URLs
        List<string> imageUrls = new List<string>();
        HtmlNodeCollection imgNodes = doc.DocumentNode.SelectNodes("//img[@data-src]");

        if (imgNodes != null)
        {
            //MessageBox.Show($"{imgNodes.Count}");
            foreach (HtmlNode imgNode in imgNodes)
            {
                string imageUrl = imgNode.GetAttributeValue("data-src", "");
                if (!string.IsNullOrEmpty(imageUrl))
                {
                    imageUrls.Add(imageUrl);
                }
            }
        }

        // Display image URLs in a ListBox
        ImageListBox.ItemsSource = imageUrls;

        if (SearchTextBox.Text.Length != 0)
        {
            nextBtn.Visibility = Visibility.Visible;
        }
    }


    private void SearchTextBox_OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            SearchButton_Click(sender,e);
        }
    }

    private void ImageListBox_OnSizeChanged(object sender, SizeChangedEventArgs e)
    {
        ImageListBox.Width = this.Width;
        ImageListBox.Height = this.Height;
       
    }

    private void NextClick(object sender, RoutedEventArgs e)
    {
        count += 20;
        nextPage = $"&tbm=isch&start={count}";
        SearchButton_Click(sender,e);
    }
}

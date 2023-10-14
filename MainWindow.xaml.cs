using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.AspNetCore.Http;
using Image = System.Drawing.Image;

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
    int pageCounter = 0;
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
            SearchButton_Click(sender, e);
        }
    }

    private void ImageListBox_OnSizeChanged(object sender, SizeChangedEventArgs e)
    {
        ImageListBox.Width = this.Width;
        ImageListBox.Height = this.Height;

    }

    private void NextClick(object sender, RoutedEventArgs e)
    {
        if (pageCounter == 30)
        {
            count = 0;
        }
        count += 20;
        nextPage = $"&tbm=isch&start={count}";
        SearchButton_Click(sender, e);
        pageCounter ++;
    }

    private string selectedImageUrl = "";
    //string fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot/Images", "selected_image.png");
    private void ImageListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ImageListBox.SelectedItem != null)
        {
            selectedImageUrl = ImageListBox.SelectedItem.ToString();
        }
    }

    private void FileDownLoad(object sender, AsyncCompletedEventArgs e)
    {
        if (ImageListBox.SelectedItem != null)
        {
            MessageBox.Show("Downloaded....");
        }
        
    }

    private void SaveImage_Click(object sender, RoutedEventArgs e)
    {
        if (ImageListBox.SelectedItem != null)
        {
            if (!string.IsNullOrEmpty(selectedImageUrl))
            {
                using (WebClient client = new WebClient())
                {
                    if (!Directory.Exists("../../../Images"))
                        Directory.CreateDirectory("../../../Images");


                    var fileName = "../../../Images/" + Guid.NewGuid() + $"{SearchTextBox.Text}.jpg";
                    client.DownloadFileCompleted += new AsyncCompletedEventHandler(FileDownLoad!);
                    Uri imageUrl = new Uri(selectedImageUrl);
                    client.DownloadFileAsync(imageUrl, fileName);
                }
            }
        }
        else
        {
            MessageBox.Show("Not selected");
        }
    }

}

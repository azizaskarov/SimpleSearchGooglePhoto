using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using OpenQA.Selenium.DevTools.V115.DOM;

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
    int imageSaveCounter = 0;
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
        pageCounter++;
    }

    private string selectedImageUrl = "";
    //string fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot/Images", "selected_image.png");
    
        private void ImageListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ImageListBox.SelectedItems.Count > 6)
            {
               ImageListBox.SelectedItems.RemoveAt(6);
            }
        }

    private void FileDownLoad(object sender, AsyncCompletedEventArgs e)
    {
        if (ImageListBox.SelectedItem != null)
        {

            MessageBox.Show($"{count}  Downloaded....");
        }
    }

    private void SaveImage_Click(object sender, RoutedEventArgs e)
    {
        //var win = new ImagesWiew();
        //win.ShowDialog();

        foreach (var selectedItem in ImageListBox.SelectedItems)
        {
            if (selectedItem != null)
            {
                if (!string.IsNullOrEmpty(selectedItem.ToString()))
                {

                    if (imageSaveCounter == 6)
                    {
                        MessageBox.Show("6 tadan kop rasm yuklay olmaysiz");
                        return;
                    }

                    using (WebClient client = new WebClient())
                    {
                        if (!Directory.Exists("../../../Images"))
                            Directory.CreateDirectory("../../../Images");


                        var fileName = "../../../Images/" + Guid.NewGuid() + $"{SearchTextBox.Text}.jpg";
                        client.DownloadFileCompleted += new AsyncCompletedEventHandler(FileDownLoad!);
                        Uri imageUrl = new Uri(selectedItem.ToString()!);
                        client.DownloadFileAsync(imageUrl, fileName);
                        imageSaveCounter++;
                    }
                }
            }
            else
            {
                MessageBox.Show("Not selected");
            }
        }
    }



    private void ExitBtn_Click(object sender, RoutedEventArgs e)
    {
        this.Close();
    }

    private void GridMouseDown(object sender, MouseButtonEventArgs e)
    {

        if (e.ChangedButton == MouseButton.Left)
            this.DragMove();
    }

}

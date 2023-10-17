using System;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;
using SimpleSearchGooglePhoto.Images;

namespace SimpleSearchGooglePhoto;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {

        InitializeComponent();
        IsImageAlready("asd");
        SearchTextBox.Focus();
    }


    int count = 0;
    int imageSaveCounter = 0;
    int pageCounter = 0;
    private string nextPage = "&tbm=isch&start=0";



    //https://www.google.com/search?q=coca+cola&oq=coca+cola&gs_lcrp=EgZjaHJvbWUqBwgAEAAYjwIyBwgAEAAYjwIyDQgBEC4YxwEY0QMYgAQyBggCEEUYQDINCAMQLhivARjHARiABDIHCAQQABiABDIHCAUQABiABDIHCAYQABiABDIHCAcQABiABNIBCDE3OTFqMGo3qAIAsAIA&sourceid=chrome&ie=UTF-8


    private ImageControl imageControlMain;
    //private void SearchButton_Click(object sender, RoutedEventArgs e)
    //{

    //    string searchQuery = SearchTextBox.Text;
    //    string googleUrl = $"https://www.google.com/search?q=" + searchQuery + $"{nextPage}";

    //    HtmlWeb web = new HtmlWeb();
    //    HtmlDocument doc = web.Load(googleUrl);

    //    List<string> imageUrls = new List<string>();
    //    HtmlNodeCollection imgNodes = doc.DocumentNode.SelectNodes("//img[@data-src]");

    //    var imageControls = new List<ImageControl>();
    //    if (imgNodes != null)
    //    {
    //        foreach (HtmlNode imgNode in imgNodes)
    //        {
    //            string imageUrl = imgNode.GetAttributeValue("data-src", "");
    //            if (!string.IsNullOrEmpty(imageUrl))
    //            {
    //                imageUrls.Add(imageUrl);
    //                var imageControl = new ImageControl(this);
    //                imageControl.ImageUrl = imageUrl;
    //                imageControls.Add(imageControl);
    //                imageControlMain = imageControl;
    //            }
    //        }
    //    }

    //    ImageListControl.ItemsSource = imageControls;

    //    if (SearchTextBox.Text.Length != 0)
    //    {
    //        nextBtn.Visibility = Visibility.Visible;
    //    }
    //}

    private void SearchButton_Click(object sender, RoutedEventArgs e)
    {
        string searchQuery = SearchTextBox.Text;
        int numImagesToFetch = 300; 
        int imagesPerPage = 20;   
        int numPagesToFetch = (numImagesToFetch - 1) / imagesPerPage + 1; 

        List<string> imageUrls = new List<string>();
        HtmlWeb web = new HtmlWeb();

        for (int page = 0; page < numPagesToFetch; page++)
        {
            int startOffset = page * imagesPerPage;
            string googleUrl = $"https://www.google.com/search?q={searchQuery}&tbm=isch&start={startOffset}";
            HtmlDocument doc = web.Load(googleUrl);

            HtmlNodeCollection imgNodes = doc.DocumentNode.SelectNodes("//img[@data-src]");

            if (imgNodes != null)
            {
                
                foreach (HtmlNode imgNode in imgNodes)
                {
                    string imageUrl = imgNode.GetAttributeValue("data-src", "");
                    if (!string.IsNullOrEmpty(imageUrl))
                    {
                        imageUrls.Add(imageUrl);

                        if (imageUrls.Count >= numImagesToFetch)
                            break;
                    }
                }
            }

            if (imageUrls.Count >= numImagesToFetch)
                break;
        }

        List<ImageControl> imageControls = new List<ImageControl>();
        foreach (string imageUrl in imageUrls)
        {
            var imageControl = new ImageControl(this);
            imageControl.ImageUrl = imageUrl;
            imageControls.Add(imageControl);
        }

        ImageListControl.ItemsSource = imageControls;
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
        ImageListControl.Width = this.Width;
        ImageListControl.Height = this.Height;

    }

    private void NextClick(object sender, RoutedEventArgs e)
    {
        if (pageCounter == 30)
        {
            count = 0;
        }
        count += 20;
        nextPage = $"&tbm=isch&start={count}";
        SearchButton_Click(sender,e);
        pageCounter++;

    }



    private List<string> imageUrls = new List<string>();

    private void SaveImage_Click(object sender, RoutedEventArgs e)
    {

        if (Helper.ImageUrls.Count != 0)
        {
            foreach (var imageUrl in Helper.ImageUrls)
            {
                using (WebClient client = new WebClient())
                {
                    if (!Directory.Exists("../../../Images"))
                        Directory.CreateDirectory("../../../Images");


                    var fileName = "../../../Images/" + Guid.NewGuid() + $"{SearchTextBox.Text}.jpg";
                    Uri url = new Uri(imageUrl);
                    if (!imageUrls.Exists(i => i.Equals(imageUrl)))
                    {
                        client.DownloadFileAsync(url, fileName);
                        imageUrls.Add(imageUrl);
                        imageSaveCounter++;
                    }
                    else
                    {
                        MessageBox.Show("Already exists");
                    }

                }
            }
           
        }
        else
        {
            MessageBox.Show("Not selected");
        }
    }

    private bool IsImageAlready(string imageUrl)
    {
        string folderPath = @"../../../Images/";
        var files = Directory.GetFiles(folderPath).ToList();
        if (files.Exists(url => url.Equals(imageUrl)))
        {
            return true;
        }
        return false;
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

    private void BackBtn_OnClick(object sender, RoutedEventArgs e)
    {
        count -= 20;
        nextPage = $"&tbm=isch&start={count}";
        SearchButton_Click(sender, e);
    }

    private void SelectedImages_OnClick(object sender, RoutedEventArgs e)
    {


        var win = new SelectedImages(this, imageControlMain);
        win.ShowDialog();
    }
}


public static class Helper
{
    public static List<string> ImageUrls = new();
}

//private void ImageListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
//{
//    if (ImageListBox.SelectedItems.Count > 6)
//    {
//        ImageListBox.SelectedItems.RemoveAt(6);
//    }

//    foreach (var selectedItem in ImageListBox.SelectedItems)
//    {
//        string imageUrl = selectedItem as string;
//        if (!string.IsNullOrEmpty(imageUrl) && !imageUrls.Contains(imageUrl))
//        {
//            imageUrls.Add(imageUrl);
//        }
//    }

//    MessageBox.Show($"{imageUrls.Count}");
//}



//private List<string> imageUrls = new List<string>();
//private void FileDownLoad(object sender, AsyncCompletedEventArgs e)
//{
//    if (ImageListBox.SelectedItem != null)
//    {

//        MessageBox.Show($"{count}  Downloaded....");
//    }
//}
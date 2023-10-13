using HtmlAgilityPack;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;

namespace SimpleSearchGooglePhoto;

/// <summary>
/// Interaction logic for ImagesWiew.xaml
/// </summary>
public partial class ImagesWiew : Window
{
    public ImagesWiew()
    {
        InitializeComponent();
    }

    public void Load()
    {


        List<string> imageUrls = new List<string>();

        //foreach (HtmlNode imgNode in )
        //{
        //    string imageUrl = imgNode.GetAttributeValue("data-src", "");
        //    if (!string.IsNullOrEmpty(imageUrl))
        //    {
        //        imageUrls.Add(imageUrl);
        //    }
        //}

        ImageListBox.ItemsSource = imageUrls;
    }
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SimpleSearchGooglePhoto;

/// <summary>
/// Interaction logic for ImageControl.xaml
/// </summary>
public partial class ImageControl : UserControl
{

    static int imageControlButtonCounter;
    public bool isClicked = false;
    public MainWindow ParentWindow;

    public ImageControl(MainWindow mainWindow)
    {
        InitializeComponent();
        this.ParentWindow = mainWindow;
    }

    public object ImageUrl
    {
        get => imageUrl.Source.ToString()!;
        set
        {
            BitmapImage image = new BitmapImage(new Uri(value.ToString()!));
            imageUrl.Source = image;

        }
    }

    public object ImageLabel
    {
        get => textImage.Text.ToString()!;
        set => textImage.Text = value.ToString();
    }

    private void ImageControl_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {


        var items = ParentWindow.ImageListControl.ItemsSource as List<ImageControl>;

        var count = items.Where(that => that.isClicked == true).Count();


       


            if (count != 6)
            {
                isClicked = !isClicked;

                if (isClicked && Helper.ImageUrls.Count < 6)
                {
                    imageControlButtonCounter++;
                    this.Background = new SolidColorBrush(Colors.CadetBlue);
                    Helper.ImageUrls.Add((string)ImageUrl);

                    if (items.Any(t => t.textImage.Visibility != Visibility.Visible))
                    {
                        var first = items.Where(that => that.isClicked == true).First();
                        first.textImage.Visibility = Visibility.Visible;
                    }

                    textImage.Visibility = Visibility.Collapsed;
                }
                else
                {
                    textImage.Visibility = Visibility.Collapsed;
                    this.Background = new SolidColorBrush(Colors.Azure);
                    Helper.ImageUrls.Remove((string)ImageUrl);

                    //var first1 = items.Where(that => that.isClicked == true).FirstOrDefault();
                    //if (first1 != null)
                    //{
                    //    first1.textImage.Visibility = Visibility.Visible;
                    //}
                }
            }
            else if (count! == 6)
            {
                if (isClicked)
                {
                    isClicked = !isClicked;
                    textImage.Visibility = Visibility.Collapsed;
                    this.Background = new SolidColorBrush(Colors.Azure);
                    Helper.ImageUrls.Remove((string)ImageUrl);



                    //var first1 = items.Where(that => that.isClicked == true).FirstOrDefault();
                    //if (first1 != null)
                    //{
                    //    first1.textImage.Visibility = Visibility.Visible;
                    //}
                }

            }
    }
}
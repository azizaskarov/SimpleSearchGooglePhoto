using System;
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

    public DateTime SelectEdDateTime { get; set; }
    public object ImageLabel
    {
        get => textImage.Text.ToString()!;
        set => textImage.Text = value.ToString();
    }

    private void ImageControl_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        var items = ParentWindow.ImageListControl.ItemsSource as List<ImageControl>;
        var count = items!.Count(that => that.isClicked == true);

        var first = items!.FirstOrDefault(that => that.isClicked == true);
        if (first != null)
        {
            first.textImage.Visibility = Visibility.Visible;
        }

        if (count != 6)
        {
            isClicked = !isClicked;

            if (isClicked && Helper.ImageUrls.Count < 6)
            {

                this.Background = new SolidColorBrush(Colors.CadetBlue);
                Helper.ImageUrls.Add((string)ImageUrl);
                SelectEdDateTime = DateTime.UtcNow;
                textImage.Visibility = Visibility.Collapsed;
            }
            else
            {
                textImage.Visibility = Visibility.Collapsed;
                this.Background = new SolidColorBrush(Colors.Azure);
                Helper.ImageUrls.Remove((string)ImageUrl);
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
            }

        }
    }
}


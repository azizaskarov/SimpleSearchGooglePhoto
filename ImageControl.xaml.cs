using System;
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
    private bool isClicked = false;
    public ImageControl()
    {
        InitializeComponent();
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
        isClicked = !isClicked;

        if (isClicked)
        {
            textImage.Visibility = Visibility.Visible;
            this.Background = new SolidColorBrush(Colors.CadetBlue);
            imageControlButtonCounter += 1;
        }
        else
        {
            textImage.Visibility = Visibility.Collapsed;
            this.Background = new SolidColorBrush(Colors.Azure);
            imageControlButtonCounter -= 1;
        }
    }
}
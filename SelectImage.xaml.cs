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
using System.Windows.Navigation;
using System.Windows.Shapes;
using SimpleSearchGooglePhoto.Images;

namespace SimpleSearchGooglePhoto
{
    /// <summary>
    /// Interaction logic for SelectImage.xaml
    /// </summary>
    public partial class SelectImage : UserControl
    {
        public SelectImage(MainWindow parentWindow, SelectedImages selected, ImageControl imageControl)
        {
            InitializeComponent();
            this.selected = selected;
            this.imageControl = imageControl;
            this.ParentWindow = parentWindow;
        }
        ImageControl imageControl;
        private SelectedImages selected;
        MainWindow ParentWindow;
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


        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Helper.ImageUrls.Remove(ImageUrl.ToString()!);

            var items = ParentWindow.ImageListControl.ItemsSource as List<ImageControl>;

            var count = items.Where(that => that.isClicked == true).Count();





            if (count != 6)
            {
                imageControl.isClicked = !imageControl.isClicked;

                if (imageControl.isClicked && Helper.ImageUrls.Count < 6)
                {
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
                if (imageControl.isClicked)
                {
                    imageControl.isClicked = !imageControl.isClicked;
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

            selected.Load();


        }
    }
}

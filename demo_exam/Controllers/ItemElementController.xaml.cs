using demo_exam.Models;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace lab2.Controllers
{
    public partial class ItemElementController : UserControl
    {
        private string _projectPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        public ItemElementController(Equipment element)
        {
            InitializeComponent();
            DataContext = element;

            string path;

            if (string.IsNullOrEmpty(element.ImagePath))
            {
                path = Path.Combine(_projectPath, "Images", "Defaults", "picture.png");
            }
            else
            {
                path = Path.Combine(_projectPath, "Images", element.ImagePath);
            }

            byte[] imageBytes = File.ReadAllBytes(path);

            using (MemoryStream ms = new MemoryStream(imageBytes))
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = ms;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                BoxImage.Source = bitmapImage;
            }

            if (element.Discount > 15)
            {
                // #2E8B57
                BoxDiscont.Foreground = new SolidColorBrush(Color.FromRgb(46, 139, 87));
                BoxDiscont.TextDecorations.Add(TextDecorations.Underline);
            }

            if (element.Discount > 0)
            {
                BoxPrice.TextDecorations.Add(TextDecorations.Strikethrough);
                BoxPrice.Foreground = new SolidColorBrush(Colors.Red);
                BoxNewPrice.Text = (element.Price - element.Price * element.Discount / 100).ToString();
            }

            if (element.Count == 0)
            {
                BoxCount.TextDecorations.Add(TextDecorations.Strikethrough);
                BoxCount.Foreground = new SolidColorBrush(Colors.Yellow);
            }

        }
    }
}

using demo_exam.Models;
using Microsoft.Win32;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Xml.Linq;


namespace lab2.Views
{
    public partial class AddOrEditElement : Window
    {
        DatabaseContext _db;
        Equipment _element = null;
        string _imagePath;
        private readonly string _projectPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

        public AddOrEditElement(Equipment element = null)
        {
            _db = new DatabaseContext();

            InitializeComponent();
            InitializeComboboxes();

            if (element != null)
            {
                this.Title = "Редактирование";
                BoxTitle.Text = "Редактирование";
                ActionButton.Content = "Изменить";
                ImageButton.Content = "Изменить изображение";

                _element = element;

                InitializeProduct();
            } else
            {
                this.Title = "Добавление";
                BoxTitle.Text = "Добавление";
                ActionButton.Content = "Добавить";
                ImageButton.Content = "Добавить изображение";

                string path = Path.Combine(_projectPath, "Images", "picture.png");

                byte[] imageBytes = File.ReadAllBytes(path);

                using (MemoryStream ms = new MemoryStream(imageBytes))
                {
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.StreamSource = ms;
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.EndInit();

                    ImageBox.Source = bitmapImage;
                }
            }
        }

        private void InitializeComboboxes()
        {
            var units = _db.Units.ToList();
            var manufacturers = _db.Manufacturers.ToList();
            var suppliers = _db.Suppliers.ToList();
            var categories = _db.EquipmentTypes.ToList();

            UnitCombobox.ItemsSource = units;
            ManufacturerCombobox.ItemsSource = manufacturers;
            SupplierCombobox.ItemsSource = suppliers;
            CategoryCombobox.ItemsSource = categories;
        }

        private void InitializeProduct()
        {
            _imagePath = _element.ImagePath;

            string path;

            if (string.IsNullOrEmpty(_imagePath))
            {
                path = Path.Combine(_projectPath, "Images", "picture.png");
            }
            else
            {
                path = Path.Combine(_projectPath, "Images", _imagePath);
            }

            byte[] imageBytes = File.ReadAllBytes(path);

            using (MemoryStream ms = new MemoryStream(imageBytes))
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = ms;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                ImageBox.Source = bitmapImage;
            }

            ArticleTextBox.Text = _element.Article;
            NameTextBox.Text = _element.Name;
            CategoryCombobox.SelectedItem = _element.Type;
            DescriptionTextBox.Text = _element.Description;
            ManufacturerCombobox.SelectedItem = _element.Manufacturer;
            SupplierCombobox.SelectedItem = _element.Supplier;
            PriceTextBox.Text = _element.Price.ToString();
            UnitCombobox.SelectedItem = _element.Unit;
            WarehouseCountTextBox.Text = _element.Count.ToString();
            DiscountTextBox.Text = _element.Discount.ToString();
        }

        private void ActionButtonClick(object sender, RoutedEventArgs e)
        {
            if (
                string.IsNullOrWhiteSpace(ArticleTextBox.Text) ||
                string.IsNullOrWhiteSpace(NameTextBox.Text) ||
                CategoryCombobox.SelectedItem == null ||
                string.IsNullOrWhiteSpace(DescriptionTextBox.Text) ||
                ManufacturerCombobox.SelectedItem == null ||
                SupplierCombobox.SelectedItem == null ||
                string.IsNullOrWhiteSpace(PriceTextBox.Text) ||
                UnitCombobox.SelectedItem == null ||
                string.IsNullOrWhiteSpace(WarehouseCountTextBox.Text) ||
                string.IsNullOrWhiteSpace(DiscountTextBox.Text)
            )
            {
                MessageBox.Show("Все поля должны быть заполнены");
                return;
            }

            try
            {
                //var name = _db.ProductNames.FirstOrDefault(q => q.Name == NameTextBox.Text);
                //if (name == null)
                //{
                //    _db.ProductNames.Add(new ProductName() { Name = NameTextBox.Text });
                //    _db.SaveChanges();
                //    name = _db.ProductNames.FirstOrDefault(q => q.Name == NameTextBox.Text);
                //}

                //name заменить/удалить
                string name = NameTextBox.Text;

                string article = ArticleTextBox.Text;
                EquipmentType category = (EquipmentType)CategoryCombobox.SelectedItem;
                string description = DescriptionTextBox.Text;
                Manufacturer manufacturer = (Manufacturer)ManufacturerCombobox.SelectedItem;
                Supplier supplier = (Supplier)SupplierCombobox.SelectedItem;
                double price = double.Parse(PriceTextBox.Text);
                Unit unit = (Unit)UnitCombobox.SelectedItem;
                int warehouseCount = int.Parse(WarehouseCountTextBox.Text);
                int discount = int.Parse(DiscountTextBox.Text);


                if (price < 0 || discount < 0 || warehouseCount < 0)
                {
                    MessageBox.Show("Поля цена, количество товара на складе и скидка не должны быть отрицательными.");
                    return;
                }

                if (_element == null)
                {
                    var new_element = new Equipment()
                    {
                        Article = article,
                        Name = name,
                        Unit = unit,
                        Price = price,
                        Manufacturer = manufacturer,
                        Supplier = supplier,
                        Type = category,
                        Discount = discount,
                        Count = warehouseCount,
                        Description = description,
                        ImagePath = _imagePath
                    };

                    _db.Equipments.Add(new_element);
                }
                else
                {
                    _element.Article = article;
                    _element.Name = name;
                    _element.Unit = unit;
                    _element.Price = price;
                    _element.Manufacturer = manufacturer;
                    _element.Supplier = supplier;
                    _element.Type = category;
                    _element.Discount = discount;
                    _element.Count = warehouseCount;
                    _element.Description = description;
                    _element.ImagePath = _imagePath;

                    _db.Equipments.Update(_element);
                }

                _db.SaveChanges();
                DialogResult = true;
            }
            catch (FormatException ex)
            {
                MessageBox.Show($"Ошибка формата: {ex.Message}");
            }
            catch (OverflowException ex)
            {
                MessageBox.Show($"Ошибка переполнения {ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private void ImageButtonClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Файлы рисунков (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png;";
            bool? result = openFileDialog.ShowDialog();

            if (result == true)
            {
                string filePath = openFileDialog.FileName;
                string fileName = openFileDialog.SafeFileName;

                BitmapImage bitmapNewImage = new BitmapImage(new Uri(filePath));
                int imageWidth = bitmapNewImage.PixelWidth;
                int imageHeight = bitmapNewImage.PixelHeight;

                if (imageHeight <= 300 && imageHeight <= 300)
                {
                    try
                    {
                        if (!File.Exists(Path.Combine(_projectPath, "Images", fileName)))
                        {
                            File.Copy(filePath, Path.Combine(_projectPath, "Images", fileName));
                        }

                        string path = Path.Combine(_projectPath, "Images", fileName);

                        byte[] imageBytes = File.ReadAllBytes(path);

                        using (MemoryStream ms = new MemoryStream(imageBytes))
                        {
                            BitmapImage bitmapImage = new BitmapImage();
                            bitmapImage.BeginInit();
                            bitmapImage.StreamSource = ms;
                            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                            bitmapImage.EndInit();

                            ImageBox.Source = bitmapImage;
                        }

                        _imagePath = fileName;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
                else
                {
                    MessageBox.Show("Не подходящий размер файла");
                }
            }
        }
        private void ButtonExit(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}

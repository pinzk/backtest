using demo_exam.Models;
using lab2.Controllers;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

//Equipment
namespace lab2.Views
{
    public partial class Main : Window
    {
        DatabaseContext _db;
        private List<Equipment> _elements;
        private string _sortParam;
        private readonly string _projectPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

        public Main(User user = null)
        {
            InitializeComponent();
            _db = new DatabaseContext();

            if (user != null)
            {
                BoxUserName.Text = user.FullName;

                switch (user.Role.Name)
                {
                    case "Менеджер":
                        PanelFind.Visibility = Visibility.Visible;
                        InitializeComboboxes();
                        break;
                    case "Администратор":
                        PanelFind.Visibility = Visibility.Visible;
                        PanelCRUD.Visibility = Visibility.Visible;
                        BoxElements.MouseDoubleClick += ButtonEditElement;
                        InitializeComboboxes();
                        break;
                }

            }
            else
            {
                BoxUserName.Text = "Гость";
            }

            DrawSortable();
        }

        private void InitializeComboboxes()
        {
            List<Manufacturer> filterElements = new List<Manufacturer>
            {
                new Manufacturer()
                {
                    Id = -1,
                    Name = "Все поставщики"
                }
            };

            filterElements.AddRange(_db.Manufacturers.ToList());

            ComboFilter.ItemsSource = filterElements;
        }

        private void DrawElements(List<Equipment> elements)
        {
            BoxElements.ItemsSource = elements.Select(q => new ItemElementController(q));
        }

        private void DrawSortable()
        {
            _elements = _db.Equipments.Include(q => q.Unit)
                .Include(q => q.Type)
                .Include(q => q.Supplier)
                .Include(q => q.Orders)
                .Include(q => q.Manufacturer)
                .ToList();

            _elements = _elements.Where(q =>
                q.Description.Contains(BoxFind.Text) ||
                q.Name.Contains(BoxFind.Text) ||
                q.Article.Contains(BoxFind.Text)
            ).Where(q =>
                q.Manufacturer.Id == ((Manufacturer)ComboFilter.SelectedItem).Id ||
                q.Manufacturer.Id == -1
            ).ToList();

            if (_sortParam == "По возрастанию")
            {
                _elements = _elements.OrderBy(q => q.Count).ToList();
            }
            else
            {
                _elements = _elements.OrderByDescending(q => q.Count).ToList();
            }

            DrawElements(_elements);
        }

        private void BoxFindTextChanged(object sender, TextChangedEventArgs e)
        {
            DrawSortable();
        }

        private void ButtonCreateElement(object sender, RoutedEventArgs e)
        {
            var w = new AddOrEditElement();
            w.ShowDialog();

            DrawSortable();
        }

        private void ButtonEditElement(object sender, MouseButtonEventArgs e)
        {
            ListBox list = sender as ListBox;
            if (list.SelectedItem == null)
                return;

            ItemElementController item = list.SelectedItem as ItemElementController;
            Equipment element = item.DataContext as Equipment;

            var w = new AddOrEditElement(element);
            w.ShowDialog();

            DrawSortable();
        }

        private void ButtonDeleteElement(object sender, RoutedEventArgs e)
        {
            ListBox list = BoxElements;
            if (list.SelectedItem == null)
                return;

            ItemElementController item = list.SelectedItem as ItemElementController;
            Equipment element = item.DataContext as Equipment;

            MessageBoxResult result = MessageBox.Show("Вы дейстивтельно хотите удалить?", "Удаление", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                var isUsed = _db.Orders.FirstOrDefault(x => x.Equipment.Id == element.Id);
                if (isUsed != null)
                {
                    MessageBox.Show("Вы не можете удалить оборудование, которое находится в заказе");
                }
                else
                {
                    File.Delete(Path.Combine(_projectPath, "images", element.ImagePath));
                    _db.Equipments.Remove(element);
                    _db.SaveChanges();

                    DrawSortable();
                }
            }
        }

        private void ButtonExit(object sender, RoutedEventArgs e)
        {
            Authorization w = new Authorization();
            w.Show();
            this.Close();
        }

        private void RadioButtonChecked(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;

            if (radioButton.Content.ToString() == "По возрастанию")
            {
                _sortParam = "По возрастанию";
            }
            else
            {
                _sortParam = "По убыванию";
            }

            DrawSortable();
        }

        private void ComboFilterSelection(object sender, SelectionChangedEventArgs e)
        {
            DrawSortable();
        }
    }
}

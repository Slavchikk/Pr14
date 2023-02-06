using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
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
using System.Xml.Linq;

namespace WpfApp3
{
    /// <summary>
    /// Логика взаимодействия для AddServices.xaml
    /// </summary>
    public partial class AddServices : Page
    {
        Service services; // объект, в котором будет хранится данные о новой или отредактированной услуге
        double procentInCel;// для перевода процента в дробное число для записи  в БД
       
        bool flagUpdate = false;  // для определения, создаем мы новый объект или редактируем старый
        public AddServices()
        {
            InitializeComponent();
        }
        public AddServices(int id)
        {
            InitializeComponent();
            services = BaseClass.EM.Service.FirstOrDefault(x => x.ID == id);
            flagUpdate = true;
            TBTitle.Text = services.Title;
            TBCost.Text = services.Cost.ToString("#,##"); ;
            TBDurat.Text = Convert.ToString(services.DurationInSeconds);
            TBDiscon.Text = Convert.ToString(services.Discount * 100);
            TBDescrip.Text = Convert.ToString(services.Description);
            TBIndex.Visibility = Visibility.Visible;
            TBIndex.Text = "идентификатор: " + services.ID;
            TBMain.Text = "Редактирование услуги";
            btnAdd.Content = "Редактировать";
            if (services.MainImagePath != null)
            {
               string path = services.MainImagePath;
                img.Source = new BitmapImage(new Uri(path, UriKind.Relative));
            }
            else
            {
               
                img.Source = new BitmapImage(new Uri("..\\Resources\\nophoto.jpg", UriKind.Relative));
            }
        }
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            double checkClock;
            try
            {


                if (string.IsNullOrWhiteSpace(TBTitle.Text))
                {
                    MessageBox.Show("Поле наименование не может быть пусто");
                    return;
                }
                if (string.IsNullOrWhiteSpace(TBCost.Text))
                {
                    MessageBox.Show("Поле стоимость не может быть пусто");
                    return;
                }
                if (string.IsNullOrWhiteSpace(TBDurat.Text))
                {
                    MessageBox.Show("Поле продолжительность не может быть пустым");
                    return;
                }
                if (Convert.ToInt32(TBDurat.Text) < 0)
                {
                    MessageBox.Show("Продолжительность (сек) не может быть отрицательным");
                    return;
                }
                checkClock = 14400;
                if(checkClock <= Convert.ToInt32(TBDurat.Text))
                {
                    MessageBox.Show("Продолжительность (сек) не может быть больше 4 часов");
                    return;
                }

                if (!string.IsNullOrWhiteSpace(TBDiscon.Text))
                {
                    if (Convert.ToInt32(TBDiscon.Text) > 100)
                    {
                        MessageBox.Show("Поле скидка не может иметь больше 100%");
                        return;
                    }
                }

                if (!flagUpdate) {
                    services = new Service();
                    List<Service> service = BaseClass.EM.Service.Where(x => x.Title == TBTitle.Text).ToList();
                    if (service.Count > 0)
                    {
                        MessageBox.Show("Такая услуга уже есть!");

                    }
                }
              

                services.Title = TBTitle.Text;
                services.Cost = Convert.ToDecimal(TBCost.Text);
                services.DurationInSeconds = Convert.ToInt32(TBDurat.Text);

                if (string.IsNullOrWhiteSpace(TBDiscon.Text))
                    services.Discount = null;

                else
                {
                    procentInCel = Convert.ToDouble(TBDiscon.Text) / 100;
                    services.Discount = procentInCel;
                }

                if (string.IsNullOrWhiteSpace(TBDescrip.Text))
                    services.Description = null;

                else
                    services.Description = TBDescrip.Text;


                if(!flagUpdate) {
                    BaseClass.EM.Service.Add(services);
                }
               
                BaseClass.EM.SaveChanges();
                if(flagUpdate)
                {
                    MessageBox.Show("Услуга изменена");
                    FrameClass.MainFrame.Navigate(new ListServices());
                }
                else
                {
                    MessageBox.Show("Услуга добавлена");
                    FrameClass.MainFrame.Navigate(new ListServices());
                }
            }
            catch(Exception ex)
            {
                if (flagUpdate)
                    MessageBox.Show("Что-то пошло не по плану при изменении данных");               
                else
                    MessageBox.Show("Что-то пошло не по плану при добавлении данных");
                
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            FrameClass.MainFrame.Navigate(new ListServices(1));
        }
    }
}

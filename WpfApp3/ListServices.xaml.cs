using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
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
using WpfApp3;

namespace WpfApp3
{
    /// <summary>
    /// Логика взаимодействия для ListServices.xaml
    /// </summary>
    public partial class ListServices : Page
    {
        List<Service> services;
        bool checkAdm = false;
        public ListServices()
        {
            InitializeComponent();

           
            SortOrFilt();
            checkBoxAdm.IsChecked = false;

        }
        public ListServices(int i)
        {
            InitializeComponent();

            btnAdd.Visibility = Visibility.Visible;
            
            SortOrFilt();
            checkAdm= true;
            checkBoxAdm.IsChecked = true;

        }
        private void SortOrFilt()
        {
            services = BaseClass.EM.Service.ToList();
            TbFirst.Text = services.Count.ToString();

            if (CBDiscount.SelectedIndex != 0 ) //фильтрация
            {
                switch (CBDiscount.SelectedIndex) 
                {
                    case 1:
                        {
                            services = BaseClass.EM.Service.Where(x => x.Discount >= 0 && x.Discount < 0.05).ToList();
                        }
                        break;
                    case 2:
                        {
                            services = BaseClass.EM.Service.Where(x => x.Discount >= 0.05 && x.Discount < 0.15).ToList();
                        }
                        break;
                    case 3:
                        {
                            services = BaseClass.EM.Service.Where(x => x.Discount >= 0.15 && x.Discount < 0.30).ToList();
                        }
                        break;
                    case 4:
                        {
                            services = BaseClass.EM.Service.Where(x => x.Discount >= 0.30 && x.Discount < 0.70).ToList();
                        }
                        break;
                    case 5:
                        {
                            services = BaseClass.EM.Service.Where(x => x.Discount >= 0.70 && x.Discount < 1).ToList();
                        }
                        break;
                }
            }
            else if(CBDiscount.SelectedIndex != -1)
            {
                services = BaseClass.EM.Service.ToList();
            }



            if ((CBSort.SelectedIndex != -1)) //сортировка
            {

                if (CBSort.SelectedIndex == 0)
                {
                    services = services.OrderBy(x => x.DiscPrice).ToList();
                }
                else
                {
                    services = services.OrderByDescending(x => x.DiscPrice).ToList();
                }


            }



            if (!string.IsNullOrWhiteSpace(TbFindTitle.Text))//По названию
            {
                services = services.Where(x => x.Title.ToLower().Contains(TbFindTitle.Text.ToLower())).ToList();
            }
            if (!string.IsNullOrWhiteSpace(TbFindDescription.Text))//По описанию
            {
                services = services.Where(x => x.Description.ToLower().Contains(TbFindDescription.Text.ToLower())).ToList();
            }

            TbSecond.Text = services.Count.ToString();
            listTable.ItemsSource = services;
        }

        private void TBCost_Loaded(object sender, RoutedEventArgs e)
        {
             TextBlock textB = (TextBlock)sender;
            if (textB.Uid != null)
            {
                textB.Visibility = Visibility.Visible;
            }
            else
            {
                textB.Visibility = Visibility.Collapsed;
            }
        }

        private void TBDiscount_Loaded(object sender, RoutedEventArgs e)
        {
            TextBlock textB = (TextBlock)sender;
            if (textB.Uid != null)
            {
                textB.Visibility = Visibility.Visible;
            }
            else
            {
                textB.Visibility = Visibility.Collapsed;
            }
        }

        private void cbSort_Chang(object sender, SelectionChangedEventArgs e)
        {
            SortOrFilt();
        }

        private void cbFiltr_Chang(object sender, SelectionChangedEventArgs e)
        {
            SortOrFilt();
        }

        private void TbFindTitle_TextChanged(object sender, TextChangedEventArgs e)
        {
            SortOrFilt();
        }

        private void TbFindDescription_TextChanged(object sender, TextChangedEventArgs e)
        {
            SortOrFilt();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            FrameClass.MainFrame.Navigate(new AddServices());
        }

        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            FrameClass.MainFrame.Navigate(new AddServices(Convert.ToInt32(btn.Uid)));
           
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool checkDelete = true;
                Button btn = (Button)sender;
                int ind = (Convert.ToInt32(btn.Uid));
                ClientService clientService = new ClientService();
                Service services = BaseClass.EM.Service.FirstOrDefault(x => x.ID == ind);
                for (int i = 0; i < BaseClass.EM.ClientService.ToList().Count; i++)
                {
                    if (clientService.ServiceID == (Convert.ToInt32(btn.Uid)))
                    {
                        checkDelete = false;
                        MessageBox.Show("Есть информация о записях на услуги, удаление запрещено");
                    }

                }
                if (checkDelete)
                {

                    BaseClass.EM.Service.Remove(services);
                    BaseClass.EM.SaveChanges();
                    MessageBox.Show("Услуга удалена");
                    FrameClass.MainFrame.Navigate(new ListServices());
                }
            }

            catch(Exception ex)
            {
                MessageBox.Show("При удалении возникла  ошибка");
            }
        }

       

        private void checkBoxAdm_Click(object sender, RoutedEventArgs e)
        {
            if(!checkAdm)
            {
                FrameClass.MainFrame.Navigate(new PageAdmin());
               
                
            }
            else
            {
                FrameClass.MainFrame.Navigate(new ListServices());
            }
        }

        private void btnDelete_Loaded(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            if (checkAdm)
            {
                btn.Visibility = Visibility.Visible;
            }
            else
            { btn.Visibility = Visibility.Hidden; }
        }

        private void btnInsert_Loaded(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            if (checkAdm)
            {
                btn.Visibility= Visibility.Visible;
            }
            else
            { btn.Visibility= Visibility.Hidden; }
        }

        private void btnAddService_Loaded(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            if (checkAdm)
            {
                btn.Visibility = Visibility.Visible;
            }
            else
            { btn.Visibility = Visibility.Hidden; }
        }

        private void btnAddService_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;

            FrameClass.MainFrame.Navigate(new AddRecord(Convert.ToInt32(btn.Uid)));
        }
    }
}

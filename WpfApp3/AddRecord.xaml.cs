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

namespace WpfApp3
{
    /// <summary>
    /// Логика взаимодействия для AddRecord.xaml
    /// </summary>
    public partial class AddRecord : Page
    {
        Service services;
      
        public AddRecord(int ind)
        {
            InitializeComponent();
            services = BaseClass.EM.Service.FirstOrDefault(x => x.ID == ind);
            TBTitle.Text = "Наименование услуги " +  services.Title;
            TBDuration.Text = "Продолжительность составляет "+services.DirMin.ToString() + "  минут";
            CBClients.ItemsSource = BaseClass.EM.Client.ToList();
            CBClients.SelectedValuePath = "ID";
            CBClients.DisplayMemberPath = "fio";

        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime dateTime = DTStart.SelectedDate.Value;
                ClientService clientService = new ClientService();
                clientService.ClientID = Convert.ToInt32(CBClients.SelectedValue);
                clientService.ServiceID = services.ID;
                clientService.StartTime = dateTime;
                BaseClass.EM.ClientService.Add(clientService);
                BaseClass.EM.SaveChanges();
                MessageBox.Show("Клиент записан");
                FrameClass.MainFrame.Navigate(new ListServices(1));
            }
            catch
            {
                MessageBox.Show("Что то пошло не так");
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            FrameClass.MainFrame.Navigate(new ListServices(1));
        }
    }
}

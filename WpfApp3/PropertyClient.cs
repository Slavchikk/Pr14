using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp3
{
    public partial class Client
    {

        public string fio //фамилия имя и отчество выводиться как одно поле
        {
            get
            {
               return  FirstName + "  " +  LastName + "  "+ Patronymic;
            }
        }
    }
}

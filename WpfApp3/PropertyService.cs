using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WpfApp3
{
    public partial class Service
    {
        public double DiscPrice
        {
            get
            {
                if (Discount != null)
                {

                    double onePers = Convert.ToDouble(Cost) / 100;
                    double price = Convert.ToDouble(Cost)  - onePers * (Convert.ToDouble(Discount) * 100);
                    return price;
                }
                else
                {
                    return Convert.ToDouble(Cost);
                }
            }
        }
        public float DirMin
        {
            get
            {
                float time = DurationInSeconds / 60;
                return time;
            }
        }
        public SolidColorBrush PropertyColor
        {
            get
            {
              if(Discount != null)
                
                    return Brushes.LightGreen;
               else
                    return Brushes.White;


            }
        }
    }
}

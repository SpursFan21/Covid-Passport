using _106_A2_M1.ViewModel;
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

namespace _106_A2_M1.View.UserFrames
{
    /// <summary>
    /// Interaction logic for UserMyVaccinePassFrame_QR2.xaml
    /// </summary>
    public partial class UserMyVaccinePassFrame_QR2 : UserControl
    {
        private BitmapImage _qrImage;

        public BitmapImage QRImage
        {
            get { return _qrImage; }
            set
            {
                if (_qrImage != value)
                {
                    _qrImage = value;
                    //OnPropertyChanged(nameof(Image));
                }
            }
        }
        public UserMyVaccinePassFrame_QR2(BitmapImage _image)
        {
            InitializeComponent();
            QRImage = _image;
        }
    }
}

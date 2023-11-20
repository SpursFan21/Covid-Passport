using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace _106_A2_M1.ViewModel
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        private string _frameTitle; // Title text for each frame
        public string FrameTitle
        {
            get { return _frameTitle; } // Access the string
            set
            {
                _frameTitle = value;
                OnPropertyChanged(nameof(FrameTitle)); // Notify property changed to update the UI
            }
        }

        private UserControl _currentDisplayFrame;
        public UserControl CurrentDisplayFrame
        {
            get
            {
                return _currentDisplayFrame;
            }

            set
            {
                _currentDisplayFrame = value;
                OnPropertyChanged("CurrentDisplayFrame");
            }
        }
        protected void ShowErrorPopup(string errorMessage)
        {
            // Using MessageBox as a simple example
            MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            // Alternatively, you can create a custom dialog for more control over the appearance and behavior
        }
    }
}

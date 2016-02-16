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
using J2534DotNet;

namespace Flasher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        public bool _action;

        public MainWindow()
        {
            InitializeComponent();
        }

        public void SetButtonState()
        {
            if (_action)
            {
                btn_Action.Content = "Disconnect";

                InterfaceComboBox.IsEditable = false;
                InterfaceComboBox.IsHitTestVisible = false;
                InterfaceComboBox.Focusable = false;

                rbtn_125K.IsHitTestVisible = false;
                rbtn_125K.Focusable = false;
                rbtn_250K.IsHitTestVisible = false;
                rbtn_250K.Focusable = false;
                rbtn_500K.IsHitTestVisible = false;
                rbtn_500K.Focusable = false;

                StatusBox.Document.Blocks.Clear();

            }
            else
            {
                btn_Action.Content = "Connect";
                
                InterfaceComboBox.IsEditable = true;
                InterfaceComboBox.IsHitTestVisible = true;
                InterfaceComboBox.Focusable = true;

                rbtn_125K.IsHitTestVisible = true;
                rbtn_125K.Focusable = true;
                rbtn_250K.IsHitTestVisible = true;
                rbtn_250K.Focusable = true;
                rbtn_500K.IsHitTestVisible = true;
                rbtn_500K.Focusable = true;
            }
        }

        //custom class filterring DiCE compatible devices
        public class SuitableJ2534Devices
        {
            private List<J2534Device> DiCEDevicesList = new List<J2534Device> { };

            public SuitableJ2534Devices()
            {
                foreach (J2534Device Dev in J2534Detect.ListDevices())
                {
                    if(Dev.IsDiCECompatible) { DiCEDevicesList.Add(Dev); }
                    //DiCEDevicesList.Add(Dev);
                }
            }
            public List<J2534Device> List {get { return DiCEDevicesList; } }
        }

        private void InterfaceComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            //fill combobox items list
            DataContext = new SuitableJ2534Devices();
        }

        private void window_Loaded(object sender, RoutedEventArgs e)
        {
            _action = false;
            this.SetButtonState();
            rbtn_125K.IsChecked = true;            
        }

        private void btn_Action_Click(object sender, RoutedEventArgs e)
        {
            _action = _action ^ true;
            this.SetButtonState();
        }

        //private void InterfaceComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //J2534 passThru = new J2534();

        //Load the library of selected interface
        //passThru.LoadLibrary((J2534Device)(sender as ComboBox).SelectedItem);

        //this.StatusBox.AppendText("\nSelected Interface : "+((J2534Device)(sender as ComboBox).SelectedItem).Name);

        // When we are done with the device, we can free the library.
        //passThru.FreeLibrary();
        //}

    }
}

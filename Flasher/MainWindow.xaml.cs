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
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new SuitableJ2534Devices();
        }


        public class SuitableJ2534Devices
        {
            private List<J2534Device> J2534DeviceList = new List<J2534Device> { };

            public SuitableJ2534Devices()
            {
                foreach (J2534Device Dev in J2534Detect.ListDevices())
                {
                    if(Dev.IsDiCECompatible) { J2534DeviceList.Add(Dev); }
                }
            }
            public List<J2534Device> List {get { return J2534DeviceList; } }
        }

    }

   


}

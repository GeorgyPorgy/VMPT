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
        private bool _action;
        private J2534 passThru;
        private FlowDocument mcFlowDoc = new FlowDocument();
        private Paragraph para = new Paragraph();

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
            this.StatusBox.Document.Blocks.Clear();
            this.SetButtonState();
            rbtn_125K.IsChecked = true;            
        }

        public delegate void UpdateTextCallback(string message);
        private void UpdateText(string message)
        {
            para.Inlines.Add(message + "\n");
            mcFlowDoc.Blocks.Add(para);
            this.StatusBox.Document = mcFlowDoc;
            this.StatusBox.ScrollToEnd();
        }

        private void btn_Action_Click(object sender, RoutedEventArgs e)
        {

            if (!_action)
            {
                passThru = new J2534();

                this.UpdateText("\n--- Session start " + DateTime.Now.ToString() + " --- ");
                //this.StatusBox.Dispatcher.Invoke(new UpdateTextCallback(this.UpdateText), new object[] { "\n---Session start " + DateTime.Now.ToString() + " ---" });

                this.UpdateText(((J2534Device)InterfaceComboBox.SelectedItem).Name + " interface being used");
                //this.StatusBox.Dispatcher.Invoke(new UpdateTextCallback(this.UpdateText), new object[] { ((J2534Device)InterfaceComboBox.SelectedItem).Name + " interface being used" });
                if (!passThru.LoadLibrary((J2534Device)InterfaceComboBox.SelectedItem))
                {
                    this.UpdateText("Failed to load interface library!");
                    //this.StatusBox.Dispatcher.Invoke(new UpdateTextCallback(this.UpdateText), new object[] { "Failed to load interface library!" });
                    _action = false;
                }

                _action = true;
            }
            else
            {
                this.UpdateText(((J2534Device)InterfaceComboBox.SelectedItem).Name + " interface is released");
                //this.StatusBox.Dispatcher.Invoke(new UpdateTextCallback(this.UpdateText), new object[] { ((J2534Device)InterfaceComboBox.SelectedItem).Name + " interface is released" });
                if (!passThru.FreeLibrary())
                {
                    this.UpdateText("Failed to unload interface library!");
                    //this.StatusBox.Dispatcher.Invoke(new UpdateTextCallback(this.UpdateText), new object[] { "Failed to unload interface library!" });
                }

                _action = false;
            }

            //if (!_action) { this.StatusBox.Dispatcher.Invoke(new UpdateTextCallback(this.UpdateText), new object[] { "---Session end " + DateTime.Now.ToString() + " ---" }); }
            if (!_action) { this.UpdateText("--- Session end " + DateTime.Now.ToString() + " ---" ); }

            this.SetButtonState();
        }

    }
}

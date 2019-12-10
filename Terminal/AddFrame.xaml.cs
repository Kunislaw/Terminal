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
using System.Windows.Shapes;

namespace Terminal
{
    /// <summary>
    /// Interaction logic for AddFrame.xaml
    /// </summary>
    public partial class AddFrame : Window
    {
        private MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;

        public AddFrame()
        {
            InitializeComponent();
            Owner = Application.Current.MainWindow;
        }

        private void AddFrameButton_Click(object sender, RoutedEventArgs e)
        {
            string frameName = FrameNameTextBlock.Text;
            string frame = FrameTextBlock.Text;
            mainWindow.config.addFrame(frameName, frame);
            mainWindow.config.saveConfig();
            mainWindow.config.readConfig();
            mainWindow.config.refreshFrameList(mainWindow);
            this.Close();
        }
    }
}

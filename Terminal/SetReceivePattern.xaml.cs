using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Windows.Shapes;

namespace Terminal
{
    /// <summary>
    /// Interaction logic for SetReceivePattern.xaml
    /// </summary>
    public partial class SetReceivePattern : Window
    {
        private MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;

        public SetReceivePattern()
        {
            InitializeComponent();
            Owner = Application.Current.MainWindow;
            ReceivePatternTextBox.Text = mainWindow.patternReceive;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (ReceivePatternTextBox.Text.Length > 0)
            {
                mainWindow.patternReceive = ReceivePatternTextBox.Text;
                Close();
            } else
            {
                MessageBox.Show("Podano błędny wzór wiadomości odbierania\nNależy używać tylko liter i cyfr\nPrzykładowy poprawny wzór: abc1", "Terminal", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

    }
}

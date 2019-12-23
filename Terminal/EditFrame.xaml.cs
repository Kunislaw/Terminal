using System;
using System.Windows;
using System.Windows.Controls;

namespace Terminal
{
    /// <summary>
    /// Interaction logic for EditFrame.xaml
    /// </summary>
    public partial class EditFrame : Window
    {
        private MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
        private ListBoxItem selectedItem;
        private FramesClipboard selectedFrame;

        public EditFrame()
        {
            InitializeComponent();
            Owner = Application.Current.MainWindow;

            selectedItem = (ListBoxItem)mainWindow.FramesListBox.SelectedItem;
            selectedFrame = mainWindow.config.framesClipboard.Find(x => x.name.Equals((string)selectedItem.Content));
            FrameNameTextBlock.Text = selectedFrame.name;

            if (selectedFrame.frame.format.Equals("HEX"))
            {
                RadioButton_EditFrame_ASCII.IsChecked = false;
                RadioButton_EditFrame_HEX.IsChecked = true;
                FrameTextBlock.Text = "0x" + BitConverter.ToString(selectedFrame.frame.frameStructure).Replace("-", " 0x");
            }
            else
            {
                RadioButton_EditFrame_ASCII.IsChecked = true;
                RadioButton_EditFrame_HEX.IsChecked = false;
                string frameContent = "";
                for (int i = 0; i < selectedFrame.frame.frameStructure.Length; i++)
                    frameContent += (char)selectedFrame.frame.frameStructure[i];
                FrameTextBlock.Text = frameContent;
            }
        }

        private void EditFrameButton_Click(object sender, RoutedEventArgs e)
        {
            string frameName = FrameNameTextBlock.Text;
            Frame frame = new Frame();

            if (RadioButton_EditFrame_ASCII.IsChecked == true)
            {
                frame = new Frame(FrameTextBlock.Text, true);
            }

            if (RadioButton_EditFrame_HEX.IsChecked == true)
            {
                frame = new Frame(FrameTextBlock.Text, false);
            }

            if (frame.frameStructure != null)
            {
                selectedFrame.name = frameName;
                selectedFrame.lastModification = DateTime.Now.ToString("d-M-y HH:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo);
                selectedFrame.frame = frame;
                mainWindow.config.saveConfig();
                mainWindow.config.readConfig();
                mainWindow.config.refreshFrameList(mainWindow);
                this.Close();
            }
            else
            {
                //Zle sparsowane, wyswietlic jakiegos messageBox'a
            }
        }
    }
}

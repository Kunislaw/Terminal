using System.Windows;


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
            Frame frame = new Frame();
            if(RadioButton_AddFrame_ASCII.IsChecked == true)
            {
                frame = new Frame(FrameTextBlock.Text, true);

            }
            if (RadioButton_AddFrame_HEX.IsChecked == true)
            {
                frame = new Frame(FrameTextBlock.Text, false);

            }
            if(frame.frameStructure != null)
            {
                mainWindow.config.addFrame(frameName, frame);
                mainWindow.config.saveConfig();
                mainWindow.config.readConfig();
                mainWindow.config.refreshFrameList(mainWindow);
                this.Close();
            } else
            {
                //Zle sparsowane
            }



        }
    }
}


namespace _30167671Tester
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class OpeningWindow
    {

        public OpeningWindow()
        {
            InitializeComponent();
            this.MouseLeftButtonDown += (sender, e) => this.DragMove();//ウィンドウ全体でドラッグ可能にする
        }



        private void label30167671_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            State.TestItem = ITEM._30167671;
            var pwaTestWin = new MainWindow();
            State.VmMainWindow.Model = "30167671000";
            pwaTestWin.Show();
            this.Close();
        }

        private void label30221500_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            State.TestItem = ITEM._30221500;
            var pwaTestWin = new MainWindow();
            State.VmMainWindow.Model = "30221500000";
            pwaTestWin.Show();
            this.Close();
        }

        private void labelPrint_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }

        private void metroWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {

        }
    }
}

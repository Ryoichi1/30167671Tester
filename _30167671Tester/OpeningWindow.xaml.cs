
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
            State.testMode = TEST_MODE._30167671;
            var pwaTestWin = new MainWindow();
            pwaTestWin.Show();
            this.Close();
        }

        private void label30221500_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            State.testMode = TEST_MODE._30221500;
            var pwaTestWin = new MainWindow();
            pwaTestWin.Show();
            this.Close();
        }

        private void labelPrint_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }


    }
}

using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Threading;
using System.Linq;

namespace _30167671Tester
{
    /// <summary>
    /// Interaction logic for BasicPage1.xaml
    /// </summary>
    public partial class Mente
    {
        private SolidColorBrush ButtonOnBrush = new SolidColorBrush();
        private SolidColorBrush ButtonOffBrush = new SolidColorBrush();
        private const double ButtonOpacity = 0.4;

        public Mente()
        {
            InitializeComponent();

            ButtonOnBrush.Color = Colors.DodgerBlue;
            ButtonOffBrush.Color = Colors.Transparent;
            ButtonOnBrush.Opacity = ButtonOpacity;
            ButtonOffBrush.Opacity = ButtonOpacity;

            PlotWav.DataContext = State.VmTestResults;

            rb位相制御.IsChecked = true;//default
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            buttonPow.Background = Brushes.Transparent;

            //以下は時間がかかる処理のため、非同期にしないと別ページに遷移した時に若干フリーズする
            Task.Run(() =>
            {
                General.PowerSupply(false);
            });

        }


        bool FlagPow;
        private void buttonPow_Click(object sender, RoutedEventArgs e)
        {
            if (!DS_5107B.InitTestSetting())
            {
                MessageBox.Show("試験設定エラー");
                return;
            }
            
            if (!DS_5107B.GetWav())
            {
                MessageBox.Show("波形取得エラー");
                return;
            }

            

            var NewWav = new System.Collections.Generic.List<OxyPlot.DataPoint>();

            int i = 0;
            DS_5107B.List_Wav.ForEach(w =>
            {
                NewWav.Add(new OxyPlot.DataPoint(i, w));
                i++;
            });


            State.VmTestResults.ListLimitHi = NewWav;
            State.VmTestResults.ListWav = NewWav;

        }


        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            State.VmComm.RX = "";
            State.VmComm.TX = "";
        }




    }
}

using System.Windows;
using System.Windows.Media;
using static _30167671Tester.General;
using System.Collections.Generic;
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
            canvasOscillo.DataContext = State.VmMente;
            CanvasComm.DataContext = State.VmComm;

            rb位相制御.IsChecked = true;//default

            buttonPow.Background = OffBrush;
            buttonPort.Content = "COM1 CLOSEする";
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            General.PowerSupply(false);
            if (!IsPortOpen)
                Target.OpenPort();

        }


        private void buttonWav_Click(object sender, RoutedEventArgs e)
        {
            var dataList = new List<double>();
            if (rb位相制御.IsChecked == true)
            {
                foreach (var d in Enumerable.Range(0, 600))
                {
                    if ((50 <= d && d < 100) || (150 <= d && d < 200) || (250 <= d && d < 300) ||
                        (350 <= d && d < 400) || (450 <= d && d < 500) || (550 <= d && d < 600))  
                    {
                        dataList.Add(0);
                        continue;
                    }
                    double deg = d * (360 * 3 / 600.0);
                    double rad = System.Math.PI * deg / 180.0;
                    double y = 10.0 * System.Math.Sin(rad - System.Math.PI/2);
                    dataList.Add(y);
                }
            }
            else
            {
                foreach (var d in Enumerable.Range(0, 600))
                {
                    if ((0 <= d && d < 50) || (150 <= d && d < 250) || (350 <= d && d < 450) || (550 <= d && d <= 600))
                    {
                        dataList.Add(0);
                        continue;
                    }
                    double deg = d * (360 * 6 / 600.0);
                    double rad = System.Math.PI * deg / 180.0;
                    double y = 10.0 * System.Math.Sin(rad);
                    dataList.Add(y);
                }
            }

            var matterWav = new System.Collections.Generic.List<OxyPlot.DataPoint>();

            int i = 0;
            dataList.ForEach(w =>
            {
                matterWav.Add(new OxyPlot.DataPoint(i, w));
                i++;
            });

            //波形の太さはxaml側で設定している
            State.VmTestResults.ListMasterWav = matterWav;

            //マスターデータ保存
            if (rb位相制御.IsChecked == true)
            {
                Save(dataList, Constants.PathMaster位相制御);
            }
            else
            {
                Save(dataList, Constants.PathMasterサイクル制御);
            }

        }

        private void Save(List<double> dataList, string filePath)
        {
            string strSeparator = ",";                        // 連結するときに追加する文字列(今回はカンマ)
            string strLine = string.Join(strSeparator, dataList);

            // appendをtrueにすると，既存のファイルに追記
            //         falseにすると，ファイルを新規作成する
            var append = false;

            // 出力用のファイルを開く
            using (var sw = new System.IO.StreamWriter(filePath, append, System.Text.Encoding.GetEncoding("Shift_JIS")))
            {
                sw.WriteLine(strLine);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            State.VmComm.RX = "";
        }

        bool FlagPow = false;
        private void buttonPow_Click(object sender, RoutedEventArgs e)
        {
            FlagPow = !FlagPow;
            if (FlagPow)
            {
                PowerSupply(true);
                buttonPow.Background = OnBrush;
            }
            else
            {
                PowerSupply(false);
                buttonPow.Background = OffBrush;
            }
        }

        bool IsPortOpen = true;
        private void buttonPort_Click(object sender, RoutedEventArgs e)
        {
            IsPortOpen = !IsPortOpen;
            if (IsPortOpen)
            {
                Target.OpenPort();
                buttonPort.Content = "COM1 CLOSEする";
            }
            else
            {
                Target.ClosePort();
                buttonPort.Content = "COM1 OPENする";
            }
        }

        private void tbCommtLog_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            tbCommLog.ScrollToEnd();
        }
    }
}

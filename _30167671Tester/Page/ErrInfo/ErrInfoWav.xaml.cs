using System.Windows;

namespace _30167671Tester
{
    /// <summary>
    /// ErrInfoWav.xaml の相互作用ロジック
    /// </summary>
    public partial class ErrInfoWav
    {

        public ErrInfoWav()
        {
            InitializeComponent();
            canvasOscillo.DataContext = State.VmMente;
            PlotWav.DataContext = State.VmTestResults;
        }


        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            SetWavData();
        }

        private void SetWavData()
        {
            State.VmMente.ConfOscillo = TEST_SCR駆動回路.testMode == TEST_SCR駆動回路.MODE.位相制御 ? "5V/Div  5mS/Div" : "5V/Div  10mS/Div";

            //計測した波形の表示
            var NewWav = new System.Collections.Generic.List<OxyPlot.DataPoint>();
            int i = 0;
            TEST_SCR駆動回路.FilterData.ForEach(w =>
            {
                NewWav.Add(new OxyPlot.DataPoint(i, w));
                i++;
            });

            //波形の太さはxaml側で設定している
            State.VmTestResults.ListWav = NewWav;

            //マスター波形の表示
            var masterWav = new System.Collections.Generic.List<OxyPlot.DataPoint>();
            int j = 0;
            TEST_SCR駆動回路.masterDataList.ForEach(w =>
            {
                masterWav.Add(new OxyPlot.DataPoint(j, w));
                j++;
            });


            //波形の太さはxaml側で設定している
            State.VmTestResults.ListMasterWav = masterWav;

        }

        private void buttonReturn_Click(object sender, RoutedEventArgs e)
        {
            State.VmMainWindow.TabIndex = 0;
        }
    }
}

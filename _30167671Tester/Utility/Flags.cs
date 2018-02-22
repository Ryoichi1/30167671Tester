using System.Windows.Media;

namespace _30167671Tester
{
    public static class Flags
    {

        public static bool OtherPage { get; set; }
        public static bool ReturnFromOtherPage { get; set; }

        //試験開始時に初期化が必要なフラグ
        public static bool StopInit周辺機器 { get; set; }
        public static bool Initializing周辺機器 { get; set; }
        public static bool EnableTestStart { get; set; }
        public static bool StopUserInputCheck { get; set; }
        public static bool Testing { get; set; }
        public static bool PowerOn { get; set; }//メイン電源ON/OFF
        public static bool ShowErrInfo { get; set; }
        public static bool AddDecision { get; set; }


        public static bool MetalModeSw { get; set; }
        public static bool MetalMode { get; set; }
        public static bool BgmOn { get; set; }

        public static bool ClickStopButton { get; set; }
        public static bool Click確認Button { get; set; }

        public static bool DialogReturn { get; set; }// OK/CANSELダイアログボックスの戻り値


        public static bool ShowLabelPage { get; set; }

        private static SolidColorBrush RetryPanelBrush = new SolidColorBrush();
        private static SolidColorBrush StatePanelOkBrush = new SolidColorBrush();
        private static SolidColorBrush StatePanelNgBrush = new SolidColorBrush();
        private const double StatePanelOpacity = 0.3;

        static Flags()
        {
            RetryPanelBrush.Color = Colors.DodgerBlue;
            RetryPanelBrush.Opacity = StatePanelOpacity;

            StatePanelOkBrush.Color = Colors.DodgerBlue;
            StatePanelOkBrush.Opacity = StatePanelOpacity;
            StatePanelNgBrush.Color = Colors.DeepPink;
            StatePanelNgBrush.Opacity = StatePanelOpacity;
        }

        //周辺機器ステータス
        private static bool _StateEpx64;
        public static bool StateEpx64
        {
            get { return _StateEpx64; }
            set
            {
                _StateEpx64 = value;
                State.VmTestStatus.ColorEpx64 = value ? StatePanelOkBrush : StatePanelNgBrush;
            }
        }

        private static bool _State7012;
        public static bool State7012
        {
            get { return _State7012; }
            set
            {
                _State7012 = value;
                State.VmTestStatus.Color7012 = value ? StatePanelOkBrush : StatePanelNgBrush;
            }
        }

        private static bool _State5107B;
        public static bool State5107B
        {
            get { return _State5107B; }
            set
            {
                _State5107B = value;
                State.VmTestStatus.Color5107B = value ? StatePanelOkBrush : StatePanelNgBrush;
            }
        }

        private static bool _State323x;
        public static bool State323x
        {
            get { return _State323x; }
            set
            {
                _State323x = value;
                State.VmTestStatus.Color323x = value ? StatePanelOkBrush : StatePanelNgBrush;
            }
        }


        private static bool _StateWavGen;
        public static bool StateWavGen
        {
            get { return _StateWavGen; }
            set
            {
                _StateWavGen = value;
                State.VmTestStatus.ColorGenerator = value ? StatePanelOkBrush : StatePanelNgBrush;
            }
        }



        private static bool _Retry;
        public static bool Retry
        {
            get { return _Retry; }
            set
            {
                _Retry = value;
                State.VmTestStatus.RetryLabelVis = value ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
            }
        }


        public static bool AllOk周辺機器接続 { get; set; }

        //フラグ
        private static bool _SetOperator;
        public static bool SetOperator
        {
            get { return _SetOperator; }
            set
            {
                _SetOperator = value;
                if (value)
                {
                    if (State.VmMainWindow.Operator == "畔上" || State.VmMainWindow.Operator == "畔上2")
                    {
                        State.VmTestStatus.UnitTestEnable = true;
                    }
                    else
                    {
                        State.VmTestStatus.UnitTestEnable = false;
                        State.VmTestStatus.CheckUnitTest = false;
                    }
                }
                else
                {
                    State.VmMainWindow.Operator = "";
                    State.VmTestStatus.UnitTestEnable = false;
                    State.VmTestStatus.CheckUnitTest = false;
                    State.VmMainWindow.SelectIndex = -1;


                }
            }
        }


        private static bool _SetOpecode;
        public static bool SetOpecode
        {
            get { return _SetOpecode; }

            set
            {
                _SetOpecode = value;

                if (value)
                {
                    State.VmMainWindow.ReadOnlyOpecode = true;
                }
                else
                {
                    State.VmMainWindow.ReadOnlyOpecode = false;
                    State.VmMainWindow.Opecode = "";
                    State.VmMainWindow.SerialNumber = "";
                }

            }
        }

    }
}

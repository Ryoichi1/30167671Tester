
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace _30167671Tester
{
    public class TestCommand
    {

        //デリゲートの宣言
        public Action RefreshDataContext;//Test.Xaml内でテスト結果をクリアするために使用すする
        public Action SbRingLoad;
        public Action SbPass;
        public Action SbFail;
        public Action StopButtonBlinkOn;
        public Action StopButtonBlinkOff;

        private bool FlagTestTime;

        DropShadowEffect effect判定表示PASS;
        DropShadowEffect effect判定表示FAIL;

        public TestCommand()
        {
            effect判定表示PASS = new DropShadowEffect();
            effect判定表示PASS.Color = Colors.Aqua;
            effect判定表示PASS.Direction = 0;
            effect判定表示PASS.ShadowDepth = 0;
            effect判定表示PASS.Opacity = 1.0;
            effect判定表示PASS.BlurRadius = 80;

            effect判定表示FAIL = new DropShadowEffect();
            effect判定表示FAIL.Color = Colors.HotPink; ;
            effect判定表示FAIL.Direction = 0;
            effect判定表示FAIL.ShadowDepth = 0;
            effect判定表示FAIL.Opacity = 1.0;
            effect判定表示FAIL.BlurRadius = 40;

        }

        public async Task StartCheck()
        {
            var dis = App.Current.Dispatcher;
            while (true)
            {
                await Task.Run(() =>
                {
                    RETRY:
                    while (true)
                    {
                        if (Flags.OtherPage) break;
                        //Thread.Sleep(200);

                        //作業者名、工番が正しく入力されているかの判定
                        if (!Flags.SetOperator)
                        {
                            State.VmTestStatus.Message = Constants.MessOperator;
                            Flags.EnableTestStart = false;
                            continue;
                        }

                        if (!Flags.SetOpecode)
                        {
                            State.VmTestStatus.Message = Constants.MessOpecode;
                            Flags.EnableTestStart = false;
                            continue;
                        }

                        General.CheckAll周辺機器フラグ();
                        if (!Flags.AllOk周辺機器接続)
                        {
                            State.VmTestStatus.Message = Constants.MessConnect;
                            Flags.EnableTestStart = false;
                            continue;
                        }

                        dis.BeginInvoke(StopButtonBlinkOn);
                        State.VmTestStatus.Message = Constants.MessSet;
                        Flags.EnableTestStart = true;
                        Flags.Click確認Button = false;

                        while (true)
                        {
                            if (Flags.OtherPage || Flags.Click確認Button)
                            {
                                dis.BeginInvoke(StopButtonBlinkOff);
                                return;
                            }

                            if (!Flags.SetOperator || !Flags.SetOpecode)
                            {
                                dis.BeginInvoke(StopButtonBlinkOff);
                                goto RETRY;
                            }
                        }
                    }

                });

                if (Flags.OtherPage)
                {
                    //Flags.PressOpenCheckBeforeTest = true;
                    return;
                }


                State.VmMainWindow.EnableOtherButton = false;
                State.VmTestStatus.StartButtonContent = Constants.停止;
                State.VmTestStatus.TestSettingEnable = false;
                State.VmMainWindow.OperatorEnable = false;
                await Test();//メインルーチンへ


                //試験合格後、ラベル貼り付けページを表示する場合は下記のステップを追加すること
                if (Flags.ShowLabelPage) return;

                //日常点検合格、一項目試験合格、試験NGの場合は、Whileループを繰り返す
                //通常試験合格の場合は、ラベル貼り付けフォームがロードされた時点で、一旦StartCheckメソッドを終了します
                //その後、ラベル貼り付けフォームが閉じられた後に、Test.xamlがリロードされ、そのフォームロードイベントでStartCheckメソッドがコールされます

            }

        }

        private void Timer()
        {
            var t = Task.Run(() =>
            {
                //Stopwatchオブジェクトを作成する
                var sw = new System.Diagnostics.Stopwatch();
                sw.Start();
                while (FlagTestTime)
                {
                    Thread.Sleep(50);
                    State.VmTestStatus.TestTime = sw.Elapsed.ToString().Substring(3, 8);
                }
                sw.Stop();
            });
        }

        //メインルーチン
        public async Task Test()
        {
            Flags.Click確認Button = false;
            Flags.Testing = true;
            Flags.Retry = false;

            General.SetMetalMode();
            General.SetBgm();
            State.VmTestStatus.Message = Constants.MessWait;

            //現在のテーマ透過度の保存
            State.CurrentThemeOpacity = State.VmMainWindow.ThemeOpacity;
            General.SetRadius(true);

            State.VmMainWindow.ThemeBlurEffectRadius = 25;



            await Task.Delay(500);

            FlagTestTime = true;
            Timer();

            int FailStepNo = 0;
            int RetryCnt = 0;//リトライ用に使用する
            string FailTitle = "";


            var テスト項目最新 = new List<TestSpecs>();
            if (State.VmTestStatus.CheckUnitTest == true)
            {
                //チェックしてある項目の百の桁の解析
                var re = Int32.Parse(State.VmTestStatus.UnitTestName.Split('_').ToArray()[0]);
                int 上位桁 = Int32.Parse(State.VmTestStatus.UnitTestName.Substring(0, (re >= 1000) ? 2 : 1));
                var 抽出データ = State.テスト項目.Where(p => (p.Key / 100) == 上位桁);
                foreach (var p in 抽出データ)
                {
                    テスト項目最新.Add(new TestSpecs(p.Key, p.Value, p.PowSw));
                }
            }
            else
            {
                テスト項目最新 = State.テスト項目;
            }



            try
            {
                //IO初期化
                General.ResetIo();
                Thread.Sleep(400);



                foreach (var d in テスト項目最新.Select((s, i) => new { i, s }))
                {
                    Retry:
                    State.VmTestStatus.Spec = "規格値 : ---";
                    State.VmTestStatus.MeasValue = "計測値 : ---";
                    Flags.AddDecision = true;

                    //試験開始時は、CN10はチェッカー側ハーネスに接続する
                    General.SetCn10to6ダイヤル抵抗();
                    //General.SetCn10toChecker();

                    SetTestLog(d.s.Key.ToString() + "_" + d.s.Value);

                    if (d.s.PowSw)
                    {
                        if (!Flags.PowerOn)
                        {
                            General.PowerSupply(true);

                        }
                    }
                    else
                    {
                        General.PowerSupply(false);
                        await Task.Delay(100);
                    }

                    switch (d.s.Key)
                    {

                        case 100://コネクタ実装チェック
                            General.io.ReadInputData(EPX64R.PORT.P7);
                            byte re = (byte)(General.io.P7InputData & 0x0F);
                            if (re != 0x00) goto case 5000;
                            break;

                        case 200://テストプログラム書き込み
                            if (State.VmTestStatus.CheckWriteTestFwPass == true) break;
                            if (await 書き込み.WriteFw()) break;
                            goto case 5000;

                        case 300://電源電圧ﾁｪｯｸ +12V
                            if (await TEST_電源電圧.CheckVolt(TEST_電源電圧.VOL_CH._12V)) break;
                            goto case 5000;

                        case 301://電源電圧ﾁｪｯｸ +5V
                            if (await TEST_電源電圧.CheckVolt(TEST_電源電圧.VOL_CH._5V)) break;
                            goto case 5000;

                        case 302://電源電圧ﾁｪｯｸ +3.3V
                            if (await TEST_電源電圧.CheckVolt(TEST_電源電圧.VOL_CH._3_3V)) break;
                            goto case 5000;

                        case 303://電源電圧ﾁｪｯｸ AVDD
                            if (await TEST_電源電圧.CheckVolt(TEST_電源電圧.VOL_CH.AVDD)) break;
                            goto case 5000;

                        case 304://電源電圧ﾁｪｯｸ AVCC
                            if (await TEST_電源電圧.CheckVolt(TEST_電源電圧.VOL_CH.AVCC)) break;
                            goto case 5000;

                        case 305://電源電圧ﾁｪｯｸ VREF
                            if (await TEST_電源電圧.CheckVolt(TEST_電源電圧.VOL_CH.VREF)) break;
                            goto case 5000;

                        case 306://電源電圧ﾁｪｯｸ AVCCD
                            if (await TEST_電源電圧.CheckVolt(TEST_電源電圧.VOL_CH.AVCCD)) break;
                            goto case 5000;

                        case 307://電源電圧ﾁｪｯｸ S5V
                            if (await TEST_電源電圧.CheckVolt(TEST_電源電圧.VOL_CH.S5V)) break;
                            goto case 5000;

                        case 400://入力回路ﾁｪｯｸ　OFF
                            if (await TEST_入力回路チェック.CheckDIN(TEST_入力回路チェック.MODE.OFF)) break;
                            goto case 5000;

                        case 401://入力回路ﾁｪｯｸ　ON
                            if (await TEST_入力回路チェック.CheckDIN(TEST_入力回路チェック.MODE.ON)) break;
                            goto case 5000;

                        case 500://入力回路ﾁｪｯｸ　OFF
                            if (await TEST_入力回路チェック.CheckANA2_P(TEST_入力回路チェック.MODE.OFF)) break;
                            goto case 5000;

                        case 501://入力回路ﾁｪｯｸ　ON
                            if (await TEST_入力回路チェック.CheckANA2_P(TEST_入力回路チェック.MODE.ON)) break;
                            goto case 5000;

                        case 600://SCR駆動回路ﾁｪｯｸ　位相制御ﾓｰﾄﾞ
                            var dialog_600 = new DialogPic("①周波数入力切切替を上向き\r\n②内部電流選択を下向き\r\n③FGと50/60Hz入力を接続\r\n④オシロと電流モニタを接続", DialogPic.NAME.その他);
                            dialog_600.ShowDialog();

                            await TEST_SCR駆動回路.Set();

                            //電源ONする処理
                            if (await TEST_SCR駆動回路.CheckWave(TEST_SCR駆動回路.MODE.位相制御)) break;


                            goto case 5000;

                        case 601://SCR駆動回路ﾁｪｯｸ　サイクル制御ﾓｰﾄﾞ
                            if (Flags.Retry)
                            {
                                General.PowerSupply(false);
                                await Task.Delay(300);
                                if (!await TEST_SCR駆動回路.Set())
                                {

                                    goto case 5000;
                                }
                            }
                            if (await TEST_SCR駆動回路.CheckWave(TEST_SCR駆動回路.MODE.サイクル制御))
                            {
                                WaveFormGenerator.SourceOff();
                                break;
                            }

                            goto case 5000;

                        case 700://AD入力回路ﾁｪｯｸ　AN_A8
                            if (await TEST_AD入力チェック.CheckANA1_P(TEST_AD入力チェック.CH.ANA8)) break;
                            goto case 5000;

                        case 701://AD入力回路ﾁｪｯｸ　AN_A4
                            if (await TEST_AD入力チェック.CheckANA1_P(TEST_AD入力チェック.CH.ANA4)) break;
                            goto case 5000;

                        case 702://AD入力回路ﾁｪｯｸ　AN_A0
                            if (await TEST_AD入力チェック.CheckANA1_P(TEST_AD入力チェック.CH.ANA0)) break;
                            goto case 5000;

                        case 703://AD入力回路ﾁｪｯｸ　AN_A9
                            if (await TEST_AD入力チェック.CheckANA1_P(TEST_AD入力チェック.CH.ANA9)) break;
                            goto case 5000;

                        case 704://AD入力回路ﾁｪｯｸ　AN_A5
                            if (await TEST_AD入力チェック.CheckANA1_P(TEST_AD入力チェック.CH.ANA5)) break;
                            goto case 5000;

                        case 705://AD入力回路ﾁｪｯｸ　AN_A1
                            if (await TEST_AD入力チェック.CheckANA1_P(TEST_AD入力チェック.CH.ANA1)) break;
                            goto case 5000;

                        case 706://AD入力回路ﾁｪｯｸ　AN_A10
                            if (await TEST_AD入力チェック.CheckANA1_P(TEST_AD入力チェック.CH.ANA10)) break;
                            goto case 5000;

                        case 707://AD入力回路ﾁｪｯｸ　AN_A6
                            if (await TEST_AD入力チェック.CheckANA1_P(TEST_AD入力チェック.CH.ANA6)) break;
                            goto case 5000;

                        case 708://AD入力回路ﾁｪｯｸ　AN_A2
                            if (await TEST_AD入力チェック.CheckANA1_P(TEST_AD入力チェック.CH.ANA2)) break;
                            goto case 5000;

                        case 709://AD入力回路ﾁｪｯｸ　AN_A11
                            if (await TEST_AD入力チェック.CheckANA1_P(TEST_AD入力チェック.CH.ANA11)) break;
                            goto case 5000;

                        case 710://AD入力回路ﾁｪｯｸ　AN_A7
                            if (await TEST_AD入力チェック.CheckANA1_P(TEST_AD入力チェック.CH.ANA7)) break;
                            goto case 5000;

                        case 711://AD入力回路ﾁｪｯｸ　AN_A3
                            if (await TEST_AD入力チェック.CheckANA1_P(TEST_AD入力チェック.CH.ANA3)) break;
                            goto case 5000;

                        case 800://パルス入力回路チェック（左）
                            if (await TEST_パルス入力回路.CheckFLW(TEST_パルス入力回路.MODE.LEFT)) break;
                            goto case 5000;

                        case 801://パルス入力回路チェック（中）
                            if (await TEST_パルス入力回路.CheckFLW(TEST_パルス入力回路.MODE.MIDDLE)) break;
                            goto case 5000;

                        case 802://パルス入力回路チェック（右）
                            if (await TEST_パルス入力回路.CheckFLW(TEST_パルス入力回路.MODE.RIGHT)) break;
                            goto case 5000;

                        case 900://比例弁回転動作チェック（モータAB 左） 
                            if (await TEST_比例弁回転動作.CheckPWPV(TEST_比例弁回転動作.MODE.Motor_L)) break;
                            goto case 5000;

                        case 901://比例弁回転動作チェック（モータAB 右） 
                            if (await TEST_比例弁回転動作.CheckPWPV(TEST_比例弁回転動作.MODE.Motor_R)) break;
                            goto case 5000;

                        case 1000://警報用Pt100回路ﾁｪｯｸ　発報点
                            if (await TEST_警報点.Check警報点()) break;
                            goto case 5000;

                        case 1001://警報用Pt100回路ﾁｪｯｸ　断線
                            if (await TEST_警報点.CheckDisconnection()) break;
                            goto case 5000;

                        case 1100://AC電源電圧読取り回路ﾁｪｯｸ
                            if (await TEST_出力回路.CheckAN_P(TEST_出力回路.MODE.READ_V)) break;
                            goto case 5000;

                        case 1200://負荷電流読取り回路ﾁｪｯｸ　CT1
                            if (await TEST_出力回路.CheckAN_P(TEST_出力回路.MODE.READ_I_1)) break;
                            goto case 5000;

                        case 1201://負荷電流読取り回路ﾁｪｯｸ　CT2
                            if (await TEST_出力回路.CheckAN_P(TEST_出力回路.MODE.READ_I_2)) break;
                            goto case 5000;

                        case 1300://RS232Cチェック
                            if (await TEST_通信.CheckRS232C()) break;
                            goto case 5000;

                        case 1301://RS485-1チェック
                            if (await TEST_通信.CheckRS485_1()) break;
                            goto case 5000;

                        case 1302://RS485-2チェック
                            if (await TEST_通信.CheckRS485_2()) break;
                            goto case 5000;

                        case 1303://表示基板チェック
                            if (await TEST_通信.CheckDISP()) break;
                            goto case 5000;

                        case 1400://Vrefチェック
                            if (await TEST_VREF.SetVref()) break;
                            goto case 5000;

                        case 1401://Vrefチェック 再
                            if (await TEST_VREF.CheckVref()) break;
                            goto case 5000;

                        case 1500://出力回路ﾁｪｯｸ　ﾃﾞｼﾞﾀﾙ出力
                            if (await TEST_出力回路.CheckOUTP()) break;
                            goto case 5000;

                        case 1600://PWTMP_A 電流温度モニタ1 DV9
                            if (await TEST_出力回路.CheckPWTMP_A(TEST_出力回路.DV_CH.DV9)) break;
                            goto case 5000;

                        case 1601://PWTMP_A 電流温度モニタ2 DV11
                            if (await TEST_出力回路.CheckPWTMP_A(TEST_出力回路.DV_CH.DV11)) break;
                            goto case 5000;

                        case 1700://PWTMP_V 電圧温度モニタ1 DV10
                            if (await TEST_出力回路.CheckPWTMP_V(TEST_出力回路.DV_CH.DV10)) break;
                            goto case 5000;

                        case 1701://PWTMP_V 電圧温度モニタ2 DV12
                            if (await TEST_出力回路.CheckPWTMP_V(TEST_出力回路.DV_CH.DV12)) break;
                            goto case 5000;

                        case 1800://PWINV インバータ回転司令 DV13
                            if (await TEST_出力回路.CheckPWINV()) break;
                            goto case 5000;

                        case 1900://PWIOUT 操作量 電流1 DV1
                            if (await TEST_出力回路.CheckPWIOUT(TEST_出力回路.DV_CH.DV1)) break;
                            goto case 5000;

                        case 1901://PWIOUT 操作量 電流2 DV3
                            if (await TEST_出力回路.CheckPWIOUT(TEST_出力回路.DV_CH.DV3)) break;
                            goto case 5000;

                        case 1902://PWIOUT 操作量 電流3 DV5
                            if (await TEST_出力回路.CheckPWIOUT(TEST_出力回路.DV_CH.DV5)) break;
                            goto case 5000;

                        case 1903://PWIOUT 操作量 電流4 DV7
                            if (await TEST_出力回路.CheckPWIOUT(TEST_出力回路.DV_CH.DV7)) break;
                            goto case 5000;

                        case 2000://PWVOUT 操作量 電圧1 DV2
                            if (await TEST_出力回路.CheckPWVOUT(TEST_出力回路.DV_CH.DV2)) break;
                            goto case 5000;

                        case 2001://PWVOUT 操作量 電圧2 DV4
                            if (await TEST_出力回路.CheckPWVOUT(TEST_出力回路.DV_CH.DV4)) break;
                            goto case 5000;

                        case 2002://PWVOUT 操作量 電圧3 DV6
                            if (await TEST_出力回路.CheckPWVOUT(TEST_出力回路.DV_CH.DV6)) break;
                            goto case 5000;

                        case 2003://PWVOUT 操作量 電圧4 DV8
                            if (await TEST_出力回路.CheckPWVOUT(TEST_出力回路.DV_CH.DV8)) break;
                            goto case 5000;

                        case 2100://PV1,2 調整
                            Flags.AddDecision = false;
                            State.VmTestStatus.TestLog += "\r\n";
                            var mess = "①プレスを開けて、CN18とCN26にケーブルを接続してください\r\n②CN1とCN3にダイヤル抵抗器を接続してください";
                            var dialog = new DialogPic(mess, DialogPic.NAME.その他);
                            dialog.ShowDialog();
                            if (!await TEST_PT100.SetPT100(TEST_PT100.MODE.PV12, TEST_PT100.POINT.FIRST)) goto case 5000;
                            if (!await TEST_PT100.SetPT100(TEST_PT100.MODE.PV12, TEST_PT100.POINT.SECOND)) goto case 5000;
                            if (!await TEST_PT100.SetPT100(TEST_PT100.MODE.PV12, TEST_PT100.POINT.THIRD)) goto case 5000;
                            if (!await TEST_PT100.ReadPt100(TEST_PT100.NAME_PV.PV1)) goto case 5000;
                            if (!await TEST_PT100.ReadPt100(TEST_PT100.NAME_PV.PV2)) goto case 5000;
                            if (!await TEST_PT100.Check60(TEST_PT100.MODE.PV12)) goto case 5000;
                            break;

                        case 2200://PV3,4 調整
                            Flags.AddDecision = false;
                            State.VmTestStatus.TestLog += "\r\n";
                            mess = "①プレスを開けて、CN18とCN26にケーブルを接続してください\r\n②CN5とCN7にダイヤル抵抗器を接続してください";
                            dialog = new DialogPic(mess, DialogPic.NAME.その他);
                            dialog.ShowDialog();
                            if (!await TEST_PT100.SetPT100(TEST_PT100.MODE.PV34, TEST_PT100.POINT.FIRST)) goto case 5000;
                            if (!await TEST_PT100.SetPT100(TEST_PT100.MODE.PV34, TEST_PT100.POINT.SECOND)) goto case 5000;
                            if (!await TEST_PT100.SetPT100(TEST_PT100.MODE.PV34, TEST_PT100.POINT.THIRD)) goto case 5000;
                            if (!await TEST_PT100.ReadPt100(TEST_PT100.NAME_PV.PV3)) goto case 5000;
                            if (!await TEST_PT100.ReadPt100(TEST_PT100.NAME_PV.PV4)) goto case 5000;
                            if (!await TEST_PT100.Check60(TEST_PT100.MODE.PV34)) goto case 5000;
                            break;

                        case 2600://Pt100ｾﾝｻｰ断線ﾁｪｯｸ Normal
                            if (await TEST_PT100センサ断線.CheckNormal()) break;
                            goto case 5000;
                        case 2601://Pt100ｾﾝｻｰ断線ﾁｪｯｸ A断線
                            if (await TEST_PT100センサ断線.Check断線(TEST_PT100センサ断線.MODE.A断線)) break;
                            goto case 5000;
                        case 2602://Pt100ｾﾝｻｰ断線ﾁｪｯｸ B断線
                            if (await TEST_PT100センサ断線.Check断線(TEST_PT100センサ断線.MODE.B_1断線)) break;
                            goto case 5000;
                        case 2603://Pt100ｾﾝｻｰ断線ﾁｪｯｸ B'断線
                            if (await TEST_PT100センサ断線.Check断線(TEST_PT100センサ断線.MODE.B_2断線)) break;
                            goto case 5000;

                        case 2700://電流入力回路調整 I_1
                            Flags.AddDecision = false;
                            State.VmTestStatus.TestLog += "\r\n";
                            dialog = new DialogPic("ケーブルを外して、プレスを閉じてください！！！", DialogPic.NAME.その他);
                            dialog.ShowDialog();
                            if (!await TEST_電流入力回路.SetInputI(TEST_電流入力回路.NAME.I_1)) goto case 5000;
                            if (!await TEST_電流入力回路.CheckInputI(TEST_電流入力回路.NAME.I_1)) goto case 5000;
                            break;

                        case 2701://電流入力回路調整 I_2
                            Flags.AddDecision = false;
                            State.VmTestStatus.TestLog += "\r\n";
                            if (!await TEST_電流入力回路.SetInputI(TEST_電流入力回路.NAME.I_2)) goto case 5000;
                            if (!await TEST_電流入力回路.CheckInputI(TEST_電流入力回路.NAME.I_2)) goto case 5000;
                            break;

                        case 2702://電流入力回路調整 I_3
                            Flags.AddDecision = false;
                            State.VmTestStatus.TestLog += "\r\n";
                            if (!await TEST_電流入力回路.SetInputI(TEST_電流入力回路.NAME.I_3)) goto case 5000;
                            if (!await TEST_電流入力回路.CheckInputI(TEST_電流入力回路.NAME.I_3)) goto case 5000;
                            break;

                        case 2703://電流入力回路調整 I_4
                            Flags.AddDecision = false;
                            State.VmTestStatus.TestLog += "\r\n";
                            if (!await TEST_電流入力回路.SetInputI(TEST_電流入力回路.NAME.I_4)) goto case 5000;
                            if (!await TEST_電流入力回路.CheckInputI(TEST_電流入力回路.NAME.I_4)) goto case 5000;
                            break;

                        case 2800://電圧入力回路調整 V_1
                            Flags.AddDecision = false;
                            State.VmTestStatus.TestLog += "\r\n";
                            if (!await TEST_電圧入力回路.SetInputV(TEST_電圧入力回路.MODE.V_1)) goto case 5000;
                            if (!await TEST_電圧入力回路.CheckInputV(TEST_電圧入力回路.MODE.V_1)) goto case 5000;
                            break;

                        case 2801://電圧入力回路調整 V_2
                            Flags.AddDecision = false;
                            State.VmTestStatus.TestLog += "\r\n";
                            if (!await TEST_電圧入力回路.SetInputV(TEST_電圧入力回路.MODE.V_2)) goto case 5000;
                            if (!await TEST_電圧入力回路.CheckInputV(TEST_電圧入力回路.MODE.V_2)) goto case 5000;
                            break;

                        case 2802://電圧入力回路調整 V_3
                            Flags.AddDecision = false;
                            State.VmTestStatus.TestLog += "\r\n";
                            if (!await TEST_電圧入力回路.SetInputV(TEST_電圧入力回路.MODE.V_3)) goto case 5000;
                            if (!await TEST_電圧入力回路.CheckInputV(TEST_電圧入力回路.MODE.V_3)) goto case 5000;
                            break;

                        case 2803://電圧入力回路調整 V_4
                            Flags.AddDecision = false;
                            State.VmTestStatus.TestLog += "\r\n";
                            if (!await TEST_電圧入力回路.SetInputV(TEST_電圧入力回路.MODE.V_4)) goto case 5000;
                            if (!await TEST_電圧入力回路.CheckInputV(TEST_電圧入力回路.MODE.V_4)) goto case 5000;
                            break;

                        case 2900://2線式液漏れセンサ回路 調整
                            Flags.AddDecision = false;
                            State.VmTestStatus.TestLog += "\r\n";
                            if (!await TEST_2線式液漏れセンサ回路.SetLeak()) goto case 5000;
                            if (!await TEST_2線式液漏れセンサ回路.CheckLeak()) goto case 5000;
                            break;



                        case 5000://NGだっときの処理
                            if (Flags.AddDecision) SetTestLog("---- FAIL\r\n");
                            FailStepNo = d.s.Key;
                            FailTitle = d.s.Value;

                            Flags.Retry = true;
                            WaveFormGenerator.SourceOff();
                            HIOKI7012.StopSource();
                            General.PowerSupply(false);
                            General.ResetIo();
                            State.VmTestStatus.IsActiveRing = false;//リング表示してる可能性があるので念のため消す処理

                            if (Flags.ClickStopButton) goto CHECK_RETRY;

                            if (RetryCnt++ != Constants.RetryCount)
                            {
                                //リトライ履歴リスト更新
                                State.RetryLogList.Add(FailStepNo.ToString() + "," + FailTitle + "," + System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                                goto Retry;

                            }

                            CHECK_RETRY:
                            General.PlaySoundLoop(General.soundAlarm);
                            var YesNoResult = MessageBox.Show("この項目はＮＧですがリトライしますか？", "", MessageBoxButtons.YesNo);
                            General.StopSound();

                            //何が選択されたか調べる
                            if (YesNoResult == DialogResult.Yes)
                            {
                                RetryCnt = 0;
                                Flags.ClickStopButton = false;
                                RETRY_INIT:
                                State.VmTestStatus.Message = "周辺機器を初期化しています。。。";
                                var reInit = await General.InitAll();
                                State.VmTestStatus.Message = Constants.MessWait;
                                if (!reInit)
                                {
                                    var YesNoResult2 = MessageBox.Show("周辺機器の初期化に失敗しました\r\nリトライしますか？？", "", MessageBoxButtons.YesNo);
                                    if (YesNoResult2 == DialogResult.Yes)
                                    {
                                        goto RETRY_INIT;
                                    }
                                    goto FAIL;
                                }


                                goto Retry;
                            }

                            goto FAIL;//自動リトライ後の作業者への確認はしない


                    }
                    //↓↓各ステップが合格した時の処理です↓↓
                    if (Flags.AddDecision) SetTestLog("---- PASS\r\n");

                    State.VmTestStatus.IsActiveRing = false;

                    //リトライステータスをリセットする
                    RetryCnt = 0;
                    Flags.Retry = false;

                    await Task.Run(() =>
                    {
                        var CurrentProgValue = State.VmTestStatus.進捗度;
                        var NextProgValue = (int)(((d.i + 1) / (double)テスト項目最新.Count()) * 100);
                        var 変化量 = NextProgValue - CurrentProgValue;
                        foreach (var p in Enumerable.Range(1, 変化量))
                        {
                            State.VmTestStatus.進捗度 = CurrentProgValue + p;
                            if (State.VmTestStatus.CheckUnitTest == false)
                            {
                                Thread.Sleep(10);
                            }
                        }
                    });
                    if (Flags.ClickStopButton) goto FAIL;
                }


                //↓↓すべての項目が合格した時の処理です↓↓
                General.ResetIo();
                WaveFormGenerator.SourceOff();
                HIOKI7012.StopSource();
                await Task.Delay(500);
                State.VmTestStatus.Message = Constants.MessRemove;
                State.VmTestStatus.StartButtonContent = Constants.確認;
                State.VmTestStatus.StartButtonEnable = false;

                //通しで試験した時は検査データを保存する
                if (State.VmTestStatus.CheckUnitTest != true) //null or False アプリ立ち上げ時はnullになっている！
                {
                    if (!General.SaveTestData())
                    {
                        FailStepNo = 5000;
                        FailTitle = "検査データ保存";
                        goto FAIL_DATA_SAVE;
                    }
                }

                FlagTestTime = false;

                State.VmTestStatus.Colorlabel判定 = Brushes.AntiqueWhite;
                State.VmTestStatus.Decision = Flags.MetalMode ? "WIN" : "PASS";
                State.VmTestStatus.ColorDecision = effect判定表示PASS;

                ResetRing();
                SetDecision();
                SbPass();

                //通しで試験が合格したときの処理です(検査データを保存して、シリアルナンバーをインクリメントする)
                if (State.VmTestStatus.CheckUnitTest != true) //null or False アプリ立ち上げ時はnullになっている！
                {
                    //当日試験合格数をインクリメント ビューモデルはまだ更新しない
                    State.Setting.TodayOkCount++;

                    //これ重要！！！ シリアルナンバーをインクリメントし、次の試験に備える ビューモデルはまだ更新しない
                    State.NewSerial++;

                    Flags.ShowLabelPage = true;
                    General.PlaySound(General.soundPass);
                    await Task.Delay(3900);
                    State.VmTestStatus.StartButtonEnable = true;
                    return;
                }
                else
                {
                    State.VmTestStatus.Message = Constants.MessRemove;
                    Flags.ShowLabelPage = false;

                    State.VmTestStatus.StartButtonEnable = true;
                    StopButtonBlinkOn();
                    await Task.Run(() =>
                    {
                        while (true)
                        {
                            if (Flags.Click確認Button)
                            {
                                break;
                            }
                            Thread.Sleep(100);
                        }
                    });
                    StopButtonBlinkOff();
                    return;
                }


                //不合格時の処理
                FAIL:


                General.ResetIo();
                await Task.Delay(500);
                FAIL_DATA_SAVE:


                FlagTestTime = false;
                State.VmTestStatus.Message = Constants.MessRemove;
                State.VmTestStatus.StartButtonContent = Constants.確認;
                State.VmTestStatus.StartButtonEnable = true;

                //当日試験不合格数をインクリメント ビューモデルはまだ更新しない
                State.Setting.TodayNgCount++;
                await Task.Delay(100);

                State.VmTestStatus.Colorlabel判定 = Brushes.AliceBlue;
                State.VmTestStatus.Decision = "FAIL";
                State.VmTestStatus.ColorDecision = effect判定表示FAIL;

                SetErrorMessage(FailStepNo, FailTitle);

                var NgDataList = new List<string>()
                                    {
                                        State.VmMainWindow.Opecode,
                                        State.VmMainWindow.Operator,
                                        System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                                        State.VmTestStatus.FailInfo,
                                        State.VmTestStatus.Spec,
                                        State.VmTestStatus.MeasValue
                                    };

                General.SaveNgData(NgDataList);


                ResetRing();
                SetDecision();
                SetErrInfo();
                SbFail();

                General.PlaySound(General.soundFail);
                StopButtonBlinkOn();
                await Task.Run(() =>
                {
                    while (true)
                    {
                        if (Flags.Click確認Button)
                        {
                            break;
                        }
                        Thread.Sleep(100);
                    }
                });
                StopButtonBlinkOff();

                return;

            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("想定外の例外発生DEATH！！！\r\n申し訳ありませんが再起動してください");
                Environment.Exit(0);

            }
            finally
            {
                General.ResetIo();
                SbRingLoad();

                if (Flags.ShowLabelPage)
                {
                    State.uriOtherInfoPage = new Uri("Page/Test/ラベル貼り付け.xaml", UriKind.Relative);
                    State.VmMainWindow.TabIndex = 3;
                }
                else
                {
                    General.ResetViewModel();
                    RefreshDataContext();
                }
            }

        }

        //フォームきれいにする処理いろいろ
        private void ClearForm()
        {
            SbRingLoad();
            RefreshDataContext();
        }

        private void SetErrorMessage(int stepNo, string title)
        {
            if (Flags.ClickStopButton)
            {
                State.VmTestStatus.FailInfo = "エラーコード ---     強制停止";
            }
            else
            {
                State.VmTestStatus.FailInfo = "エラーコード " + stepNo.ToString("00") + "   " + title + "異常";
            }
        }

        //テストログの更新
        private void SetTestLog(string addData)
        {
            State.VmTestStatus.TestLog += addData;
        }

        private void ResetRing()
        {
            State.VmTestStatus.RingVisibility = System.Windows.Visibility.Hidden;

        }

        private void SetDecision()
        {
            State.VmTestStatus.DecisionVisibility = System.Windows.Visibility.Visible;
        }

        private void SetErrInfo()
        {
            State.VmTestStatus.ErrInfoVisibility = System.Windows.Visibility.Visible;
        }



    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using static System.Threading.Thread;
using static _30167671Tester.General;


namespace _30167671Tester
{
    public static class TEST_出力回路
    {
        public enum MODE { READ_V, READ_I_1, READ_I_2, OUTP_ON, OUTP_OFF1, OUTP_OFF2, INTERLOCK1, INTERLOCK2, INTERLOCK3 }
        public enum DV_CH { DV1, DV2, DV3, DV4, DV5, DV6, DV7, DV8, DV9, DV10, DV11, DV12, DV13, }

        public static bool resFormDirect { get; set; }



        private static System.Timers.Timer Tm;
        private static bool FlagTimer;

        static TEST_出力回路()
        {
            //タイマー（ウィンドウハンドル取得用）の設定
            Tm = new System.Timers.Timer();
            Tm.Enabled = false;
            Tm.Interval = 10000;
            Tm.Elapsed += new ElapsedEventHandler(tm_Tick);
        }

        //タイマーイベントハンドラ
        private static void tm_Tick(object source, ElapsedEventArgs e)
        {
            Tm.Stop();
            FlagTimer = false;//タイムアウト
        }


        public static bool SetFG_6_5Vrms()
        {
            try
            {
                double 入力電圧初期値 = 3.275;//Vrms
                WaveFormGenerator.サイン波出力(50.0, 50.0, 入力電圧初期値, 0.0);
                double buff = 入力電圧初期値;
                Tm.Stop();
                Tm.Interval = 18000;
                FlagTimer = true;
                Tm.Start();
                while (true)
                {
                    if (!FlagTimer || Flags.ClickStopButton) return false;
                    Application.DoEvents();
                    Hioki3239.GetAcVolt(Hioki3239.ACV_Range.R20V);
                    if (Hioki3239.VoltData >= 6.497 && Hioki3239.VoltData <= 6.503) break;

                    if (Hioki3239.VoltData < 6.497)
                    {
                        buff += 0.002;
                        WaveFormGenerator.ChangeVoltage(buff);

                    }
                    else if (Hioki3239.VoltData > 6.503)
                    {
                        buff -= 0.002;
                        WaveFormGenerator.ChangeVoltage(buff);
                    }
                    Sleep(200);

                }
                return true;
            }
            catch
            {
                return false;
            }

        }

        public static bool SetFG_5_7Vrms()
        {
            try
            {
                double 入力電圧初期値 = 2.85;//Vrms
                WaveFormGenerator.サイン波出力(50.0, 50.0, 入力電圧初期値, 0.0);

                return true;
            }
            catch
            {
                return false;
            }

        }

        public static async Task<bool> CheckAN_P(MODE mode)
        {
            bool result = false;
            bool result上限 = false;
            bool result下限 = false;

            string Data = "";

            const int numSamples = 10;
            int offset = 0;

            //ローカル関数の定義■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
            List<double> GetData()
            {
                General.ClearCommlog();
                Target.SendData("ANB_P");

                while (true)//取り込んだ通信データが規定行数(サンプリング数)になるまで待つ
                {
                    if (Flags.ClickStopButton) return null;
                    if (General.CountNewline() == numSamples + 2) break;
                }
                Target.Escape();
                Sleep(1500);
                int 検索開始位置 = 0;
                var List = new List<double>();
                foreach (var i in Enumerable.Range(0, numSamples))
                {
                    var log = State.VmComm.RX;
                    int FoundIndex = log.IndexOf("ANB_P,", 検索開始位置);
                    int 改行位置 = log.IndexOf("\r\n", FoundIndex);
                    var 取り出し1行 = log.Substring(FoundIndex, 改行位置 - FoundIndex);
                    var dataList = 取り出し1行.Split(',');

                    List.Add(Double.Parse(dataList[offset]));
                    検索開始位置 = FoundIndex + 1;
                }
                return List;

            };
            //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

            try
            {
                var mess = "";
                switch (mode)
                {
                    case MODE.READ_V:
                        mess = "①FGと50/60Hz入力を接続する\r\n②チェッカーの50/60Hz切替を上向きにする";
                        break;
                    case MODE.READ_I_1:
                        mess = "①チェッカーのカレントトランス1,2入力切替を上向きにする\r\n②FGをカレントトランス外部入力＜１＞に接続する";
                        break;
                    case MODE.READ_I_2:
                        mess = "①チェッカーのカレントトランス1,2入力切替を上向きにする\r\n②FGをカレントトランス外部入力＜２＞に接続する";
                        break;
                }
                var dialog = new DialogPic(mess, DialogPic.NAME.その他);
                dialog.ShowDialog();

                return await Task<bool>.Run(() =>
                {
                    //電源ONする処理
                    General.PowerSupply(true);
                    if (!General.CheckDemo表示())
                        return false;

                    Sleep(1200);
                    switch (mode)
                    {
                        case MODE.READ_V:
                            offset = 3;
                            // マルチメータで出力値を計測しながらFGを調整する
                            General.Set3229Meas(General.MEAS3229_CH.CN11_1);//マルチメータ切り替え処理を記述
                            if (!SetFG_6_5Vrms()) return false;
                            break;
                        case MODE.READ_I_1:
                            offset = 4;
                            // FG出力値は固
                            if (!SetFG_5_7Vrms()) return false;
                            break;
                        case MODE.READ_I_2:
                            offset = 5;
                            // FG出力値は固定
                            if (!SetFG_5_7Vrms()) return false;
                            break;
                    }

                    Sleep(500);
                    var ListData_ = GetData();
                    if (ListData_ == null) return false;

                    //安定後のデータを取得する
                    //最後から5ヶのデータをサンプリング値として取得する（前半は値が安定していない可能性があるため）
                    //5個飛ばして5個を抽出(インデックスが5から10)
                    var ListData = ListData_.Skip(5).Take(5).ToList<double>();

                    switch (mode)
                    {
                        case MODE.READ_V:
                            result下限 = ListData.All(data => data >= State.TestSpec.電圧換算値_Min);
                            result上限 = ListData.All(data => data <= State.TestSpec.電圧換算値_Max);
                            result = result下限 && result上限;
                            break;
                        case MODE.READ_I_1:
                        case MODE.READ_I_2:
                            ListData.Sort();
                            result下限 = ListData[2] >= State.TestSpec.電流換算値_Min;//中央値で判定
                            result上限 = ListData[2] <= State.TestSpec.電流換算値_Max;//中央値で判定
                            //result下限 = ListData.All(data => data >= State.testSpec.電流換算値_Min);//初期の頃の判定方法
                            //result上限 = ListData.All(data => data <= State.testSpec.電流換算値_Max);//初期の頃の判定方法
                            result = result下限 && result上限;
                            break;
                    }



                    ListData.Sort();
                    if (result)
                    {
                        Data = ListData[2].ToString("F1");//中央値
                    }
                    else
                    {
                        if (!result下限)
                        {
                            Data = ListData[0].ToString("F1");//Min
                        }
                        else
                        {
                            Data = ListData[4].ToString("F1");//Max
                        }
                    }

                    return result;
                });
            }
            catch
            {
                return false;
            }
            finally
            {
                WaveFormGenerator.SourceOff();
                General.PowerSupply(false);

                //ビューモデルの更新
                switch (mode)
                {
                    case MODE.READ_V:
                        State.VmTestResults.VolConverted = Data + "Vrms";
                        State.VmTestResults.ColVolConverted = result ? OffBrush : NgBrush;
                        break;
                    case MODE.READ_I_1:
                        State.VmTestResults.CT1 = Data + "Arms";
                        State.VmTestResults.ColCT1 = result ? OffBrush : NgBrush;
                        break;
                    case MODE.READ_I_2:
                        State.VmTestResults.CT2 = Data + "Arms";
                        State.VmTestResults.ColCT2 = result ? OffBrush : NgBrush;
                        break;
                }

            }

        }



        public static async Task<bool> CheckPWTMP_A(DV_CH ch)
        {

            bool result0 = false;
            bool result150 = false;
            bool result300 = false;

            string Data0 = "";
            string Data150 = "";
            string Data300 = "";



            try
            {
                return await Task<bool>.Run(() =>
                {
                    switch (ch)
                    {
                        case DV_CH.DV9:
                            General.Set7012Meas(General.MEAS_CH.DV9);
                            break;
                        case DV_CH.DV11:
                            General.Set7012Meas(General.MEAS_CH.DV11);
                            break;
                    }

                    //電源ONする処理
                    General.PowerSupply(true);
                    if (!General.CheckDemo表示())
                        return false;

                    Target.SendData("PWTMP_A 0 0");
                    Sleep(2500);
                    HIOKI7012.MeasureDcV(); var Meas0 = HIOKI7012.VoltData * 10;

                    Target.SendData("PWTMP_A 150 150");
                    Sleep(2500);
                    HIOKI7012.MeasureDcV(); var Meas150 = HIOKI7012.VoltData * 10;

                    Target.SendData("PWTMP_A 300 300");
                    Sleep(2500);
                    HIOKI7012.MeasureDcV(); var Meas300 = HIOKI7012.VoltData * 10;

                    result0 = (State.TestSpec.Temp_A_0_Min <= Meas0 && Meas0 <= State.TestSpec.Temp_A_0_Max);
                    result150 = (State.TestSpec.Temp_A_150_Min <= Meas150 && Meas150 <= State.TestSpec.Temp_A_150_Max);
                    result300 = (State.TestSpec.Temp_A_300_Min <= Meas300 && Meas300 <= State.TestSpec.Temp_A_300_Max);

                    Data0 = Meas0.ToString("F2") + "mA";
                    Data150 = Meas150.ToString("F2") + "mA";
                    Data300 = Meas300.ToString("F2") + "mA";

                    return result0 && result150 && result300;

                });
            }
            catch
            {
                return false;
            }
            finally
            {
                General.PowerSupply(false);
                switch (ch)
                {
                    case DV_CH.DV9:
                        State.VmTestResults.DV9_0 = Data0;
                        State.VmTestResults.DV9_150 = Data150;
                        State.VmTestResults.DV9_300 = Data300;
                        State.VmTestResults.ColDV9_0 = result0 ? OffBrush : NgBrush;
                        State.VmTestResults.ColDV9_150 = result150 ? OffBrush : NgBrush;
                        State.VmTestResults.ColDV9_300 = result300 ? OffBrush : NgBrush;
                        break;
                    case DV_CH.DV11:
                        State.VmTestResults.DV11_0 = Data0;
                        State.VmTestResults.DV11_150 = Data150;
                        State.VmTestResults.DV11_300 = Data300;
                        State.VmTestResults.ColDV11_0 = result0 ? OffBrush : NgBrush;
                        State.VmTestResults.ColDV11_150 = result150 ? OffBrush : NgBrush;
                        State.VmTestResults.ColDV11_300 = result300 ? OffBrush : NgBrush;
                        break;
                }
            }

        }
        public static async Task<bool> CheckPWTMP_V(DV_CH ch)
        {


            bool result0 = false;
            bool result40 = false;
            bool result75 = false;

            string Data0 = "";
            string Data40 = "";
            string Data75 = "";



            try
            {
                return await Task<bool>.Run(() =>
                {
                    switch (ch)
                    {
                        case DV_CH.DV10:
                            General.Set7012Meas(General.MEAS_CH.DV10);
                            break;
                        case DV_CH.DV12:
                            General.Set7012Meas(General.MEAS_CH.DV12);
                            break;
                    }

                    //電源ONする処理
                    General.PowerSupply(true);
                    if (!General.CheckDemo表示())
                        return false;

                    Target.SendData("PWTMP_V 0 0");
                    Sleep(1500);
                    HIOKI7012.MeasureDcV(); var Meas0 = HIOKI7012.VoltData;

                    Target.SendData("PWTMP_V 40 40");
                    Sleep(1500);
                    HIOKI7012.MeasureDcV(); var Meas40 = HIOKI7012.VoltData;

                    Target.SendData("PWTMP_V 75 75");
                    Sleep(1500);
                    HIOKI7012.MeasureDcV(); var Meas75 = HIOKI7012.VoltData;

                    result0 = (State.TestSpec.Temp_V_0_Min <= Meas0 && Meas0 <= State.TestSpec.Temp_V_0_Max);
                    result40 = (State.TestSpec.Temp_V_40_Min <= Meas40 && Meas40 <= State.TestSpec.Temp_V_40_Max);
                    result75 = (State.TestSpec.Temp_V_75_Min <= Meas75 && Meas75 <= State.TestSpec.Temp_V_75_Max);

                    Data0 = Meas0.ToString("F2") + "V";
                    Data40 = Meas40.ToString("F2") + "V";
                    Data75 = Meas75.ToString("F2") + "V";

                    return result0 && result40 && result75;

                });
            }
            catch
            {
                return false;
            }
            finally
            {
                General.PowerSupply(false);
                switch (ch)
                {
                    case DV_CH.DV10:
                        State.VmTestResults.DV10_0 = Data0;
                        State.VmTestResults.DV10_40 = Data40;
                        State.VmTestResults.DV10_75 = Data75;
                        State.VmTestResults.ColDV10_0 = result0 ? OffBrush : NgBrush;
                        State.VmTestResults.ColDV10_40 = result40 ? OffBrush : NgBrush;
                        State.VmTestResults.ColDV10_75 = result75 ? OffBrush : NgBrush;
                        break;
                    case DV_CH.DV12:
                        State.VmTestResults.DV12_0 = Data0;
                        State.VmTestResults.DV12_40 = Data40;
                        State.VmTestResults.DV12_75 = Data75;
                        State.VmTestResults.ColDV12_0 = result0 ? OffBrush : NgBrush;
                        State.VmTestResults.ColDV12_40 = result40 ? OffBrush : NgBrush;
                        State.VmTestResults.ColDV12_75 = result75 ? OffBrush : NgBrush;
                        break;
                }
            }
        }

        public static async Task<bool> CheckPWINV()
        {


            bool result0 = false;
            bool result1800 = false;
            bool result3600 = false;

            string Data0 = "";
            string Data1800 = "";
            string Data3600 = "";



            try
            {
                return await Task<bool>.Run(() =>
                {
                    General.Set7012Meas(General.MEAS_CH.DV13);

                    //電源ONする処理
                    General.PowerSupply(true);
                    if (!General.CheckDemo表示())
                        return false;

                    Target.SendData("PWINV 0");
                    Sleep(1500);
                    HIOKI7012.MeasureDcV(); var Meas0 = HIOKI7012.VoltData;

                    Target.SendData("PWINV 1800");
                    Sleep(1500);
                    HIOKI7012.MeasureDcV(); var Meas1800 = HIOKI7012.VoltData;

                    Target.SendData("PWINV 3600");
                    Sleep(1500);
                    HIOKI7012.MeasureDcV(); var Meas3600 = HIOKI7012.VoltData;

                    result0 = (State.TestSpec.Inv_0_Min <= Meas0 && Meas0 <= State.TestSpec.Inv_0_Max);
                    result1800 = (State.TestSpec.Inv_1800_Min <= Meas1800 && Meas1800 <= State.TestSpec.Inv_1800_Max);
                    result3600 = (State.TestSpec.Inv_3600_Min <= Meas3600 && Meas3600 <= State.TestSpec.Inv_3600_Max);

                    Data0 = Meas0.ToString("F2") + "V";
                    Data1800 = Meas1800.ToString("F2") + "V";
                    Data3600 = Meas3600.ToString("F2") + "V";

                    return result0 && result1800 && result3600;

                });
            }
            catch
            {
                return false;
            }
            finally
            {
                General.PowerSupply(false);
                State.VmTestResults.DV13_0 = Data0;
                State.VmTestResults.DV13_1800 = Data1800;
                State.VmTestResults.DV13_3600 = Data3600;
                State.VmTestResults.ColDV13_0 = result0 ? OffBrush : NgBrush;
                State.VmTestResults.ColDV13_1800 = result1800 ? OffBrush : NgBrush;
                State.VmTestResults.ColDV13_3600 = result3600 ? OffBrush : NgBrush;
            }
        }

        public static async Task<bool> CheckPWIOUT(DV_CH ch)
        {


            bool result0 = false;
            bool result50 = false;
            bool result100 = false;

            string Data0 = "";
            string Data50 = "";
            string Data100 = "";

            try
            {
                return await Task<bool>.Run(() =>
                {
                    switch (ch)
                    {
                        case DV_CH.DV1:
                            General.Set7012Meas(General.MEAS_CH.DV1);
                            break;
                        case DV_CH.DV3:
                            General.Set7012Meas(General.MEAS_CH.DV3);
                            break;
                        case DV_CH.DV5:
                            General.Set7012Meas(General.MEAS_CH.DV5);
                            break;
                        case DV_CH.DV7:
                            General.Set7012Meas(General.MEAS_CH.DV7);
                            break;
                    }

                    //電源ONする処理
                    General.PowerSupply(true);
                    if (!General.CheckDemo表示())
                        return false;

                    Target.SendData("PWIOUT 0 0 0 0");
                    Sleep(1500);
                    HIOKI7012.MeasureDcV(); var Meas0 = HIOKI7012.VoltData * 10;

                    Target.SendData("PWIOUT 50 50 50 50");
                    Sleep(1500);
                    HIOKI7012.MeasureDcV(); var Meas50 = HIOKI7012.VoltData * 10;

                    Target.SendData("PWIOUT 100 100 100 100");
                    Sleep(1500);
                    HIOKI7012.MeasureDcV(); var Meas100 = HIOKI7012.VoltData * 10;

                    result0 = (State.TestSpec.Iout_0_Min <= Meas0 && Meas0 <= State.TestSpec.Iout_0_Max);
                    result50 = (State.TestSpec.Iout_50_Min <= Meas50 && Meas50 <= State.TestSpec.Iout_50_Max);
                    result100 = (State.TestSpec.Iout_100_Min <= Meas100 && Meas100 <= State.TestSpec.Iout_100_Max);

                    Data0 = Meas0.ToString("F2") + "mA";
                    Data50 = Meas50.ToString("F2") + "mA";
                    Data100 = Meas100.ToString("F2") + "mA";

                    return result0 && result50 && result100;

                });
            }
            catch
            {
                return false;
            }
            finally
            {
                General.PowerSupply(false);
                switch (ch)
                {
                    case DV_CH.DV1:
                        State.VmTestResults.DV1_0 = Data0;
                        State.VmTestResults.DV1_50 = Data50;
                        State.VmTestResults.DV1_100 = Data100;
                        State.VmTestResults.ColDV1_0 = result0 ? OffBrush : NgBrush;
                        State.VmTestResults.ColDV1_50 = result50 ? OffBrush : NgBrush;
                        State.VmTestResults.ColDV1_100 = result100 ? OffBrush : NgBrush;
                        break;
                    case DV_CH.DV3:
                        State.VmTestResults.DV3_0 = Data0;
                        State.VmTestResults.DV3_50 = Data50;
                        State.VmTestResults.DV3_100 = Data100;
                        State.VmTestResults.ColDV3_0 = result0 ? OffBrush : NgBrush;
                        State.VmTestResults.ColDV3_50 = result50 ? OffBrush : NgBrush;
                        State.VmTestResults.ColDV3_100 = result100 ? OffBrush : NgBrush;
                        break;
                    case DV_CH.DV5:
                        State.VmTestResults.DV5_0 = Data0;
                        State.VmTestResults.DV5_50 = Data50;
                        State.VmTestResults.DV5_100 = Data100;
                        State.VmTestResults.ColDV5_0 = result0 ? OffBrush : NgBrush;
                        State.VmTestResults.ColDV5_50 = result50 ? OffBrush : NgBrush;
                        State.VmTestResults.ColDV5_100 = result100 ? OffBrush : NgBrush;
                        break;
                    case DV_CH.DV7:
                        State.VmTestResults.DV7_0 = Data0;
                        State.VmTestResults.DV7_50 = Data50;
                        State.VmTestResults.DV7_100 = Data100;
                        State.VmTestResults.ColDV7_0 = result0 ? OffBrush : NgBrush;
                        State.VmTestResults.ColDV7_50 = result50 ? OffBrush : NgBrush;
                        State.VmTestResults.ColDV7_100 = result100 ? OffBrush : NgBrush;
                        break;
                }
            }
        }
        public static async Task<bool> CheckPWVOUT(DV_CH ch)
        {
            bool result0 = false;
            bool result50 = false;
            bool result100 = false;

            string Data0 = "";
            string Data50 = "";
            string Data100 = "";

            try
            {
                return await Task<bool>.Run(() =>
                {
                    switch (ch)
                    {
                        case DV_CH.DV2:
                            General.Set7012Meas(General.MEAS_CH.DV2);
                            break;
                        case DV_CH.DV4:
                            General.Set7012Meas(General.MEAS_CH.DV4);
                            break;
                        case DV_CH.DV6:
                            General.Set7012Meas(General.MEAS_CH.DV6);
                            break;
                        case DV_CH.DV8:
                            General.Set7012Meas(General.MEAS_CH.DV8);
                            break;
                    }


                    //電源ONする処理
                    General.PowerSupply(true);
                    if (!General.CheckDemo表示())
                        return false;

                    Target.SendData("PWVOUT 0 0 0 0");
                    Sleep(1500);
                    HIOKI7012.MeasureDcV(); var Meas0 = HIOKI7012.VoltData;

                    Target.SendData("PWVOUT 50 50 50 50");
                    Sleep(1500);
                    HIOKI7012.MeasureDcV(); var Meas50 = HIOKI7012.VoltData;

                    Target.SendData("PWVOUT 100 100 100 100");
                    Sleep(1500);
                    HIOKI7012.MeasureDcV(); var Meas100 = HIOKI7012.VoltData;

                    result0 = (State.TestSpec.Vout_0_Min <= Meas0 && Meas0 <= State.TestSpec.Vout_0_Max);
                    result50 = (State.TestSpec.Vout_50_Min <= Meas50 && Meas50 <= State.TestSpec.Vout_50_Max);
                    result100 = (State.TestSpec.Vout_100_Min <= Meas100 && Meas100 <= State.TestSpec.Vout_100_Max);

                    Data0 = Meas0.ToString("F2") + "V";
                    Data50 = Meas50.ToString("F2") + "V";
                    Data100 = Meas100.ToString("F2") + "V";

                    return result0 && result50 && result100;

                });
            }
            catch
            {
                return false;
            }
            finally
            {
                General.PowerSupply(false);
                switch (ch)
                {
                    case DV_CH.DV2:
                        State.VmTestResults.DV2_0 = Data0;
                        State.VmTestResults.DV2_50 = Data50;
                        State.VmTestResults.DV2_100 = Data100;
                        State.VmTestResults.ColDV2_0 = result0 ? OffBrush : NgBrush;
                        State.VmTestResults.ColDV2_50 = result50 ? OffBrush : NgBrush;
                        State.VmTestResults.ColDV2_100 = result100 ? OffBrush : NgBrush;
                        break;
                    case DV_CH.DV4:
                        State.VmTestResults.DV4_0 = Data0;
                        State.VmTestResults.DV4_50 = Data50;
                        State.VmTestResults.DV4_100 = Data100;
                        State.VmTestResults.ColDV4_0 = result0 ? OffBrush : NgBrush;
                        State.VmTestResults.ColDV4_50 = result50 ? OffBrush : NgBrush;
                        State.VmTestResults.ColDV4_100 = result100 ? OffBrush : NgBrush;
                        break;
                    case DV_CH.DV6:
                        State.VmTestResults.DV6_0 = Data0;
                        State.VmTestResults.DV6_50 = Data50;
                        State.VmTestResults.DV6_100 = Data100;
                        State.VmTestResults.ColDV6_0 = result0 ? OffBrush : NgBrush;
                        State.VmTestResults.ColDV6_50 = result50 ? OffBrush : NgBrush;
                        State.VmTestResults.ColDV6_100 = result100 ? OffBrush : NgBrush;
                        break;
                    case DV_CH.DV8:
                        State.VmTestResults.DV8_0 = Data0;
                        State.VmTestResults.DV8_50 = Data50;
                        State.VmTestResults.DV8_100 = Data100;
                        State.VmTestResults.ColDV8_0 = result0 ? OffBrush : NgBrush;
                        State.VmTestResults.ColDV8_50 = result50 ? OffBrush : NgBrush;
                        State.VmTestResults.ColDV8_100 = result100 ? OffBrush : NgBrush;
                        break;
                }
            }
        }

        public static async Task<bool> CheckOUTP()
        {
            try
            {
                var re = await Task<bool>.Run(() =>
                {
                    //電源ONする処理
                    General.PowerSupply(true);
                    return General.CheckDemo表示();
                });

                if (!re) return false;

                Target.SendData("OUTP_ON");
                var dialog = new Dialog出力回路(MODE.OUTP_ON);
                dialog.ShowDialog();
                if (!Flags.DialogReturn) return false;


                Target.SendData("OUTP_OFF");
                dialog = new Dialog出力回路(MODE.OUTP_OFF1);
                dialog.ShowDialog();
                if (!Flags.DialogReturn) return false;

                await Task.Delay(1000);

                dialog = new Dialog出力回路(MODE.OUTP_OFF2);
                dialog.ShowDialog();
                if (!Flags.DialogReturn) return false;


                General.PowerSupply(false);
                await Task.Delay(500);
                General.PowerSupply(true);

                dialog = new Dialog出力回路(MODE.INTERLOCK1);
                dialog.ShowDialog();
                if (!Flags.DialogReturn) return false;

                dialog = new Dialog出力回路(MODE.INTERLOCK2);
                dialog.ShowDialog();
                if (!Flags.DialogReturn) return false;

                dialog = new Dialog出力回路(MODE.INTERLOCK3);
                dialog.ShowDialog();
                if (!Flags.DialogReturn) return false;

                var dialog2 = new DialogPic("チェッカーの外部非常停止SWを下向きにしてください", DialogPic.NAME.その他);
                dialog2.ShowDialog();
                return true;

            }
            catch
            {
                return false;
            }
            finally
            {
                General.PowerSupply(false);
            }

        }



    }
}

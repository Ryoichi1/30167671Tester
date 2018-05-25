using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Threading.Thread;
using static _30167671Tester.State;
using static _30167671Tester.General;



using System.IO;
using System.Drawing.Imaging;


namespace _30167671Tester
{
    public static class TEST_SCR駆動回路
    {

        public enum MODE { 位相制御, サイクル制御 }
        public static MODE testMode { get; set; }



        public static List<double> FilterData;//オシロから受信したデータをローパスフィルタ通したもの
        public static List<double> masterDataList = new List<double>();

        public static bool SetFG_6Vrms()
        {
            double value = WaveFormGenerator.Flag33220 ? 0.00025 : 0.001;
            try
            {
                double 入力電圧初期値 = 3.020;//Vrms
                double OutBuff = 0;
                WaveFormGenerator.サイン波出力(50.0, 50.0, 入力電圧初期値, 0.0);
                OutBuff = 入力電圧初期値;
                //AGI33220A.サイン波出力(50.0, 50.0, 入力電圧初期値, 0.0);

                var Tm = new GeneralTimer(15000);
                Tm.Start();
                while (true)
                {
                    if (Tm.FlagTimeout || Flags.ClickStopButton) return false;
                    Hioki3239.GetAcVolt(Hioki3239.ACV_Range.R20V);
                    if (Hioki3239.VoltData >= 6.000 && Hioki3239.VoltData <= 6.005) break;

                    if (Hioki3239.VoltData < 6.000)
                    {
                        OutBuff += value;
                        WaveFormGenerator.ChangeVoltage(OutBuff);
                        //AGI33220A.ChangeVoltage(入力電圧初期値 + (0.001 * cnt));

                    }
                    else if (Hioki3239.VoltData > 6.005)
                    {
                        OutBuff -= value;
                        WaveFormGenerator.ChangeVoltage(OutBuff);
                        //AGI33220A.ChangeVoltage(入力電圧初期値 - (0.001 * cnt));
                    }
                    Sleep(500);

                }
                return true;
            }
            catch
            {
                return false;
            }

        }

        public static bool GetWavData()
        {
            try
            {

                var dataList = new List<double>();
                FilterData = General.osc.GetWav();
                return (FilterData != null);
            }
            catch
            {
                return false;
            }

        }

        /// <summary>
        /// sw = true：正転での確認    false：反転での確認
        /// </summary>
        /// <param name="sw"></param>
        /// <returns></returns>
        private static double Check波形(MODE mode)
        {
            const double w = 1.75;//

            masterDataList.Clear();

            string buff = General.ReadCsv(mode == MODE.位相制御 ? Constants.PathMaster位相制御 : Constants.PathMasterサイクル制御);

            // カンマ区切りで分割して配列に格納する
            var strArray = buff.Split(',').ToList();
            strArray.ToList().ForEach(s => masterDataList.Add(Double.Parse(s)));

            int OkCount = 0;

            foreach (var i in Enumerable.Range(0, 600))
            {
                if (masterDataList[i] - w < FilterData[i] && FilterData[i] < masterDataList[i] + w) OkCount++;
            }

            return (OkCount / 600.0) * 100.0;


        }

        public static async Task<bool> Set()
        {
            return await Task<bool>.Run(() =>
            {
                //電源ONする処理
                General.PowerSupply(true);
                if (!General.CheckDemo表示()) return false;
                Sleep(1500);
                return SetFG_6Vrms();//ファンクションジェネレータを調整する
            });
        }

        public static async Task<bool> CheckWave(MODE mode)
        {
            //検査規格 変更の可能性はないので定数宣言する
            const double 波形一致率 = 80;//波形一致率80%以上を合格とする
            const double MinPeak = 8.0;
            const double MaxPeak = 10.6;

            bool result = false;
            bool resultPeak1 = false;
            bool resultPeak2 = false;
            bool resultMatch = false;

            double peak1 = 0;
            double peak2 = 0;
            double 一致率 = 0;

            int RetryCntCycle = 0;

            try
            {
                return await Task<bool>.Run(() =>
                {

                    //モードによりオシロの設定を変更する(時間軸の設定のみ)
                    if (mode == MODE.位相制御)
                        General.osc.Set位相制御();
                    else
                        General.osc.Setサイクル制御();

                    Sleep(1000);

                    //波形出力コマンド送信
                    RetryCicle:
                    if (mode == MODE.位相制御)
                    {
                        Target.SendData("MVP 50 50");
                        Sleep(3500);
                    }
                    else
                    {
                        Target.SendData("MVP 50 50");
                        Sleep(4000);
                        Target.SendData("MVC 50");
                        Sleep(3500);
                    }

                    //波形データ取得
                    if (!GetWavData()) return false;
                    Sleep(1000);

                    //ピークの判定
                    peak1 = General.osc.ReadPeak_P();
                    peak2 = General.osc.ReadPeak_M();
                    resultPeak1 = (peak1 >= MinPeak) && (peak1 <= MaxPeak);
                    resultPeak2 = (-peak2 >= MinPeak) && (-peak2 <= MaxPeak);

                    一致率 = Check波形(mode);
                    //波形判定
                    resultMatch = 一致率 > 波形一致率;

                    //サイクル制御の波形は、反転してしまうことがあるのでリトライする
                    if (mode == MODE.サイクル制御 && !resultMatch)
                    {
                        if (RetryCntCycle++ != 3)//3回リトライ
                        {
                            Target.Escape();
                            Sleep(1000);
                            goto RetryCicle;
                        }
                    }

                    return result = resultMatch && resultPeak1 && resultPeak2;
                });
            }
            catch
            {
                return false;
            }
            finally
            {
                if (mode == MODE.位相制御)
                {
                    VmTestResults.OnTime位相 = "5msec";
                    VmTestResults.OffTime位相 = "5msec";
                    VmTestResults.Peak1位相 = $"+Vp:{peak1.ToString("F1")}V";
                    VmTestResults.Peak2位相 = $"-Vp:{peak2.ToString("F1")}V";
                    VmTestResults.Wav位相 = $"{一致率.ToString("F0")}%";
                    VmTestResults.ColPeak1位相 = resultPeak1 ? OffBrush : NgBrush;
                    VmTestResults.ColPeak2位相 = resultPeak2 ? OffBrush : NgBrush;
                    VmTestResults.ColWav位相 = resultMatch ? OffBrush : NgBrush;
                }
                else
                {
                    VmTestResults.OnTimeサイクル = "5msec";
                    VmTestResults.OffTimeサイクル = "5msec";
                    VmTestResults.Peak1サイクル = $"+Vp:{peak1.ToString("F1")}V";
                    VmTestResults.Peak2サイクル = $"-Vp:{peak2.ToString("F1")}V";
                    VmTestResults.Wavサイクル = $"{一致率.ToString("F0")}%";
                    VmTestResults.ColPeak1サイクル = resultPeak1 ? OffBrush : NgBrush;
                    VmTestResults.ColPeak2サイクル = resultPeak2 ? OffBrush : NgBrush;
                    VmTestResults.ColWavサイクル = resultMatch ? OffBrush : NgBrush;
                }

                if (!resultMatch)
                {
                    testMode = mode;
                    State.uriOtherInfoPage = new Uri("Page/ErrInfo/ErrInfoWav.xaml", UriKind.Relative);
                    State.VmTestStatus.EnableButtonErrInfo = System.Windows.Visibility.Visible;
                }
            }
        }

        public static async Task<List<double>> GetMasterData(MODE mode)
        {
            try
            {
                return await Task<bool>.Run(() =>
                {

                    //電源ONする処理
                    General.PowerSupply(true);
                    if (!General.CheckDemo表示()) return null;
                    Sleep(1500);

                    if (!SetFG_6Vrms()) return null;//ファンクションジェネレータを調整する
                    Sleep(500);

                    //モードによりオシロの設定を変更する(時間軸の設定のみ)
                    if (mode == MODE.位相制御)
                        General.osc.Set位相制御();
                    else
                        General.osc.Setサイクル制御();

                    Sleep(1000);

                    //波形出力コマンド送信
                    if (mode == MODE.位相制御)
                    {
                        Target.SendData("MVP 50 50");
                        Sleep(3500);
                    }
                    else
                    {
                        Target.SendData("MVP 50 50");
                        Sleep(4000);
                        Target.SendData("MVC 50");
                        Sleep(3500);
                    }

                    //波形データ取得
                    if (!GetWavData()) return null;


                    return FilterData;
                });
            }
            catch
            {
                return null;
            }
            finally
            {
                WaveFormGenerator.SourceOff();
                General.PowerSupply(false);
            }
        }

    }
}

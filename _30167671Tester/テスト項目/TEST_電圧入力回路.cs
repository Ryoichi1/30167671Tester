using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Threading.Thread;
using static _30167671Tester.General;

namespace _30167671Tester
{
    class TEST_電圧入力回路
    {
        public enum MODE { V_1, V_2, V_3, V_4 }

        public static async Task<bool> SetInputV(MODE mode)
        {
            bool FlagTimeout = false;
            System.Timers.Timer Tm;

            //タイマー（ウィンドウハンドル取得用）の設定
            Tm = new System.Timers.Timer();
            Tm.Enabled = false;
            Tm.Interval = 10000;
            Tm.Elapsed += (o, e) =>
            {
                Tm.Stop();
                FlagTimeout = true;
            };


            bool result第一調整点 = false;
            bool result第二調整点 = false;

            string Data第一調整点 = "";
            string Data第二調整点 = "";

            string Data第一調整点再 = "";
            string Data第二調整点再 = "";

            State.VmTestStatus.TestLog += $" {mode.ToString()} 調整";

            //ローカル関数の定義■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
            string GetData(string command)
            {
                General.ClearCommlog();
                Target.SendData(command);

                Tm.Stop();
                FlagTimeout = false;
                Tm.Start();
                while (true)//取り込んだ通信データが規定行数(サンプリング数)になるまで待つ
                {
                    if (FlagTimeout || Flags.ClickStopButton) return null;
                    if (General.CountNewline() == 2) break;
                }
                Target.Escape();
                Sleep(1000);
                var log = State.VmComm.RX;
                int FoundIndex = log.IndexOf(command + ",");
                int 改行位置 = log.IndexOf("\r\n", FoundIndex);
                var 取り出し1行 = log.Substring(FoundIndex, 改行位置 - FoundIndex);
                var dataList = 取り出し1行.Split(',');

                return dataList[2].Substring(1);//一文字目がスペースなので削除

            };

            List<string> GetData再(string command)
            {
                General.ClearCommlog();
                Target.SendData(command);

                Tm.Stop();
                FlagTimeout = false;
                Tm.Start();
                while (true)//取り込んだ通信データが規定行数(サンプリング数)になるまで待つ
                {
                    if (FlagTimeout || Flags.ClickStopButton) return null;
                    if (General.CountNewline() == 2) break;
                }
                Target.Escape();
                Sleep(1000);
                var log = State.VmComm.RX;
                int FoundIndex = log.IndexOf(command + ",");
                int 改行位置 = log.IndexOf("\r\n", FoundIndex);
                var 取り出し1行 = log.Substring(FoundIndex, 改行位置 - FoundIndex);
                var dataList = 取り出し1行.Split(',');

                return new List<string>() { dataList[1].Substring(1), dataList[2].Substring(1) }; //一文字目がスペースなので削除

            };
            //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

            try
            {
                return await Task<bool>.Run(() =>
                {
                    string command = "";
                    switch (mode)
                    {
                        case MODE.V_1:
                            //メッセージボックス表示("CN2にシグナルソース（電圧端子）を接続してください");
                            General.Set7012Source(General.SOURCE_CH.V1);
                            command = "V1";
                            break;
                        case MODE.V_2:
                            //メッセージボックス表示("CN4にシグナルソース（電圧端子）を接続してください");
                            General.Set7012Source(General.SOURCE_CH.V2);
                            command = "V2";
                            break;
                        case MODE.V_3:
                            //メッセージボックス表示("CN6にシグナルソース（電圧端子）を接続してください");
                            General.Set7012Source(General.SOURCE_CH.V3);
                            command = "V3";
                            break;
                        case MODE.V_4:
                            //メッセージボックス表示("CN8にシグナルソース（電圧端子）を接続してください");
                            General.Set7012Source(General.SOURCE_CH.V4);
                            command = "V4";
                            break;
                    }
                    Sleep(500);
                    //電源ONする処理
                    General.PowerSupply(true);
                    if (!General.CheckDemo表示())
                        return false;

                    HIOKI7012.OutDcV(0.500);
                    Sleep(2000);
                    Data第一調整点 = GetData(command + "1");
                    if (Data第一調整点 == null) return false;
                    Sleep(500);

                    HIOKI7012.OutDcV(10.000);
                    Sleep(2000);
                    Data第二調整点 = GetData(command + "2");
                    if (Data第二調整点 == null) return false;
                    Sleep(500);

                    General.PowerSupply(false);
                    Sleep(1000);

                    //電源ONする処理
                    General.PowerSupply(true);
                    if (!General.CheckDemo表示())
                        return false;

                    var ListData再 = GetData再("@" + command + "*");
                    if (ListData再 == null) return false;

                    Data第一調整点再 = ListData再[0];
                    Data第二調整点再 = ListData再[1];

                    result第一調整点 = Data第一調整点 == Data第一調整点再;
                    result第二調整点 = Data第二調整点 == Data第二調整点再;

                    return result第一調整点 && result第二調整点;

                });
            }
            catch
            {
                return false;
            }
            finally
            {
                State.VmTestStatus.TestLog += result第一調整点 && result第二調整点 ? "---PASS\r\n" : "---FAIL\r\n";
                HIOKI7012.StopSource();
                General.PowerSupply(false);

                switch (mode)
                {
                    case MODE.V_1:
                        State.VmTestResults.V1_1 = Data第一調整点 + "h";
                        State.VmTestResults.V1_2 = Data第二調整点 + "h";
                        State.VmTestResults.V1_1RE = Data第一調整点再 + "h";
                        State.VmTestResults.V1_2RE = Data第二調整点再 + "h";
                        State.VmTestResults.ColV1_1 = result第一調整点 ? OffBrush : NgBrush;
                        State.VmTestResults.ColV1_2 = result第二調整点 ? OffBrush : NgBrush;
                        State.VmTestResults.ColV1_1RE = result第一調整点 ? OffBrush : NgBrush;
                        State.VmTestResults.ColV1_2RE = result第二調整点 ? OffBrush : NgBrush;
                        break;
                    case MODE.V_2:
                        State.VmTestResults.V2_1 = Data第一調整点 + "h";
                        State.VmTestResults.V2_2 = Data第二調整点 + "h";
                        State.VmTestResults.V2_1RE = Data第一調整点再 + "h";
                        State.VmTestResults.V2_2RE = Data第二調整点再 + "h";
                        State.VmTestResults.ColV2_1 = result第一調整点 ? OffBrush : NgBrush;
                        State.VmTestResults.ColV2_2 = result第二調整点 ? OffBrush : NgBrush;
                        State.VmTestResults.ColV2_1RE = result第一調整点 ? OffBrush : NgBrush;
                        State.VmTestResults.ColV2_2RE = result第二調整点 ? OffBrush : NgBrush;
                        break;
                    case MODE.V_3:
                        State.VmTestResults.V3_1 = Data第一調整点 + "h";
                        State.VmTestResults.V3_2 = Data第二調整点 + "h";
                        State.VmTestResults.V3_1RE = Data第一調整点再 + "h";
                        State.VmTestResults.V3_2RE = Data第二調整点再 + "h";
                        State.VmTestResults.ColV3_1 = result第一調整点 ? OffBrush : NgBrush;
                        State.VmTestResults.ColV3_2 = result第二調整点 ? OffBrush : NgBrush;
                        State.VmTestResults.ColV3_1RE = result第一調整点 ? OffBrush : NgBrush;
                        State.VmTestResults.ColV3_2RE = result第二調整点 ? OffBrush : NgBrush;
                        break;
                    case MODE.V_4:
                        State.VmTestResults.V4_1 = Data第一調整点 + "h";
                        State.VmTestResults.V4_2 = Data第二調整点 + "h";
                        State.VmTestResults.V4_1RE = Data第一調整点再 + "h";
                        State.VmTestResults.V4_2RE = Data第二調整点再 + "h";
                        State.VmTestResults.ColV4_1 = result第一調整点 ? OffBrush : NgBrush;
                        State.VmTestResults.ColV4_2 = result第二調整点 ? OffBrush : NgBrush;
                        State.VmTestResults.ColV4_1RE = result第一調整点 ? OffBrush : NgBrush;
                        State.VmTestResults.ColV4_2RE = result第二調整点 ? OffBrush : NgBrush;
                        break;
                }

            }
        }




        public static async Task<bool> CheckInputV(MODE mode)
        {
            await Task.Delay(1500);
            bool result = false;
            bool result下限 = false;
            bool result上限 = false;

            string resultData = "";

            const int SampleCnt = 5;

            var ListData = new List<double>();

            State.VmTestStatus.TestLog += $" {mode.ToString()} 5V確認";

            //ローカル関数の定義■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
            void GetData()
            {
                General.ClearCommlog();
                Target.SendData("V_IN");

                while (true)//取り込んだ通信データが規定行数(サンプリング数)になるまで待つ
                {
                    if (Flags.ClickStopButton) return;
                    if (General.CountNewline() == SampleCnt + 2) break;
                }
                Target.Escape();
                int offset = 0;
                switch (mode)
                {
                    case MODE.V_1:
                        offset = 1;
                        break;
                    case MODE.V_2:
                        offset = 2;
                        break;
                    case MODE.V_3:
                        offset = 3;
                        break;
                    case MODE.V_4:
                        offset = 4;
                        break;
                }


                int 検索開始位置 = 0;

                var log = State.VmComm.RX;
                foreach (var i in Enumerable.Range(0, SampleCnt))
                {
                    int FoundIndex = log.IndexOf("V_IN,", 検索開始位置);
                    int 改行位置 = log.IndexOf("\r\n", FoundIndex);
                    var 取り出し1行 = log.Substring(FoundIndex, 改行位置 - FoundIndex);
                    var dataList = 取り出し1行.Split(',');

                    ListData.Add(Double.Parse(dataList[offset]));
                    検索開始位置 = FoundIndex + 1;
                }
            };

            //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

            try
            {
                return await Task<bool>.Run(() =>
                {

                    //電源ONする処理
                    General.PowerSupply(true);
                    if (!General.CheckDemo表示())
                        return false;

                    HIOKI7012.OutDcV(5.000);
                    Sleep(2000);
                    GetData();
                    Sleep(500);
                    HIOKI7012.StopSource();
                    General.PowerSupply(false);

                    result下限 = ListData.All(data => data >= State.TestSpec.V_5V_Min);
                    result上限 = ListData.All(data => data <= State.TestSpec.V_5V_Max);
                    result = result下限 && result上限;

                    ListData.Sort();
                    if (result)
                    {
                        resultData = ListData[SampleCnt / 2].ToString("F3") + "V";//中央値
                    }
                    else
                    {
                        if (!result下限)
                        {
                            resultData = ListData[0].ToString("F3") + "V";//Min
                        }
                        else
                        {
                            resultData = ListData[SampleCnt - 1].ToString("F3") + "V";//Max
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
                State.VmTestStatus.TestLog += result? "---PASS\r\n" : "---FAIL\r\n";
                HIOKI7012.StopSource();
                General.PowerSupply(false);


                switch (mode)
                {
                    case MODE.V_1:
                        State.VmTestResults.V1_5V = resultData;
                        State.VmTestResults.ColV1_5V = result ? OffBrush : NgBrush;
                        break;
                    case MODE.V_2:
                        State.VmTestResults.V2_5V = resultData;
                        State.VmTestResults.ColV2_5V = result ? OffBrush : NgBrush;
                        break;
                    case MODE.V_3:
                        State.VmTestResults.V3_5V = resultData;
                        State.VmTestResults.ColV3_5V = result ? OffBrush : NgBrush;
                        break;
                    case MODE.V_4:
                        State.VmTestResults.V4_5V = resultData;
                        State.VmTestResults.ColV4_5V = result ? OffBrush : NgBrush;
                        break;
                }

            }
        }

    }
}

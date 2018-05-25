using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Threading.Thread;
using static _30167671Tester.General;

namespace _30167671Tester
{
    class TEST_電流入力回路
    {

        public enum NAME { I_1, I_2, I_3, I_4 }
        public enum POINT { FIRST, SECOND }

        private static void SetSourc(NAME name)
        {
            switch (name)
            {
                case NAME.I_1:
                    Set7012Source(SOURCE_CH.I1);
                    break;
                case NAME.I_2:
                    Set7012Source(SOURCE_CH.I2);
                    break;
                case NAME.I_3:
                    Set7012Source(SOURCE_CH.I3);
                    break;
                case NAME.I_4:
                    Set7012Source(SOURCE_CH.I4);
                    break;
            }
        }


        public static async Task<bool> SetInputI(NAME name)
        {
            bool result第一調整点 = false;
            bool result第二調整点 = false;

            string Data第一調整点 = "";
            string Data第二調整点 = "";

            string Data第一調整点再 = "";
            string Data第二調整点再 = "";

            State.VmTestStatus.TestLog += $" {name.ToString()} 調整";

            //ローカルの定義■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
            string GetData(string command)
            {
                General.ClearCommlog();
                Target.SendData(command);

                while (true)//取り込んだ通信データが規定行数(サンプリング数)になるまで待つ
                {
                    if (Flags.ClickStopButton) return null;
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

                while (true)//取り込んだ通信データが規定行数(サンプリング数)になるまで待つ
                {
                    if (Flags.ClickStopButton) return null;
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
                    SetSourc(name);
                    Sleep(1000);

                    string command = "";
                    switch (name)
                    {
                        case NAME.I_1:
                            //MessageBox.Show("CN2にシグナルソース（電流端子）を接続してください");
                            command = "I1";
                            break;
                        case NAME.I_2:
                            //MessageBox.Show("CN4にシグナルソース（電流端子）を接続してください");
                            command = "I2";
                            break;
                        case NAME.I_3:
                            //MessageBox.Show("CN6にシグナルソース（電流端子）を接続してください");
                            command = "I3";
                            break;
                        case NAME.I_4:
                            //MessageBox.Show("CN8にシグナルソース（電流端子）を接続してください");
                            command = "I4";
                            break;
                    }
                    Sleep(500);

                    //電源ONする処理
                    General.PowerSupply(true);
                    if (!General.CheckDemo表示())
                        return false;

                    HIOKI7012.OutDcI(0.000);
                    Sleep(2000);
                    Data第一調整点 = GetData(command + "1");
                    if (Data第一調整点 == null) return false;
                    Sleep(500);

                    HIOKI7012.OutDcI(20.000);
                    Sleep(2000);
                    Data第二調整点 = GetData(command + "2");
                    if (Data第二調整点 == null) return false;
                    Sleep(500);
                    HIOKI7012.StopSource();

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
                State.VmTestStatus.TestLog += result第一調整点 && result第二調整点? "---PASS\r\n" :"---FAIL\r\n";
                HIOKI7012.StopSource();
                General.PowerSupply(false);

                switch (name)
                {
                    case NAME.I_1:
                        State.VmTestResults.I1_1 = Data第一調整点 + "h";
                        State.VmTestResults.I1_2 = Data第二調整点 + "h";
                        State.VmTestResults.I1_1RE = Data第一調整点再 + "h";
                        State.VmTestResults.I1_2RE = Data第二調整点再 + "h";
                        State.VmTestResults.ColI1_1 = result第一調整点 ? OffBrush : NgBrush;
                        State.VmTestResults.ColI1_2 = result第二調整点 ? OffBrush : NgBrush;
                        State.VmTestResults.ColI1_1RE = result第一調整点 ? OffBrush : NgBrush;
                        State.VmTestResults.ColI1_2RE = result第二調整点 ? OffBrush : NgBrush;
                        break;
                    case NAME.I_2:
                        State.VmTestResults.I2_1 = Data第一調整点 + "h";
                        State.VmTestResults.I2_2 = Data第二調整点 + "h";
                        State.VmTestResults.I2_1RE = Data第一調整点再 + "h";
                        State.VmTestResults.I2_2RE = Data第二調整点再 + "h";
                        State.VmTestResults.ColI2_1 = result第一調整点 ? OffBrush : NgBrush;
                        State.VmTestResults.ColI2_2 = result第二調整点 ? OffBrush : NgBrush;
                        State.VmTestResults.ColI2_1RE = result第一調整点 ? OffBrush : NgBrush;
                        State.VmTestResults.ColI2_2RE = result第二調整点 ? OffBrush : NgBrush;
                        break;
                    case NAME.I_3:
                        State.VmTestResults.I3_1 = Data第一調整点 + "h";
                        State.VmTestResults.I3_2 = Data第二調整点 + "h";
                        State.VmTestResults.I3_1RE = Data第一調整点再 + "h";
                        State.VmTestResults.I3_2RE = Data第二調整点再 + "h";
                        State.VmTestResults.ColI3_1 = result第一調整点 ? OffBrush : NgBrush;
                        State.VmTestResults.ColI3_2 = result第二調整点 ? OffBrush : NgBrush;
                        State.VmTestResults.ColI3_1RE = result第一調整点 ? OffBrush : NgBrush;
                        State.VmTestResults.ColI3_2RE = result第二調整点 ? OffBrush : NgBrush;
                        break;
                    case NAME.I_4:
                        State.VmTestResults.I4_1 = Data第一調整点 + "h";
                        State.VmTestResults.I4_2 = Data第二調整点 + "h";
                        State.VmTestResults.I4_1RE = Data第一調整点再 + "h";
                        State.VmTestResults.I4_2RE = Data第二調整点再 + "h";
                        State.VmTestResults.ColI4_1 = result第一調整点 ? OffBrush : NgBrush;
                        State.VmTestResults.ColI4_2 = result第二調整点 ? OffBrush : NgBrush;
                        State.VmTestResults.ColI4_1RE = result第一調整点 ? OffBrush : NgBrush;
                        State.VmTestResults.ColI4_2RE = result第二調整点 ? OffBrush : NgBrush;
                        break;
                }
            }
        }




        public static async Task<bool> CheckInputI(NAME name)
        {
            await Task.Delay(1500);
            bool result = false;
            bool result下限 = false;
            bool result上限 = false;

            string resultData = "";

            const int SampleCnt = 5;

            var ListData = new List<double>();

            State.VmTestStatus.TestLog += $" {name.ToString()} 10mA確認";

            //ラムダ式の定義■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
            Action GetData = () =>
            {
                General.ClearCommlog();
                Target.SendData("I_IN");

                while (true)//取り込んだ通信データが規定行数(サンプリング数)になるまで待つ
                {
                    if (Flags.ClickStopButton) return;
                    if (General.CountNewline() == SampleCnt + 2) break;
                }
                Target.Escape();
                int offset = 0;
                switch (name)
                {
                    case NAME.I_1:
                        offset = 1;
                        break;
                    case NAME.I_2:
                        offset = 2;
                        break;
                    case NAME.I_3:
                        offset = 3;
                        break;
                    case NAME.I_4:
                        offset = 4;
                        break;
                }


                int 検索開始位置 = 0;
                var log = State.VmComm.RX;
                foreach (var i in Enumerable.Range(0, SampleCnt))
                {
                    int FoundIndex = log.IndexOf("I_IN,", 検索開始位置);
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
                    SetSourc(name);
                    Sleep(1000);

                    //電源ONする処理
                    General.PowerSupply(true);
                    if (!General.CheckDemo表示())
                        return false;

                    HIOKI7012.OutDcI(10.000);
                    Sleep(2000);
                    GetData();
                    Sleep(500);
                    HIOKI7012.StopSource();
                    General.PowerSupply(false);

                    result下限 = ListData.All(data => data >= State.TestSpec.I_10mA_Min);
                    result上限 = ListData.All(data => data <= State.TestSpec.I_10mA_Max);
                    result = result下限 && result上限;

                    ListData.Sort();
                    if (result)
                    {
                        resultData = ListData[SampleCnt / 2].ToString("F3") + "mA";//中央値
                    }
                    else
                    {
                        if (!result下限)
                        {
                            resultData = ListData[0].ToString("F3") + "mA";//Min
                        }
                        else
                        {
                            resultData = ListData[SampleCnt - 1].ToString("F3") + "mA";//Max
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
                State.VmTestStatus.TestLog += result? "---PASS\r\n" :"---FAIL\r\n";
                HIOKI7012.StopSource();
                General.PowerSupply(false);

                switch (name)
                {
                    case NAME.I_1:
                        State.VmTestResults.I1_10mA = resultData;
                        State.VmTestResults.ColI1_10mA = result ? OffBrush : NgBrush;
                        break;
                    case NAME.I_2:
                        State.VmTestResults.I2_10mA = resultData;
                        State.VmTestResults.ColI2_10mA = result ? OffBrush : NgBrush;
                        break;
                    case NAME.I_3:
                        State.VmTestResults.I3_10mA = resultData;
                        State.VmTestResults.ColI3_10mA = result ? OffBrush : NgBrush;
                        break;
                    case NAME.I_4:
                        State.VmTestResults.I4_10mA = resultData;
                        State.VmTestResults.ColI4_10mA = result ? OffBrush : NgBrush;
                        break;

                }
            }
        }




    }
}

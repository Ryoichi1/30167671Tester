using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Threading.Thread;
using static _30167671Tester.State;
using static _30167671Tester.General;

namespace _30167671Tester
{
    class TEST_PT100センサ断線
    {
        public enum MODE { A断線, B_1断線, B_2断線 }

        public static async Task<bool> CheckNormal()
        {
            bool resultPV1 = false;
            bool resultPV1下限 = false;
            bool resultPV1上限 = false;

            bool resultPV2 = false;
            bool resultPV2下限 = false;
            bool resultPV2上限 = false;

            bool resultPV3 = false;
            bool resultPV3下限 = false;
            bool resultPV3上限 = false;

            bool resultPV4 = false;
            bool resultPV4下限 = false;
            bool resultPV4上限 = false;

            string dataPV1 = "";
            string dataPV2 = "";
            string dataPV3 = "";
            string dataPV4 = "";

            const int SampleCnt = 5;

            var ListDataPV1 = new List<double>();
            var ListDataPV2 = new List<double>();
            var ListDataPV3 = new List<double>();
            var ListDataPV4 = new List<double>();


            //ラムダ式の定義■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
            Func<bool> GetData = () =>
            {
                try
                {
                    General.ClearCommlog();
                    Target.SendData("TEMP");

                    while (true)//取り込んだ通信データが規定行数(サンプリング数)になるまで待つ
                    {
                        if (Flags.ClickStopButton) return false;
                        if (General.CountNewline() == SampleCnt + 2) break;
                    }
                    Target.Escape();
                    Sleep(1000);

                    int 検索開始位置 = 0;
                    var log = State.VmComm.RX;
                    foreach (var i in Enumerable.Range(0, SampleCnt))
                    {
                        int FoundIndex = log.IndexOf("TEMP,", 検索開始位置);
                        int 改行位置 = log.IndexOf("\r\n", FoundIndex);
                        var 取り出し1行 = log.Substring(FoundIndex, 改行位置 - FoundIndex);
                        var dataList = 取り出し1行.Split(',');

                        ListDataPV1.Add(Double.Parse(dataList[1]));
                        ListDataPV2.Add(Double.Parse(dataList[2]));
                        ListDataPV3.Add(Double.Parse(dataList[3]));
                        ListDataPV4.Add(Double.Parse(dataList[4]));

                        検索開始位置 = FoundIndex + 1;
                    }

                    return true;
                }
                catch
                {
                    return false;
                }


            };

            //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

            try
            {
                var dialog = new DialogPic("①CN1,3,5,7にチェッカーからのケーブルを接続してください\r\n②チェッカーの断線スイッチをすべて下に向けてください", DialogPic.NAME.その他);
                dialog.ShowDialog();

                return await Task<bool>.Run(() =>
                {
                    //電源ONする処理
                    General.PowerSupply(true);
                    if (!General.CheckDemo表示())
                        return false;

                    if (!GetData()) return false;
                    Sleep(500);

                    General.PowerSupply(false);
                    Sleep(1000);

                    resultPV1下限 = ListDataPV1.All(data => data >= State.TestSpec.PT100_NormalTemp_Min);
                    resultPV1上限 = ListDataPV1.All(data => data <= State.TestSpec.PT100_NormalTemp_Max);
                    resultPV1 = resultPV1下限 && resultPV1上限;

                    resultPV2下限 = ListDataPV2.All(data => data >= State.TestSpec.PT100_NormalTemp_Min);
                    resultPV2上限 = ListDataPV2.All(data => data <= State.TestSpec.PT100_NormalTemp_Max);
                    resultPV2 = resultPV2下限 && resultPV2上限;

                    resultPV3下限 = ListDataPV3.All(data => data >= State.TestSpec.PT100_NormalTemp_Min);
                    resultPV3上限 = ListDataPV3.All(data => data <= State.TestSpec.PT100_NormalTemp_Max);
                    resultPV3 = resultPV3下限 && resultPV3上限;

                    resultPV4下限 = ListDataPV4.All(data => data >= State.TestSpec.PT100_NormalTemp_Min);
                    resultPV4上限 = ListDataPV4.All(data => data <= State.TestSpec.PT100_NormalTemp_Max);
                    resultPV4 = resultPV4下限 && resultPV4上限;


                    ListDataPV1.Sort();
                    if (resultPV1)
                    {
                        dataPV1 = ListDataPV1[SampleCnt / 2].ToString("F1") + "℃";//中央値
                    }
                    else
                    {
                        if (!resultPV1下限)
                        {
                            dataPV1 = ListDataPV1[0].ToString("F1") + "℃";//Min
                        }
                        else
                        {
                            dataPV1 = ListDataPV1[SampleCnt - 1].ToString("F1") + "℃";//Max
                        }
                    }

                    ListDataPV2.Sort();
                    if (resultPV2)
                    {
                        dataPV2 = ListDataPV2[SampleCnt / 2].ToString("F1") + "℃";//中央値
                    }
                    else
                    {
                        if (!resultPV2下限)
                        {
                            dataPV2 = ListDataPV2[0].ToString("F1") + "℃";//Min
                        }
                        else
                        {
                            dataPV2 = ListDataPV2[SampleCnt - 1].ToString("F1") + "℃";//Max
                        }
                    }

                    ListDataPV3.Sort();
                    if (resultPV3)
                    {
                        dataPV3 = ListDataPV3[SampleCnt / 2].ToString("F1") + "℃";//中央値
                    }
                    else
                    {
                        if (!resultPV3下限)
                        {
                            dataPV3 = ListDataPV3[0].ToString("F1") + "℃";//Min
                        }
                        else
                        {
                            dataPV3 = ListDataPV3[SampleCnt - 1].ToString("F1") + "℃";//Max
                        }
                    }

                    ListDataPV4.Sort();
                    if (resultPV4)
                    {
                        dataPV4 = ListDataPV4[SampleCnt / 2].ToString("F1") + "℃";//中央値
                    }
                    else
                    {
                        if (!resultPV4下限)
                        {
                            dataPV4 = ListDataPV4[0].ToString("F1") + "℃";//Min
                        }
                        else
                        {
                            dataPV4 = ListDataPV4[SampleCnt - 1].ToString("F1") + "℃";//Max
                        }
                    }

                    return resultPV1 && resultPV2 && resultPV3 && resultPV4;
                });
            }
            catch
            {
                return false;
            }
            finally
            {
                General.PowerSupply(false);

                VmTestResults.PV1_N = dataPV1;
                VmTestResults.PV2_N = dataPV2;
                VmTestResults.PV3_N = dataPV3;
                VmTestResults.PV4_N = dataPV4;
                VmTestResults.ColPV1_N = resultPV1 ? OffBrush : NgBrush;
                VmTestResults.ColPV2_N = resultPV2 ? OffBrush : NgBrush;
                VmTestResults.ColPV3_N = resultPV3 ? OffBrush : NgBrush;
                VmTestResults.ColPV4_N = resultPV4 ? OffBrush : NgBrush;

            }
        }

        public static async Task<bool> Check断線(MODE mode)
        {
            bool resultPV1 = false;
            bool resultPV2 = false;
            bool resultPV3 = false;
            bool resultPV4 = false;

            var ListDataPV1 = new List<string>();
            var ListDataPV2 = new List<string>();
            var ListDataPV3 = new List<string>();
            var ListDataPV4 = new List<string>();

            const string messA = "Circ.CUT";
            const string messB = "Circ.SHORT";

            const int SampleCnt = 3;

            //ラムダ式の定義■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
            Func<bool> GetData = () =>
            {
                try
                {
                    General.ClearCommlog();
                    Target.SendData("TEMP");

                    while (true)//取り込んだ通信データが規定行数(サンプリング数)になるまで待つ
                    {
                        if (Flags.ClickStopButton) return false;
                        if (General.CountNewline() == SampleCnt + 2) break;
                    }
                    Target.Escape();
                    Sleep(1000);

                    int 検索開始位置 = 0;
                    var log = VmComm.RX;
                    foreach (var i in Enumerable.Range(0, SampleCnt))
                    {
                        int FoundIndex = log.IndexOf("TEMP,", 検索開始位置);
                        int 改行位置 = log.IndexOf("\r\n", FoundIndex);
                        var 取り出し1行 = log.Substring(FoundIndex, 改行位置 - FoundIndex);
                        var dataList = 取り出し1行.Split(',');

                        ListDataPV1.Add(dataList[1].Substring(1));//一文字目がスペースのため
                        ListDataPV2.Add(dataList[2].Substring(1));
                        ListDataPV3.Add(dataList[3].Substring(1));
                        ListDataPV4.Add(dataList[4].Substring(1));

                        検索開始位置 = FoundIndex + 1;
                    }
                    return true;
                }
                catch
                {
                    return false;
                }

            };

            //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

            try
            {
                var dialogMess = "";
                switch (mode)
                {
                    case MODE.A断線:
                        dialogMess = "チェッカーのA線をすべて上に向けてください";
                        break;
                    case MODE.B_1断線:
                        dialogMess = "チェッカーのB線をすべて上に向けてください";
                        break;
                    case MODE.B_2断線:
                        dialogMess = "チェッカーのB’線をすべて上に向けてください";
                        break;
                }

                var dialog = new DialogPic(dialogMess, DialogPic.NAME.その他);
                dialog.ShowDialog();

                return await Task<bool>.Run(() =>
                {
                    //電源ONする処理
                    General.PowerSupply(true);
                    if (!General.CheckDemo表示())
                        return false;

                    if (!GetData()) return false;
                    Sleep(500);

                    General.PowerSupply(false);
                    Sleep(1000);

                    string mess = "";
                    if (mode == MODE.A断線)
                    {
                        mess = messA;
                    }
                    else
                    {
                        mess = messB;
                    }

                    resultPV1 = ListDataPV1.All(data => data == mess);
                    resultPV2 = ListDataPV1.All(data => data == mess);
                    resultPV3 = ListDataPV1.All(data => data == mess);
                    resultPV4 = ListDataPV1.All(data => data == mess);

                    return resultPV1 && resultPV2 && resultPV3 && resultPV4;
                });
            }
            catch
            {
                return false;
            }
            finally
            {
                General.PowerSupply(false);

                switch (mode)
                {
                    case MODE.A断線:
                        VmTestResults.PV1_A = resultPV1 ? "OK" : "NG";
                        VmTestResults.PV2_A = resultPV2 ? "OK" : "NG";
                        VmTestResults.PV3_A = resultPV3 ? "OK" : "NG";
                        VmTestResults.PV4_A = resultPV4 ? "OK" : "NG";
                        VmTestResults.ColPV1_A = resultPV1 ? OffBrush : NgBrush;
                        VmTestResults.ColPV2_A = resultPV2 ? OffBrush : NgBrush;
                        VmTestResults.ColPV3_A = resultPV3 ? OffBrush : NgBrush;
                        VmTestResults.ColPV4_A = resultPV4 ? OffBrush : NgBrush;
                        break;
                    case MODE.B_1断線:
                        VmTestResults.PV1_B1 = resultPV1 ? "OK" : "NG";
                        VmTestResults.PV2_B1 = resultPV2 ? "OK" : "NG";
                        VmTestResults.PV3_B1 = resultPV3 ? "OK" : "NG";
                        VmTestResults.PV4_B1 = resultPV4 ? "OK" : "NG";
                        VmTestResults.ColPV1_B1 = resultPV1 ? OffBrush : NgBrush;
                        VmTestResults.ColPV2_B1 = resultPV2 ? OffBrush : NgBrush;
                        VmTestResults.ColPV3_B1 = resultPV3 ? OffBrush : NgBrush;
                        VmTestResults.ColPV4_B1 = resultPV4 ? OffBrush : NgBrush;
                        break;
                    case MODE.B_2断線:
                        VmTestResults.PV1_B2 = resultPV1 ? "OK" : "NG";
                        VmTestResults.PV2_B2 = resultPV2 ? "OK" : "NG";
                        VmTestResults.PV3_B2 = resultPV3 ? "OK" : "NG";
                        VmTestResults.PV4_B2 = resultPV4 ? "OK" : "NG";
                        VmTestResults.ColPV1_B2 = resultPV1 ? OffBrush : NgBrush;
                        VmTestResults.ColPV2_B2 = resultPV2 ? OffBrush : NgBrush;
                        VmTestResults.ColPV3_B2 = resultPV3 ? OffBrush : NgBrush;
                        VmTestResults.ColPV4_B2 = resultPV4 ? OffBrush : NgBrush;
                        break;
                }






            }
        }


    }
}

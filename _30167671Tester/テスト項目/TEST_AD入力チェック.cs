using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Threading.Thread;
using static _30167671Tester.General;

namespace _30167671Tester
{
    public static class TEST_AD入力チェック
    {

        public static Action<string> メッセージボックス表示;

        public enum CH { ANA0, ANA1, ANA2, ANA3, ANA4, ANA5, ANA6, ANA7, ANA8, ANA9, ANA10, ANA11 }

        const int offsetAna0 = 1;
        const int offsetAna1 = 2;
        const int offsetAna2 = 3;
        const int offsetAna3 = 4;
        const int offsetAna4 = 5;
        const int offsetAna5 = 6;
        const int offsetAna6 = 7;
        const int offsetAna7 = 8;
        const int offsetAna8 = 9;
        const int offsetAna9 = 10;
        const int offsetAna10 = 11;
        const int offsetAna11 = 12;

        const int numSamples = 3;//サンプリング数はここで変更する
                                 //※奇数で指定すること(中央値を計測結果として表示するため)

        public static async Task<bool> CheckANA1_P(CH ch)
        {

            bool result0v = false;
            bool result5v = false;
            bool result10v = false;

            bool result0v下限 = false;
            bool result0v上限 = false;
            bool result5v下限 = false;
            bool result5v上限 = false;
            bool result10v下限 = false;
            bool result10v上限 = false;

            string Data0v = "";
            string Data5v = "";
            string Data10v = "";

            int offset = 0;

            //ローカル関数の定義■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
            List<double> GetData(double OutVal)
            {
                General.ClearCommlog();
                //シグナルソースから電圧出力
                HIOKI7012.OutDcV(OutVal);
                Sleep(1000);

                Target.SendData("ANA1_P");

                while (true)//取り込んだ通信データが規定行数(サンプリング数)になるまで待つ
                {
                    if (Flags.ClickStopButton) return null;
                    if (General.CountNewline() == numSamples + 1) break;
                }
                Target.Escape();
                Sleep(1500);
                HIOKI7012.StopSource();
                Sleep(400);
                int 検索開始位置 = 0;
                var List = new List<double>();
                foreach (var i in Enumerable.Range(0, numSamples))
                {
                    var log = State.VmComm.RX;
                    int FoundIndex = log.IndexOf("ANA1_P,", 検索開始位置);
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
                return await Task<bool>.Run(() =>
                {
                    switch (ch)
                    {
                        case CH.ANA0:
                            offset = offsetAna0;
                            Set7012Source(SOURCE_CH.A0);
                            break;
                        case CH.ANA1:
                            offset = offsetAna1;
                            Set7012Source(SOURCE_CH.A1);
                            break;
                        case CH.ANA2:
                            offset = offsetAna2;
                            Set7012Source(SOURCE_CH.A2);
                            break;
                        case CH.ANA3:
                            offset = offsetAna3;
                            Set7012Source(SOURCE_CH.A3);
                            break;
                        case CH.ANA4:
                            offset = offsetAna4;
                            Set7012Source(SOURCE_CH.A4);
                            break;
                        case CH.ANA5:
                            offset = offsetAna5;
                            Set7012Source(SOURCE_CH.A5);
                            break;
                        case CH.ANA6:
                            offset = offsetAna6;
                            Set7012Source(SOURCE_CH.A6);
                            break;
                        case CH.ANA7:
                            offset = offsetAna7;
                            Set7012Source(SOURCE_CH.A7);
                            break;
                        case CH.ANA8:
                            offset = offsetAna8;
                            Set7012Source(SOURCE_CH.A8);
                            break;
                        case CH.ANA9:
                            offset = offsetAna9;
                            Set7012Source(SOURCE_CH.A9);
                            break;
                        case CH.ANA10:
                            offset = offsetAna10;
                            Set7012Source(SOURCE_CH.A10);
                            break;
                        case CH.ANA11:
                            offset = offsetAna11;
                            Set7012Source(SOURCE_CH.A11);
                            break;
                    }
                    //電源ONする処理
                    General.PowerSupply(true);
                    if (!General.CheckDemo表示())
                        return false;

                    var List_Data0v = GetData(0.0);
                    var List_Data5v = GetData(5.0);
                    var List_Data10v = GetData(10.0);
                    if (List_Data0v == null || List_Data5v == null || List_Data10v == null) return false;

                    result0v下限 = List_Data0v.All(data => data >= State.TestSpec.AdInput_0Min);
                    result0v上限 = List_Data0v.All(data => data <= State.TestSpec.AdInput_0Max);
                    result0v = result0v下限 && result0v上限;

                    result5v下限 = List_Data5v.All(data => data >= State.TestSpec.AdInput_5Min);
                    result5v上限 = List_Data5v.All(data => data <= State.TestSpec.AdInput_5Max);
                    result5v = result5v下限 && result5v上限;

                    result10v下限 = List_Data10v.All(data => data >= State.TestSpec.AdInput_10Min);
                    result10v上限 = List_Data10v.All(data => data <= State.TestSpec.AdInput_10Max);
                    result10v = result10v下限 && result10v上限;

                    List_Data0v.Sort();
                    if (result0v)
                    {
                        Data0v = List_Data0v[numSamples / 2].ToString("F3");//中央値
                    }
                    else
                    {
                        if (!result0v下限)
                        {
                            Data0v = List_Data0v[0].ToString("F3");//Min
                        }
                        else
                        {
                            Data0v = List_Data0v[numSamples - 1].ToString("F3");//Max
                        }
                    }

                    List_Data5v.Sort();
                    if (result5v)
                    {
                        Data5v = List_Data5v[numSamples / 2].ToString("F3");//中央値
                    }
                    else
                    {
                        if (!result5v下限)
                        {
                            Data5v = List_Data5v[0].ToString("F3");//Min
                        }
                        else
                        {
                            Data5v = List_Data5v[numSamples - 1].ToString("F3");//Max
                        }
                    }

                    List_Data10v.Sort();
                    if (result10v)
                    {
                        Data10v = List_Data10v[numSamples / 2].ToString("F3");//中央値
                    }
                    else
                    {
                        if (!result10v下限)
                        {
                            Data10v = List_Data10v[0].ToString("F3");//Min
                        }
                        else
                        {
                            Data10v = List_Data10v[numSamples - 1].ToString("F3");//Max
                        }
                    }

                    return result0v && result5v && result10v;

                });
            }
            catch
            {
                return false;
            }
            finally
            {
                General.PowerSupply(false);
                Sleep(300);

                //ビューモデルの更新
                switch (ch)
                {
                    case CH.ANA0:
                        State.VmTestResults.A0_0 = Data0v;
                        State.VmTestResults.A0_5 = Data5v;
                        State.VmTestResults.A0_10 = Data10v;
                        State.VmTestResults.ColA0_0 = result0v ? OffBrush : NgBrush;
                        State.VmTestResults.ColA0_5 = result5v ? OffBrush : NgBrush;
                        State.VmTestResults.ColA0_10 = result10v ? OffBrush : NgBrush;
                        break;
                    case CH.ANA1:
                        State.VmTestResults.A1_0 = Data0v;
                        State.VmTestResults.A1_5 = Data5v;
                        State.VmTestResults.A1_10 = Data10v;
                        State.VmTestResults.ColA1_0 = result0v ? OffBrush : NgBrush;
                        State.VmTestResults.ColA1_5 = result5v ? OffBrush : NgBrush;
                        State.VmTestResults.ColA1_10 = result10v ? OffBrush : NgBrush;
                        break;
                    case CH.ANA2:
                        State.VmTestResults.A2_0 = Data0v;
                        State.VmTestResults.A2_5 = Data5v;
                        State.VmTestResults.A2_10 = Data10v;
                        State.VmTestResults.ColA2_0 = result0v ? OffBrush : NgBrush;
                        State.VmTestResults.ColA2_5 = result5v ? OffBrush : NgBrush;
                        State.VmTestResults.ColA2_10 = result10v ? OffBrush : NgBrush;
                        break;
                    case CH.ANA3:
                        State.VmTestResults.A3_0 = Data0v;
                        State.VmTestResults.A3_5 = Data5v;
                        State.VmTestResults.A3_10 = Data10v;
                        State.VmTestResults.ColA3_0 = result0v ? OffBrush : NgBrush;
                        State.VmTestResults.ColA3_5 = result5v ? OffBrush : NgBrush;
                        State.VmTestResults.ColA3_10 = result10v ? OffBrush : NgBrush;
                        break;
                    case CH.ANA4:
                        State.VmTestResults.A4_0 = Data0v;
                        State.VmTestResults.A4_5 = Data5v;
                        State.VmTestResults.A4_10 = Data10v;
                        State.VmTestResults.ColA4_0 = result0v ? OffBrush : NgBrush;
                        State.VmTestResults.ColA4_5 = result5v ? OffBrush : NgBrush;
                        State.VmTestResults.ColA4_10 = result10v ? OffBrush : NgBrush;
                        break;
                    case CH.ANA5:
                        State.VmTestResults.A5_0 = Data0v;
                        State.VmTestResults.A5_5 = Data5v;
                        State.VmTestResults.A5_10 = Data10v;
                        State.VmTestResults.ColA5_0 = result0v ? OffBrush : NgBrush;
                        State.VmTestResults.ColA5_5 = result5v ? OffBrush : NgBrush;
                        State.VmTestResults.ColA5_10 = result10v ? OffBrush : NgBrush;
                        break;
                    case CH.ANA6:
                        State.VmTestResults.A6_0 = Data0v;
                        State.VmTestResults.A6_5 = Data5v;
                        State.VmTestResults.A6_10 = Data10v;
                        State.VmTestResults.ColA6_0 = result0v ? OffBrush : NgBrush;
                        State.VmTestResults.ColA6_5 = result5v ? OffBrush : NgBrush;
                        State.VmTestResults.ColA6_10 = result10v ? OffBrush : NgBrush;
                        break;
                    case CH.ANA7:
                        State.VmTestResults.A7_0 = Data0v;
                        State.VmTestResults.A7_5 = Data5v;
                        State.VmTestResults.A7_10 = Data10v;
                        State.VmTestResults.ColA7_0 = result0v ? OffBrush : NgBrush;
                        State.VmTestResults.ColA7_5 = result5v ? OffBrush : NgBrush;
                        State.VmTestResults.ColA7_10 = result10v ? OffBrush : NgBrush;
                        break;
                    case CH.ANA8:
                        State.VmTestResults.A8_0 = Data0v;
                        State.VmTestResults.A8_5 = Data5v;
                        State.VmTestResults.A8_10 = Data10v;
                        State.VmTestResults.ColA8_0 = result0v ? OffBrush : NgBrush;
                        State.VmTestResults.ColA8_5 = result5v ? OffBrush : NgBrush;
                        State.VmTestResults.ColA8_10 = result10v ? OffBrush : NgBrush;
                        break;
                    case CH.ANA9:
                        State.VmTestResults.A9_0 = Data0v;
                        State.VmTestResults.A9_5 = Data5v;
                        State.VmTestResults.A9_10 = Data10v;
                        State.VmTestResults.ColA9_0 = result0v ? OffBrush : NgBrush;
                        State.VmTestResults.ColA9_5 = result5v ? OffBrush : NgBrush;
                        State.VmTestResults.ColA9_10 = result10v ? OffBrush : NgBrush;
                        break;
                    case CH.ANA10:
                        State.VmTestResults.A10_0 = Data0v;
                        State.VmTestResults.A10_5 = Data5v;
                        State.VmTestResults.A10_10 = Data10v;
                        State.VmTestResults.ColA10_0 = result0v ? OffBrush : NgBrush;
                        State.VmTestResults.ColA10_5 = result5v ? OffBrush : NgBrush;
                        State.VmTestResults.ColA10_10 = result10v ? OffBrush : NgBrush;
                        break;
                    case CH.ANA11:
                        State.VmTestResults.A11_0 = Data0v;
                        State.VmTestResults.A11_5 = Data5v;
                        State.VmTestResults.A11_10 = Data10v;
                        State.VmTestResults.ColA11_0 = result0v ? OffBrush : NgBrush;
                        State.VmTestResults.ColA11_5 = result5v ? OffBrush : NgBrush;
                        State.VmTestResults.ColA11_10 = result10v ? OffBrush : NgBrush;
                        break;

                }

            }
        }





























    }
}

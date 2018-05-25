using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Threading.Thread;
using static _30167671Tester.General;

namespace _30167671Tester
{
    static class TEST_パルス入力回路
    {
        public enum MODE { RIGHT, MIDDLE, LEFT }

        const int numSamples = 3;//サンプリング数はここで変更する
        //※奇数で指定すること(中央値を計測結果として表示するため)

        public static async Task<bool> CheckFLW(MODE mode)
        {

            bool result1 = false;
            bool result2 = false;
            bool result3 = false;

            bool result1下限 = false;
            bool result1上限 = false;
            bool result2下限 = false;
            bool result2上限 = false;
            bool result3下限 = false;
            bool result3上限 = false;

            string Data1 = "";
            string Data2 = "";
            string Data3 = "";

            var List_Data1 = new List<double>();
            var List_Data2 = new List<double>();
            var List_Data3 = new List<double>();


            //ローカル関数の定義■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
            bool GetData()
            {
                try
                {
                    Target.SendData("FLW");

                    while (true)//取り込んだ通信データが規定行数(サンプリング数)になるまで待つ
                    {
                        if (Flags.ClickStopButton) return false;
                        if (General.CountNewline() == numSamples + 2) break;
                    }
                    Target.Escape();
                    Sleep(1500);
                    int 検索開始位置 = 0;
                    foreach (var i in Enumerable.Range(0, numSamples))
                    {
                        var log = State.VmComm.RX;
                        int FoundIndex = log.IndexOf("FLW,", 検索開始位置);
                        int 改行位置 = log.IndexOf("\r\n", FoundIndex);
                        var 取り出し1行 = log.Substring(FoundIndex, 改行位置 - FoundIndex);
                        var dataList = 取り出し1行.Split(',');

                        List_Data1.Add(Double.Parse(dataList[1]));
                        List_Data2.Add(Double.Parse(dataList[2]));
                        List_Data3.Add(Double.Parse(dataList[3]));
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
                var mess = "";
                switch (mode)
                {
                    case MODE.LEFT:
                        mess = "チェッカーのパルス入力ＶＲを左いっぱいに回してください";
                        break;
                    case MODE.MIDDLE:
                        mess = "チェッカーのパルス入力ＶＲを真ん中にしてください";
                        break;
                    case MODE.RIGHT:
                        mess = "チェッカーのパルス入力ＶＲを右いっぱいに回してください";
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

                    if (!GetData()) return false;

                    double Min = 0;
                    double Max = 0;
                    switch (mode)//規格範囲の決定
                    {
                        case MODE.LEFT:
                            Min = State.TestSpec.Pulse_L_Min;
                            Max = State.TestSpec.Pulse_L_Max;
                            break;
                        case MODE.MIDDLE:
                            Min = State.TestSpec.Pulse_M_Min;
                            Max = State.TestSpec.Pulse_M_Max;
                            break;
                        case MODE.RIGHT:
                            Min = State.TestSpec.Pulse_R_Min;
                            Max = State.TestSpec.Pulse_R_Max;
                            break;

                    }



                    result1下限 = List_Data1.All(data => data >= Min);
                    result1上限 = List_Data1.All(data => data <= Max);
                    result1 = result1下限 && result1上限;

                    result2下限 = List_Data2.All(data => data >= Min);
                    result2上限 = List_Data2.All(data => data <= Max);
                    result2 = result2下限 && result2上限;

                    result3下限 = List_Data3.All(data => data >= Min);
                    result3上限 = List_Data3.All(data => data <= Max);
                    result3 = result3下限 && result3上限;

                    List_Data1.Sort();
                    if (result1)
                    {
                        Data1 = List_Data1[numSamples / 2].ToString("F1");//中央値
                    }
                    else
                    {
                        if (!result1下限)
                        {
                            Data1 = List_Data1[0].ToString("F1");//Min
                        }
                        else
                        {
                            Data1 = List_Data1[numSamples - 1].ToString("F1");//Max
                        }
                    }

                    List_Data2.Sort();
                    if (result2)
                    {
                        Data2 = List_Data2[numSamples / 2].ToString("F1");//中央値
                    }
                    else
                    {
                        if (!result2下限)
                        {
                            Data2 = List_Data2[0].ToString("F1");//Min
                        }
                        else
                        {
                            Data2 = List_Data2[numSamples - 1].ToString("F1");//Max
                        }
                    }

                    List_Data3.Sort();
                    if (result3)
                    {
                        Data3 = List_Data3[numSamples / 2].ToString("F1");//中央値
                    }
                    else
                    {
                        if (!result3下限)
                        {
                            Data3 = List_Data3[0].ToString("F1");//Min
                        }
                        else
                        {
                            Data3 = List_Data3[numSamples - 1].ToString("F1");//Max
                        }
                    }

                    return result1 && result2 && result3;

                });
            }
            catch
            {
                return false;
            }
            finally
            {
                General.PowerSupply(false);

                //ビューモデルの更新
                switch (mode)
                {
                    case MODE.LEFT:
                        State.VmTestResults.Pulse1L = Data1;
                        State.VmTestResults.Pulse2L = Data2;
                        State.VmTestResults.Pulse3L = Data3;
                        State.VmTestResults.ColPulse1L = result1 ? OffBrush : NgBrush;
                        State.VmTestResults.ColPulse2L = result2 ? OffBrush : NgBrush;
                        State.VmTestResults.ColPulse3L = result3 ? OffBrush : NgBrush;
                        break;
                    case MODE.MIDDLE:
                        State.VmTestResults.Pulse1M = Data1;
                        State.VmTestResults.Pulse2M = Data2;
                        State.VmTestResults.Pulse3M = Data3;
                        State.VmTestResults.ColPulse1M = result1 ? OffBrush : NgBrush;
                        State.VmTestResults.ColPulse2M = result2 ? OffBrush : NgBrush;
                        State.VmTestResults.ColPulse3M = result3 ? OffBrush : NgBrush;
                        break;
                    case MODE.RIGHT:
                        State.VmTestResults.Pulse1R = Data1;
                        State.VmTestResults.Pulse2R = Data2;
                        State.VmTestResults.Pulse3R = Data3;
                        State.VmTestResults.ColPulse1R = result1 ? OffBrush : NgBrush;
                        State.VmTestResults.ColPulse2R = result2 ? OffBrush : NgBrush;
                        State.VmTestResults.ColPulse3R = result3 ? OffBrush : NgBrush;
                        break;

                }
            }
        }






    }
}

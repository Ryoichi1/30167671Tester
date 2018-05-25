using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static System.Threading.Thread;
using static _30167671Tester.General;

namespace _30167671Tester
{
    static class TEST_VREF
    {
        public static async Task<bool> SetVref()
        {
            string HexStr = "";
            bool result = false;

            try
            {
                return await Task<bool>.Run(() =>
                {
                    Sleep(1000);
                    //電源ONする処理
                    General.PowerSupply(true);
                    if (!General.CheckDemo表示())
                        return false;

                    Target.SendData("VREFSET");

                    while (true)//取り込んだ通信データが規定行数(サンプリング数)になるまで待つ
                    {
                        if (Flags.ClickStopButton) return false;
                        if (General.CountNewline() == 2) break;
                    }
                    Target.Escape();

                    var log = State.VmComm.RX;
                    var FoundIndex = log.IndexOf("VREFSET,");
                    var 改行位置 = log.IndexOf("\r\n", FoundIndex);
                    var 取り出し1行 = log.Substring(FoundIndex, 改行位置 - FoundIndex);
                    var dataList = 取り出し1行.Split(',');
                    HexStr = dataList[2].Substring(1);//一文字目がスペースなので削除する
                    State.VmTestResults.Vref = HexStr + "h";
                    var value = Convert.ToInt32(HexStr, 16);

                    return result = (Convert.ToInt32(State.TestSpec.Vref調整_Min, 16) <= value && value <= Convert.ToInt32(State.TestSpec.Vref調整_Max, 16));

                });
            }
            catch
            {
                return false;
            }
            finally
            {
                General.PowerSupply(false);
                Sleep(2000);

                //VMの更新
                State.VmTestResults.ColVref_Re = result ? OffBrush : NgBrush;

            }
        }

        public static async Task<bool> CheckVref()
        {
            string HexStr = "";
            bool result = false;

            try
            {
                return await Task<bool>.Run(() =>
                {
                    //一回電源OFF→ONして再確認
                    General.PowerSupply(true);
                    Sleep(2000);
                    //デモ画面確認
                    if (!General.CheckDemo表示())
                        return false;
                    //通信ログクリア
                    General.ClearCommlog();

                    Target.SendData("@VREF*");

                    while (true)//取り込んだ通信データが規定行数(サンプリング数)になるまで待つ
                    {
                        if (Flags.ClickStopButton) return false;
                        if (General.CountNewline() == 2) break;
                    }
                    Target.Escape();

                    var log = State.VmComm.RX;
                    var FoundIndex = log.IndexOf("@VREF*,");
                    var 改行位置 = log.IndexOf("\r\n", FoundIndex);
                    var 取り出し1行 = log.Substring(FoundIndex, 改行位置 - FoundIndex);
                    var dataList = 取り出し1行.Split(',');
                    HexStr = dataList[1].Substring(1);//一文字目がスペースなので削除する
                    State.VmTestResults.Vref_Re = HexStr + "h";

                    var vrefData = State.VmTestResults.Vref.Trim('h');//末尾にhが付加されている 
                    result = (HexStr == vrefData);
                    return result;

                });
            }
            catch
            {
                return false;
            }
            finally
            {
                General.PowerSupply(false);

                //VMの更新
                State.VmTestResults.ColVref_Re = result ? OffBrush : NgBrush;
            }
        }

    }
}

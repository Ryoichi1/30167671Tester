using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Threading.Thread;
using static _30167671Tester.General;


namespace _30167671Tester
{
    class TEST_2線式液漏れセンサ回路
    {

        public static async Task<bool> SetLeak()
        {

            bool result調整点 = false;
            string Data調整点 = "";
            string Data調整点再 = "";

            State.VmTestStatus.TestLog += " 80kΩ調整";


            //ローカル関数の定義■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
            string GetData()
            {

                Target.SendData("LS11");

                while (true)//取り込んだ通信データが規定行数(サンプリング数)になるまで待つ
                {
                    if (Flags.ClickStopButton) return null;
                    if (General.CountNewline() == 2) break;
                }
                Target.Escape();
                Sleep(1000);
                var log = State.VmComm.RX;
                int FoundIndex = log.IndexOf("LS11,");
                int 改行位置 = log.IndexOf("\r\n", FoundIndex);
                var 取り出し1行 = log.Substring(FoundIndex, 改行位置 - FoundIndex);
                var dataList = 取り出し1行.Split(',');

                return dataList[2].Substring(1);//一文字目がスペースなので削除

            };

            string GetData再()
            {

                Target.SendData("LSR");

                while (true)//取り込んだ通信データが規定行数(サンプリング数)になるまで待つ
                {
                    if (Flags.ClickStopButton) return null;
                    if (General.CountNewline() == 2) break;
                }
                Target.Escape();
                Sleep(1000);
                var log = State.VmComm.RX;
                int FoundIndex = log.IndexOf("LSR,");
                int 改行位置 = log.IndexOf("\r\n", FoundIndex);
                var 取り出し1行 = log.Substring(FoundIndex, 改行位置 - FoundIndex);
                var dataList = 取り出し1行.Split(',');

                return dataList[1].Substring(1); //一文字目がスペースなので削除

            };
            //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

            try
            {
                var mess = "①チェッカーの終端抵抗切替 内部/外部スイッチを上向きにする\r\n" +
                                    "②チェッカーの漏液センサ(2線)端子とデジタル抵抗器を接続する\r\n" +
                                    "③デジタル抵抗器を80kΩに設定する";
                var dialog = new DialogPic(mess, DialogPic.NAME.その他);
                dialog.ShowDialog();
                return await Task<bool>.Run(() =>
                {
                    //電源ONする処理
                    General.PowerSupply(true);
                    if (!General.CheckDemo表示())
                        return false;

                    Data調整点 = GetData();
                    if (Data調整点 == null) return false;
                    Sleep(500);

                    General.PowerSupply(false);
                    Sleep(1000);

                    //電源ONする処理
                    General.PowerSupply(true);
                    if (!General.CheckDemo表示())
                        return false;

                    Data調整点再 = GetData再();
                    if (Data調整点再 == null) return false;

                    result調整点 = Data調整点 == Data調整点再;

                    return result調整点;

                });
            }
            catch
            {
                return false;
            }
            finally
            {
                State.VmTestStatus.TestLog += result調整点? "---PASS\r\n" :"---FAIL\r\n";
                General.PowerSupply(false);

                State.VmTestResults.Res80k = Data調整点 + "h";
                State.VmTestResults.Res80kRE = Data調整点再 + "h";
                State.VmTestResults.ColRes80k = result調整点 ? OffBrush : NgBrush;
                State.VmTestResults.ColRes80kRE = result調整点 ? OffBrush : NgBrush;
            }
        }




        public static async Task<bool> CheckLeak()
        {

            bool result = false;
            bool result下限 = false;
            bool result上限 = false;

            string resultData = "";

            const int SampleCnt = 20;

            State.VmTestStatus.TestLog += " 20kΩ確認";

            //ローカル関数の定義■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
            List<double> GetData()
            {
                var listData = new List<double>();
                Target.SendData("LS_R");

                while (true)//取り込んだ通信データが規定行数(サンプリング数)になるまで待つ
                {
                    if (Flags.ClickStopButton) return null;
                    if (General.CountNewline() == SampleCnt + 2) break;
                }
                Target.Escape();

                int 検索開始位置 = 0;
                var log = State.VmComm.RX;
                foreach (var i in Enumerable.Range(0, SampleCnt))
                {
                    int FoundIndex = log.IndexOf("LS_R,", 検索開始位置);
                    int 改行位置 = log.IndexOf("\r\n", FoundIndex);
                    var 取り出し1行 = log.Substring(FoundIndex, 改行位置 - FoundIndex);
                    var dataList = 取り出し1行.Split(',');

                    listData.Add(Double.Parse(dataList[1]));
                    検索開始位置 = FoundIndex + 1;
                }
                return listData;
            };

            //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

            try
            {
                var dialog = new DialogPic("デジタル抵抗器を20kΩに設定してください", DialogPic.NAME.その他);
                dialog.ShowDialog();

                return await Task<bool>.Run(() =>
                {
                    //電源ONする処理
                    General.PowerSupply(true);
                    if (!General.CheckDemo表示())
                        return false;

                    var ListData = GetData();
                    if (ListData == null) return false;

                    //最後から5ヶのデータをサンプリング値として取得する（前半は値が安定していない可能性があるため）
                    //15個飛ばして5個を抽出(インデックスが15から20)
                    var 安定後のデータ = ListData.Skip(15).Take(5).ToList<double>();

                    result下限 = 安定後のデータ.All(data => data >= State.TestSpec.液漏れ20k_Min);
                    result上限 = 安定後のデータ.All(data => data <= State.TestSpec.液漏れ20k_Max);
                    result = result下限 && result上限;

                    安定後のデータ.Sort();
                    if (result)
                    {
                        resultData = 安定後のデータ[安定後のデータ.Count() / 2].ToString("F2") + "kΩ";//中央値
                    }
                    else
                    {
                        if (!result下限)
                        {
                            resultData = 安定後のデータ[0].ToString("F2") + "kΩ";//Min
                        }
                        else
                        {
                            resultData = 安定後のデータ[安定後のデータ.Count() - 1].ToString("F2") + "kΩ";//Max
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
                General.PowerSupply(false);
                State.VmTestResults.Res20k = resultData;
                State.VmTestResults.ColRes20k = result ? OffBrush : NgBrush;

            }
        }
    }
}

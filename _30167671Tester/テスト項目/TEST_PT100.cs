using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Threading.Thread;
using static _30167671Tester.State;
using static _30167671Tester.General;

namespace _30167671Tester
{
    class TEST_PT100
    {
        public enum MODE { PV12, PV34 }
        public enum NAME_PV { PV1, PV2, PV3, PV4 }
        public enum POINT { FIRST, SECOND, THIRD }

        public static async Task<bool> CheckComm()
        {
            return await Task<bool>.Run(() =>
            {
                General.PowerSupply(true);
                return General.CheckDemo表示();
            });
        }


        public static async Task<bool> SetPT100(MODE mode, POINT point)
        {

            var result1 = false;
            var result2 = false;
            var strData1 = "";
            var strData2 = "";

            var mess = "";
            var max = 0.0;
            var min = 0.0;

            var cmd1 = "";
            var cmd2 = "";

            State.VmTestStatus.TestLog += $" {mode.ToString()} {point.ToString()}調整";

            //ローカル関数の定義■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
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
            //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

            try
            {
                await Task.Delay(500);
                if (!await CheckComm()) return false;

                switch (point)
                {
                    case POINT.FIRST:
                        max = State.TestSpec.PV第一Max;
                        min = State.TestSpec.PV第一Min;
                        mess = "ダイヤル抵抗器を108.96オームに設定してください";
                        cmd1 = mode == MODE.PV12 ? "PV11" : "PV31";
                        cmd2 = mode == MODE.PV12 ? "PV21" : "PV41";
                        General.PlaySound(General.sound108_96);
                        break;
                    case POINT.SECOND:
                        max = State.TestSpec.PV第二Max;
                        min = State.TestSpec.PV第二Min;
                        mess = "ダイヤル抵抗器を149.83オームに設定してください";
                        cmd1 = mode == MODE.PV12 ? "PV12" : "PV32";
                        cmd2 = mode == MODE.PV12 ? "PV22" : "PV42";
                        General.PlaySound(General.sound149_83);
                        break;
                    case POINT.THIRD:
                        max = State.TestSpec.PV第三Max;
                        min = State.TestSpec.PV第三Min;
                        mess = "ダイヤル抵抗器を183.19オームに設定してください";
                        cmd1 = mode == MODE.PV12 ? "PV13" : "PV33";
                        cmd2 = mode == MODE.PV12 ? "PV23" : "PV43";
                        General.PlaySound(General.sound183_19);
                        break;
                }

                var dialog = new DialogPic(mess, DialogPic.NAME.その他, soundSw: false);
                dialog.ShowDialog();
                await Task.Delay(4000);

                await Task.Run(() =>
                {
                    strData1 = GetData(cmd1);
                    strData2 = GetData(cmd2);

                });

                if (strData1 == null || strData2 == null)
                    return false;

                //第一調整点の調整値が適正な範囲内にあるかチェック
                int Data1 = Convert.ToInt32(strData1, 16);
                int Data2 = Convert.ToInt32(strData2, 16);

                result1 = min < Data1 && Data1 < max;
                result2 = min < Data2 && Data2 < max;

                return result1 && result2;
            }
            catch
            {
                return false;
            }
            finally
            {
                State.VmTestStatus.TestLog += result1 && result2? "---PASS\r\n" :"---FAIL\r\n";
                General.PowerSupply(false);

                if (mode == MODE.PV12)
                {
                    switch (point)
                    {
                        case POINT.FIRST:
                            VmTestResults.PV1_1 = strData1 + "h";
                            VmTestResults.PV2_1 = strData2 + "h";
                            VmTestResults.ColPV1_1 = result1 ? OffBrush : NgBrush;
                            VmTestResults.ColPV2_1 = result2 ? OffBrush : NgBrush;
                            break;
                        case POINT.SECOND:
                            VmTestResults.PV1_2 = strData1 + "h";
                            VmTestResults.PV2_2 = strData2 + "h";
                            VmTestResults.ColPV1_2 = result1 ? OffBrush : NgBrush;
                            VmTestResults.ColPV2_2 = result2 ? OffBrush : NgBrush;
                            break;
                        case POINT.THIRD:
                            VmTestResults.PV1_3 = strData1 + "h";
                            VmTestResults.PV2_3 = strData2 + "h";
                            VmTestResults.ColPV1_3 = result1 ? OffBrush : NgBrush;
                            VmTestResults.ColPV2_3 = result2 ? OffBrush : NgBrush;
                            break;
                    }
                }
                else
                {
                    switch (point)
                    {
                        case POINT.FIRST:
                            VmTestResults.PV3_1 = strData1 + "h";
                            VmTestResults.PV4_1 = strData2 + "h";
                            VmTestResults.ColPV3_1 = result1 ? OffBrush : NgBrush;
                            VmTestResults.ColPV4_1 = result2 ? OffBrush : NgBrush;
                            break;
                        case POINT.SECOND:
                            VmTestResults.PV3_2 = strData1 + "h";
                            VmTestResults.PV4_2 = strData2 + "h";
                            VmTestResults.ColPV3_2 = result1 ? OffBrush : NgBrush;
                            VmTestResults.ColPV4_2 = result2 ? OffBrush : NgBrush;
                            break;
                        case POINT.THIRD:
                            VmTestResults.PV3_3 = strData1 + "h";
                            VmTestResults.PV4_3 = strData2 + "h";
                            VmTestResults.ColPV3_3 = result1 ? OffBrush : NgBrush;
                            VmTestResults.ColPV4_3 = result2 ? OffBrush : NgBrush;
                            break;
                    }
                }

            }
        }


        public static async Task<bool> ReadPt100(NAME_PV name)
        {
            bool result1 = false;
            bool result2 = false;
            bool result3 = false;

            string Data1 = "";
            string Data2 = "";
            string Data3 = "";

            string Data1再 = "";
            string Data2再 = "";
            string Data3再 = "";

            State.VmTestStatus.TestLog += $" {name.ToString()} 読み出し";
            //ローカル関数の定義■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
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

                return new List<string>() { dataList[1].Substring(1), dataList[2].Substring(1), dataList[3].Substring(1) }; //一文字目がスペースなので削除

            };
            //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

            try
            {
                await Task.Delay(1500);//EEPROMの読み出し確認なので電源OFFして少し待つ
                if (!await CheckComm()) return false;
                return await Task<bool>.Run(() =>
                {
                    var cmd = "";
                    switch (name)
                    {
                        case NAME_PV.PV1:
                            cmd = "@PV1*";
                            Data1 = State.VmTestResults.PV1_1.Trim('h');
                            Data2 = State.VmTestResults.PV1_2.Trim('h');
                            Data3 = State.VmTestResults.PV1_3.Trim('h');
                            break;
                        case NAME_PV.PV2:
                            cmd = "@PV2*";
                            Data1 = State.VmTestResults.PV2_1.Trim('h');
                            Data2 = State.VmTestResults.PV2_2.Trim('h');
                            Data3 = State.VmTestResults.PV2_3.Trim('h');
                            break;
                        case NAME_PV.PV3:
                            cmd = "@PV3*";
                            Data1 = State.VmTestResults.PV3_1.Trim('h');
                            Data2 = State.VmTestResults.PV3_2.Trim('h');
                            Data3 = State.VmTestResults.PV3_3.Trim('h');
                            break;
                        case NAME_PV.PV4:
                            cmd = "@PV4*";
                            Data1 = State.VmTestResults.PV4_1.Trim('h');
                            Data2 = State.VmTestResults.PV4_2.Trim('h');
                            Data3 = State.VmTestResults.PV4_3.Trim('h');
                            break;
                    }

                    var List再 = GetData再(cmd);
                    if (List再 == null) return false;

                    Data1再 = List再[0];
                    Data2再 = List再[1];
                    Data3再 = List再[2];

                    result1 = Data1再 == Data1;
                    result2 = Data2再 == Data2;
                    result3 = Data3再 == Data3;

                    return result1 && result2 && result3;
                });
            }
            catch
            {
                return false;
            }
            finally
            {
                State.VmTestStatus.TestLog += result1 && result2 && result3? "---PASS\r\n" :"---FAIL\r\n";
                General.PowerSupply(false);

                switch (name)
                {
                    case NAME_PV.PV1:
                        State.VmTestResults.PV1_1RE = Data1再 + "h";
                        State.VmTestResults.PV1_2RE = Data2再 + "h";
                        State.VmTestResults.PV1_3RE = Data3再 + "h";
                        State.VmTestResults.ColPV1_1RE = result1 ? OffBrush : NgBrush;
                        State.VmTestResults.ColPV1_2RE = result2 ? OffBrush : NgBrush;
                        State.VmTestResults.ColPV1_3RE = result3 ? OffBrush : NgBrush;
                        break;
                    case NAME_PV.PV2:
                        State.VmTestResults.PV2_1RE = Data1再 + "h";
                        State.VmTestResults.PV2_2RE = Data2再 + "h";
                        State.VmTestResults.PV2_3RE = Data3再 + "h";
                        State.VmTestResults.ColPV2_1RE = result1 ? OffBrush : NgBrush;
                        State.VmTestResults.ColPV2_2RE = result2 ? OffBrush : NgBrush;
                        State.VmTestResults.ColPV2_3RE = result3 ? OffBrush : NgBrush;
                        break;
                    case NAME_PV.PV3:
                        State.VmTestResults.PV3_1RE = Data1再 + "h";
                        State.VmTestResults.PV3_2RE = Data2再 + "h";
                        State.VmTestResults.PV3_3RE = Data3再 + "h";
                        State.VmTestResults.ColPV3_1RE = result1 ? OffBrush : NgBrush;
                        State.VmTestResults.ColPV3_2RE = result2 ? OffBrush : NgBrush;
                        State.VmTestResults.ColPV3_3RE = result3 ? OffBrush : NgBrush;
                        break;
                    case NAME_PV.PV4:
                        State.VmTestResults.PV4_1RE = Data1再 + "h";
                        State.VmTestResults.PV4_2RE = Data2再 + "h";
                        State.VmTestResults.PV4_3RE = Data3再 + "h";
                        State.VmTestResults.ColPV4_1RE = result1 ? OffBrush : NgBrush;
                        State.VmTestResults.ColPV4_2RE = result2 ? OffBrush : NgBrush;
                        State.VmTestResults.ColPV4_3RE = result3 ? OffBrush : NgBrush;
                        break;
                }

            }
        }



        public static async Task<bool> Check60(MODE mode)
        {

            bool result1 = false;
            bool result1_temp下限 = false;
            bool result1_temp上限 = false;

            bool result2 = false;
            bool result2_temp下限 = false;
            bool result2_temp上限 = false;

            string Data1_temp = "";
            string Data2_temp = "";

            const int SampleCnt = 3;

            var List1 = new List<double>();
            var List2 = new List<double>();

            State.VmTestStatus.TestLog += $" {mode.ToString()} 60℃確認";

            //ローカル関数の定義■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
            void GetData()
            {

                Target.SendData("TEMP");

                while (true)//取り込んだ通信データが規定行数(サンプリング数)になるまで待つ
                {
                    if (Flags.ClickStopButton) return;
                    if (General.CountNewline() == SampleCnt + 2) break;
                }
                Target.Escape();
                int offset1;
                int offset2;
                if (mode == MODE.PV12)
                {
                    offset1 = 1; offset2 = 2;
                }
                else
                {
                    offset1 = 3; offset2 = 4;
                }

                var log = VmComm.RX;
                int 検索開始位置 = 0;
                foreach (var i in Enumerable.Range(0, SampleCnt))
                {
                    int FoundIndex = log.IndexOf("TEMP,", 検索開始位置);
                    int 改行位置 = log.IndexOf("\r\n", FoundIndex);
                    var 取り出し1行 = log.Substring(FoundIndex, 改行位置 - FoundIndex);
                    var dataList = 取り出し1行.Split(',');

                    List1.Add(Double.Parse(dataList[offset1]));
                    List2.Add(Double.Parse(dataList[offset2]));
                    検索開始位置 = FoundIndex + 1;
                }
            };

            //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

            try
            {
                await Task.Delay(500);
                //電源ONする処理
                General.PowerSupply(true);
                if (!General.CheckDemo表示())
                    return false;
                General.PlaySound(General.sound123_24);
                var dialog = new DialogPic("ダイヤル抵抗器を123.24オームに設定してください", DialogPic.NAME.その他, soundSw: false);
                dialog.ShowDialog();
                await Task.Delay(1500);
                GetData();


                General.PowerSupply(false);

                result1_temp下限 = List1.All(data => data >= State.TestSpec.Temp60_Min);
                result1_temp上限 = List1.All(data => data <= State.TestSpec.Temp60_Max);
                result1 = result1_temp下限 && result1_temp上限;

                result2_temp下限 = List2.All(data => data >= State.TestSpec.Temp60_Min);
                result2_temp上限 = List2.All(data => data <= State.TestSpec.Temp60_Max);
                result2 = result2_temp下限 && result2_temp上限;


                List1.Sort();
                if (result1)
                {
                    Data1_temp = List1[SampleCnt / 2].ToString("F2") + "℃";//中央値
                }
                else
                {
                    if (!result1_temp下限)
                    {
                        Data1_temp = List1[0].ToString("F2") + "℃";//Min
                    }
                    else
                    {
                        Data1_temp = List1[SampleCnt - 1].ToString("F2") + "℃";//Max
                    }
                }

                List2.Sort();
                if (result2)
                {
                    Data2_temp = List2[SampleCnt / 2].ToString("F2") + "℃";//中央値
                }
                else
                {
                    if (!result2_temp下限)
                    {
                        Data2_temp = List2[0].ToString("F2") + "℃";//Min
                    }
                    else
                    {
                        Data2_temp = List2[SampleCnt - 1].ToString("F2") + "℃";//Max
                    }
                }

                return result1 && result2;

            }
            catch
            {
                return false;
            }
            finally
            {
                State.VmTestStatus.TestLog += result1 && result2? "---PASS\r\n" :"---FAIL\r\n";
                General.PowerSupply(false);

                if (mode == MODE.PV12)
                {
                    VmTestResults.PV1_123_24 = Data1_temp;
                    VmTestResults.PV2_123_24 = Data2_temp;
                    VmTestResults.ColPV1_123_24 = result1 ? OffBrush : NgBrush;
                    VmTestResults.ColPV2_123_24 = result2 ? OffBrush : NgBrush;
                }
                else
                {
                    VmTestResults.PV3_123_24 = Data1_temp;
                    VmTestResults.PV4_123_24 = Data2_temp;
                    VmTestResults.ColPV3_123_24 = result1 ? OffBrush : NgBrush;
                    VmTestResults.ColPV4_123_24 = result2 ? OffBrush : NgBrush;
                }
            }
        }








    }
}

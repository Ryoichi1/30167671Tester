using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static _30167671Tester.State;
using static _30167671Tester.General;
using static System.Threading.Thread;

namespace _30167671Tester
{
    public static class TEST_入力回路チェック
    {
        public enum MODE { OFF, ON }

        //DINコマンドに対する応答
        public const string OFF_PA = "1000";
        public const string OFF_PB = "5E15";
        public const string OFF_PC = "800F";
        public const string OFF_PE = "0C30";
        public const string OFF_PH = "0038";
        public const string OFF_PL = "0000";

        public const string ON_PA = "EFFF";
        public const string ON_PB = "1E15";
        public const string ON_PC = "7F30";
        public const string ON_PE = "0C33";
        public const string ON_PH = "003C";
        public const string ON_PL = "0000";

        //DIN エラーメッセージ用データ
        public const string ERR_PA = "CN12,CN16, CN17";
        public const string ERR_PB = "CN10,CN21";
        public const string ERR_PC = "CN20";
        public const string ERR_PE = "CN15";
        public const string ERR_PH = "CN15";
        public const string ERR_PL = "CN22";

        public static string ErrMessage { get; private set; }


        public static async Task<bool> CheckDIN(MODE mode)
        {
            ErrMessage = "";

            bool result = false;
            bool rePA = false;
            bool rePB = false;
            bool rePC = false;
            bool rePE = false;
            bool rePH = false;
            bool rePL = false;

            string ExPA = "";
            string ExPB = "";
            string ExPC = "";
            string ExPE = "";
            string ExPH = "";
            string ExPL = "";

            string DataPA = "";
            string DataPB = "";
            string DataPC = "";
            string DataPE = "";
            string DataPH = "";
            string DataPL = "";


            try
            {
                var dialog = new DialogPic(mode == MODE.OFF ? "赤枠のスイッチをすべてOFF（↓）にしてください" : "赤枠のスイッチをすべてON（↑）にしてください", DialogPic.NAME.その他);
                dialog.ShowDialog();
                if (!Flags.DialogReturn) return false;

                return await Task<bool>.Run(() =>
                {
                    //電源ONする処理
                    General.PowerSupply(true);
                    if (!General.CheckDemo表示())
                        return false;

                    Target.SendData("DIN");

                    while (true)
                    {
                        if (Flags.ClickStopButton) return false;
                        if (General.CountNewline() == 5) break;
                    }
                    Target.Escape();
                    var log = State.VmComm.RX;
                    int FoundIndex = log.IndexOf("DIN,", 0);
                    DataPA = log.Substring(FoundIndex + 5, 4);
                    DataPB = log.Substring(FoundIndex + 11, 4);
                    DataPC = log.Substring(FoundIndex + 17, 4);
                    DataPE = log.Substring(FoundIndex + 23, 4);
                    DataPH = log.Substring(FoundIndex + 29, 4);
                    DataPL = log.Substring(FoundIndex + 35, 4);

                    switch (mode)
                    {
                        case MODE.OFF:
                            ExPA = OFF_PA;
                            ExPB = OFF_PB;
                            ExPC = OFF_PC;
                            ExPE = OFF_PE;
                            ExPH = OFF_PH;
                            ExPL = OFF_PL;
                            break;
                        case MODE.ON:
                            ExPA = ON_PA;
                            ExPB = ON_PB;
                            ExPC = ON_PC;
                            ExPE = ON_PE;
                            ExPH = ON_PH;
                            ExPL = ON_PL;
                            break;
                    }

                    rePA = (DataPA == ExPA);
                    rePB = (DataPB == ExPB);
                    rePC = (DataPC == ExPC);
                    rePE = (DataPE == ExPE);
                    rePH = (DataPH == ExPH);
                    rePL = (DataPL == ExPL);

                    result = rePA && rePB && rePC && rePE && rePH && rePL;

                    //エラーメッセージの作成
                    ErrMessage += rePA ? "" : ERR_PA + "\r\n";
                    ErrMessage += rePB ? "" : ERR_PB + "\r\n";
                    ErrMessage += rePC ? "" : ERR_PC + "\r\n";
                    ErrMessage += rePE ? "" : ERR_PE + "\r\n";
                    ErrMessage += rePH ? "" : ERR_PH + "\r\n";
                    ErrMessage += rePL ? "" : ERR_PL + "\r\n";
                    ErrMessage += "の接続を確認してください";

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

                if (mode == MODE.OFF)
                {
                    VmTestResults.PA_OFF = DataPA;
                    VmTestResults.PB_OFF = DataPB;
                    VmTestResults.PC_OFF = DataPC;
                    VmTestResults.PE_OFF = DataPE;
                    VmTestResults.PH_OFF = DataPH;
                    VmTestResults.PL_OFF = DataPL;

                    VmTestResults.ColPA_OFF = rePA ? OffBrush : NgBrush;
                    VmTestResults.ColPB_OFF = rePB ? OffBrush : NgBrush;
                    VmTestResults.ColPC_OFF = rePC ? OffBrush : NgBrush;
                    VmTestResults.ColPE_OFF = rePE ? OffBrush : NgBrush;
                    VmTestResults.ColPH_OFF = rePH ? OffBrush : NgBrush;
                    VmTestResults.ColPL_OFF = rePL ? OffBrush : NgBrush;
                }
                else
                {
                    VmTestResults.PA_ON = DataPA;
                    VmTestResults.PB_ON = DataPB;
                    VmTestResults.PC_ON = DataPC;
                    VmTestResults.PE_ON = DataPE;
                    VmTestResults.PH_ON = DataPH;
                    VmTestResults.PL_ON = DataPL;

                    VmTestResults.ColPA_ON = rePA ? OffBrush : NgBrush;
                    VmTestResults.ColPB_ON = rePB ? OffBrush : NgBrush;
                    VmTestResults.ColPC_ON = rePC ? OffBrush : NgBrush;
                    VmTestResults.ColPE_ON = rePE ? OffBrush : NgBrush;
                    VmTestResults.ColPH_ON = rePH ? OffBrush : NgBrush;
                    VmTestResults.ColPL_ON = rePL ? OffBrush : NgBrush;
                }

                if (mode == MODE.ON)
                {
                    var dialog = new DialogPic("赤枠のスイッチをすべてOFF（↓）に戻してください", DialogPic.NAME.その他);
                    dialog.ShowDialog();
                    await Task.Delay(300);
                }
            }
        }

        public static async Task<bool> CheckANA2_P(MODE mode)
        {
            List<bool> ListResult = new List<bool>();
            List<string> ListData = new List<string>();
            const int numSamples = 5;//サンプリング数はここで変更する

            bool resultAna12 = false;
            bool resultAna13 = false;
            bool resultAna14 = false;
            bool resultAna15 = false;
            bool resultAna17 = false;
            bool resultAna23 = false;
            bool resultAna24 = false;
            bool resultAna25 = false;
            bool resultAna26 = false;
            bool resultAna27 = false;

            string Ana12Data = "";
            string Ana13Data = "";
            string Ana14Data = "";
            string Ana15Data = "";
            string Ana17Data = "";
            string Ana23Data = "";
            string Ana24Data = "";
            string Ana25Data = "";
            string Ana26Data = "";
            string Ana27Data = "";

            var List_ANA12 = new List<double>();
            var List_ANA13 = new List<double>();
            var List_ANA14 = new List<double>();
            var List_ANA15 = new List<double>();
            var List_ANA17 = new List<double>();
            var List_ANA23 = new List<double>();
            var List_ANA24 = new List<double>();
            var List_ANA25 = new List<double>();
            var List_ANA26 = new List<double>();
            var List_ANA27 = new List<double>();

            try
            {
                var dialog = new DialogPic(mode == MODE.OFF ? "①青枠のスイッチをすべてOFF（↓）する\r\n②製品SW1の1-5をOFF（↓）する\r\n③表示基板のSW11の2番をOFF（↓）する" :
                                                              "①青枠のスイッチをすべてON（↑）する\r\n②製品SW1の1-5をON（↑）する\r\n③表示基板のSW11の2番をON（↑）する",
                                                               DialogPic.NAME.その他);
                dialog.ShowDialog();
                if (!Flags.DialogReturn) return false;

                return await Task<bool>.Run(() =>
                {
                    //電源ONする処理
                    General.PowerSupply(true);
                    //デモ画面確認
                    if (!General.CheckDemo表示())
                        return false;

                    Target.SendData("ANA2_P");

                    while (true)//取り込んだ通信データが7行以上になるまで待つ
                    {
                        if (Flags.ClickStopButton) return false;
                        if (General.CountNewline() > numSamples + 2) break;
                    }
                    Target.Escape();
                    int 検索開始位置 = 0;
                    foreach (var i in Enumerable.Range(0, numSamples))
                    {
                        var log = VmComm.RX;
                        int FoundIndex = log.IndexOf("ANA2_P,", 検索開始位置);
                        List_ANA12.Add(Double.Parse(log.Substring(FoundIndex + 8, 5)));
                        List_ANA13.Add(Double.Parse(log.Substring(FoundIndex + 15, 5)));
                        List_ANA14.Add(Double.Parse(log.Substring(FoundIndex + 22, 5)));
                        List_ANA15.Add(Double.Parse(log.Substring(FoundIndex + 29, 5)));
                        List_ANA17.Add(Double.Parse(log.Substring(FoundIndex + 43, 5)));
                        List_ANA23.Add(Double.Parse(log.Substring(FoundIndex + 50, 5)));
                        List_ANA24.Add(Double.Parse(log.Substring(FoundIndex + 57, 5)));
                        List_ANA25.Add(Double.Parse(log.Substring(FoundIndex + 64, 5)));
                        List_ANA26.Add(Double.Parse(log.Substring(FoundIndex + 71, 5)));
                        List_ANA27.Add(Double.Parse(log.Substring(FoundIndex + 78, 5)));
                        検索開始位置 = FoundIndex + 1;
                    }
                    if (mode == MODE.OFF)
                    {
                        resultAna12 = List_ANA12.Max() <= State.TestSpec.Ana12_15_1;
                        resultAna13 = List_ANA13.Max() <= State.TestSpec.Ana12_15_1;
                        resultAna14 = List_ANA14.Max() <= State.TestSpec.Ana12_15_1;
                        resultAna15 = List_ANA15.Max() <= State.TestSpec.Ana12_15_1;
                        resultAna17 = List_ANA17.Min() >= State.TestSpec.Ana17_27_1;
                        resultAna23 = List_ANA23.Min() >= State.TestSpec.Ana17_27_1;
                        resultAna24 = List_ANA24.Min() >= State.TestSpec.Ana17_27_1;
                        resultAna25 = List_ANA25.Min() >= State.TestSpec.Ana17_27_1;
                        resultAna26 = List_ANA26.Min() >= State.TestSpec.Ana17_27_1;
                        resultAna27 = List_ANA27.Min() >= State.TestSpec.Ana17_27_1;

                        Ana12Data = List_ANA12.Max().ToString("F3");
                        Ana13Data = List_ANA13.Max().ToString("F3");
                        Ana14Data = List_ANA14.Max().ToString("F3");
                        Ana15Data = List_ANA15.Max().ToString("F3");
                        Ana17Data = List_ANA17.Min().ToString("F3");
                        Ana23Data = List_ANA23.Min().ToString("F3");
                        Ana24Data = List_ANA24.Min().ToString("F3");
                        Ana25Data = List_ANA25.Min().ToString("F3");
                        Ana26Data = List_ANA26.Min().ToString("F3");
                        Ana27Data = List_ANA27.Min().ToString("F3");

                    }
                    else
                    {
                        resultAna12 = List_ANA12.Min() >= State.TestSpec.Ana12_15_2;
                        resultAna13 = List_ANA13.Min() >= State.TestSpec.Ana12_15_2;
                        resultAna14 = List_ANA14.Min() >= State.TestSpec.Ana12_15_2;
                        resultAna15 = List_ANA15.Min() >= State.TestSpec.Ana12_15_2;
                        resultAna17 = List_ANA17.Max() <= State.TestSpec.Ana17_27_2;
                        resultAna23 = List_ANA23.Max() <= State.TestSpec.Ana17_27_2;
                        resultAna24 = List_ANA24.Max() <= State.TestSpec.Ana17_27_2;
                        resultAna25 = List_ANA25.Max() <= State.TestSpec.Ana17_27_2;
                        resultAna26 = List_ANA26.Max() <= State.TestSpec.Ana17_27_2;
                        resultAna27 = List_ANA27.Max() <= State.TestSpec.Ana17_27_2;
                        Ana12Data = List_ANA12.Min().ToString("F3");
                        Ana13Data = List_ANA13.Min().ToString("F3");
                        Ana14Data = List_ANA14.Min().ToString("F3");
                        Ana15Data = List_ANA15.Min().ToString("F3");
                        Ana17Data = List_ANA17.Max().ToString("F3");
                        Ana23Data = List_ANA23.Max().ToString("F3");
                        Ana24Data = List_ANA24.Max().ToString("F3");
                        Ana25Data = List_ANA25.Max().ToString("F3");
                        Ana26Data = List_ANA26.Max().ToString("F3");
                        Ana27Data = List_ANA27.Max().ToString("F3");

                    }

                    return resultAna12 && resultAna13 && resultAna14 && resultAna15 &&
                            resultAna17 && resultAna23 && resultAna24 && resultAna25 && resultAna26 && resultAna27;

                });
            }
            catch
            {
                return false;
            }
            finally
            {
                //
                if (mode == MODE.OFF)
                {
                    VmTestResults.ANA12_1 = Ana12Data;
                    VmTestResults.ANA13_1 = Ana13Data;
                    VmTestResults.ANA14_1 = Ana14Data;
                    VmTestResults.ANA15_1 = Ana15Data;
                    VmTestResults.ANA17_1 = Ana17Data;
                    VmTestResults.ANA23_1 = Ana23Data;
                    VmTestResults.ANA24_1 = Ana24Data;
                    VmTestResults.ANA25_1 = Ana25Data;
                    VmTestResults.ANA26_1 = Ana26Data;
                    VmTestResults.ANA27_1 = Ana27Data;
                    VmTestResults.ColANA12_1 = resultAna12 ? OffBrush : NgBrush;
                    VmTestResults.ColANA13_1 = resultAna13 ? OffBrush : NgBrush;
                    VmTestResults.ColANA14_1 = resultAna14 ? OffBrush : NgBrush;
                    VmTestResults.ColANA15_1 = resultAna15 ? OffBrush : NgBrush;
                    VmTestResults.ColANA17_1 = resultAna17 ? OffBrush : NgBrush;
                    VmTestResults.ColANA23_1 = resultAna23 ? OffBrush : NgBrush;
                    VmTestResults.ColANA24_1 = resultAna24 ? OffBrush : NgBrush;
                    VmTestResults.ColANA25_1 = resultAna25 ? OffBrush : NgBrush;
                    VmTestResults.ColANA26_1 = resultAna26 ? OffBrush : NgBrush;
                    VmTestResults.ColANA27_1 = resultAna27 ? OffBrush : NgBrush;
                }
                else
                {
                    VmTestResults.ANA12_2 = Ana12Data;
                    VmTestResults.ANA13_2 = Ana13Data;
                    VmTestResults.ANA14_2 = Ana14Data;
                    VmTestResults.ANA15_2 = Ana15Data;
                    VmTestResults.ANA17_2 = Ana17Data;
                    VmTestResults.ANA23_2 = Ana23Data;
                    VmTestResults.ANA24_2 = Ana24Data;
                    VmTestResults.ANA25_2 = Ana25Data;
                    VmTestResults.ANA26_2 = Ana26Data;
                    VmTestResults.ANA27_2 = Ana27Data;
                    VmTestResults.ColANA12_2 = resultAna12 ? OffBrush : NgBrush;
                    VmTestResults.ColANA13_2 = resultAna13 ? OffBrush : NgBrush;
                    VmTestResults.ColANA14_2 = resultAna14 ? OffBrush : NgBrush;
                    VmTestResults.ColANA15_2 = resultAna15 ? OffBrush : NgBrush;
                    VmTestResults.ColANA17_2 = resultAna17 ? OffBrush : NgBrush;
                    VmTestResults.ColANA23_2 = resultAna23 ? OffBrush : NgBrush;
                    VmTestResults.ColANA24_2 = resultAna24 ? OffBrush : NgBrush;
                    VmTestResults.ColANA25_2 = resultAna25 ? OffBrush : NgBrush;
                    VmTestResults.ColANA26_2 = resultAna26 ? OffBrush : NgBrush;
                    VmTestResults.ColANA27_2 = resultAna27 ? OffBrush : NgBrush;
                }
                General.PowerSupply(false);

                if (mode == MODE.ON)
                {
                    var dialog = new DialogPic("①青枠のスイッチをすべてOFF（↓）する\r\n②製品SW1の1-5をOFF（↓）する\r\n③表示基板のSW11の2番をOFF（↓）する", DialogPic.NAME.その他);
                    dialog.ShowDialog();
                    await Task.Delay(300);
                }
            }
        }




    }
}

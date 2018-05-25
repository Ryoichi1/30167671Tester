using System.Threading.Tasks;
using static System.Threading.Thread;

namespace _30167671Tester
{
    static class TEST_比例弁回転動作
    {
        public enum MODE { Motor_L, Motor_R }

        public static async Task<bool> CheckPWPV(MODE mode)
        {

            try
            {
                var mess = "";
                switch (mode)
                {
                    case MODE.Motor_L:
                        mess = "パルスモータが ひだり に４回転するのを確認してください\r\nＡ（左） → Ｂ（右）の順に回ります\r\nOKボタンを押すと回ります";
                        break;
                    case MODE.Motor_R:
                        mess = "パルスモータが みぎ に４回転するのを確認してください\r\nＡ（左） → Ｂ（右）の順に回ります\r\nOKボタンを押すと回ります";
                        break;

                }
                var dialog = new DialogPic(mess, DialogPic.NAME.その他);
                dialog.ShowDialog();

                var re = await Task<bool>.Run(() =>
                {
                    //電源ONする処理
                    General.PowerSupply(true);
                    if (!General.CheckDemo表示())
                        return false;

                    switch (mode)
                    {
                        case MODE.Motor_L:

                            Target.SendData("PWPV1 800 0");//モータＡ 左回転
                            var tm = new GeneralTimer(11000);
                            tm.Start();
                            while (true)
                            {
                                if (tm.FlagTimeout) return false;
                                if (State.VmComm.RX.Contains(">>"))
                                    break;
                            }
                            Sleep(150);
                            General.ClearCommlog();
                            Target.SendData("PWPV2 800 0");//モータＢ 左回転
                            tm.Start();
                            while (true)
                            {
                                if (tm.FlagTimeout) return false;
                                if (State.VmComm.RX.Contains(">>"))
                                    break;
                            }
                            break;
                        case MODE.Motor_R:
                            Target.SendData("PWPV1 0 800");//モータＡ 右回転
                            var tm2 = new GeneralTimer(11000);
                            tm2.Start();
                            while (true)
                            {
                                if (tm2.FlagTimeout) return false;
                                if (State.VmComm.RX.Contains(">>"))
                                    break;
                            }
                            Sleep(150);
                            General.ClearCommlog();
                            Target.SendData("PWPV2 0 800");//モータＢ 右回転
                            tm2.Start();
                            while (true)
                            {
                                if (tm2.FlagTimeout) return false;
                                if (State.VmComm.RX.Contains(">>"))
                                    break;
                            }
                            break;

                    }
                    return true;
                });

                if (!re) return false;

                dialog = new DialogPic("モータが正しく回転しましたか？", DialogPic.NAME.その他, soundSw: false);
                dialog.ShowDialog();

                return Flags.DialogReturn;

            }
            catch
            {
                return false;
            }
            finally
            {
                General.PowerSupply(false);
            }

        }





    }
}

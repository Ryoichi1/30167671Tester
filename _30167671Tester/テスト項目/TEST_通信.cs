using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Threading.Thread;

namespace _30167671Tester
{
    static class TEST_通信
    {
        public static async Task<bool> CheckDISP()
        {
            try
            {
                var re = await Task<bool>.Run(() =>
                {
                    Sleep(1000);
                    //電源ONする処理
                    General.PowerSupply(true);
                    return General.CheckDemo表示();
                });

                if (!re) return false;
                var mess = "OKボタンを押して表示基板の検査をしてください\r\n①LEDが左上から順に点灯すること\r\n②0～9の数字が順に点灯すること\r\n③各ボタンを押しブザーが鳴ること";
                var dialog = new DialogPic(mess, DialogPic.NAME.その他);
                dialog.ShowDialog();

                Target.SendData("DISP");
                await Task.Delay(5000);

                dialog = new DialogPic("表示基板の機能は正常ですか？", DialogPic.NAME.その他);
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

        public static async Task<bool> CheckRS232C()
        {

            try
            {
                return await Task<bool>.Run(() =>
                {

                    //電源ONする処理
                    General.PowerSupply(true);
                    if (!General.CheckDemo表示())
                        return false;

                    Target.SendData("RS232C");

                    while (true)//取り込んだ通信データが7行以上になるまで待つ
                    {
                        if (Flags.ClickStopButton) return false;
                        if (General.CountNewline() == 2) break;
                    }

                    return (State.VmComm.RX.IndexOf("232C Host->Host(loopback)[19200dbps] = OK") >= 0);

                });
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

        public static async Task<bool> CheckRS485_1()
        {

            try
            {
                return await Task<bool>.Run(() =>
                {
                    Sleep(1000);
                    //電源ONする処理
                    General.PowerSupply(true);
                    if (!General.CheckDemo表示())
                        return false;

                    Target.SendData("RS485_1");

                    while (true)//取り込んだ通信データが7行以上になるまで待つ
                    {
                        if (Flags.ClickStopButton) return false;
                        if (General.CountNewline() == 3) break;
                    }

                    return (State.VmComm.RX.IndexOf("485(2w) Host->Mst/Slv[19200dbps] = OK") >= 0) && (State.VmComm.RX.IndexOf("485(2w) Mst/Slv->Host[19200dbps] = OK") >= 0);

                });
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

        public static async Task<bool> CheckRS485_2()
        {
            try
            {
                return await Task<bool>.Run(() =>
                {
                    Sleep(1000);
                    //電源ONする処理
                    General.PowerSupply(true);
                    if (!General.CheckDemo表示())
                        return false;

                    Target.SendData("RS485_2");

                    while (true)//取り込んだ通信データが7行以上になるまで待つ
                    {
                        if (Flags.ClickStopButton) return false;
                        if (General.CountNewline() == 3) break;
                    }

                    return (State.VmComm.RX.IndexOf("485(2w) Host->Mst/Slv[19200dbps] = OK") >= 0) && (State.VmComm.RX.IndexOf("485(2w) Mst/Slv->Host[19200dbps] = OK") >= 0);

                });
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

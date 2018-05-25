using System.Threading.Tasks;
using static System.Threading.Thread;

namespace _30167671Tester
{
    public static class 書き込み
    {
        public static async Task<bool> WriteFw()
        {
            bool result = false;
            try
            {
                Target.ClosePort();//FDTとバッティングするためいったん閉じる
                var dialog = new DialogPic("SW1-7番をONしてください", DialogPic.NAME.その他);
                dialog.ShowDialog();
                if (!Flags.DialogReturn) return false;

                General.PowerSupply(true);
                Sleep(1000);
                result = await FDT.WriteFirmware(Constants.FdtPath, State.TestSpec.FirmwareSum);
                General.PowerSupply(false);//電源ON→OFF
                Target.OpenPort();//次ステップに備えてポートを開いておく
                return result;  

            }
            catch
            {
                return result = false;
            }
            finally
            {
                if (result)
                {
                    var dialog = new DialogPic("①SW1-7番をOFFしてください\r\n②SW2、SW3をすべてONしてください", DialogPic.NAME.その他);
                    dialog.ShowDialog();
                }
                else
                {
                    State.VmTestStatus.Spec = "規格値 : チェックサム ";
                    State.VmTestStatus.MeasValue = "計測値 : チェックサム ";
                }
            }


        }



    }
}

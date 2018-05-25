using System;
using System.Threading.Tasks;
using static System.Threading.Thread;
using static _30167671Tester.General;

namespace _30167671Tester
{
    static class TEST_警報点
    {
        public static double 警報点抵抗値 { get; set; }

        public static async Task<bool> Check警報点()
        {
            bool result = false;
            警報点抵抗値 = 0;
            try
            {
                General.SetCn10to6ダイヤル抵抗();
                var dialog = new DialogPic("６ダイヤル抵抗器を１５７．１オームに設定してね", DialogPic.NAME.その他);
                dialog.ShowDialog();

                General.PowerSupply(true);
                await Task.Delay(2000);

                dialog = new DialogPic("抵抗値を少しずつ上げて、\r\nチェッカーの温度警報ランプが点灯したらOKを押してね", DialogPic.NAME.その他);
                dialog.ShowDialog();

                var dialoginput = new DialogInput();
                dialoginput.ShowDialog();

                var resValue = Double.Parse(State.VmTestResults.AlarmPoint);

                result = (State.TestSpec.警報発報点_Min <= resValue && resValue <= State.TestSpec.警報発報点_Max);
                return result;
            }
            catch
            {
                return false;
            }
            finally
            {
                General.PowerSupply(false);

                //VM更新
                State.VmTestResults.AlarmPoint += "Ω" ;//finally句に入る前は、数値だけが入力されているので注意
                State.VmTestResults.ColAlarmPoint = result ? OffBrush : NgBrush;
            }
        }

        public static async Task<bool> CheckDisconnection()
        {
            var dialog = new DialogPic("６ダイヤル抵抗器を１５０オームくらいに設定してね\r\n少数点以下は適当でOKだよ", DialogPic.NAME.その他);
            dialog.ShowDialog();
            General.PowerSupply(true);

            dialog = new DialogPic("チェッカーの温度警報LEDは消えていますか？？", DialogPic.NAME.その他);
            dialog.ShowDialog();
            if (!Flags.DialogReturn) return false;

            await Task.Delay(1500);
            General.SetCn10to6ダイヤル抵抗Bのみ();

            dialog = new DialogPic("チェッカーの温度警報LEDが点灯しましたか？？", DialogPic.NAME.その他);
            dialog.ShowDialog();
            if (!Flags.DialogReturn) return false;


            General.SetCn10to6ダイヤル抵抗();
            General.PowerSupply(false);
            await Task.Delay(1500);
            General.PowerSupply(true);

            dialog = new DialogPic("チェッカーの温度警報LEDは消えていますか？？", DialogPic.NAME.その他);
            dialog.ShowDialog();
            if (!Flags.DialogReturn) return false;

            await Task.Delay(1500);
            General.SetCn10to6ダイヤル抵抗Aのみ();

            dialog = new DialogPic("チェッカーの温度警報LEDが点灯しましたか？？", DialogPic.NAME.その他);
            dialog.ShowDialog();
            if (!Flags.DialogReturn) return false;

            General.SetCn10to6ダイヤル抵抗();
            General.PowerSupply(false);
            await Task.Delay(1500);
            General.PowerSupply(true);

            dialog = new DialogPic("チェッカーの温度警報LEDは消えていますか？？", DialogPic.NAME.その他);
            dialog.ShowDialog();
            if (!Flags.DialogReturn) return false;


            General.PowerSupply(false);
            //General.SetCn10toChecker();
            General.SetCn10to6ダイヤル抵抗();
            //MessageBox.Show("①製品のCN10に専用抵抗器（１２３．２４Ω）を接続してください");

            return true;
        }
    }
}

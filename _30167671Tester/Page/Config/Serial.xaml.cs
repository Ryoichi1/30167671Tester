using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace _30167671Tester
{
    /// <summary>
    /// EditOpeList.xaml の相互作用ロジック
    /// </summary>
    public partial class Serial
    {

        public Serial()
        {
            this.InitializeComponent();

            var CurrentYear = "";
            var CurrentMonth = "";


            if (State.TestItem == ITEM._30167671)
            {
                CurrentYear = "20" + State.Setting.NextSerial30167671.Substring(0, 2);
                CurrentMonth = Int32.Parse(State.Setting.NextSerial30167671.Substring(2, 2)).ToString();
            }
            else
            {
                CurrentYear = "20" + State.Setting.NextSerial30221500.Substring(0, 2);
                CurrentMonth = Int32.Parse(State.Setting.NextSerial30221500.Substring(2, 2)).ToString();
            }


            var y = DateTime.Now.Year;
            cbYear.Items.Add((y - 1).ToString());
            cbYear.Items.Add(y.ToString());

            foreach (var i in Enumerable.Range(1, 12))
                cbMonth.Items.Add(i.ToString());

            int j = 0;
            foreach (var c in cbYear.Items)
            {
                if (CurrentYear == c.ToString())
                {
                    cbYear.SelectedIndex = j;
                    break;
                }
                j++;
            }

            int k = 0;
            foreach (var c in cbMonth.Items)
            {
                if (CurrentMonth == c.ToString())
                {
                    cbMonth.SelectedIndex = k;
                    break;
                }
                k++;
            }
        }

        private async void buttonSave_Click(object sender, RoutedEventArgs e)
        {

            if (cbYear.SelectedIndex == -1 || cbMonth.SelectedIndex == -1)
                return;

            buttonSave.Background = Brushes.DodgerBlue;
            //保存する処理
            var NewYear = cbYear.SelectedItem.ToString().Substring(2);
            var NewMonth = Int32.Parse(cbMonth.SelectedItem.ToString()).ToString("D2");
            if (State.TestItem == ITEM._30167671)
                State.Setting.NextSerial30167671 = NewYear + NewMonth + "00001";
            else
                State.Setting.NextSerial30221500 = NewYear + NewMonth + "00001";

            State.SetSerialInfo();
            State.VmMainWindow.SerialNumber = State.シリアルナンバー年月部分 + State.NewSerial.ToString("D5");

            General.PlaySound(General.soundBattery);
            await Task.Delay(150);
            buttonSave.Background = Brushes.Transparent;
        }

    }
}

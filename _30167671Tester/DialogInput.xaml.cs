using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace _30167671Tester
{
    /// <summary>
    /// Dialog.xaml の相互作用ロジック
    /// </summary>
    public partial class DialogInput
    {

        public DialogInput()
        {
            InitializeComponent();

            this.MouseLeftButtonDown += (sender, e) => this.DragMove();//ウィンドウ全体でドラッグ可能にする

            this.DataContext = State.VmTestResults;
        }

        private async void ButtonOk_Click(object sender, RoutedEventArgs e)
        {
            if (!Double.TryParse(State.VmTestResults.AlarmPoint, out double resValue))
            {
                tbResValue.Background = Brushes.HotPink;
                await Task.Delay(300);
                tbResValue.Background = Brushes.White;
                State.VmTestResults.AlarmPoint = "";
                return;
            }

            Flags.DialogReturn = true;
            this.Close();
        }



        private void metroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            State.VmTestResults.AlarmPoint = "";
            General.PlaySound(General.soundNotice);
            tbResValue.Focus();
        }


        bool FlagButtonOkSelected = true;


        private void ButtonOk_GotFocus(object sender, RoutedEventArgs e)
        {
            ButtonOk.Background = General.DialogOnBrush;
            FlagButtonOkSelected = true;
        }



        private void ButtonOk_LostFocus(object sender, RoutedEventArgs e)
        {
            ButtonOk.Background = Brushes.Transparent;
        }

        private void ButtonOk_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!FlagButtonOkSelected)
            {
                ButtonOk.Background = General.DialogOnBrush;
            }
        }



        private void ButtonOk_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!FlagButtonOkSelected)
            {
                ButtonOk.Background = Brushes.Transparent;
            }
        }

        private void tbOpecode_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }
    }
}

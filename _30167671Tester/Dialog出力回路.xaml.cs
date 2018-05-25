using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace _30167671Tester
{
    /// <summary>
    /// Dialog.xaml の相互作用ロジック
    /// </summary>
    public partial class Dialog出力回路
    {
        private TEST_出力回路.MODE mode;
        public Dialog出力回路(TEST_出力回路.MODE mode)
        {
            InitializeComponent();

            this.MouseLeftButtonDown += (sender, e) => this.DragMove();//ウィンドウ全体でドラッグ可能にする
            this.mode = mode;
        }

        private void ButtonOk_Click(object sender, RoutedEventArgs e)
        {
            Flags.DialogReturn = true;
            this.Close();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            Flags.DialogReturn = false;
            this.Close();
        }

        private void metroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            General.PlaySound(General.soundNotice);
            ButtonOk.Focus();

            var rootPath = "Resources/Pic出力回路/";
            switch (mode)
            {
                case TEST_出力回路.MODE.OUTP_ON:
                    imagePic.Source = new BitmapImage(new Uri(rootPath + "汎用出力12点灯_.jpg", UriKind.Relative));
                    labelMessage.Content = "チェッカーのLEDが点灯していますか？";
                    break;
                case TEST_出力回路.MODE.OUTP_OFF1:
                    imagePic.Source = new BitmapImage(new Uri(rootPath + "その他点灯_.jpg", UriKind.Relative));
                    labelMessage.Content = "チェッカーのLEDが点灯していますか？";
                    break;
                case TEST_出力回路.MODE.OUTP_OFF2:
                    imagePic.Source = new BitmapImage(new Uri(rootPath + "基板LED点灯_.jpg", UriKind.Relative));
                    labelMessage.Content = "基板のLEDが点灯していますか？";
                    break;
                case TEST_出力回路.MODE.INTERLOCK1:
                    imagePic.Source = new BitmapImage(new Uri(rootPath + "Interlock1.jpg", UriKind.Relative));
                    labelMessage.Content = "外部非常停止SWを上向きにすると、\r\nインターロックLEDが点灯しますか？";
                    break;
                case TEST_出力回路.MODE.INTERLOCK2:
                    imagePic.Source = new BitmapImage(new Uri(rootPath + "Interlock2.jpg", UriKind.Relative));
                    labelMessage.Content = "インバータ過昇温SWを上向きにすると、\r\nインターロックLEDが消灯しますか？";
                    break;
                case TEST_出力回路.MODE.INTERLOCK3:
                    if (State.TestItem == ITEM._30167671)
                    {
                        imagePic.Source = new BitmapImage(new Uri(rootPath + "Interlock30167671.jpg", UriKind.Relative));
                        labelMessage.Content = "インバータ過昇温SWを下向きにしても、\r\nインターロックLEDが消灯したままですか？";
                    }
                    else
                    {
                        imagePic.Source = new BitmapImage(new Uri(rootPath + "Interlock30221500.jpg", UriKind.Relative));
                        labelMessage.Content = "インバータ過昇温SWを下向きにすると、\r\nインターロックLEDが点灯しますか？";
                    }
                    break;

            }

        }


        bool FlagButtonOkSelected = true;


        private void ButtonOk_GotFocus(object sender, RoutedEventArgs e)
        {
            ButtonOk.Background = General.DialogOnBrush;
            FlagButtonOkSelected = true;
        }

        private void ButtonCancel_GotFocus(object sender, RoutedEventArgs e)
        {
            ButtonCancel.Background = General.DialogOnBrush;
            FlagButtonOkSelected = false;
        }

        private void ButtonOk_LostFocus(object sender, RoutedEventArgs e)
        {
            ButtonOk.Background = Brushes.Transparent;
        }

        private void ButtonCancel_LostFocus(object sender, RoutedEventArgs e)
        {
            ButtonCancel.Background = Brushes.Transparent;
        }

        private void ButtonOk_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!FlagButtonOkSelected)
            {
                ButtonOk.Background = General.DialogOnBrush;
                ButtonCancel.Background = Brushes.Transparent;
            }
        }

        private void ButtonCancel_MouseEnter(object sender, MouseEventArgs e)
        {
            if (FlagButtonOkSelected)
            {
                ButtonCancel.Background = General.DialogOnBrush;
                ButtonOk.Background = Brushes.Transparent;
            }
        }

        private void ButtonOk_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!FlagButtonOkSelected)
            {
                ButtonOk.Background = Brushes.Transparent;
                ButtonCancel.Background = General.DialogOnBrush;
            }
        }

        private void ButtonCancel_MouseLeave(object sender, MouseEventArgs e)
        {
            if (FlagButtonOkSelected)
            {
                ButtonCancel.Background = Brushes.Transparent;
                ButtonOk.Background = General.DialogOnBrush;
            }
        }
    }
}

using System.Text;
using System.IO.Ports;
using static System.Threading.Thread;

namespace _30167671Tester
{
    public static class Target
    {
        public static SerialPort Port; 
        public static string RecieveData { get; private set; } 
        public static bool 通信ステータス { get; private set; } 

        static Target()
        {
            Port = new SerialPort();
        }

        //イニシャライズ
        public static bool OpenPort()
        {
            var result = false;

            try
            {
                if (!Port.IsOpen)
                {
                    //ポート1 RS232Cの設定
                    Port.PortName = "COM1";//COM1固定とする
                    Port.BaudRate = 19200;
                    Port.DataBits = 7;
                    Port.Parity = System.IO.Ports.Parity.None;
                    Port.StopBits = System.IO.Ports.StopBits.One;
                    Port.DataReceived += (object sender, SerialDataReceivedEventArgs e) =>
                    {
                        RecieveData = Port.ReadExisting();
                        State.VmComm.RX += RecieveData;
                    };
                    Port.NewLine = "\r\n";
                    Port.ReadTimeout = 5000;
                    Port.Open();
                }
                return result = true;
            }
            catch
            {
                return result = false;
            }
            finally
            {
                if (!result) ClosePort();
            }
        }



        //**************************************************************************
        //Targetに文字列を送信する
        //**************************************************************************
        public static bool SendData(string Data)
        {
            try
            {
                ClearBuff();//受信バッファのクリア
                Port.WriteLine(Data);
                return 通信ステータス = true;
            }
            catch
            {
                return 通信ステータス = false;
            }
        }

        //**************************************************************************
        //TargetにESCを送信する 
        //Targetからの連続受信を止めるときに使用する
        //**************************************************************************
        public static void Escape()
        {
            SendData(Encoding.ASCII.GetString(new byte[] { 0x1B }));
            Sleep(200);
        }

        //**************************************************************************
        //COMポートを閉じる処理
        //**************************************************************************   
        public static void ClosePort()
        {
            if (Port.IsOpen) Port.Close();
        }


        //**************************************************************************
        //受信バッファをクリアする
        //**************************************************************************
        private static void ClearBuff()
        {
            Port.DiscardInBuffer();
        }

    }
}

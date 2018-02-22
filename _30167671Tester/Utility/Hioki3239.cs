using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Windows.Forms;
using System.Timers;
using System.Threading;

namespace _30167671Tester
{
    class Hioki3239
    {
        //定数の宣言(クラスメンバーになります)
        public enum ErrorCode { Normal, ResponseErr, OverRange, DataErr, TimeoutErr, MeasErr, OpenErr, InitErr, SendErr, Other }
        public enum DCV_Range { R200mV, R2000mV, R20V, R200V, R1000V, 省略 }
        public enum ACV_Range { R2000mV, R20V, R200V, R700V, 省略 }

        private const string ID_323x = "HIOKI,323";
        private const string ComName = "USB Serial Port";


        //変数の宣言（インスタンスメンバーになります）
        private static SerialPort port;
        private static string RecieveData;

        public static double VoltData { get; private set; }//計測したDCVデータ
        public static double AmpData { get; private set; }//計測したDCAデータ
        public static double ResData { get; private set; }//計測した抵抗値データ
        public static ErrorCode State3239 { get; set; }//Hioki3229のｽﾃｰﾀｽ



        //コンストラクタ
        static Hioki3239()
        {
            port = new SerialPort();
        }


        //**************************************************************************
        //HIOKI3239 COMポートのオープン
        //引数：
        //戻値：bool
        //**************************************************************************
        public static bool Init323x()
        {
            try
            {
                //Comポートリストの取得
                var ListComNo = FindSerialPort.GetComNo(ComName);
                if (ListComNo.Count() == 0)
                    return false;

                foreach (var c in ListComNo)
                {
                    //Agilent34401A用のシリアルポート設定
                    port.PortName = c;
                    port.BaudRate = 9600;
                    port.DataBits = 8;
                    port.Parity = System.IO.Ports.Parity.None;
                    port.StopBits = System.IO.Ports.StopBits.One;
                    port.DtrEnable = true;//これ設定しないとコマンド送るたびにErrorになります！
                    port.NewLine = ("\r\n");
                    port.Open();
                    //クエリ送信
                    Thread.Sleep(100);
                    port.WriteLine("*IDN?");
                    if (GetRecieveData() && RecieveData.Contains(ID_323x))
                    {
                        FindSerialPort.SetComOpenStatus(c, true);
                        return true;
                    }
                    //開いたポートが間違っているのでいったん閉じる
                    port.Close();
                }

                State3239 = ErrorCode.OpenErr;
                return false;
            }
            catch
            {
                port.Close();
                State3239 = ErrorCode.OpenErr;
                return false;
            }
        }


        //**************************************************************************
        //シリアルポートを閉じる
        //引数：
        //戻値：bool
        //**************************************************************************
        public static bool ClosePort()
        {
            if (port.IsOpen) port.Close();
            return true;


        }

        //**************************************************************************
        //コマンドを送信する
        //引数：コマンド
        //戻値：bool
        //**************************************************************************
        private static bool SendCommand(string command, string parameter = null)
        {
            //コマンド送信前にステータスを初期化する
            State3239 = ErrorCode.Normal;

            try
            {
                port.DiscardInBuffer();//データ送信前に受信バッファのクリア
                port.WriteLine(command);//コマンド送信
                return true;
            }
            catch
            {
                State3239 = ErrorCode.SendErr;
                return false;
            }
        }

        //**************************************************************************
        //受信データを読み取る
        //引数：指定時間（ｍｓｅｃ）
        //戻値：bool
        //**************************************************************************
        private static bool GetRecieveData(int time = 500)
        {
            try
            {
                RecieveData = "";//念のため初期化
                port.ReadTimeout = time;
                RecieveData = port.ReadLine();
                return true;
            }
            catch
            {
                State3239 = ErrorCode.TimeoutErr;
                return false;
            }
        }


        //**************************************************************************
        //DC電圧の測定
        //引数：レンジを指定する電圧値
        //戻値：bool
        //**************************************************************************
        public static bool GetDcVolt(DCV_Range range)
        {
            string レンジ;

            switch (range)
            {
                case DCV_Range.R200mV:
                    レンジ = "199.999E-03";
                    break;
                case DCV_Range.R2000mV:
                    レンジ = "1.99999";
                    break;
                case DCV_Range.R20V:
                    レンジ = "19.9999";
                    break;
                case DCV_Range.R200V:
                    レンジ = "199.999";
                    break;
                default:
                    レンジ = "19.9999";
                    break;
            }

            double buff = 0;
            try
            {
                SendCommand(":SYST:HEAD 0");
                SendCommand("FUNC 'VOLTage:DC';VOLT:RANG " + レンジ);
                SendCommand(":SAMP:RATE MED");
                SendCommand(":INIT:CONT 1");
                SendCommand(":MEAS:VOLT:DC?");

                if (!GetRecieveData()) return false;

                return Double.TryParse(RecieveData, out buff);
            }
            catch
            {
                return false;
            }
            finally
            {
                VoltData = buff;
                SendCommand(":INIT:CONT 1");
            }

        }

        //**************************************************************************
        //AC電圧の測定
        //引数：レンジを指定する電圧値
        //戻値：bool
        //**************************************************************************
        public static bool GetAcVolt(ACV_Range range)
        {
            string レンジ;

            switch (range)
            {
                case ACV_Range.R2000mV:
                    レンジ = "1.99999";
                    break;
                case ACV_Range.R20V:
                    レンジ = "19.9999";
                    break;
                case ACV_Range.R200V:
                    レンジ = "199.999";
                    break;

                default:
                    レンジ = "19.9999";
                    break;
            }

            double buff = 0;
            try
            {
                SendCommand(":SYST:HEAD 0");
                SendCommand("FUNC 'VOLTage:AC';VOLT:RANG " + レンジ);
                SendCommand(":SAMP:RATE MED");
                SendCommand(":INIT:CONT 1");
                SendCommand(":MEAS:VOLT:AC?");

                if (!GetRecieveData(2000)) return false;//AC電圧測定は少し時間かかります

                return Double.TryParse(RecieveData, out buff);
            }
            catch
            {
                return false;
            }
            finally
            {
                VoltData = buff;
                SendCommand(":INIT:CONT 1");
            }

        }

        //**************************************************************************
        //抵抗値の測定
        //引数：レンジを指定する電圧値
        //戻値：bool
        //**************************************************************************
        public static bool GetResistance()
        {
            double buff = 0;
            try
            {
                SendCommand(":SYST:HEAD 0");
                SendCommand("FUNC 'RES';RES:RANG 250");
                SendCommand(":SAMP:RATE MED");
                SendCommand(":INIT:CONT 1");
                SendCommand(":MEAS:RES?");

                if (!GetRecieveData(2000)) return false;

                return Double.TryParse(RecieveData, out buff);
            }
            catch
            {
                return false;
            }
            finally
            {
                ResData = buff;
                SendCommand(":INIT:CONT 1");
            }

        }




























    }
}

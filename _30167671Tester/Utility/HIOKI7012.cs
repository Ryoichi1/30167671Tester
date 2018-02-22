using System;
using System.IO.Ports;
using System.Threading;


namespace _30167671Tester
{

    public static class HIOKI7012
    {
        //列挙型の宣言
        public enum ErrorCode { Normal, CmdErr, TimeoutErr, OpenErr, SendErr, Other }
        public enum FUNC_MODE { CV_2_5, CV_25, CC_25, TC_0 }
        public enum MEAS_MODE { V_2_5, V_25, A_25m, TEMP }

        //定数の宣言
        private const string SS7012_ID = "HIOKI,SS7012, Ver 1.03";
        private const string ComName = "Prolific USB-to-Serial Comm Port";

        //パブリックメンバ
        public static double VoltData { get; private set; }//計測した電圧値
        public static ErrorCode State7012 { get; set; }

        //プライベートメンバ
        private static SerialPort port;
        private static string RecieveData;//7012から受信した生データ

        //静的コンストラクタ
        static HIOKI7012()
        {
            port = new SerialPort();
        }

        //**************************************************************************
        //COMポートのオープン
        //**************************************************************************
        public static bool Init7012()
        {
            try
            {
                //TODO あとでfindserialクラスのメソッドでcomポート探す
                //FindSerialPort.GetDeviceNames();
                //var portName = FindSerialPort.GetComNo(ComName);
                //if (portName == null) return false;//
                if (!port.IsOpen)
                {
                    //シリアルポート設定
                    port.PortName = "COM14";
                    //port.PortName = portName;
                    port.BaudRate = 9600;
                    port.DataBits = 8;
                    port.Parity = System.IO.Ports.Parity.None;
                    port.StopBits = System.IO.Ports.StopBits.One;
                    port.NewLine = ("\r\n");
                    port.Open();
                }
                return (SendQuery("*IDN?") && RecieveData == SS7012_ID);
            }
            catch
            {
                State7012 = ErrorCode.OpenErr;
                ClosePort();
                return false;
            }
        }
        //**************************************************************************
        //SS7012に設定コマンドを送る
        //引数：なし
        //戻値：bool
        //**************************************************************************
        private static bool SendCommand(string cmd)
        {
            //送信処理
            try
            {
                port.DiscardInBuffer();//COM受信バッファクリア
                port.WriteLine(cmd);
            }
            catch
            {
                State7012 = ErrorCode.SendErr;
                return false;
            }

            //受信処理 
            //設定コマンド送信時は、OK または CMD ERR が返ってきます
            try
            {
                RecieveData = "";//初期化
                port.ReadTimeout = 1500;
                RecieveData = port.ReadLine();
                if (RecieveData == "OK")
                {
                    State7012 = ErrorCode.Normal;
                    return true;
                }
                else
                {
                    State7012 = ErrorCode.CmdErr;
                    return false;
                }
            }
            catch
            {
                State7012 = ErrorCode.TimeoutErr;
                return false;
            }
        }

        //**************************************************************************
        //SS7012に設定コマンドを送る
        //引数：なし
        //戻値：bool
        //**************************************************************************
        private static bool SendQuery(string cmd)
        {
            //送信処理
            try
            {
                port.DiscardInBuffer();//COM受信バッファクリア
                port.WriteLine(cmd);
            }
            catch
            {
                State7012 = ErrorCode.SendErr;
                return false;
            }

            //受信処理 
            //設定コマンド送信時は、OK または CMD ERR が返ってきます
            try
            {
                RecieveData = "";//初期化
                port.ReadTimeout = 1500;
                RecieveData = port.ReadLine();
                if (RecieveData != "CMD ERR")
                {
                    State7012 = ErrorCode.Normal;
                    return true;
                }
                else
                {
                    State7012 = ErrorCode.CmdErr;
                    return false;
                }
            }
            catch
            {
                State7012 = ErrorCode.TimeoutErr;
                return false;
            }
        }

        //**************************************************************************
        //DC電圧を出力する
        //引数：なし
        //戻値：bool
        //**************************************************************************       
        public static bool MeasureDcV(MEAS_MODE mode = MEAS_MODE.V_25)
        {
            bool result = false;
            double buff = 0;

            try
            {
                //現在の測定ファンクションが何かチェックする
                if (!SendQuery("FCM?")) return false;

                if (mode == MEAS_MODE.V_2_5)
                {
                    //測定ファンクションの切り替え 2.5Vレンジ（-2.8000 ～ +2.8000V）
                    if (HIOKI7012.RecieveData != "1")
                    {
                        if (!SendCommand("FCM 1")) return false;
                        Thread.Sleep(2000);
                    }
                }
                else
                {
                    //測定ファンクションの切り替え 25Vレンジ（-28.000 ～ +28.000V）
                    if (HIOKI7012.RecieveData != "2")
                    {
                        if (!SendCommand("FCM 2")) return false;
                        Thread.Sleep(2000);
                    }
                }

                if (!SendQuery("RDV?")) return false; //入力が測定範囲外の場合"CMD ERR"が返ってくる

                return result = Double.TryParse(RecieveData, out buff);
            }
            catch
            {
                return false;
            }
            finally
            {
                if (result) VoltData = buff;
            }
        }

        //**************************************************************************
        //DC電圧を出力する
        //引数：なし
        //戻値：bool
        //**************************************************************************       
        public static bool OutDcV(double outValue)
        {
            try
            {
                if (!StopSource()) return false;

                //ファンクションの切り替え & 出力電圧の設定
                if (outValue >= 0 && outValue <= 2.5)
                {
                    if (!SendCommand("FCC 0")) return false;
                    if (!SendCommand("CVV " + outValue.ToString("F4"))) return false;
                }
                else if (outValue > 2.5 && outValue <= 25)
                {
                    if (!SendCommand("FCC 1")) return false;
                    if (!SendCommand("CVV " + outValue.ToString("F3"))) return false;
                }
                else//0～25V以外の出力値は設定させない
                {
                    return false;
                }

                //出力開始
                return SendCommand("OUT 1");
            }
            catch
            {
                StopSource();
                return false;
            }
        }

        //**************************************************************************
        //DC電流を出力する
        //引数：なし
        //戻値：bool
        //**************************************************************************       
        public static bool OutDcI(double outValue)
        {
            try
            {
                if (!StopSource()) return false;
                //ファンクションの切り替え & 出力電圧の設定
                if (!SendCommand("FCC 2")) return false;
                if (!SendCommand("CCA " + outValue.ToString("F3"))) return false;

                //出力開始
                return SendCommand("OUT 1");
            }
            catch
            {
                StopSource();
                return false;
            }
        }

        //**************************************************************************
        //K熱電対起電力を出力する
        //引数：なし
        //戻値：bool
        //**************************************************************************       
        public static bool OutTc_K(double outValue)
        {
            try
            {
                if (!StopSource()) return false;

                //ファンクションの切り替え & 出力電圧の設定
                if (!SendCommand("FCC 3")) return false;
                if (!SendCommand("TCC K, " + outValue.ToString("F1"))) return false;

                //出力開始
                return SendCommand("OUT 1");
            }
            catch
            {
                StopSource();
                return false;
            }
        }

        public static bool StopSource()
        {
            return SendCommand("OUT 0");
        }

        //**************************************************************************
        //COMポートを閉じる処理
        //引数：なし
        //戻値：なし
        //**************************************************************************   
        public static void ClosePort()
        {
            StopSource();
            if (port.IsOpen) port.Close();
        }

    }


}
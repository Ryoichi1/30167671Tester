using System;
using System.Collections.Generic;
using System.Linq;
using System.IO.Ports;
using System.Threading;
using System.Timers;

namespace _30167671Tester
{
    public static class DS_5107B
    {
        private const string ID_DS_5105B = "IWATSU,DS-5107B";
        private const string ComName = "USB Serial Port";

        private static string VerticalRange = "1.0";
        private static string VerticalOffset = "0.0";

        //列挙型の宣言
        public enum ErrorCode { Normal, CommandErr, ResponseErr, TimeoutErr, OpenErr, InitErr, SendErr, Other }

        //変数の宣言
        public static SerialPort Port;
        public static string RecieveData { get; private set; }

        public static List<double> List_Wav;//ヘッダデータを削除し、換算した電圧値を入れる


        //プロパティの宣言
        public static ErrorCode State { get; private set; }//BRINGOのｽﾃｰﾀｽ

        //コンストラクタ
        static DS_5107B()
        {
            Port = new SerialPort();
        }
        //Bringoの初期化
        public static bool InitPort()
        {
            try
            {
                //Comポートリストの取得
                var ListComNo = FindSerialPort.GetComNo(ComName);
                if (ListComNo.Count() == 0)
                    return false;

                foreach (var c in ListComNo)
                {
                    //Host側シリアルポート設定
                    Port.PortName = c;
                    Port.BaudRate = 38400;
                    Port.DataBits = 8;
                    Port.Parity = System.IO.Ports.Parity.None;
                    Port.StopBits = System.IO.Ports.StopBits.One;
                    //Port.RtsEnable = true;
                    //Port.DtrEnable = true;
                    Port.NewLine = "\n";
                    Port.Open();
                    //クエリ送信
                    Thread.Sleep(500);
                    if (SendCommand("*IDN?", true) && RecieveData.Contains(ID_DS_5105B))
                    {
                        FindSerialPort.SetComOpenStatus(c, true);
                        return true;
                    }

                    //開いたポートが間違っているのでいったん閉じる
                    Port.Close();

                }

                State = ErrorCode.OpenErr;
                return false;
            }
            catch
            {
                Port.Close();
                State = ErrorCode.OpenErr;
                return false;
            }
        }



        public static bool ClosePort()
        {
            SendCommand(":KEY:LOCK DIS");
            FindSerialPort.SetComOpenStatus(Port.PortName, false);
            if (Port.IsOpen) Port.Close();
            return true;
        }

        //**************************************************************************
        //BRINGOⅡにコマンドを送信する
        //引数：コマンド
        //戻値：bool
        //**************************************************************************
        private static bool SendCommand(string command, bool DoReadData = false)
        {
            //コマンド送信前にステータスを初期化する
            State = ErrorCode.Normal;
            try
            {
                Port.DiscardInBuffer();//データ送信前に受信バッファのクリア
                Port.WriteLine(command);
                if (DoReadData)
                {
                    return GetRecieveData();
                }
                return true;
            }
            catch
            {
                State = ErrorCode.SendErr;
                return false;
            }
            finally
            {
                Thread.Sleep(200);
            }
        }

        //**************************************************************************
        //BRINGOⅡからの受信データを読み取る
        //引数：指定時間（ｍｓｅｃ）
        //戻値：bool
        //**************************************************************************
        private static bool GetRecieveData(int time = 1000)
        {
            try
            {
                Port.ReadTimeout = time;
                RecieveData = Port.ReadLine();
                return true;
            }
            catch
            {
                State = ErrorCode.TimeoutErr;
                return false;
            }
        }

        //**************************************************************************
        //試験設定
        //**************************************************************************
        public static bool InitTestSetting()
        {
            //水平軸関連コマンド：水平軸、[Horizontal]メニュー関連の設定/読出
            if (!SendCommand(":TIM:SCAL 0.0005")) goto FAIL;//水平軸ﾚﾝｼﾞの設定 msec/div
            if (!SendCommand(":TIM:OFFS 0")) goto FAIL;//ﾄﾘｶﾞﾃﾞｲﾚｲの設定

            //CH関連コマンド：垂直軸、[CH]メニュー関連の設定/読出
            if (!SendCommand(":CHAN1:SCAL " + VerticalRange)) goto FAIL;//垂直軸レンジ(V)設定
            if (!SendCommand(":CHAN1:DISP ON")) goto FAIL;//波形表示のｵﾝ/ｵﾌの設定
            if (!SendCommand(":CHAN2:DISP OFF")) goto FAIL;//波形表示のｵﾝ/ｵﾌの設定
            if (!SendCommand(":CHAN1:PROB 10")) goto FAIL;//プローブ減衰比の設定
            if (!SendCommand(":CHAN1:BWL OFF")) goto FAIL;//帯域制限ｵﾝ/ｵﾌの設定
            if (!SendCommand(":CHAN1:COUP DC")) goto FAIL;//ｶｯﾌﾟﾘﾝｸﾞの設定
            if (!SendCommand(":CHAN1:FILT OFF")) goto FAIL;//ﾃﾞｲｼﾞﾌｨﾙﾀの設定
            if (!SendCommand(":CHAN1:INV OFF")) goto FAIL;//波形の極性反転ｵﾝ/ｵﾌの設定
            if (!SendCommand(":CHAN1:OFFS " + VerticalOffset)) goto FAIL;//ｵﾌｾｯﾄﾚﾍﾞﾙの設定

            //その他設定
            if (!SendCommand(":TRIG:MODE EDGE")) goto FAIL;//ﾄﾘｶﾞﾓｰﾄﾞの設定
            if (!SendCommand(":TRIG:EDGE:SOUR CHAN1")) goto FAIL;//ﾄﾘｶﾞｿｰｽ信号の設定
            if (!SendCommand(":TRIG:EDGE:LEV 1.0")) goto FAIL;//ﾄﾘｶﾞﾚﾍﾞﾙの設定
            if (!SendCommand(":TRIG:EDGE:SWE AUTO")) goto FAIL;//ﾄﾘｶﾞ掃引の設定
            if (!SendCommand(":TRIG:EDGE:COUP DC")) goto FAIL;//ﾄﾘｶﾞ結合の設定
            if (!SendCommand(":TRIG:EDGE:SLOP POS")) goto FAIL;//ｴｯｼﾞﾄﾘｶﾞのｽﾛ-ﾌﾟの設定

            return true;
            FAIL:
            return false;
        }


        //**************************************************************************
        //波形データの取得
        //**************************************************************************
        public static void SetRun()
        {
            SendCommand(":RUN");
        }

        public static void SetStop()
        {
            SendCommand(":STOP");
        }

        public static bool GetWav()
        {
            List_Wav = new List<double>();
            SetRun();
            Thread.Sleep(3000);
            var ListByteData_換算前 = new List<byte>();
            var ListDoubleData = new List<double>();

            SendCommand(":WAV:DATA? CHAN1");

            try
            {
                int cnt = 0;
                var tm = new GeneralTimer(5000);
                tm.Start();
                while (true)
                {
                    if (tm.FlagTimeout)
                    {
                        return false;
                    }
                    if (Port.BytesToRead == 0) continue;
                    var buff = Port.ReadByte();
                    ListByteData_換算前.Add((byte)buff);
                    if (++cnt == 604)
                    {
                        //この時点で、ListByteData_換算前には波形データ（604 <4(ﾍｯﾀﾞ) + 600(ﾃﾞｰﾀ) = 604>）が入っている
                        //注意１：ListByteDataの添字[0]~[3]はヘッダデータのため削除する
                        //注意２：ListByteDataの値は生のバイナリデータであるため、別クラスで使用するためには下記の換算処理が必要となる
                        //電圧(V) = {(125-AD 値)/25}×垂直軸レンジ - OFFSET

                        var dataList = ListByteData_換算前.Skip(4).ToList<byte>();
                        double 垂直軸レンジ = Double.Parse(VerticalRange);
                        double OFFSET = Double.Parse(VerticalOffset);


                        dataList.ForEach(d =>
                        {
                            List_Wav.Add((((125 - d) / 25.0) * 垂直軸レンジ) - OFFSET);
                        });

                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                SetStop();
            }
        }


        public static bool 位相制御設定()
        {
            return SendCommand(":TIM:SCAL 0.005");//水平軸ﾚﾝｼﾞの設定 時間軸5msec/div
        }

        public static bool サイクル制御設定()
        {
            return SendCommand(":TIM:SCAL 0.01");//水平軸ﾚﾝｼﾞの設定 時間軸10msec/div
        }

        public static string ReadFreq()
        {
            return SendCommand(":MEAS:FREQ?") ? RecieveData : "";
        }

        public static string ReadPeak1()
        {
            return SendCommand(":MEAS:VMAX?") ? RecieveData : "";
        }

        public static string ReadPeak2()
        {
            return SendCommand(":MEAS:VMIN?") ? RecieveData : "";
        }



    }
}

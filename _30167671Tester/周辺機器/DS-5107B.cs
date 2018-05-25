using System;
using System.Collections.Generic;
using System.Linq;
using System.IO.Ports;
using System.Threading;
using System.Timers;

namespace _30167671Tester
{
    public class DS_5107B : IOscilloscope
    {
        //定数
        private const string ID_DS_5105B = "IWATSU,DS-5107B";
        private const string ComName = "USB Serial Port";
        private static string VerticalRange = "5.0";
        private static string VerticalOffset = "0.0";
        private static string TrigLev = "0.0";
        //private static string TrigLev = "1.0";


        //フィールド
        private SerialPort Port;
        private string RecieveData { get; set; }

        public DS_5107B()
        {
            Port = new SerialPort();
        }

        //初期化
        bool IOscilloscope.Init()
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
                        SetRun();
                        FindSerialPort.SetComOpenStatus(c, true);
                        return true;
                    }

                    //開いたポートが間違っているのでいったん閉じる
                    Port.Close();

                }

                return false;
            }
            catch
            {
                Port.Close();
                return false;
            }
        }

        bool IOscilloscope.Close()
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
        private bool SendCommand(string command, bool DoReadData = false)
        {
            //コマンド送信前にステータスを初期化する
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
        private bool GetRecieveData(int time = 1000)
        {
            try
            {
                Port.ReadTimeout = time;
                RecieveData = Port.ReadLine();
                return true;
            }
            catch
            {
                return false;
            }
        }

        //**************************************************************************
        //試験設定
        //**************************************************************************
        bool IOscilloscope.SetBasicConf()
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
            if (!SendCommand(":CHAN1:OFFS 0.0")) goto FAIL;//ｵﾌｾｯﾄﾚﾍﾞﾙの設定

            //その他設定
            if (!SendCommand(":TRIG:MODE EDGE")) goto FAIL;//ﾄﾘｶﾞﾓｰﾄﾞの設定
            if (!SendCommand(":TRIG:EDGE:SOUR CHAN1")) goto FAIL;//ﾄﾘｶﾞｿｰｽ信号の設定
            if (!SendCommand(":TRIG:EDGE:LEV " + TrigLev)) goto FAIL;//ﾄﾘｶﾞﾚﾍﾞﾙの設定
            if (!SendCommand(":TRIG:EDGE:SWE AUTO")) goto FAIL;//ﾄﾘｶﾞ掃引の設定
            if (!SendCommand(":TRIG:EDGE:COUP DC")) goto FAIL;//ﾄﾘｶﾞ結合の設定
            if (!SendCommand(":TRIG:EDGE:SLOP POS")) goto FAIL;//ｴｯｼﾞﾄﾘｶﾞのｽﾛ-ﾌﾟの設定

            return true;
            FAIL:
            return false;
        }

        bool IOscilloscope.Set位相制御()
        {
            //水平軸関連コマンド：水平軸、[Horizontal]メニュー関連の設定/読出
            return SendCommand(":TIM:SCAL 0.005");//水平軸ﾚﾝｼﾞの設定 5msec/div
        }
        bool IOscilloscope.Setサイクル制御()
        {
            //水平軸関連コマンド：水平軸、[Horizontal]メニュー関連の設定/読出
            return SendCommand(":TIM:SCAL 0.01");//水平軸ﾚﾝｼﾞの設定 10msec/div
        }

        //**************************************************************************
        //波形データの取得
        //**************************************************************************
        private void SetRun()
        {
            SendCommand(":RUN");
        }

        private void SetStop()
        {
            SendCommand(":STOP");
        }

        List<double> IOscilloscope.GetWav()
        {
            var List_Wav = new List<double>();
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
                    if (tm.FlagTimeout || Flags.ClickStopButton)
                    {
                        return null;
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

                        return List_Wav;
                    }
                }
            }
            catch
            {
                return null;
            }
        }


        double IOscilloscope.ReadFreq()
        {
            if (SendCommand(":MEAS:FREQ?", DoReadData:true))
            {
                return Double.TryParse(RecieveData, out double value) ? value : 0;
            }
            else
            {
                return 0;
            }
        }

        double IOscilloscope.ReadPeak_P()
        {
            if (SendCommand(":MEAS:VMAX?", DoReadData:true))
            {
                return Double.TryParse(RecieveData, out double value) ? value : 0;
            }
            else
            {
                return 0;
            }

        }

        double IOscilloscope.ReadPeak_M()
        {
            if (SendCommand(":MEAS:VMIN?", DoReadData:true))
            {
                return Double.TryParse(RecieveData, out double value) ? value : 0;
            }
            else
            {
                return 0;
            }
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Threading.Tasks;
using System.Media;
using System.Windows.Media;
using System;
using static System.Threading.Thread;

namespace _30167671Tester
{


    public static class General
    {

        //インスタンス変数の宣言
        public static EPX64R io;
        public static SoundPlayer player = null;
        public static SoundPlayer soundPass = null;
        public static SoundPlayer soundPassLong = null;
        public static SoundPlayer soundFail = null;
        public static SoundPlayer soundAlarm = null;
        public static SoundPlayer soundKuru = null;
        public static SoundPlayer soundCutin = null;
        public static SoundPlayer soundContinue = null;
        public static SoundPlayer soundBattery = null;
        public static SoundPlayer soundBgm1 = null;
        public static SoundPlayer soundBgm2 = null;
        public static SoundPlayer soundSerialLabel = null;
        public static SoundPlayer soundTotsugeki = null;
        public static SoundPlayer soundNotice = null;


        public static SolidColorBrush DialogOnBrush = new SolidColorBrush();
        public static SolidColorBrush OnBrush = new SolidColorBrush();
        public static SolidColorBrush OffBrush = new SolidColorBrush();
        public static SolidColorBrush NgBrush = new SolidColorBrush();


        static General()
        {
            //オーディオリソースを取り出す
            General.soundPass = new SoundPlayer(@"Resources\Wav\Pass.wav");
            General.soundPassLong = new SoundPlayer(@"Resources\Wav\PassLong.wav");
            General.soundFail = new SoundPlayer(@"Resources\Wav\Fail.wav");
            General.soundAlarm = new SoundPlayer(@"Resources\Wav\Alarm.wav");
            General.soundKuru = new SoundPlayer(@"Resources\Wav\Kuru.wav");
            General.soundCutin = new SoundPlayer(@"Resources\Wav\CutIn.wav");
            General.soundContinue = new SoundPlayer(@"Resources\Wav\Continue.wav");
            General.soundBgm1 = new SoundPlayer(@"Resources\Wav\bgm01.wav");
            General.soundBgm2 = new SoundPlayer(@"Resources\Wav\bgm02.wav");
            General.soundBattery = new SoundPlayer(@"Resources\Wav\battery.wav");
            General.soundTotsugeki = new SoundPlayer(@"Resources\Wav\Totsugeki.wav");
            General.soundSerialLabel = new SoundPlayer(@"Resources\Wav\BGM_Label.wav");
            General.soundNotice = new SoundPlayer(@"Resources\Wav\Notice.wav");

            OffBrush.Color = Colors.Transparent;

            DialogOnBrush.Color = Colors.DodgerBlue;
            DialogOnBrush.Opacity = 0.6;

            OnBrush.Color = Colors.DodgerBlue;
            OnBrush.Opacity = 0.4;

            NgBrush.Color = Colors.HotPink;
            NgBrush.Opacity = 0.4;
        }

        //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
        //画面表示、音、その他
        public static void Show()
        {
            var T = 0.3;
            var t = 0.005;

            State.Setting.OpacityTheme = State.VmMainWindow.ThemeOpacity;
            //10msec刻みでT秒で元のOpacityに戻す
            int times = (int)(T / t);

            State.VmMainWindow.ThemeOpacity = 0;
            Task.Run(() =>
            {
                while (true)
                {

                    State.VmMainWindow.ThemeOpacity += State.Setting.OpacityTheme / (double)times;
                    Thread.Sleep((int)(t * 1000));
                    if (State.VmMainWindow.ThemeOpacity >= State.Setting.OpacityTheme) return;

                }
            });
        }

        public static void SetRadius(bool sw)
        {
            var T = 0.45;//アニメーションが完了するまでの時間（秒）
            var t = 0.005;//（秒）

            //5msec刻みでT秒で元のOpacityに戻す
            int times = (int)(T / t);


            Task.Run(() =>
            {
                if (sw)
                {
                    while (true)
                    {
                        State.VmMainWindow.ThemeBlurEffectRadius += 25 / (double)times;
                        Thread.Sleep((int)(t * 1000));
                        if (State.VmMainWindow.ThemeBlurEffectRadius >= 25) return;

                    }
                }
                else
                {
                    var CurrentRadius = State.VmMainWindow.ThemeBlurEffectRadius;
                    while (true)
                    {
                        CurrentRadius -= 25 / (double)times;
                        if (CurrentRadius > 0)
                        {
                            State.VmMainWindow.ThemeBlurEffectRadius = CurrentRadius;
                        }
                        else
                        {
                            State.VmMainWindow.ThemeBlurEffectRadius = 0;
                            return;
                        }
                        Thread.Sleep((int)(t * 1000));
                    }
                }

            });
        }

        public static void SetMetalMode()
        {
            //メタルモードにするかどうかの判定（夕方7時～朝6時 もしくは日曜日は突入します）
            //（１）日曜日か？
            var week = System.DateTime.Now.DayOfWeek;
            if (week == DayOfWeek.Sunday)
            {
                Flags.MetalMode = true;
                return;
            }

            //（２）夕方7時～朝6時か？
            var Time = Int32.Parse(System.DateTime.Now.ToString("HH"));
            Flags.MetalMode = (Time >= 19 || Time >= 0 && Time <= 6);
        }

        public static void SetBgm()
        {
            if (!Flags.MetalMode || State.VmTestStatus.CheckUnitTest == true) return;

            //メタルモードイネーブルチェック追加
            if (!Flags.MetalModeSw) return;

            PlaySound2(soundTotsugeki);
            Thread.Sleep(400);
            PlaySoundLoop(soundBgm2);

        }

        public static void SetCutIn()
        {

            //メタルモードにするかどうかの判定（夕方5時～朝6時まで突入）
            var battleTime = Int32.Parse(System.DateTime.Now.ToString("HH"));
            if (battleTime >= 17 || (battleTime >= 0 && battleTime <= 6))
            {
                Flags.MetalMode = true;

                //シード値を指定しないとシード値として Environment.TickCount が使用される
                System.Random r = new System.Random();

                //0以上100未満の乱数を整数で返す
                int random = r.Next(100);
                if (random > 50)
                {
                    PlaySound(soundCutin);
                }
                else
                {
                    PlaySound(soundKuru);
                }

            }
            else
            {
                Flags.MetalMode = false;
            }
        }

        //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
        //データ保存
        public static bool SaveRetryLog()
        {
            if (State.RetryLogList.Count() == 0) return true;

            string fileName_RetryLog = "";
            switch (State.testMode)
            {
                case TEST_MODE._30167671:
                    fileName_RetryLog = Constants.fileName_RetryLog30167671;
                    break;
                case TEST_MODE._30221500:
                    fileName_RetryLog = Constants.fileName_RetryLog30221500;
                    break;

            }


            //出力用のファイルを開く appendをtrueにすると既存のファイルに追記、falseにするとファイルを新規作成する
            using (var sw = new System.IO.StreamWriter(fileName_RetryLog, true, Encoding.GetEncoding("Shift_JIS")))
            {
                try
                {
                    State.RetryLogList.ForEach(d =>
                    {
                        sw.WriteLine(d);
                    });

                    return true;
                }
                catch
                {
                    return false;
                }
            }

        }

        private static List<string> MakePassTestDataPwa()//TODO:
        {
            var ListData = new List<string>
            {
                State.VmMainWindow.SerialNumber,
                "AssemblyVer " + State.AssemblyInfo,
                "TestSpecVer " + State.TestSpec.TestSpecVer,
                System.DateTime.Now.ToString("yyyy年MM月dd日(ddd) HH:mm:ss"),
                State.VmMainWindow.Operator,

                State.TestSpec.FirmwareName,
                State.TestSpec.FirmwareSum,

                //TODO:



            };

            return ListData;
        }

        public static bool SaveTestData()
        {
            try
            {
                string PassDataFolderPath = "";
                List<string> dataList = new List<string>();
                dataList = MakePassTestDataPwa();

                switch (State.testMode)
                {
                    case TEST_MODE._30167671:
                        PassDataFolderPath = Constants.PassData30167671FolderPath;
                        break;
                    case TEST_MODE._30221500:
                        PassDataFolderPath = Constants.PassData30221500FolderPath;
                        break;
                }

                var OkDataFilePath = PassDataFolderPath + State.VmMainWindow.Opecode + ".csv";

                if (!System.IO.File.Exists(OkDataFilePath))
                {
                    //既存検査データがなければ新規作成
                    File.Copy(PassDataFolderPath + "Format.csv", OkDataFilePath);
                }

                // リストデータをすべてカンマ区切りで連結する
                string stCsvData = string.Join(",", dataList);

                // appendをtrueにすると，既存のファイルに追記
                //         falseにすると，ファイルを新規作成する
                var append = true;

                // 出力用のファイルを開く
                using (var sw = new System.IO.StreamWriter(OkDataFilePath, append, Encoding.GetEncoding("Shift_JIS")))
                {
                    sw.WriteLine(stCsvData);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool SaveNgData(List<string> dataList)
        {
            try
            {
                string FailDataFolderPath = "";

                switch (State.testMode)
                {
                    case TEST_MODE._30167671:
                        FailDataFolderPath = Constants.FailData30167671FolderPath;
                        break;
                    case TEST_MODE._30221500:
                        FailDataFolderPath = Constants.FailData30221500FolderPath;
                        break;

                }

                var NgDataFilePath = FailDataFolderPath + State.VmMainWindow.Opecode + ".csv";
                if (!File.Exists(NgDataFilePath))
                {
                    //既存検査データがなければ新規作成
                    File.Copy(FailDataFolderPath + "FormatNg.csv", NgDataFilePath);
                }

                var stArrayData = dataList.ToArray();
                // リストデータをすべてカンマ区切りで連結する
                string stCsvData = string.Join(",", stArrayData);

                // appendをtrueにすると，既存のファイルに追記
                //         falseにすると，ファイルを新規作成する
                var append = true;

                // 出力用のファイルを開く
                using (var sw = new System.IO.StreamWriter(NgDataFilePath, append, Encoding.GetEncoding("Shift_JIS")))
                {
                    sw.WriteLine(stCsvData);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
        //検査データからの最終シリアル№読み出し
        public static string LoadSerial(string filePath, string opecode)//最後のシリアル№を返す
        {
            try
            {
                string buff = General.ReadCsv(filePath);

                //最終行のシリアルナンバーを取得する
                var startIndex = buff.LastIndexOf(opecode) + 14;
                foreach (var i in Enumerable.Range(0, 2))
                {
                    startIndex = buff.IndexOf(",", startIndex);
                }

                string serial = buff.Substring(startIndex + 1, 9);
                return serial;
            }
            catch
            {
                return "";
            }
        }

        private static string ReadCsv(string filePath)
        {
            try
            {
                // StringBuilder クラスの新しいインスタンスを生成する
                System.Text.StringBuilder sbTarget = new System.Text.StringBuilder();

                // ある程度、使用するサイズが決まっている場合は指定しておく
                //sbTarget.Capacity = 1000000;

                // csvファイルを開く
                using (var sr = new System.IO.StreamReader(filePath, Encoding.GetEncoding("Shift_JIS")))
                {
                    // ストリームの末尾まで繰り返す
                    while (!sr.EndOfStream)
                    {
                        // ファイルから一行読み込む
                        var line = sr.ReadToEnd();
                        // 出力する
                        sbTarget.Append(line);
                    }
                }

                return sbTarget.ToString();
            }
            catch
            {
                return "";
            }
        }

        //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
        //電源制御、リセット処理
        public static void PowerSupply(bool sw)
        {
            if (sw) ClearCommlog();//電源再投入する度にログをクリアする
            io.OutBit(EPX64R.PORT.P5, EPX64R.BIT.b2, sw ? EPX64R.OUT.H : EPX64R.OUT.L);
            Flags.PowerOn = sw;
        }

        public static void ResetIo()
        {
            io.OutByte(EPX64R.PORT.P0, 0x00);
            io.OutByte(EPX64R.PORT.P1, 0x00);
            io.OutByte(EPX64R.PORT.P2, 0x00);
            io.OutByte(EPX64R.PORT.P3, 0x00);
            io.OutByte(EPX64R.PORT.P4, 0x00);
            io.OutByte(EPX64R.PORT.P5, 0x00);
            io.OutByte(EPX64R.PORT.P6, 0x00);

            Flags.PowerOn = false;
        }


        //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
        //ビューモデルクリア処理
        public static void ResetViewModel()//TODO:
        {
            State.VmMainWindow.SerialNumber = State.VmMainWindow.SerialNumber.Substring(0, 6) + State.NewSerial.ToString("D3");
            //ViewModel OK台数、NG台数、Total台数の更新
            State.VmTestStatus.OkCount = State.Setting.TodayOkCount.ToString() + "台";
            State.VmTestStatus.NgCount = State.Setting.TodayNgCount.ToString() + "台";
            State.VmTestStatus.Message = Constants.MessSet;


            State.VmTestStatus.DecisionVisibility = System.Windows.Visibility.Hidden;
            State.VmTestStatus.ErrInfoVisibility = System.Windows.Visibility.Hidden;
            State.VmTestStatus.RingVisibility = System.Windows.Visibility.Visible;

            State.VmTestStatus.TestTime = "00:00";
            State.VmTestStatus.進捗度 = 0;
            State.VmTestStatus.TestLog = "";

            State.VmTestStatus.FailInfo = "";
            State.VmTestStatus.Spec = "";
            State.VmTestStatus.MeasValue = "";


            //試験結果のクリア
            State.VmTestResults = new ViewModelTestResult();

            //通信ログのクリア
            State.VmComm.TX = "";
            State.VmComm.RX = "";


            State.VmMainWindow.EnableOtherButton = true;

            //各種フラグの初期化
            Flags.PowerOn = false;
            Flags.ClickStopButton = false;
            Flags.Testing = false;


            //テーマ透過度を元に戻す
            General.SetRadius(false);

            State.VmTestStatus.RetryLabelVis = System.Windows.Visibility.Hidden;
            State.VmTestStatus.TestSettingEnable = true;
            State.VmMainWindow.OperatorEnable = true;

            //コネクタチェックでエラーになると表示されたままになるので隠す（誤動作防止！！！）
            State.VmTestStatus.EnableButtonErrInfo = System.Windows.Visibility.Hidden;

        }

        public static void ClearCommlog()
        {
            State.VmComm.RX = "";
        }


        //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
        //通信ログ解析
        public static int CountNewline()
        {
            int total = 0;
            int 検索開始位置 = 0;
            while (true)
            {
                int foundIndex = State.VmComm.RX.IndexOf("\r\n", 検索開始位置);
                if (foundIndex < 0) break;
                total++;
                検索開始位置 = foundIndex + 1;
            }
            return total;
        }

        public static bool CheckDemo表示()
        {
            try
            {
                var tm = new GeneralTimer(6000);
                tm.Start();
                while (true)
                {
                    if (tm.FlagTimeout) return false;
                    if (State.VmComm.RX.Contains("Program Number PFA3097\r\n\r\n>> "))
                    {
                        tm.stop();
                        Sleep(300);
                        ClearCommlog();
                        return true;
                    }
                }

            }
            catch
            {
                ClearCommlog();
                return false;
            }

        }

        //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
        //WAVファイル再生

        //非同期で再生
        public static void PlaySound(SoundPlayer p)
        {
            //再生されているときは止める
            if (player != null)
                player.Stop();

            //waveファイルを読み込む
            player = p;
            //最後まで再生し終えるまで待機する
            player.Play();
        }

        //同期で再生
        public static void PlaySound2(SoundPlayer p)
        {
            //再生されているときは止める
            if (player != null)
                player.Stop();

            //waveファイルを読み込む
            player = p;
            //最後まで再生し終えるまで待機する
            player.PlaySync();

        }

        public static void PlaySoundLoop(SoundPlayer p)
        {
            //再生されているときは止める
            if (player != null)
                player.Stop();

            //waveファイルを読み込む
            player = p;
            //最後まで再生し終えるまで待機する
            player.PlayLooping();
        }

        //再生されているWAVEファイルを止める
        public static void StopSound()
        {
            if (player != null)
            {
                player.Stop();
                player.Dispose();
                player = null;
            }
        }

        //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
        //イニシャライズ処理
        public static void CheckAll周辺機器フラグ()
        {
            Flags.AllOk周辺機器接続 = (Flags.StateEpx64 && Flags.State7012 && Flags.State5107B && Flags.State323x && Flags.StateWavGen);
        }

        public static void Init周辺機器()//TODO:
        {

            Flags.Initializing周辺機器 = true;

            //EPX64Sの初期化
            bool StopEpx64 = false;
            Task.Run(() =>
            {
                //IOボードの初期化
                io = new EPX64R();
                while (true)
                {
                    if (Flags.StopInit周辺機器)
                    {
                        break;
                    }

                    Flags.StateEpx64 = General.io.InitEpx64R(0x7F);//0111 1111  ※P7入力 その他出力
                    if (Flags.StateEpx64)
                    {
                        //IOボードのリセット（出力をすべてLする）
                        ResetIo();
                        break;
                    }

                    Sleep(500);
                }
                StopEpx64 = true;
            });

            //HIOKI7012の初期化
            bool Stop7012 = false;
            Task.Run(() =>
            {
                while (true)
                {
                    if (Flags.StopInit周辺機器)
                    {
                        break;
                    }

                    Flags.State7012 = HIOKI7012.Init7012();
                    if (Flags.State7012)
                        HIOKI7012.StopSource();

                    Sleep(500);
                }
                Stop7012 = true;
            });


            //マルチメータの初期化
            bool Stop323x = false;
            Task.Run(() =>
            {
                while (true)
                {
                    if (Flags.StopInit周辺機器)
                    {
                        break;
                    }

                    Flags.State323x = Hioki3239.Init323x();
                    if (Flags.State323x)
                        break;

                    Thread.Sleep(400);
                }
                Stop323x = true;
            });

            //オシロスコープの初期化
            bool Stop5107B = false;
            Task.Run(() =>
            {
                while (true)
                {
                    if (Flags.StopInit周辺機器)
                    {
                        break;
                    }

                    Flags.State5107B = DS_5107B.InitPort();
                    if (Flags.State5107B)
                    {
                        DS_5107B.InitTestSetting();
                        break;
                    }

                    Sleep(500);
                }
                Stop5107B = true;
            });

            //ファンクションジェネレータの初期化
            bool StopWavGen = false;
            Task.Run(() =>
            {
                while (true)
                {
                    if (Flags.StopInit周辺機器)
                    {
                        break;
                    }

                    Flags.StateWavGen =  WaveFormGenerator.Initialize();
                    if (Flags.StateWavGen)
                        break;

                    Thread.Sleep(400);
                }

                StopWavGen = true;
            });



            Task.Run(() =>
            {
                while (true)
                {
                    CheckAll周辺機器フラグ();

                    //EPX64Sの初期化の中で、K100、K101の溶着チェックを行っているが、これがNGだとしてもInit周辺機器()は終了する
                    var IsAllStopped = StopEpx64 && Stop7012 && Stop5107B && Stop323x && StopWavGen;

                    if (Flags.AllOk周辺機器接続 || IsAllStopped) break;
                    Thread.Sleep(400);

                }
                Flags.Initializing周辺機器 = false;
            });
        }

        //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
        //イニシャライズ処理


        //IOユニットのリレー制御

        public enum Relay { K1, K2, K3, K4, K5, K6, K7, K8 }
        public static void SetRelay(Relay re)
        {
            General.io.OutBit(EPX64R.PORT.P6, EPX64R.BIT.b4, EPX64R.OUT.L);
            Sleep(100);
            switch (re)
            {
                case Relay.K1:
                    General.io.OutBit(EPX64R.PORT.P6, EPX64R.BIT.b1, EPX64R.OUT.L);
                    General.io.OutBit(EPX64R.PORT.P6, EPX64R.BIT.b2, EPX64R.OUT.L);
                    General.io.OutBit(EPX64R.PORT.P6, EPX64R.BIT.b3, EPX64R.OUT.L);
                    break;
                case Relay.K2:
                    General.io.OutBit(EPX64R.PORT.P6, EPX64R.BIT.b1, EPX64R.OUT.H);
                    General.io.OutBit(EPX64R.PORT.P6, EPX64R.BIT.b2, EPX64R.OUT.L);
                    General.io.OutBit(EPX64R.PORT.P6, EPX64R.BIT.b3, EPX64R.OUT.L);
                    break;
                case Relay.K3:
                    General.io.OutBit(EPX64R.PORT.P6, EPX64R.BIT.b1, EPX64R.OUT.L);
                    General.io.OutBit(EPX64R.PORT.P6, EPX64R.BIT.b2, EPX64R.OUT.H);
                    General.io.OutBit(EPX64R.PORT.P6, EPX64R.BIT.b3, EPX64R.OUT.L);
                    break;
                case Relay.K4:
                    General.io.OutBit(EPX64R.PORT.P6, EPX64R.BIT.b1, EPX64R.OUT.H);
                    General.io.OutBit(EPX64R.PORT.P6, EPX64R.BIT.b2, EPX64R.OUT.H);
                    General.io.OutBit(EPX64R.PORT.P6, EPX64R.BIT.b3, EPX64R.OUT.L);
                    break;
                case Relay.K5:
                    General.io.OutBit(EPX64R.PORT.P6, EPX64R.BIT.b1, EPX64R.OUT.L);
                    General.io.OutBit(EPX64R.PORT.P6, EPX64R.BIT.b2, EPX64R.OUT.L);
                    General.io.OutBit(EPX64R.PORT.P6, EPX64R.BIT.b3, EPX64R.OUT.H);
                    break;
                case Relay.K6:
                    General.io.OutBit(EPX64R.PORT.P6, EPX64R.BIT.b1, EPX64R.OUT.H);
                    General.io.OutBit(EPX64R.PORT.P6, EPX64R.BIT.b2, EPX64R.OUT.L);
                    General.io.OutBit(EPX64R.PORT.P6, EPX64R.BIT.b3, EPX64R.OUT.H);
                    break;
                case Relay.K7:
                    General.io.OutBit(EPX64R.PORT.P6, EPX64R.BIT.b1, EPX64R.OUT.L);
                    General.io.OutBit(EPX64R.PORT.P6, EPX64R.BIT.b2, EPX64R.OUT.H);
                    General.io.OutBit(EPX64R.PORT.P6, EPX64R.BIT.b3, EPX64R.OUT.H);
                    break;
                case Relay.K8:
                    General.io.OutBit(EPX64R.PORT.P6, EPX64R.BIT.b1, EPX64R.OUT.H);
                    General.io.OutBit(EPX64R.PORT.P6, EPX64R.BIT.b2, EPX64R.OUT.H);
                    General.io.OutBit(EPX64R.PORT.P6, EPX64R.BIT.b3, EPX64R.OUT.H);
                    break;

            }

            General.io.OutBit(EPX64R.PORT.P6, EPX64R.BIT.b4, EPX64R.OUT.H);
            Sleep(200);

        }

        public enum MEAS_CH
        {
            _12V, _5V, _3_3V, AVDD, AVCC, VREF, AVCCD, S5V,
            DV1, DV2, DV3, DV4, DV5, DV6, DV7, DV8, DV9, DV10, DV11, DV12, DV13
        }

        public enum SOURCE_CH
        {
            A0, A1, A2, A3, A4, A5, A6, A7, A8, A9, A10, A11,
            I1, I2, I3, I4,
            V1, V2, V3, V4
        }

        public enum MEAS3229_CH
        {
            CN11_1, CN11_2, CN11_3
        }


        public static void Set3229Meas(MEAS3229_CH ch)
        {
            //最初にHioki3229 MeasH/L側に接続されている端子をすべて開放する
            General.io.OutBit(EPX64R.PORT.P3, EPX64R.BIT.b7, EPX64R.OUT.L);
            General.io.OutBit(EPX64R.PORT.P4, EPX64R.BIT.b0, EPX64R.OUT.L);
            General.io.OutBit(EPX64R.PORT.P4, EPX64R.BIT.b1, EPX64R.OUT.L);
            Sleep(300);

            switch (ch)
            {
                case MEAS3229_CH.CN11_1:
                    General.io.OutBit(EPX64R.PORT.P3, EPX64R.BIT.b7, EPX64R.OUT.H);
                    break;
                case MEAS3229_CH.CN11_2:
                    General.io.OutBit(EPX64R.PORT.P4, EPX64R.BIT.b0, EPX64R.OUT.H);
                    break;
                case MEAS3229_CH.CN11_3:
                    General.io.OutBit(EPX64R.PORT.P4, EPX64R.BIT.b1, EPX64R.OUT.H);
                    break;
            }
            Sleep(500);

        }

        public static void Set7012Meas(MEAS_CH ch)
        {
            //最初にHioki7012 MeasH/L側に接続されている端子をすべて開放する
            General.io.OutBit(EPX64R.PORT.P6, EPX64R.BIT.b4, EPX64R.OUT.L);
            General.io.OutByte(EPX64R.PORT.P0, 0x00);
            General.io.OutByte(EPX64R.PORT.P1, 0x00);
            General.io.OutByte(EPX64R.PORT.P2, 0x00);
            Sleep(300);

            //H側接続
            switch (ch)
            {
                case MEAS_CH._12V:
                    SetRelay(Relay.K1);
                    break;
                case MEAS_CH._5V:
                    SetRelay(Relay.K2);
                    break;
                case MEAS_CH._3_3V:
                    SetRelay(Relay.K3);
                    break;
                case MEAS_CH.AVDD:
                    SetRelay(Relay.K4);
                    break;
                case MEAS_CH.AVCC:
                    SetRelay(Relay.K5);
                    break;
                case MEAS_CH.VREF:
                    SetRelay(Relay.K6);
                    break;
                case MEAS_CH.AVCCD:
                    SetRelay(Relay.K7);
                    break;
                case MEAS_CH.S5V:
                    SetRelay(Relay.K8);
                    break;
                case MEAS_CH.DV1:
                    General.io.OutBit(EPX64R.PORT.P0, EPX64R.BIT.b0, EPX64R.OUT.H);
                    break;
                case MEAS_CH.DV2:
                    General.io.OutBit(EPX64R.PORT.P0, EPX64R.BIT.b1, EPX64R.OUT.H);
                    break;
                case MEAS_CH.DV3:
                    General.io.OutBit(EPX64R.PORT.P0, EPX64R.BIT.b2, EPX64R.OUT.H);
                    break;
                case MEAS_CH.DV4:
                    General.io.OutBit(EPX64R.PORT.P0, EPX64R.BIT.b3, EPX64R.OUT.H);
                    break;
                case MEAS_CH.DV5:
                    General.io.OutBit(EPX64R.PORT.P0, EPX64R.BIT.b4, EPX64R.OUT.H);
                    break;
                case MEAS_CH.DV6:
                    General.io.OutBit(EPX64R.PORT.P0, EPX64R.BIT.b5, EPX64R.OUT.H);
                    break;
                case MEAS_CH.DV7:
                    General.io.OutBit(EPX64R.PORT.P0, EPX64R.BIT.b6, EPX64R.OUT.H);
                    break;
                case MEAS_CH.DV8:
                    General.io.OutBit(EPX64R.PORT.P0, EPX64R.BIT.b7, EPX64R.OUT.H);
                    break;
                case MEAS_CH.DV9:
                    General.io.OutBit(EPX64R.PORT.P1, EPX64R.BIT.b0, EPX64R.OUT.H);
                    break;
                case MEAS_CH.DV10:
                    General.io.OutBit(EPX64R.PORT.P1, EPX64R.BIT.b1, EPX64R.OUT.H);
                    break;
                case MEAS_CH.DV11:
                    General.io.OutBit(EPX64R.PORT.P1, EPX64R.BIT.b2, EPX64R.OUT.H);
                    break;
                case MEAS_CH.DV12:
                    General.io.OutBit(EPX64R.PORT.P1, EPX64R.BIT.b3, EPX64R.OUT.H);
                    break;
                case MEAS_CH.DV13:
                    General.io.OutBit(EPX64R.PORT.P1, EPX64R.BIT.b4, EPX64R.OUT.H);
                    break;

            }

            //L側接続
            switch (ch)
            {
                case MEAS_CH._12V:
                case MEAS_CH._5V:
                case MEAS_CH._3_3V:
                    General.io.OutBit(EPX64R.PORT.P1, EPX64R.BIT.b5, EPX64R.OUT.H);
                    break;
                case MEAS_CH.AVDD:
                case MEAS_CH.AVCC:
                case MEAS_CH.VREF:
                    General.io.OutBit(EPX64R.PORT.P1, EPX64R.BIT.b6, EPX64R.OUT.H);
                    break;

                case MEAS_CH.AVCCD:
                    General.io.OutBit(EPX64R.PORT.P1, EPX64R.BIT.b7, EPX64R.OUT.H);
                    break;
                case MEAS_CH.S5V:
                    General.io.OutBit(EPX64R.PORT.P2, EPX64R.BIT.b0, EPX64R.OUT.H);
                    break;
                case MEAS_CH.DV1:
                case MEAS_CH.DV2:
                case MEAS_CH.DV3:
                case MEAS_CH.DV4:
                case MEAS_CH.DV5:
                case MEAS_CH.DV6:
                case MEAS_CH.DV7:
                case MEAS_CH.DV8:
                case MEAS_CH.DV9:
                case MEAS_CH.DV10:
                case MEAS_CH.DV11:
                case MEAS_CH.DV12:
                case MEAS_CH.DV13:
                    General.io.OutBit(EPX64R.PORT.P2, EPX64R.BIT.b1, EPX64R.OUT.H);
                    break;
            }

            Sleep(1500);
        }

        public static void Set7012Source(SOURCE_CH ch)
        {
            //最初にHioki7012 SourceH/L側に接続されている端子をすべて開放する
            General.io.OutByte(EPX64R.PORT.P2, 0x00);
            General.io.OutByte(EPX64R.PORT.P3, 0x00);
            General.io.OutByte(EPX64R.PORT.P4, 0x00);
            General.io.OutBit(EPX64R.PORT.P5, EPX64R.BIT.b0, EPX64R.OUT.L);
            General.io.OutBit(EPX64R.PORT.P5, EPX64R.BIT.b1, EPX64R.OUT.L);
            Sleep(300);

            //H側接続
            switch (ch)
            {
                case SOURCE_CH.A0:
                    General.io.OutBit(EPX64R.PORT.P2, EPX64R.BIT.b2, EPX64R.OUT.H);
                    break;
                case SOURCE_CH.A1:
                    General.io.OutBit(EPX64R.PORT.P2, EPX64R.BIT.b3, EPX64R.OUT.H);
                    break;
                case SOURCE_CH.A2:
                    General.io.OutBit(EPX64R.PORT.P2, EPX64R.BIT.b4, EPX64R.OUT.H);
                    break;
                case SOURCE_CH.A3:
                    General.io.OutBit(EPX64R.PORT.P2, EPX64R.BIT.b5, EPX64R.OUT.H);
                    break;
                case SOURCE_CH.A4:
                    General.io.OutBit(EPX64R.PORT.P2, EPX64R.BIT.b6, EPX64R.OUT.H);
                    break;
                case SOURCE_CH.A5:
                    General.io.OutBit(EPX64R.PORT.P2, EPX64R.BIT.b7, EPX64R.OUT.H);
                    break;
                case SOURCE_CH.A6:
                    General.io.OutBit(EPX64R.PORT.P3, EPX64R.BIT.b0, EPX64R.OUT.H);
                    break;
                case SOURCE_CH.A7:
                    General.io.OutBit(EPX64R.PORT.P3, EPX64R.BIT.b1, EPX64R.OUT.H);
                    break;
                case SOURCE_CH.A8:
                    General.io.OutBit(EPX64R.PORT.P3, EPX64R.BIT.b2, EPX64R.OUT.H);
                    break;
                case SOURCE_CH.A9:
                    General.io.OutBit(EPX64R.PORT.P3, EPX64R.BIT.b3, EPX64R.OUT.H);
                    break;
                case SOURCE_CH.A10:
                    General.io.OutBit(EPX64R.PORT.P3, EPX64R.BIT.b4, EPX64R.OUT.H);
                    break;
                case SOURCE_CH.A11:
                    General.io.OutBit(EPX64R.PORT.P3, EPX64R.BIT.b5, EPX64R.OUT.H);
                    break;
                case SOURCE_CH.I1:
                    General.io.OutBit(EPX64R.PORT.P4, EPX64R.BIT.b2, EPX64R.OUT.H);//HL両方切り替えます
                    break;
                case SOURCE_CH.V1:
                    General.io.OutBit(EPX64R.PORT.P4, EPX64R.BIT.b3, EPX64R.OUT.H);//HL両方切り替えます
                    break;
                case SOURCE_CH.I2:
                    General.io.OutBit(EPX64R.PORT.P4, EPX64R.BIT.b4, EPX64R.OUT.H);//HL両方切り替えます
                    break;
                case SOURCE_CH.V2:
                    General.io.OutBit(EPX64R.PORT.P4, EPX64R.BIT.b5, EPX64R.OUT.H);//HL両方切り替えます
                    break;
                case SOURCE_CH.I3:
                    General.io.OutBit(EPX64R.PORT.P4, EPX64R.BIT.b6, EPX64R.OUT.H);//HL両方切り替えます
                    break;
                case SOURCE_CH.V3:
                    General.io.OutBit(EPX64R.PORT.P4, EPX64R.BIT.b7, EPX64R.OUT.H);//HL両方切り替えます
                    break;
                case SOURCE_CH.I4:
                    General.io.OutBit(EPX64R.PORT.P5, EPX64R.BIT.b0, EPX64R.OUT.H);//HL両方切り替えます
                    break;
                case SOURCE_CH.V4:
                    General.io.OutBit(EPX64R.PORT.P5, EPX64R.BIT.b1, EPX64R.OUT.H);//HL両方切り替えます
                    break;
            }

            //L側接続
            switch (ch)
            {
                case SOURCE_CH.A0:
                case SOURCE_CH.A1:

                case SOURCE_CH.A4:
                case SOURCE_CH.A5:

                case SOURCE_CH.A8:
                case SOURCE_CH.A9:

                    General.io.OutBit(EPX64R.PORT.P3, EPX64R.BIT.b6, EPX64R.OUT.H);
                    break;


                case SOURCE_CH.A2:
                case SOURCE_CH.A3:

                case SOURCE_CH.A6:
                case SOURCE_CH.A7:

                case SOURCE_CH.A10:
                case SOURCE_CH.A11:
                    General.io.OutBit(EPX64R.PORT.P4, EPX64R.BIT.b1, EPX64R.OUT.H);
                    break;

            }


            Sleep(500);
        }


        public static void SetCn10toChecker()
        {
            io.OutBit(EPX64R.PORT.P6, EPX64R.BIT.b5, EPX64R.OUT.L);
            io.OutBit(EPX64R.PORT.P6, EPX64R.BIT.b0, EPX64R.OUT.L);

            io.OutBit(EPX64R.PORT.P5, EPX64R.BIT.b7, EPX64R.OUT.H);
            io.OutBit(EPX64R.PORT.P6, EPX64R.BIT.b7, EPX64R.OUT.H);


        }

        public static void SetCn10to6ダイヤル抵抗()
        {
            io.OutBit(EPX64R.PORT.P6, EPX64R.BIT.b5, EPX64R.OUT.H);
            io.OutBit(EPX64R.PORT.P6, EPX64R.BIT.b0, EPX64R.OUT.H);

            io.OutBit(EPX64R.PORT.P5, EPX64R.BIT.b7, EPX64R.OUT.L);
            io.OutBit(EPX64R.PORT.P6, EPX64R.BIT.b7, EPX64R.OUT.L);

        }

        public static void SetCn10to6ダイヤル抵抗Aのみ()
        {
            io.OutBit(EPX64R.PORT.P6, EPX64R.BIT.b5, EPX64R.OUT.H);
            io.OutBit(EPX64R.PORT.P5, EPX64R.BIT.b7, EPX64R.OUT.L);

            io.OutBit(EPX64R.PORT.P6, EPX64R.BIT.b0, EPX64R.OUT.L);
            io.OutBit(EPX64R.PORT.P6, EPX64R.BIT.b7, EPX64R.OUT.L);
        }

        public static void SetCn10to6ダイヤル抵抗Bのみ()
        {
            io.OutBit(EPX64R.PORT.P6, EPX64R.BIT.b5, EPX64R.OUT.L);
            io.OutBit(EPX64R.PORT.P5, EPX64R.BIT.b7, EPX64R.OUT.L);

            io.OutBit(EPX64R.PORT.P6, EPX64R.BIT.b0, EPX64R.OUT.H);
            io.OutBit(EPX64R.PORT.P6, EPX64R.BIT.b7, EPX64R.OUT.L);
        }







    }

}


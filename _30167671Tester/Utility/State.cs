using System;
using System.Collections.Generic;
using System.Linq;

namespace _30167671Tester
{
    public enum TEST_MODE { _30167671, _30221500 }

    public class TestSpecs
    {
        public int Key;
        public string Value;
        public bool PowSw;

        public TestSpecs(int key, string value, bool powSW = true)
        {
            this.Key = key;
            this.Value = value;
            this.PowSw = powSW;

        }
    }

    public static class State
    {
        public static TEST_MODE testMode { get; set; }

        //データソース（バインディング用）
        public static ViewModelMainWindow VmMainWindow = new ViewModelMainWindow();
        public static ViewModelTestStatus VmTestStatus = new ViewModelTestStatus();
        public static ViewModelTestResult VmTestResults = new ViewModelTestResult();

        public static ViewModelCommunication VmComm = new ViewModelCommunication();
        public static TestCommand testCommand = new TestCommand();



        //パブリックメンバ
        public static Configuration Setting { get; set; }
        public static TestSpec TestSpec { get; set; }

        public static string CurrDir { get; set; }

        public static string AssemblyInfo { get; set; }

        public static double CurrentThemeOpacity { get; set; }

        public static Uri uriOtherInfoPage { get; set; }

        public static string LastSerial { get; set; }

        public static int NewSerial { get; set; }




        //リトライ履歴保存用リスト
        public static List<string> RetryLogList = new List<string>();

        //検査成績書関連
        public static List<string> testDataPWA = new List<string>();



        public static List<TestSpecs> テスト項目 = new List<TestSpecs>()
        {
            new TestSpecs(100, "コネクタ実装チェック", false),
            new TestSpecs(101, "CN4未半田チェック1", false),
            new TestSpecs(102, "CN4未半田チェック2", false),
            new TestSpecs(103, "JP1短絡ソケットチェック", false),

            new TestSpecs(200, "検査ソフト書き込み", false),

            new TestSpecs(300, "3Vライン消費電流チェック", false),
            new TestSpecs(301, "6Vライン消費電流チェック", false),
            new TestSpecs(302, "電源電圧チェック 5V", false),
            new TestSpecs(303, "電源電圧チェック 3.3V", true),
            new TestSpecs(304, "CN3 出力電圧チェック", true),
            new TestSpecs(305, "CN9On出力電圧チェック", true),
            new TestSpecs(306, "CN9Off出力電圧チェック", true),

            new TestSpecs(400, "粒LEDカラーチェック", false),
            new TestSpecs(401, "粒LED輝度チェック", true),
            new TestSpecs(402, "7セグ 輝度チェック", true),
            new TestSpecs(403, "7セグ 点灯チェック", true),

            new TestSpecs(500, "Bluetooth通信確認", true),
            new TestSpecs(501, "AT通信確認", true),
            new TestSpecs(502, "増設子機有りに設定", true),
            new TestSpecs(503, "RS485通信確認1", true),
            new TestSpecs(504, "RS485通信確認2", true),

            new TestSpecs(600, "SW1-SW4チェック", true),

            new TestSpecs(700, "カレントセンサチェック", true),

            new TestSpecs(800, "サーミスタ調整 5℃", true),
            new TestSpecs(801, "サーミスタチェック", true),
            new TestSpecs(802, "サーミスタ開放チェック", false),
            new TestSpecs(803, "サーミスタ短絡チェック", false),

            new TestSpecs(900, "電源基板SW2チェック", true),

            new TestSpecs(1000, "停電検出チェック", true),

            new TestSpecs(1100, "バッテリLowチェック", true),

            new TestSpecs(1200, "警報リレー出力チェック", true),

            new TestSpecs(1300, "パラメータチェック", true),

            new TestSpecs(1400, "RTCチェック", true),

        };


        //個別設定のロード
        public static void LoadConfigData()
        {
            //Configファイルのロード
            Setting = Deserialize<Configuration>(Constants.filePath_Configuration);


            VmMainWindow.ListOperator = Setting.作業者リスト;
            VmMainWindow.Theme = Setting.PathTheme;
            VmMainWindow.ThemeOpacity = Setting.OpacityTheme;

            if (Setting.日付 != DateTime.Now.ToString("yyyyMMdd"))
            {
                Setting.日付 = DateTime.Now.ToString("yyyyMMdd");
                Setting.TodayOkCount = 0;
                Setting.TodayNgCount = 0;
            }

            VmTestStatus.OkCount = Setting.TodayOkCount.ToString() + "台";
            VmTestStatus.NgCount = Setting.TodayNgCount.ToString() + "台";

            //TestSpecファイルのロード
            TestSpec = Deserialize<TestSpec>(Constants.filePath_TestSpec);
        }


        //インスタンスをXMLデータに変換する
        public static bool Serialization<T>(T obj, string xmlFilePath)
        {
            try
            {
                //XmlSerializerオブジェクトを作成
                //オブジェクトの型を指定する
                System.Xml.Serialization.XmlSerializer serializer =
                    new System.Xml.Serialization.XmlSerializer(typeof(T));
                //書き込むファイルを開く（UTF-8 BOM無し）
                System.IO.StreamWriter sw = new System.IO.StreamWriter(xmlFilePath, false, new System.Text.UTF8Encoding(false));
                //シリアル化し、XMLファイルに保存する
                serializer.Serialize(sw, obj);
                //ファイルを閉じる
                sw.Close();

                return true;

            }
            catch
            {
                return false;
            }

        }

        //XMLデータからインスタンスを生成する
        public static T Deserialize<T>(string xmlFilePath)
        {
            System.Xml.Serialization.XmlSerializer serializer;
            using (var sr = new System.IO.StreamReader(xmlFilePath, new System.Text.UTF8Encoding(false)))
            {
                serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(sr);
            }
        }

        //********************************************************
        //個別設定データの保存
        //********************************************************
        public static bool Save個別データ()
        {
            try
            {
                //Configファイルの保存
                Setting.作業者リスト = VmMainWindow.ListOperator;
                Setting.PathTheme = VmMainWindow.Theme;
                Setting.OpacityTheme = VmMainWindow.ThemeOpacity;

                Serialization<Configuration>(Setting, Constants.filePath_Configuration);


                return true;
            }
            catch
            {
                return false;

            }

        }


        public static bool LoadLastSerial(string filePath)
        {
            try
            {
                // csvファイルを開く
                using (var sr = new System.IO.StreamReader(filePath))
                {
                    var listTestResults = new List<string>();
                    // ストリームの末尾まで繰り返す
                    while (!sr.EndOfStream)
                    {
                        // ファイルから一行読み込んでリストに追加
                        listTestResults.Add(sr.ReadLine());
                    }

                    var lastData = listTestResults.Last();
                    LastSerial = lastData.Split(',')[0];
                    return true;
                }
            }
            catch
            {
                return false;
                // ファイルを開くのに失敗したとき
            }
        }

    }

}

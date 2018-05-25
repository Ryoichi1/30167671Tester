using System;
using System.Collections.Generic;
using System.Linq;

namespace _30167671Tester
{
    public enum ITEM { _30167671, _30221500 }

    public class TestSpecs
    {
        public int Key;
        public string Value;
        public bool PowSw;

        public TestSpecs(int key, string value, bool powSW = false)
        {
            this.Key = key;
            this.Value = value;
            this.PowSw = powSW;

        }
    }

    public static class State
    {
        public static ITEM TestItem { get; set; }

        //データソース（バインディング用）
        public static ViewModelMainWindow VmMainWindow = new ViewModelMainWindow();
        public static ViewModelTestStatus VmTestStatus = new ViewModelTestStatus();
        public static ViewModelTestResult VmTestResults = new ViewModelTestResult();
        public static ViewModelMente VmMente = new ViewModelMente();
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

        public static string シリアルナンバー年月部分 { get; set; }


        //リトライ履歴保存用リスト
        public static List<string> RetryLogList = new List<string>();


        public static List<TestSpecs> テスト項目 = new List<TestSpecs>()
        {
            //new TestSpecs(100, "コネクタ実装チェック"),

            //new TestSpecs(200, "ファームウェア書き込み"),

            new TestSpecs(300, "電源電圧ﾁｪｯｸ +12V", true),
            //new TestSpecs(301, "電源電圧ﾁｪｯｸ +5V", true),
            //new TestSpecs(302, "電源電圧ﾁｪｯｸ +3.3V", true),
            //new TestSpecs(303, "電源電圧ﾁｪｯｸ AVDD_12V", true),
            //new TestSpecs(304, "電源電圧ﾁｪｯｸ AVCC_5V", true),
            //new TestSpecs(305, "電源電圧ﾁｪｯｸ VREF", true),
            //new TestSpecs(306, "電源電圧ﾁｪｯｸ AVCCD_5V", true),
            //new TestSpecs(307, "電源電圧ﾁｪｯｸ S_5V", true),

            //new TestSpecs(400, "入力回路ﾁｪｯｸ　OFF"),
            //new TestSpecs(401, "入力回路ﾁｪｯｸ　ON"),

            //new TestSpecs(500, "ANﾎﾟｰﾄﾃﾞｼﾞﾀﾙ入力ﾁｪｯｸ　LOW"),
            //new TestSpecs(501, "ANﾎﾟｰﾄﾃﾞｼﾞﾀﾙ入力ﾁｪｯｸ　HIGH"),

            //new TestSpecs(600, "SCR駆動回路ﾁｪｯｸ　位相制御ﾓｰﾄﾞ"),
            //new TestSpecs(601, "SCR駆動回路ﾁｪｯｸ　ｻｲｸﾙ制御ﾓｰﾄﾞ", true),

            //new TestSpecs(700, "AD入力回路ﾁｪｯｸ　AN_A8"),
            //new TestSpecs(701, "AD入力回路ﾁｪｯｸ　AN_A4"),
            //new TestSpecs(702, "AD入力回路ﾁｪｯｸ　AN_A0"),
            //new TestSpecs(703, "AD入力回路ﾁｪｯｸ　AN_A9"),
            //new TestSpecs(704, "AD入力回路ﾁｪｯｸ　AN_A5"),
            //new TestSpecs(705, "AD入力回路ﾁｪｯｸ　AN_A1"),
            //new TestSpecs(706, "AD入力回路ﾁｪｯｸ　AN_A10"),
            //new TestSpecs(707, "AD入力回路ﾁｪｯｸ　AN_A6"),
            //new TestSpecs(708, "AD入力回路ﾁｪｯｸ　AN_A2"),
            //new TestSpecs(709, "AD入力回路ﾁｪｯｸ　AN_A11"),
            //new TestSpecs(710, "AD入力回路ﾁｪｯｸ　AN_A7"),
            //new TestSpecs(711, "AD入力回路ﾁｪｯｸ　AN_A3"),

            //new TestSpecs(800, "ﾊﾟﾙｽ入力回路ﾁｪｯｸ　左"),
            //new TestSpecs(801, "ﾊﾟﾙｽ入力回路ﾁｪｯｸ　中"),
            //new TestSpecs(802, "ﾊﾟﾙｽ入力回路ﾁｪｯｸ　右"),

            //new TestSpecs(900, "比例弁回転動作ﾁｪｯｸ　ﾓｰﾀAB左回転"),
            //new TestSpecs(901, "比例弁回転動作ﾁｪｯｸ　ﾓｰﾀAB右回転"),

            //new TestSpecs(1000, "警報用Pt100回路ﾁｪｯｸ　発報点ﾁｪｯｸ"),
            //new TestSpecs(1001, "断線ﾁｪｯｸ"),

            //new TestSpecs(1100, "AC電源電圧読取り回路ﾁｪｯｸ"),

            //new TestSpecs(1200, "負荷電流読取り回路ﾁｪｯｸ　CT1"),
            //new TestSpecs(1201, "負荷電流読取り回路ﾁｪｯｸ　CT2"),

            //new TestSpecs(1300, "RS232C通信ﾁｪｯｸ"),
            //new TestSpecs(1301, "RS485通信ﾁｪｯｸ（絶縁）"),
            //new TestSpecs(1302, "RS485通信ﾁｪｯｸ（非絶縁）"),
            //new TestSpecs(1303, "表示基板通信ﾁｪｯｸ"),

            //new TestSpecs(1400, "Vref調整"),
            //new TestSpecs(1401, "Vref調整(再)"),

            //new TestSpecs(1500, "出力回路ﾁｪｯｸ　ﾃﾞｼﾞﾀﾙ出力"),

            //new TestSpecs(1600, "出力回路ﾁｪｯｸ 電流温度ﾓﾆﾀ1"),
            //new TestSpecs(1601, "出力回路ﾁｪｯｸ 電流温度ﾓﾆﾀ2"),

            //new TestSpecs(1700, "出力回路ﾁｪｯｸ 電圧温度ﾓﾆﾀ1"),
            //new TestSpecs(1701, "出力回路ﾁｪｯｸ 電圧温度ﾓﾆﾀ2"),

            //new TestSpecs(1800, "出力回路ﾁｪｯｸ ｲﾝﾊﾞｰﾀ回転指令"),

            //new TestSpecs(1900, "出力回路ﾁｪｯｸ 出力電流1"),
            //new TestSpecs(1901, "出力回路ﾁｪｯｸ 出力電流2"),
            //new TestSpecs(1902, "出力回路ﾁｪｯｸ 出力電流3"),
            //new TestSpecs(1903, "出力回路ﾁｪｯｸ 出力電流4"),

            //new TestSpecs(2000, "出力回路ﾁｪｯｸ 出力電圧1"),
            //new TestSpecs(2001, "出力回路ﾁｪｯｸ 出力電圧2"),
            //new TestSpecs(2002, "出力回路ﾁｪｯｸ 出力電圧3"),
            //new TestSpecs(2003, "出力回路ﾁｪｯｸ 出力電圧4"),

            //new TestSpecs(2100, "Pt100ｾﾝｻｰ回路調整 PV1,2"),

            //new TestSpecs(2200, "Pt100ｾﾝｻｰ回路調整 PV3,4"),

            //new TestSpecs(2600, "Pt100ｾﾝｻｰ断線ﾁｪｯｸ Normal"),
            //new TestSpecs(2601, "Pt100ｾﾝｻｰ断線ﾁｪｯｸ A線断線"),
            //new TestSpecs(2602, "Pt100ｾﾝｻｰ断線ﾁｪｯｸ B線断線"),
            //new TestSpecs(2603, "Pt100ｾﾝｻｰ断線ﾁｪｯｸ B'線断線"),

            //new TestSpecs(2700, "電流入力回路I-1"),
            //new TestSpecs(2701, "電流入力回路I-2"),
            //new TestSpecs(2702, "電流入力回路I-3"),
            //new TestSpecs(2703, "電流入力回路I-4"),

            //new TestSpecs(2800, "電圧入力回路V-1"),
            //new TestSpecs(2801, "電圧入力回路V-2"),
            //new TestSpecs(2802, "電圧入力回路V-3"),
            //new TestSpecs(2803, "電圧入力回路V-4"),

            //new TestSpecs(2900, "2線式液漏れｾﾝｻ回路"),

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

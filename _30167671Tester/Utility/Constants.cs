
namespace _30167671Tester
{
    public static class Constants
    {
        //スタートボタンの表示
        public const string 開始 = "検査開始";
        public const string 停止 = "強制停止";
        public const string 確認 = "確認";

        //作業者へのメッセージ
        public const string MessOpecode  = "工番を入力してください";
        public const string MessOperator = "作業者名を選択してください";
        public const string MessSet  = "製品をセットして開始ボタンを押して下さい";
        public const string MessRemove   = "製品を取り外してください";
        public const string MessWait     = "検査中！　しばらくお待ちください・・・";
        public const string MessConnect  = "周辺機器の接続を確認してください！";

        public static readonly string filePath_TestSpec      = @"C:\新CPU基板\ConfigData\TestSpec.config";
        public static readonly string filePath_Configuration = @"C:\新CPU基板\ConfigData\Configuration.config";

        public static readonly string RwsPath_Product        = @"C:\新CPU基板\FW_WRITE\ForProduct\自記温度計書き込み\自記温度計書き込み.rws";

        public static readonly string Path_ManualPwaTest     = @"C:\新CPU基板\Manual_PWA_FT.pdf";

        //検査データフォルダのパス
        public static readonly string PassData30167671FolderPath  = @"C:\新CPU基板\検査データ\30167671\合格品データ\";
        public static readonly string FailData30167671FolderPath  = @"C:\新CPU基板\検査データ\30167671\不良品データ\";
        public static readonly string fileName_RetryLog30167671   = @"C:\新CPU基板\検査データ\30167671\不良品データ\" + "リトライ履歴.txt";

        public static readonly string PassData30221500FolderPath  = @"C:\新CPU基板\検査データ\30221500\合格品データ\";
        public static readonly string FailData30221500FolderPath  = @"C:\新CPU基板\検査データ\30221500\不良品データ\";
        public static readonly string fileName_RetryLog30221500   = @"C:\新CPU基板\検査データ\30221500\不良品データ\リトライ履歴.txt";
        
        public static readonly string PathMaster位相制御           = @"C:\新CPU基板\WavData\Master位相制御.txt";
        public static readonly string PathMasterサイクル制御       = @"C:\新CPU基板\WavData\Masterサイクル制御.txt";

        //Imageの透明度
        public const double OpacityMin = 0.1;
        public const double OpacityImgMin = 0.0;

        //リトライ回数
        public static readonly int RetryCount = 1;












    }
}

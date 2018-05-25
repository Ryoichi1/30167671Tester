
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
        public const string MessSet      = "製品をセットして開始ボタンを押してください";
        public const string MessRemove   = "製品を取り外してください";
        public const string MessWait     = "検査中！　しばらくお待ちください";
        public const string MessConnect  = "周辺機器の接続を確認してください！";

        //パスいろいろ
        public static readonly string rootPath = @"C:\新CPU基板\";
        public static readonly string filePath_TestSpec      = $@"{rootPath}ConfigData\TestSpec.config";
        public static readonly string filePath_Configuration = $@"{rootPath}ConfigData\Configuration.config";
        public static readonly string FdtPath                = $@"{rootPath}WriteFw\WriteFw.AWS";
        public static readonly string Path_ManualPwaTest     = $@"{rootPath}Manual_PWA_FT.pdf";

        public static readonly string PassDataPath_30167671  = $@"{rootPath}検査データ\30167671\合格品データ\";
        public static readonly string FailDataPath_30167671  = $@"{rootPath}検査データ\30167671\不良品データ\";
        public static readonly string RetryLogPath_30167671  = $@"{rootPath}検査データ\30167671\不良品データ\リトライ履歴.txt";

        public static readonly string PassDataPath_30221500  = $@"{rootPath}検査データ\30221500\合格品データ\";
        public static readonly string FailDataPath_30221500  = $@"{rootPath}検査データ\30221500\不良品データ\";
        public static readonly string RetryLogPath_30221500  = $@"{rootPath}検査データ\30221500\不良品データ\リトライ履歴.txt";
        
        public static readonly string PathMaster位相制御      = $@"{rootPath}WavData\Master位相制御.csv";
        public static readonly string PathMasterサイクル制御  = $@"{rootPath}WavData\Masterサイクル制御.csv";

        //リトライ回数
        public static readonly int RetryCount = 1;












    }
}

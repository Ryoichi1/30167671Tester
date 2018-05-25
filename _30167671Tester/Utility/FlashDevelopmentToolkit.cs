using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Timers;
using static System.Threading.Thread;


namespace _30167671Tester
{
    ///<summary>
    ///●注意事項
    ///このクラスはＦＤＴをシンプルインターフェイスで立ち上げることを前提に作ってあります
    /// </summary>

    public static class FDT
    {
        //********************************************************************************************************
        // 外部プロセスのメイン・ウィンドウを起動するためのWin32 API
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll")]
        private static extern bool IsIconic(IntPtr hWnd);
        [DllImport("user32.dll")]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")]
        static extern IntPtr FindWindowEx(IntPtr hWnd, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, int wParam, StringBuilder lParam);

        private const int WM_GETTEXT = 0x000D;

        // ShowWindowAsync関数のパラメータに渡す定義値
        private const int SW_RESTORE = 9;  // 画面を元の大きさに戻す
        //********************************************************************************************************


        //静的メンバの宣言
        private static System.Timers.Timer Tm;

        //フラグ
        private static bool FlagTimer;
        public static bool FlagWrite { get; private set; }
        public static bool FlagSum { get; private set; }


        //コンストラクタ
        static FDT()
        {
            //タイマー（ウィンドウハンドル取得用）の設定
            Tm = new System.Timers.Timer();
            Tm.Enabled = false;
            Tm.Interval = 5000;
            Tm.Elapsed += new ElapsedEventHandler(tm_Tick);
        }


        //タイマーイベントハンドラ
        private static void tm_Tick(object source, ElapsedEventArgs e)
        {
            Tm.Stop();
            FlagTimer = false;//タイムアウト

        }


        public static async  Task<bool> WriteFirmware(string fdtPath, string sum/*example 0x00DDEEFF*/)
        {
            //フラグの初期化
            FlagWrite = false;
            FlagSum = false;

            var result = await Task.Run<bool>(() =>
            {
                var Fdt = new Process();

                try
                {
                    //プロセスを作成してFDTを立ち上げる

                    Fdt.StartInfo.FileName = fdtPath;
                    Fdt.Start();
                    Fdt.WaitForInputIdle();//指定されたプロセスで未処理の入力が存在せず、ユーザーからの入力を待っている状態になるまで、またはタイムアウト時間が経過するまで待機します。

                    //FDTのウィンドウハンドル取得
                    FlagTimer = true;
                    Tm.Start();
                    IntPtr hWnd = IntPtr.Zero;//メインウィンドウのハンドル
                    while (hWnd == IntPtr.Zero)
                    {
                        Application.DoEvents();
                        if (!FlagTimer) return false;
                        hWnd = FindWindow(null, "FDT Simple Interface   (Supported Version)");
                    }

                    IntPtr ログテキストハンドル = FindWindowEx(hWnd, IntPtr.Zero, "RICHEDIT", "");

                    //FDTを最前面に表示してアクティブにする（センドキー送るため）
                    SetForegroundWindow(hWnd);
                    Sleep(1000);

                    SendKeys.SendWait("{TAB}");
                    Sleep(200);
                    SendKeys.SendWait("{ENTER}");
                    Sleep(600);
                    SendKeys.SendWait("{ENTER}");
                    
                    int MaxSize = 10000;
                    StringBuilder sb = new StringBuilder(MaxSize);
                    string LogBuff = "";

                    while (true)
                    {
                        Sleep(1000);//インターバル1秒　※インターバル無しの場合FDTがこける
                        SendMessage(ログテキストハンドル, WM_GETTEXT, MaxSize - 1, sb);
                        LogBuff = sb.ToString();
                        if (LogBuff.IndexOf("Error") >= 0) 
                            return false;
                        if (LogBuff.IndexOf("Disconnected") >= 0) break;
                    }

                    FlagWrite = (LogBuff.IndexOf("書き込みが完了しました") >= 0);
                    if (!FlagWrite) return false;

                    //チェックサムがあっているかの判定
                    FlagSum = (LogBuff.IndexOf("Checksum: " + sum) >= 0);
                    return FlagSum;

                }
                catch
                {
                    return false;
                }
                finally
                {
                    if (Fdt != null)
                    {
                        Fdt.Kill();
                        Fdt.Close();
                        Fdt.Dispose();
                    }
                }
            });

            return result;
            //E1エミュレータを切り離すする（クラス外部で処理する）

            }

        






    }

}
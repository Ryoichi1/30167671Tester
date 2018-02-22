
namespace _30167671Tester
{
    public class TestSpec
    {
        //試験用パラメータ
        public string TestSpecVer { get; set; }   //製品ファームウェアチェックサム
        public string FirmwareSum { get; set; }   //製品ファームウェアチェックサム
        public string FirmwareName { get; set; }   //製品ファームウェアのファイル名

        public double V12v_Max { get; set; }   //+12V電源電圧max
        public double V12v_Min { get; set; }   //+12V電源電圧min
        public double V5v_Max { get; set; }   //+5V電源電圧max
        public double V5v_Min { get; set; }   //+5V電源電圧min
        public double V3v_Max { get; set; }   //+3.3V電源電圧max
        public double V3v_Min { get; set; }   //+3.3V電源電圧min
        public double Avdd_Max { get; set; }   //AVDD電源電圧max
        public double Avdd_Min { get; set; }   //AVDD電源電圧min
        public double Avcc_Max { get; set; }   //AVCC電源電圧max
        public double Avcc_Min { get; set; }   //AVCC電源電圧min
        public double Vref_Max { get; set; }   //VREF電源電圧max
        public double Vref_Min { get; set; }   //VREF電源電圧min
        public double Avccd_Max { get; set; }   //AVCCD電源電圧max
        public double Avccd_Min { get; set; }   //AVCCD電源電圧min
        public double S5v_Max { get; set; }   //S_5v電源電圧max
        public double S5v_Min { get; set; }   //S_5v電源電圧min

        public double Ana12_15_1 { get; set; }   //ANﾎﾟｰﾄﾃﾞｼﾞﾀﾙ入力 ANA12-15読み取り値①上限値
        public double Ana12_15_2 { get; set; }   //ANﾎﾟｰﾄﾃﾞｼﾞﾀﾙ入力 ANA12-15読み取り値②下限値
        public double Ana17_27_1 { get; set; }   //ANﾎﾟｰﾄﾃﾞｼﾞﾀﾙ入力 ANA17-27読み取り値①下限値
        public double Ana17_27_2 { get; set; }   //ANﾎﾟｰﾄﾃﾞｼﾞﾀﾙ入力 ANA17-27読み取り値②上限値

        public double AdInput_0Max { get; set; }   //AD入力回路 0V入力電圧読み込み値max
        public double AdInput_0Min { get; set; }   //AD入力回路 0V入力電圧読み込み値min
        public double AdInput_5Max { get; set; }   //AD入力回路 5V入力電圧読み込み値max
        public double AdInput_5Min { get; set; }   //AD入力回路 5V入力電圧読み込み値min
        public double AdInput_10Max { get; set; }   //AD入力回路 10V入力電圧読み込み値max
        public double AdInput_10Min { get; set; }   //AD入力回路 10V入力電圧読み込み値min

        public double Pulse_L_Max { get; set; }   //パルス入力左max
        public double Pulse_L_Min { get; set; }   //パルス入力左min
        public double Pulse_M_Max { get; set; }   //パルス入力中max
        public double Pulse_M_Min { get; set; }   //パルス入力中min
        public double Pulse_R_Max { get; set; }   //パルス入力右max
        public double Pulse_R_Min { get; set; }   //パルス入力右min

        public double Temp_A_0_Max { get; set; }   //PWTMP_A_0 出力電流max
        public double Temp_A_0_Min { get; set; }   //PWTMP_A_0 出力電流min
        public double Temp_A_150_Max { get; set; }   //PWTMP_A_150 出力電流max
        public double Temp_A_150_Min { get; set; }   //PWTMP_A_150 出力電流min
        public double Temp_A_300_Max { get; set; }   //PWTMP_A_300 出力電流max
        public double Temp_A_300_Min { get; set; }   //PWTMP_A_300 出力電流min

        public double Temp_V_0_Max { get; set; }   //PWTMP_V_0 出力電圧max
        public double Temp_V_0_Min { get; set; }   //PWTMP_V_0 出力電圧min
        public double Temp_V_40_Max { get; set; }   //PWTMP_V_40 出力電圧max
        public double Temp_V_40_Min { get; set; }   //PWTMP_V_40 出力電圧min
        public double Temp_V_75_Max { get; set; }   //PWTMP_V_75 出力電圧max
        public double Temp_V_75_Min { get; set; }   //PWTMP_V_75 出力電圧min

        public double Inv_0_Max { get; set; }   //PWINV_0 出力電圧max
        public double Inv_0_Min { get; set; }   //PWINV_0 出力電圧min
        public double Inv_1800_Max { get; set; }   //PWINV_1800 出力電圧max
        public double Inv_1800_Min { get; set; }   //PWINV_1800 出力電圧min
        public double Inv_3600_Max { get; set; }   //PWINV_3600 出力電圧max
        public double Inv_3600_Min { get; set; }   //PWINV_3600 出力電圧min

        public double Iout_0_Max { get; set; }   //PWIOUT_0 出力電圧max
        public double Iout_0_Min { get; set; }   //PWIOUT_0 出力電圧min
        public double Iout_50_Max { get; set; }   //PWIOUT_50 出力電圧max
        public double Iout_50_Min { get; set; }   //PWIOUT_50 出力電圧min
        public double Iout_100_Max { get; set; }   //PWIOUT_100 出力電圧max
        public double Iout_100_Min { get; set; }   //PWIOUT_100 出力電圧min

        public double Vout_0_Max { get; set; }   //PWVOUT_0 出力電圧max
        public double Vout_0_Min { get; set; }   //PWVOUT_0 出力電圧min
        public double Vout_50_Max { get; set; }   //PWVOUT_50 出力電圧max
        public double Vout_50_Min { get; set; }   //PWVOUT_50 出力電圧min
        public double Vout_100_Max { get; set; }   //PWVOUT_100 出力電圧max
        public double Vout_100_Min { get; set; }   //PWVOUT_100 出力電圧min

        public double 位置制御_OnOff_Max { get; set; }   //
        public double 位置制御_OnOff_Min { get; set; }   //
        public double サイクル制御_OnOff_Max { get; set; }   //
        public double サイクル制御_OnOff_Min { get; set; }   //
        public double V0_p_Max { get; set; }   //
        public double V0_p_Min { get; set; }   //

        public double 警報発報点_Max { get; set; }   //警報用Pt100 警報発報点max
        public double 警報発報点_Min { get; set; }   //警報用Pt100 警報発報点min

        public double 電圧換算値_Max { get; set; }   //AC電源電圧読み取り回路 電圧換算値max
        public double 電圧換算値_Min { get; set; }   //AC電源電圧読み取り回路 電圧換算値min
        public double 電流換算値_Max { get; set; }   //負荷電流読み取り回路 電圧換算値max
        public double 電流換算値_Min { get; set; }   //負荷電流読み取り回路 電圧換算値min

        public string Vref調整_Max { get; set; }   //Vref調整値max
        public string Vref調整_Min { get; set; }   //Vref調整値min

        public double Temp60_Max { get; set; }   //Pt100回路調整 60℃max
        public double Temp60_Min { get; set; }   //Pt100回路調整 60℃min
        public double I_10mA_Max { get; set; }   //電流入力回路調整 10mA max
        public double I_10mA_Min { get; set; }   //電流入力回路調整 10mA min
        public double V_5V_Max { get; set; }   //電圧入力回路調整 5V max
        public double V_5V_Min { get; set; }   //電圧入力回路調整 5V min
        public double 液漏れ20k_Max { get; set; }   //二線式液漏れセンサ回路 20kΩmax
        public double 液漏れ20k_Min { get; set; }   //二線式液漏れセンサ回路 20kΩmin
        public double PT100_NormalTemp_Max { get; set; }   //二線式液漏れセンサ回路 20kΩmax
        public double PT100_NormalTemp_Min { get; set; }   //二線式液漏れセンサ回路 20kΩmin

        public int PV第一Max { get; set; }   //
        public int PV第一Min { get; set; }   //

        public int PV第二Max { get; set; }   //
        public int PV第二Min { get; set; }   //

        public int PV第三Max { get; set; }   //
        public int PV第三Min { get; set; }   //

    }
}

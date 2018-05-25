using System;
using System.Threading;
using System.Threading.Tasks;
using static System.Threading.Thread;

namespace _30167671Tester
{
    public static class TEST_電源電圧
    {
        public enum VOL_CH
        {
            _12V, _5V, _3_3V, AVDD, AVCC, VREF, AVCCD, S5V
        }

        public static void SetRelay(VOL_CH ch)
        {
            //7016のMeas端子に接続する処理
            switch (ch)
            {
                case VOL_CH._12V:
                    General.Set7012Meas(General.MEAS_CH._12V);
                    break;
                case VOL_CH._5V:
                    General.Set7012Meas(General.MEAS_CH._5V);
                    break;
                case VOL_CH._3_3V:
                    General.Set7012Meas(General.MEAS_CH._3_3V);
                    break;
                case VOL_CH.AVDD:
                    General.Set7012Meas(General.MEAS_CH.AVDD);
                    break;
                case VOL_CH.AVCC:
                    General.Set7012Meas(General.MEAS_CH.AVCC);
                    break;
                case VOL_CH.VREF:
                    General.Set7012Meas(General.MEAS_CH.VREF);
                    break;
                case VOL_CH.AVCCD:
                    General.Set7012Meas(General.MEAS_CH.AVCCD);
                    break;
                case VOL_CH.S5V:
                    General.Set7012Meas(General.MEAS_CH.S5V);
                    break;
            }
        }

        public static async Task<bool> CheckVolt(VOL_CH ch)
        {
            bool result = false;
            Double measData = 0;
            double Max = 0;
            double Min = 0;

            try
            {
                return await Task<bool>.Run(() =>
                {
                    try
                    {
                        SetRelay(ch);
                        Thread.Sleep(400);

                        switch (ch)
                        {
                            case VOL_CH._12V:
                                Max = State.TestSpec.V12v_Max;
                                Min = State.TestSpec.V12v_Min;
                                break;
                            case VOL_CH._5V:
                                Max = State.TestSpec.V5v_Max;
                                Min = State.TestSpec.V5v_Min;
                                break;
                            case VOL_CH._3_3V:
                                Max = State.TestSpec.V3v_Max;
                                Min = State.TestSpec.V3v_Min;
                                break;
                            case VOL_CH.AVDD:
                                Max = State.TestSpec.Avdd_Max;
                                Min = State.TestSpec.Avdd_Min;
                                break;
                            case VOL_CH.AVCC:
                                Max = State.TestSpec.Avcc_Max;
                                Min = State.TestSpec.Avcc_Min;
                                break;
                            case VOL_CH.VREF:
                                Max = State.TestSpec.Vref_Max;
                                Min = State.TestSpec.Vref_Min;
                                break;
                            case VOL_CH.AVCCD:
                                Max = State.TestSpec.Avccd_Max;
                                Min = State.TestSpec.Avccd_Min;
                                break;
                            case VOL_CH.S5V:
                                Max = State.TestSpec.S5v_Max;
                                Min = State.TestSpec.S5v_Min;
                                break;
                        }

                        if (ch == VOL_CH.VREF)
                        {
                            HIOKI7012.MeasureDcV(HIOKI7012.MEAS_MODE.V_2_5);
                        }
                        else
                        {
                            HIOKI7012.MeasureDcV();
                        }

                        measData = HIOKI7012.VoltData;

                        return result = (Min < measData && measData < Max);
                    }
                    catch
                    {
                        return result = false;
                    }

                });
            }
            finally
            {

                //ビューモデルの更新
                switch (ch)
                {
                    case VOL_CH._12V:
                        State.VmTestResults.Vol12V = measData.ToString("F2") + "V";
                        State.VmTestResults.ColVol12V = result ? General.OffBrush : General.NgBrush;
                        break;
                    case VOL_CH._5V:
                        State.VmTestResults.Vol5V = measData.ToString("F2") + "V";
                        State.VmTestResults.ColVol5V = result ? General.OffBrush : General.NgBrush;
                        break;
                    case VOL_CH._3_3V:
                        State.VmTestResults.Vol3_3V = measData.ToString("F2") + "V";
                        State.VmTestResults.ColVol3_3V = result ? General.OffBrush : General.NgBrush;
                        break;
                    case VOL_CH.AVDD:
                        State.VmTestResults.VolAVDD = measData.ToString("F2") + "V";
                        State.VmTestResults.ColVolAVDD = result ? General.OffBrush : General.NgBrush;
                        break;
                    case VOL_CH.AVCC:
                        State.VmTestResults.VolAVCC = measData.ToString("F2") + "V";
                        State.VmTestResults.ColVolAVCC = result ? General.OffBrush : General.NgBrush;
                        break;
                    case VOL_CH.VREF:
                        State.VmTestResults.VolVREF = measData.ToString("F4") + "V";
                        State.VmTestResults.ColVolVREF = result ? General.OffBrush : General.NgBrush;
                        break;
                    case VOL_CH.AVCCD:
                        State.VmTestResults.VolAVCCD = measData.ToString("F2") + "V";
                        State.VmTestResults.ColVolAVCCD = result ? General.OffBrush : General.NgBrush;
                        break;
                    case VOL_CH.S5V:
                        State.VmTestResults.VolS5V = measData.ToString("F2") + "V";
                        State.VmTestResults.ColVolS5V = result ? General.OffBrush : General.NgBrush;
                        break;

                }

                //NGだった場合、エラー詳細情報の規格値を更新する
                if (!result)
                {
                    if (ch == VOL_CH.VREF)
                    {
                        State.VmTestStatus.Spec = $"規格値 : {Min.ToString("F4")} ～ {Max.ToString("F4")}V";
                        State.VmTestStatus.MeasValue = "計測値 : " + measData.ToString("F4") + "V";
                    }
                    else
                    {
                        State.VmTestStatus.Spec = $"規格値 : {Min.ToString("F2")} ～ {Max.ToString("F2")}V";
                        State.VmTestStatus.MeasValue = "計測値 : " + measData.ToString("F2") + "V";
                    }

                }

            }
        }

















    }
}

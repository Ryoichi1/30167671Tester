using Microsoft.Practices.Prism.Mvvm;
using OxyPlot;
using System.Collections.Generic;
using System.Windows.Media;

namespace _30167671Tester
{
    public class ViewModelTestResult : BindableBase
    {
        private List<DataPoint> _ListMasterWav;
        public List<DataPoint> ListMasterWav { get { return _ListMasterWav; } set { this.SetProperty(ref this._ListMasterWav, value); } }


        private List<DataPoint> _ListWav;
        public List<DataPoint> ListWav { get { return _ListWav; } set { this.SetProperty(ref this._ListWav, value); } }

        //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
        //電圧チェック

        //12V電圧
        private string _Vol12V;
        public string Vol12V { get { return _Vol12V; } set { SetProperty(ref _Vol12V, value); } }

        private Brush _ColVol12V;
        public Brush ColVol12V { get { return _ColVol12V; } set { SetProperty(ref _ColVol12V, value); } }

        //5V電圧
        private string _Vol5V;
        public string Vol5V { get { return _Vol5V; } set { SetProperty(ref _Vol5V, value); } }

        private Brush _ColVol5V;
        public Brush ColVol5V { get { return _ColVol5V; } set { SetProperty(ref _ColVol5V, value); } }

        //3.3V電圧
        private string _Vol3_3V;
        public string Vol3_3V { get { return _Vol3_3V; } set { SetProperty(ref _Vol3_3V, value); } }

        private Brush _ColVol3_3V;
        public Brush ColVol3_3V { get { return _ColVol3_3V; } set { SetProperty(ref _ColVol3_3V, value); } }

        //AVdd
        private string _VolAVDD;
        public string VolAVDD { get { return _VolAVDD; } set { SetProperty(ref _VolAVDD, value); } }

        private Brush _ColVolAVDD;
        public Brush ColVolAVDD { get { return _ColVolAVDD; } set { SetProperty(ref _ColVolAVDD, value); } }

        //AVcc
        private string _VolAVCC;
        public string VolAVCC { get { return _VolAVCC; } set { SetProperty(ref _VolAVCC, value); } }

        private Brush _ColVolAVCC;
        public Brush ColVolAVCC { get { return _ColVolAVCC; } set { SetProperty(ref _ColVolAVCC, value); } }

        //VREF
        private string _VolVREF;
        public string VolVREF { get { return _VolVREF; } set { SetProperty(ref _VolVREF, value); } }

        private Brush _ColVolVREF;
        public Brush ColVolVREF { get { return _ColVolVREF; } set { SetProperty(ref _ColVolVREF, value); } }

        //AVCCD
        private string _VolAVCCD;
        public string VolAVCCD { get { return _VolAVCCD; } set { SetProperty(ref _VolAVCCD, value); } }

        private Brush _ColVolAVCCD;
        public Brush ColVolAVCCD { get { return _ColVolAVCCD; } set { SetProperty(ref _ColVolAVCCD, value); } }

        //S5V
        private string _VolS5V;
        public string VolS5V { get { return _VolS5V; } set { SetProperty(ref _VolS5V, value); } }

        private Brush _ColVolS5V;
        public Brush ColVolS5V { get { return _ColVolS5V; } set { SetProperty(ref _ColVolS5V, value); } }

        //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
        //DIN読取り OFF
        //PA
        private string _PA_OFF;
        public string PA_OFF { get { return _PA_OFF; } set { SetProperty(ref _PA_OFF, value); } }

        private Brush _ColPA_OFF;
        public Brush ColPA_OFF { get { return _ColPA_OFF; } set { SetProperty(ref _ColPA_OFF, value); } }

        //PB
        private string _PB_OFF;
        public string PB_OFF { get { return _PB_OFF; } set { SetProperty(ref _PB_OFF, value); } }

        private Brush _ColPB_OFF;
        public Brush ColPB_OFF { get { return _ColPB_OFF; } set { SetProperty(ref _ColPB_OFF, value); } }

        //PC
        private string _PC_OFF;
        public string PC_OFF { get { return _PC_OFF; } set { SetProperty(ref _PC_OFF, value); } }

        private Brush _ColPC_OFF;
        public Brush ColPC_OFF { get { return _ColPC_OFF; } set { SetProperty(ref _ColPC_OFF, value); } }

        //PE
        private string _PE_OFF;
        public string PE_OFF { get { return _PE_OFF; } set { SetProperty(ref _PE_OFF, value); } }

        private Brush _ColPE_OFF;
        public Brush ColPE_OFF { get { return _ColPE_OFF; } set { SetProperty(ref _ColPE_OFF, value); } }

        //PH
        private string _PH_OFF;
        public string PH_OFF { get { return _PH_OFF; } set { SetProperty(ref _PH_OFF, value); } }

        private Brush _ColPH_OFF;
        public Brush ColPH_OFF { get { return _ColPH_OFF; } set { SetProperty(ref _ColPH_OFF, value); } }

        //PL
        private string _PL_OFF;
        public string PL_OFF { get { return _PL_OFF; } set { SetProperty(ref _PL_OFF, value); } }

        private Brush _ColPL_OFF;
        public Brush ColPL_OFF { get { return _ColPL_OFF; } set { SetProperty(ref _ColPL_OFF, value); } }

        //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
        //DIN読取り ON
        //PA
        private string _PA_ON;
        public string PA_ON { get { return _PA_ON; } set { SetProperty(ref _PA_ON, value); } }

        private Brush _ColPA_ON;
        public Brush ColPA_ON { get { return _ColPA_ON; } set { SetProperty(ref _ColPA_ON, value); } }

        //PB
        private string _PB_ON;
        public string PB_ON { get { return _PB_ON; } set { SetProperty(ref _PB_ON, value); } }

        private Brush _ColPB_ON;
        public Brush ColPB_ON { get { return _ColPB_ON; } set { SetProperty(ref _ColPB_ON, value); } }

        //PC
        private string _PC_ON;
        public string PC_ON { get { return _PC_ON; } set { SetProperty(ref _PC_ON, value); } }

        private Brush _ColPC_ON;
        public Brush ColPC_ON { get { return _ColPC_ON; } set { SetProperty(ref _ColPC_ON, value); } }

        //PE
        private string _PE_ON;
        public string PE_ON { get { return _PE_ON; } set { SetProperty(ref _PE_ON, value); } }

        private Brush _ColPE_ON;
        public Brush ColPE_ON { get { return _ColPE_ON; } set { SetProperty(ref _ColPE_ON, value); } }

        //PH
        private string _PH_ON;
        public string PH_ON { get { return _PH_ON; } set { SetProperty(ref _PH_ON, value); } }

        private Brush _ColPH_ON;
        public Brush ColPH_ON { get { return _ColPH_ON; } set { SetProperty(ref _ColPH_ON, value); } }

        //PL
        private string _PL_ON;
        public string PL_ON { get { return _PL_ON; } set { SetProperty(ref _PL_ON, value); } }

        private Brush _ColPL_ON;
        public Brush ColPL_ON { get { return _ColPL_ON; } set { SetProperty(ref _ColPL_ON, value); } }

        //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
        //ANポートデジタル入力 読取り値１
        private string _ANA12_1;
        public string ANA12_1 { get { return _ANA12_1; } set { SetProperty(ref _ANA12_1, value); } }

        //
        private Brush _ColANA12_1;
        public Brush ColANA12_1 { get { return _ColANA12_1; } set { SetProperty(ref _ColANA12_1, value); } }

        //
        private string _ANA13_1;
        public string ANA13_1 { get { return _ANA13_1; } set { SetProperty(ref _ANA13_1, value); } }

        private Brush _ColANA13_1;
        public Brush ColANA13_1 { get { return _ColANA13_1; } set { SetProperty(ref _ColANA13_1, value); } }

        //
        private string _ANA14_1;
        public string ANA14_1 { get { return _ANA14_1; } set { SetProperty(ref _ANA14_1, value); } }

        private Brush _ColANA14_1;
        public Brush ColANA14_1 { get { return _ColANA14_1; } set { SetProperty(ref _ColANA14_1, value); } }

        //
        private string _ANA15_1;
        public string ANA15_1 { get { return _ANA15_1; } set { SetProperty(ref _ANA15_1, value); } }

        private Brush _ColANA15_1;
        public Brush ColANA15_1 { get { return _ColANA15_1; } set { SetProperty(ref _ColANA15_1, value); } }

        //
        private string _ANA17_1;
        public string ANA17_1 { get { return _ANA17_1; } set { SetProperty(ref _ANA17_1, value); } }

        private Brush _ColANA17_1;
        public Brush ColANA17_1 { get { return _ColANA17_1; } set { SetProperty(ref _ColANA17_1, value); } }

        //
        private string _ANA23_1;
        public string ANA23_1 { get { return _ANA23_1; } set { SetProperty(ref _ANA23_1, value); } }

        private Brush _ColANA23_1;
        public Brush ColANA23_1 { get { return _ColANA23_1; } set { SetProperty(ref _ColANA23_1, value); } }

        //
        private string _ANA24_1;
        public string ANA24_1 { get { return _ANA24_1; } set { SetProperty(ref _ANA24_1, value); } }

        private Brush _ColANA24_1;
        public Brush ColANA24_1 { get { return _ColANA24_1; } set { SetProperty(ref _ColANA24_1, value); } }

        //
        private string _ANA25_1;
        public string ANA25_1 { get { return _ANA25_1; } set { SetProperty(ref _ANA25_1, value); } }

        private Brush _ColANA25_1;
        public Brush ColANA25_1 { get { return _ColANA25_1; } set { SetProperty(ref _ColANA25_1, value); } }

        //
        private string _ANA26_1;
        public string ANA26_1 { get { return _ANA26_1; } set { SetProperty(ref _ANA26_1, value); } }

        private Brush _ColANA26_1;
        public Brush ColANA26_1 { get { return _ColANA26_1; } set { SetProperty(ref _ColANA26_1, value); } }

        //
        private string _ANA27_1;
        public string ANA27_1 { get { return _ANA27_1; } set { SetProperty(ref _ANA27_1, value); } }

        private Brush _ColANA27_1;
        public Brush ColANA27_1 { get { return _ColANA27_1; } set { SetProperty(ref _ColANA27_1, value); } }

        //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
        //ANポートデジタル入力 読取り値２
        private string _ANA12_2;
        public string ANA12_2 { get { return _ANA12_2; } set { SetProperty(ref _ANA12_2, value); } }

        private Brush _ColANA12_2;
        public Brush ColANA12_2 { get { return _ColANA12_2; } set { SetProperty(ref _ColANA12_2, value); } }

        //
        private string _ANA13_2;
        public string ANA13_2 { get { return _ANA13_2; } set { SetProperty(ref _ANA13_2, value); } }

        private Brush _ColANA13_2;
        public Brush ColANA13_2 { get { return _ColANA13_2; } set { SetProperty(ref _ColANA13_2, value); } }

        //
        private string _ANA14_2;
        public string ANA14_2 { get { return _ANA14_2; } set { SetProperty(ref _ANA14_2, value); } }

        private Brush _ColANA14_2;
        public Brush ColANA14_2 { get { return _ColANA14_2; } set { SetProperty(ref _ColANA14_2, value); } }

        //
        private string _ANA15_2;
        public string ANA15_2 { get { return _ANA15_2; } set { SetProperty(ref _ANA15_2, value); } }

        private Brush _ColANA15_2;
        public Brush ColANA15_2 { get { return _ColANA15_2; } set { SetProperty(ref _ColANA15_2, value); } }

        //
        private string _ANA17_2;
        public string ANA17_2 { get { return _ANA17_2; } set { SetProperty(ref _ANA17_2, value); } }

        private Brush _ColANA17_2;
        public Brush ColANA17_2 { get { return _ColANA17_2; } set { SetProperty(ref _ColANA17_2, value); } }

        //
        private string _ANA23_2;
        public string ANA23_2 { get { return _ANA23_2; } set { SetProperty(ref _ANA23_2, value); } }

        private Brush _ColANA23_2;
        public Brush ColANA23_2 { get { return _ColANA23_2; } set { SetProperty(ref _ColANA23_2, value); } }

        //
        private string _ANA24_2;
        public string ANA24_2 { get { return _ANA24_2; } set { SetProperty(ref _ANA24_2, value); } }

        private Brush _ColANA24_2;
        public Brush ColANA24_2 { get { return _ColANA24_2; } set { SetProperty(ref _ColANA24_2, value); } }

        //
        private string _ANA25_2;
        public string ANA25_2 { get { return _ANA25_2; } set { SetProperty(ref _ANA25_2, value); } }

        private Brush _ColANA25_2;
        public Brush ColANA25_2 { get { return _ColANA25_2; } set { SetProperty(ref _ColANA25_2, value); } }

        //
        private string _ANA26_2;
        public string ANA26_2 { get { return _ANA26_2; } set { SetProperty(ref _ANA26_2, value); } }

        private Brush _ColANA26_2;
        public Brush ColANA26_2 { get { return _ColANA26_2; } set { SetProperty(ref _ColANA26_2, value); } }

        //
        private string _ANA27_2;
        public string ANA27_2 { get { return _ANA27_2; } set { SetProperty(ref _ANA27_2, value); } }

        private Brush _ColANA27_2;
        public Brush ColANA27_2 { get { return _ColANA27_2; } set { SetProperty(ref _ColANA27_2, value); } }

        //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
        //SCR駆動回路
        private string _OnTime位相;
        public string OnTime位相 { get { return _OnTime位相; } set { SetProperty(ref _OnTime位相, value); } }

        private Brush _ColOnTime位相;
        public Brush ColOnTime位相 { get { return _ColOnTime位相; } set { SetProperty(ref _ColOnTime位相, value); } }

        private string _OffTime位相;
        public string OffTime位相 { get { return _OffTime位相; } set { SetProperty(ref _OffTime位相, value); } }

        private Brush _ColOffTime位相;
        public Brush ColOffTime位相 { get { return _ColOffTime位相; } set { SetProperty(ref _ColOffTime位相, value); } }

        private string _Peak1位相;
        public string Peak1位相 { get { return _Peak1位相; } set { SetProperty(ref _Peak1位相, value); } }

        private Brush _ColPeak1位相;
        public Brush ColPeak1位相 { get { return _ColPeak1位相; } set { SetProperty(ref _ColPeak1位相, value); } }

        private string _Peak2位相;
        public string Peak2位相 { get { return _Peak2位相; } set { SetProperty(ref _Peak2位相, value); } }

        private Brush _ColPeak2位相;
        public Brush ColPeak2位相 { get { return _ColPeak2位相; } set { SetProperty(ref _ColPeak2位相, value); } }

        private string _Wav位相;
        public string Wav位相 { get { return _Wav位相; } set { SetProperty(ref _Wav位相, value); } }

        private Brush _ColWav位相;
        public Brush ColWav位相 { get { return _ColWav位相; } set { SetProperty(ref _ColWav位相, value); } }


        private string _OnTimeサイクル;
        public string OnTimeサイクル { get { return _OnTimeサイクル; } set { SetProperty(ref _OnTimeサイクル, value); } }

        private Brush _ColOnTimeサイクル;
        public Brush ColOnTimeサイクル { get { return _ColOnTimeサイクル; } set { SetProperty(ref _ColOnTimeサイクル, value); } }

        private string _OffTimeサイクル;
        public string OffTimeサイクル { get { return _OffTimeサイクル; } set { SetProperty(ref _OffTimeサイクル, value); } }

        private Brush _ColOffTimeサイクル;
        public Brush ColOffTimeサイクル { get { return _ColOffTimeサイクル; } set { SetProperty(ref _ColOffTimeサイクル, value); } }

        private string _Peak1サイクル;
        public string Peak1サイクル { get { return _Peak1サイクル; } set { SetProperty(ref _Peak1サイクル, value); } }

        private Brush _ColPeak1サイクル;
        public Brush ColPeak1サイクル { get { return _ColPeak1サイクル; } set { SetProperty(ref _ColPeak1サイクル, value); } }

        private string _Peak2サイクル;
        public string Peak2サイクル { get { return _Peak2サイクル; } set { SetProperty(ref _Peak2サイクル, value); } }

        private Brush _ColPeak2サイクル;
        public Brush ColPeak2サイクル { get { return _ColPeak2サイクル; } set { SetProperty(ref _ColPeak2サイクル, value); } }

        private string _Wavサイクル;
        public string Wavサイクル { get { return _Wavサイクル; } set { SetProperty(ref _Wavサイクル, value); } }

        private Brush _ColWavサイクル;
        public Brush ColWavサイクル { get { return _ColWavサイクル; } set { SetProperty(ref _ColWavサイクル, value); } }

        //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
        //AD入力回路

        private string _A0_0;
        public string A0_0 { get { return _A0_0; } set { SetProperty(ref _A0_0, value); } }

        private Brush _ColA0_0;
        public Brush ColA0_0 { get { return _ColA0_0; } set { SetProperty(ref _ColA0_0, value); } }

        //
        private string _A0_5;
        public string A0_5 { get { return _A0_5; } set { SetProperty(ref _A0_5, value); } }

        private Brush _ColA0_5;
        public Brush ColA0_5 { get { return _ColA0_5; } set { SetProperty(ref _ColA0_5, value); } }

        //
        private string _A0_10;
        public string A0_10 { get { return _A0_10; } set { SetProperty(ref _A0_10, value); } }

        private Brush _ColA0_10;
        public Brush ColA0_10 { get { return _ColA0_10; } set { SetProperty(ref _ColA0_10, value); } }

        //
        private string _A1_0;
        public string A1_0 { get { return _A1_0; } set { SetProperty(ref _A1_0, value); } }

        private Brush _ColA1_0;
        public Brush ColA1_0 { get { return _ColA1_0; } set { SetProperty(ref _ColA1_0, value); } }

        //
        private string _A1_5;
        public string A1_5 { get { return _A1_5; } set { SetProperty(ref _A1_5, value); } }

        private Brush _ColA1_5;
        public Brush ColA1_5 { get { return _ColA1_5; } set { SetProperty(ref _ColA1_5, value); } }

        //
        private string _A1_10;
        public string A1_10 { get { return _A1_10; } set { SetProperty(ref _A1_10, value); } }

        private Brush _ColA1_10;
        public Brush ColA1_10 { get { return _ColA1_10; } set { SetProperty(ref _ColA1_10, value); } }

        //
        private string _A2_0;
        public string A2_0 { get { return _A2_0; } set { SetProperty(ref _A2_0, value); } }

        private Brush _ColA2_0;
        public Brush ColA2_0 { get { return _ColA2_0; } set { SetProperty(ref _ColA2_0, value); } }

        //
        private string _A2_5;
        public string A2_5 { get { return _A2_5; } set { SetProperty(ref _A2_5, value); } }

        private Brush _ColA2_5;
        public Brush ColA2_5 { get { return _ColA2_5; } set { SetProperty(ref _ColA2_5, value); } }

        //
        private string _A2_10;
        public string A2_10 { get { return _A2_10; } set { SetProperty(ref _A2_10, value); } }

        private Brush _ColA2_10;
        public Brush ColA2_10 { get { return _ColA2_10; } set { SetProperty(ref _ColA2_10, value); } }

        //
        private string _A3_0;
        public string A3_0 { get { return _A3_0; } set { SetProperty(ref _A3_0, value); } }

        private Brush _ColA3_0;
        public Brush ColA3_0 { get { return _ColA3_0; } set { SetProperty(ref _ColA3_0, value); } }

        //
        private string _A3_5;
        public string A3_5 { get { return _A3_5; } set { SetProperty(ref _A3_5, value); } }

        private Brush _ColA3_5;
        public Brush ColA3_5 { get { return _ColA3_5; } set { SetProperty(ref _ColA3_5, value); } }

        //
        private string _A3_10;
        public string A3_10 { get { return _A3_10; } set { SetProperty(ref _A3_10, value); } }

        private Brush _ColA3_10;
        public Brush ColA3_10 { get { return _ColA3_10; } set { SetProperty(ref _ColA3_10, value); } }

        //
        private string _A4_0;
        public string A4_0 { get { return _A4_0; } set { SetProperty(ref _A4_0, value); } }

        private Brush _ColA4_0;
        public Brush ColA4_0 { get { return _ColA4_0; } set { SetProperty(ref _ColA4_0, value); } }

        //
        private string _A4_5;
        public string A4_5 { get { return _A4_5; } set { SetProperty(ref _A4_5, value); } }

        private Brush _ColA4_5;
        public Brush ColA4_5 { get { return _ColA4_5; } set { SetProperty(ref _ColA4_5, value); } }

        //
        private string _A4_10;
        public string A4_10 { get { return _A4_10; } set { SetProperty(ref _A4_10, value); } }

        private Brush _ColA4_10;
        public Brush ColA4_10 { get { return _ColA4_10; } set { SetProperty(ref _ColA4_10, value); } }

        //
        private string _A5_0;
        public string A5_0 { get { return _A5_0; } set { SetProperty(ref _A5_0, value); } }

        private Brush _ColA5_0;
        public Brush ColA5_0 { get { return _ColA5_0; } set { SetProperty(ref _ColA5_0, value); } }

        //
        private string _A5_5;
        public string A5_5 { get { return _A5_5; } set { SetProperty(ref _A5_5, value); } }

        private Brush _ColA5_5;
        public Brush ColA5_5 { get { return _ColA5_5; } set { SetProperty(ref _ColA5_5, value); } }

        //
        private string _A5_10;
        public string A5_10 { get { return _A5_10; } set { SetProperty(ref _A5_10, value); } }

        private Brush _ColA5_10;
        public Brush ColA5_10 { get { return _ColA5_10; } set { SetProperty(ref _ColA5_10, value); } }

        //
        private string _A6_0;
        public string A6_0 { get { return _A6_0; } set { SetProperty(ref _A6_0, value); } }

        private Brush _ColA6_0;
        public Brush ColA6_0 { get { return _ColA6_0; } set { SetProperty(ref _ColA6_0, value); } }

        //
        private string _A6_5;
        public string A6_5 { get { return _A6_5; } set { SetProperty(ref _A6_5, value); } }

        private Brush _ColA6_5;
        public Brush ColA6_5 { get { return _ColA6_5; } set { SetProperty(ref _ColA6_5, value); } }

        //
        private string _A6_10;
        public string A6_10 { get { return _A6_10; } set { SetProperty(ref _A6_10, value); } }

        private Brush _ColA6_10;
        public Brush ColA6_10 { get { return _ColA6_10; } set { SetProperty(ref _ColA6_10, value); } }

        //
        private string _A7_0;
        public string A7_0 { get { return _A7_0; } set { SetProperty(ref _A7_0, value); } }

        private Brush _ColA7_0;
        public Brush ColA7_0 { get { return _ColA7_0; } set { SetProperty(ref _ColA7_0, value); } }

        //
        private string _A7_5;
        public string A7_5 { get { return _A7_5; } set { SetProperty(ref _A7_5, value); } }

        private Brush _ColA7_5;
        public Brush ColA7_5 { get { return _ColA7_5; } set { SetProperty(ref _ColA7_5, value); } }

        //
        private string _A7_10;
        public string A7_10 { get { return _A7_10; } set { SetProperty(ref _A7_10, value); } }

        private Brush _ColA7_10;
        public Brush ColA7_10 { get { return _ColA7_10; } set { SetProperty(ref _ColA7_10, value); } }

        //
        private string _A8_0;
        public string A8_0 { get { return _A8_0; } set { SetProperty(ref _A8_0, value); } }

        private Brush _ColA8_0;
        public Brush ColA8_0 { get { return _ColA8_0; } set { SetProperty(ref _ColA8_0, value); } }

        //
        private string _A8_5;
        public string A8_5 { get { return _A8_5; } set { SetProperty(ref _A8_5, value); } }

        private Brush _ColA8_5;
        public Brush ColA8_5 { get { return _ColA8_5; } set { SetProperty(ref _ColA8_5, value); } }

        //
        private string _A8_10;
        public string A8_10 { get { return _A8_10; } set { SetProperty(ref _A8_10, value); } }

        private Brush _ColA8_10;
        public Brush ColA8_10 { get { return _ColA8_10; } set { SetProperty(ref _ColA8_10, value); } }

        //
        private string _A9_0;
        public string A9_0 { get { return _A9_0; } set { SetProperty(ref _A9_0, value); } }

        private Brush _ColA9_0;
        public Brush ColA9_0 { get { return _ColA9_0; } set { SetProperty(ref _ColA9_0, value); } }

        //
        private string _A9_5;
        public string A9_5 { get { return _A9_5; } set { SetProperty(ref _A9_5, value); } }

        private Brush _ColA9_5;
        public Brush ColA9_5 { get { return _ColA9_5; } set { SetProperty(ref _ColA9_5, value); } }

        //
        private string _A9_10;
        public string A9_10 { get { return _A9_10; } set { SetProperty(ref _A9_10, value); } }

        private Brush _ColA9_10;
        public Brush ColA9_10 { get { return _ColA9_10; } set { SetProperty(ref _ColA9_10, value); } }

        //
        private string _A10_0;
        public string A10_0 { get { return _A10_0; } set { SetProperty(ref _A10_0, value); } }

        private Brush _ColA10_0;
        public Brush ColA10_0 { get { return _ColA10_0; } set { SetProperty(ref _ColA10_0, value); } }

        //
        private string _A10_5;
        public string A10_5 { get { return _A10_5; } set { SetProperty(ref _A10_5, value); } }

        private Brush _ColA10_5;
        public Brush ColA10_5 { get { return _ColA10_5; } set { SetProperty(ref _ColA10_5, value); } }

        //
        private string _A10_10;
        public string A10_10 { get { return _A10_10; } set { SetProperty(ref _A10_10, value); } }

        private Brush _ColA10_10;
        public Brush ColA10_10 { get { return _ColA10_10; } set { SetProperty(ref _ColA10_10, value); } }

        //
        private string _A11_0;
        public string A11_0 { get { return _A11_0; } set { SetProperty(ref _A11_0, value); } }

        private Brush _ColA11_0;
        public Brush ColA11_0 { get { return _ColA11_0; } set { SetProperty(ref _ColA11_0, value); } }

        //
        private string _A11_5;
        public string A11_5 { get { return _A11_5; } set { SetProperty(ref _A11_5, value); } }

        private Brush _ColA11_5;
        public Brush ColA11_5 { get { return _ColA11_5; } set { SetProperty(ref _ColA11_5, value); } }

        //
        private string _A11_10;
        public string A11_10 { get { return _A11_10; } set { SetProperty(ref _A11_10, value); } }

        private Brush _ColA11_10;
        public Brush ColA11_10 { get { return _ColA11_10; } set { SetProperty(ref _ColA11_10, value); } }

        //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
        //出力回路２
        private string _DV1_0;
        public string DV1_0 { get { return _DV1_0; } set { SetProperty(ref _DV1_0, value); } }

        private Brush _ColDV1_0;
        public Brush ColDV1_0 { get { return _ColDV1_0; } set { SetProperty(ref _ColDV1_0, value); } }

        //
        private string _DV1_50;
        public string DV1_50 { get { return _DV1_50; } set { SetProperty(ref _DV1_50, value); } }

        private Brush _ColDV1_50;
        public Brush ColDV1_50 { get { return _ColDV1_50; } set { SetProperty(ref _ColDV1_50, value); } }

        //
        private string _DV1_100;
        public string DV1_100 { get { return _DV1_100; } set { SetProperty(ref _DV1_100, value); } }

        private Brush _ColDV1_100;
        public Brush ColDV1_100 { get { return _ColDV1_100; } set { SetProperty(ref _ColDV1_100, value); } }

        //
        private string _DV2_0;
        public string DV2_0 { get { return _DV2_0; } set { SetProperty(ref _DV2_0, value); } }

        private Brush _ColDV2_0;
        public Brush ColDV2_0 { get { return _ColDV2_0; } set { SetProperty(ref _ColDV2_0, value); } }

        //
        private string _DV2_50;
        public string DV2_50 { get { return _DV2_50; } set { SetProperty(ref _DV2_50, value); } }

        private Brush _ColDV2_50;
        public Brush ColDV2_50 { get { return _ColDV2_50; } set { SetProperty(ref _ColDV2_50, value); } }

        //
        private string _DV2_100;
        public string DV2_100 { get { return _DV2_100; } set { SetProperty(ref _DV2_100, value); } }

        private Brush _ColDV2_100;
        public Brush ColDV2_100 { get { return _ColDV2_100; } set { SetProperty(ref _ColDV2_100, value); } }

        //
        private string _DV3_0;
        public string DV3_0 { get { return _DV3_0; } set { SetProperty(ref _DV3_0, value); } }

        private Brush _ColDV3_0;
        public Brush ColDV3_0 { get { return _ColDV3_0; } set { SetProperty(ref _ColDV3_0, value); } }

        //
        private string _DV3_50;
        public string DV3_50 { get { return _DV3_50; } set { SetProperty(ref _DV3_50, value); } }

        private Brush _ColDV3_50;
        public Brush ColDV3_50 { get { return _ColDV3_50; } set { SetProperty(ref _ColDV3_50, value); } }

        //
        private string _DV3_100;
        public string DV3_100 { get { return _DV3_100; } set { SetProperty(ref _DV3_100, value); } }

        private Brush _ColDV3_100;
        public Brush ColDV3_100 { get { return _ColDV3_100; } set { SetProperty(ref _ColDV3_100, value); } }

        //
        private string _DV4_0;
        public string DV4_0 { get { return _DV4_0; } set { SetProperty(ref _DV4_0, value); } }

        private Brush _ColDV4_0;
        public Brush ColDV4_0 { get { return _ColDV4_0; } set { SetProperty(ref _ColDV4_0, value); } }

        //
        private string _DV4_50;
        public string DV4_50 { get { return _DV4_50; } set { SetProperty(ref _DV4_50, value); } }

        private Brush _ColDV4_50;
        public Brush ColDV4_50 { get { return _ColDV4_50; } set { SetProperty(ref _ColDV4_50, value); } }

        //
        private string _DV4_100;
        public string DV4_100 { get { return _DV4_100; } set { SetProperty(ref _DV4_100, value); } }

        private Brush _ColDV4_100;
        public Brush ColDV4_100 { get { return _ColDV4_100; } set { SetProperty(ref _ColDV4_100, value); } }

        //
        private string _DV5_0;
        public string DV5_0 { get { return _DV5_0; } set { SetProperty(ref _DV5_0, value); } }

        private Brush _ColDV5_0;
        public Brush ColDV5_0 { get { return _ColDV5_0; } set { SetProperty(ref _ColDV5_0, value); } }

        //
        private string _DV5_50;
        public string DV5_50 { get { return _DV5_50; } set { SetProperty(ref _DV5_50, value); } }

        private Brush _ColDV5_50;
        public Brush ColDV5_50 { get { return _ColDV5_50; } set { SetProperty(ref _ColDV5_50, value); } }

        //
        private string _DV5_100;
        public string DV5_100 { get { return _DV5_100; } set { SetProperty(ref _DV5_100, value); } }

        private Brush _ColDV5_100;
        public Brush ColDV5_100 { get { return _ColDV5_100; } set { SetProperty(ref _ColDV5_100, value); } }

        //
        private string _DV6_0;
        public string DV6_0 { get { return _DV6_0; } set { SetProperty(ref _DV6_0, value); } }

        private Brush _ColDV6_0;
        public Brush ColDV6_0 { get { return _ColDV6_0; } set { SetProperty(ref _ColDV6_0, value); } }

        //
        private string _DV6_50;
        public string DV6_50 { get { return _DV6_50; } set { SetProperty(ref _DV6_50, value); } }

        private Brush _ColDV6_50;
        public Brush ColDV6_50 { get { return _ColDV6_50; } set { SetProperty(ref _ColDV6_50, value); } }

        //
        private string _DV6_100;
        public string DV6_100 { get { return _DV6_100; } set { SetProperty(ref _DV6_100, value); } }

        private Brush _ColDV6_100;
        public Brush ColDV6_100 { get { return _ColDV6_100; } set { SetProperty(ref _ColDV6_100, value); } }

        //
        private string _DV7_0;
        public string DV7_0 { get { return _DV7_0; } set { SetProperty(ref _DV7_0, value); } }

        private Brush _ColDV7_0;
        public Brush ColDV7_0 { get { return _ColDV7_0; } set { SetProperty(ref _ColDV7_0, value); } }

        //
        private string _DV7_50;
        public string DV7_50 { get { return _DV7_50; } set { SetProperty(ref _DV7_50, value); } }

        private Brush _ColDV7_50;
        public Brush ColDV7_50 { get { return _ColDV7_50; } set { SetProperty(ref _ColDV7_50, value); } }

        //
        private string _DV7_100;
        public string DV7_100 { get { return _DV7_100; } set { SetProperty(ref _DV7_100, value); } }

        private Brush _ColDV7_100;
        public Brush ColDV7_100 { get { return _ColDV7_100; } set { SetProperty(ref _ColDV7_100, value); } }

        //
        private string _DV8_0;
        public string DV8_0 { get { return _DV8_0; } set { SetProperty(ref _DV8_0, value); } }

        private Brush _ColDV8_0;
        public Brush ColDV8_0 { get { return _ColDV8_0; } set { SetProperty(ref _ColDV8_0, value); } }

        //
        private string _DV8_50;
        public string DV8_50 { get { return _DV8_50; } set { SetProperty(ref _DV8_50, value); } }

        private Brush _ColDV8_50;
        public Brush ColDV8_50 { get { return _ColDV8_50; } set { SetProperty(ref _ColDV8_50, value); } }

        //
        private string _DV8_100;
        public string DV8_100 { get { return _DV8_100; } set { SetProperty(ref _DV8_100, value); } }

        private Brush _ColDV8_100;
        public Brush ColDV8_100 { get { return _ColDV8_100; } set { SetProperty(ref _ColDV8_100, value); } }

        //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
        //出力回路１
        private string _DV9_0;
        public string DV9_0 { get { return _DV9_0; } set { SetProperty(ref _DV9_0, value); } }

        private Brush _ColDV9_0;
        public Brush ColDV9_0 { get { return _ColDV9_0; } set { SetProperty(ref _ColDV9_0, value); } }

        //
        private string _DV9_150;
        public string DV9_150 { get { return _DV9_150; } set { SetProperty(ref _DV9_150, value); } }

        private Brush _ColDV9_150;
        public Brush ColDV9_150 { get { return _ColDV9_150; } set { SetProperty(ref _ColDV9_150, value); } }

        //
        private string _DV9_300;
        public string DV9_300 { get { return _DV9_300; } set { SetProperty(ref _DV9_300, value); } }

        private Brush _ColDV9_300;
        public Brush ColDV9_300 { get { return _ColDV9_300; } set { SetProperty(ref _ColDV9_300, value); } }

        //
        private string _DV11_0;
        public string DV11_0 { get { return _DV11_0; } set { SetProperty(ref _DV11_0, value); } }

        private Brush _ColDV11_0;
        public Brush ColDV11_0 { get { return _ColDV11_0; } set { SetProperty(ref _ColDV11_0, value); } }

        //
        private string _DV11_150;
        public string DV11_150 { get { return _DV11_150; } set { SetProperty(ref _DV11_150, value); } }

        private Brush _ColDV11_150;
        public Brush ColDV11_150 { get { return _ColDV11_150; } set { SetProperty(ref _ColDV11_150, value); } }

        //
        private string _DV11_300;
        public string DV11_300 { get { return _DV11_300; } set { SetProperty(ref _DV11_300, value); } }

        private Brush _ColDV11_300;
        public Brush ColDV11_300 { get { return _ColDV11_300; } set { SetProperty(ref _ColDV11_300, value); } }

        //
        private string _DV10_0;
        public string DV10_0 { get { return _DV10_0; } set { SetProperty(ref _DV10_0, value); } }

        private Brush _ColDV10_0;
        public Brush ColDV10_0 { get { return _ColDV10_0; } set { SetProperty(ref _ColDV10_0, value); } }

        //
        private string _DV10_40;
        public string DV10_40 { get { return _DV10_40; } set { SetProperty(ref _DV10_40, value); } }

        private Brush _ColDV10_40;
        public Brush ColDV10_40 { get { return _ColDV10_40; } set { SetProperty(ref _ColDV10_40, value); } }

        //
        private string _DV10_75;
        public string DV10_75 { get { return _DV10_75; } set { SetProperty(ref _DV10_75, value); } }

        private Brush _ColDV10_75;
        public Brush ColDV10_75 { get { return _ColDV10_75; } set { SetProperty(ref _ColDV10_75, value); } }

        //
        private string _DV12_0;
        public string DV12_0 { get { return _DV12_0; } set { SetProperty(ref _DV12_0, value); } }

        private Brush _ColDV12_0;
        public Brush ColDV12_0 { get { return _ColDV12_0; } set { SetProperty(ref _ColDV12_0, value); } }

        //
        private string _DV12_40;
        public string DV12_40 { get { return _DV12_40; } set { SetProperty(ref _DV12_40, value); } }

        private Brush _ColDV12_40;
        public Brush ColDV12_40 { get { return _ColDV12_40; } set { SetProperty(ref _ColDV12_40, value); } }

        //
        private string _DV12_75;
        public string DV12_75 { get { return _DV12_75; } set { SetProperty(ref _DV12_75, value); } }

        private Brush _ColDV12_75;
        public Brush ColDV12_75 { get { return _ColDV12_75; } set { SetProperty(ref _ColDV12_75, value); } }

        //
        private string _DV13_0;
        public string DV13_0 { get { return _DV13_0; } set { SetProperty(ref _DV13_0, value); } }

        private Brush _ColDV13_0;
        public Brush ColDV13_0 { get { return _ColDV13_0; } set { SetProperty(ref _ColDV13_0, value); } }

        //
        private string _DV13_1800;
        public string DV13_1800 { get { return _DV13_1800; } set { SetProperty(ref _DV13_1800, value); } }

        private Brush _ColDV13_1800;
        public Brush ColDV13_1800 { get { return _ColDV13_1800; } set { SetProperty(ref _ColDV13_1800, value); } }

        //
        private string _DV13_3600;
        public string DV13_3600 { get { return _DV13_3600; } set { SetProperty(ref _DV13_3600, value); } }

        private Brush _ColDV13_3600;
        public Brush ColDV13_3600 { get { return _ColDV13_3600; } set { SetProperty(ref _ColDV13_3600, value); } }

        //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
        //パルス入力回路
        private string _Pulse1L;
        public string Pulse1L { get { return _Pulse1L; } set { SetProperty(ref _Pulse1L, value); } }

        private Brush _ColPulse1L;
        public Brush ColPulse1L { get { return _ColPulse1L; } set { SetProperty(ref _ColPulse1L, value); } }

        private string _Pulse1M;
        public string Pulse1M { get { return _Pulse1M; } set { SetProperty(ref _Pulse1M, value); } }

        private Brush _ColPulse1M;
        public Brush ColPulse1M { get { return _ColPulse1M; } set { SetProperty(ref _ColPulse1M, value); } }

        private string _Pulse1R;
        public string Pulse1R { get { return _Pulse1R; } set { SetProperty(ref _Pulse1R, value); } }

        private Brush _ColPulse1R;
        public Brush ColPulse1R { get { return _ColPulse1R; } set { SetProperty(ref _ColPulse1R, value); } }

        private string _Pulse2L;
        public string Pulse2L { get { return _Pulse2L; } set { SetProperty(ref _Pulse2L, value); } }

        private Brush _ColPulse2L;
        public Brush ColPulse2L { get { return _ColPulse2L; } set { SetProperty(ref _ColPulse2L, value); } }

        private string _Pulse2M;
        public string Pulse2M { get { return _Pulse2M; } set { SetProperty(ref _Pulse2M, value); } }

        private Brush _ColPulse2M;
        public Brush ColPulse2M { get { return _ColPulse2M; } set { SetProperty(ref _ColPulse2M, value); } }

        private string _Pulse2R;
        public string Pulse2R { get { return _Pulse2R; } set { SetProperty(ref _Pulse2R, value); } }

        private Brush _ColPulse2R;
        public Brush ColPulse2R { get { return _ColPulse2R; } set { SetProperty(ref _ColPulse2R, value); } }

        private string _Pulse3L;
        public string Pulse3L { get { return _Pulse3L; } set { SetProperty(ref _Pulse3L, value); } }

        private Brush _ColPulse3L;
        public Brush ColPulse3L { get { return _ColPulse3L; } set { SetProperty(ref _ColPulse3L, value); } }

        private string _Pulse3M;
        public string Pulse3M { get { return _Pulse3M; } set { SetProperty(ref _Pulse3M, value); } }

        private Brush _ColPulse3M;
        public Brush ColPulse3M { get { return _ColPulse3M; } set { SetProperty(ref _ColPulse3M, value); } }

        private string _Pulse3R;
        public string Pulse3R { get { return _Pulse3R; } set { SetProperty(ref _Pulse3R, value); } }

        private Brush _ColPulse3R;
        public Brush ColPulse3R { get { return _ColPulse3R; } set { SetProperty(ref _ColPulse3R, value); } }

        //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
        //VREF調整
        private string _Vref;
        public string Vref { get { return _Vref; } set { SetProperty(ref _Vref, value); } }

        private Brush _ColVref;
        public Brush ColVref { get { return _ColVref; } set { SetProperty(ref _ColVref, value); } }

        private string _Vref_Re;
        public string Vref_Re { get { return _Vref_Re; } set { SetProperty(ref _Vref_Re, value); } }

        private Brush _ColVref_Re;
        public Brush ColVref_Re { get { return _ColVref_Re; } set { SetProperty(ref _ColVref_Re, value); } }

        //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
        //PT100回路（警報点確認）
        private string _AlarmPoint;
        public string AlarmPoint { get { return _AlarmPoint; } set { SetProperty(ref _AlarmPoint, value); } }

        private Brush _ColAlarmPoint;
        public Brush ColAlarmPoint { get { return _ColAlarmPoint; } set { SetProperty(ref _ColAlarmPoint, value); } }

        //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
        //AC電源電圧読取り
        private string _VolConverted;
        public string VolConverted { get { return _VolConverted; } set { SetProperty(ref _VolConverted, value); } }

        private Brush _ColVolConverted;
        public Brush ColVolConverted { get { return _ColVolConverted; } set { SetProperty(ref _ColVolConverted, value); } }

        //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
        //負荷電流読取り
        private string _CT1;
        public string CT1 { get { return _CT1; } set { SetProperty(ref _CT1, value); } }

        private Brush _ColCT1;
        public Brush ColCT1 { get { return _ColCT1; } set { SetProperty(ref _ColCT1, value); } }

        private string _CT2;
        public string CT2 { get { return _CT2; } set { SetProperty(ref _CT2, value); } }

        private Brush _ColCT2;
        public Brush ColCT2 { get { return _ColCT2; } set { SetProperty(ref _ColCT2, value); } }

        //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
        //PT100センサ回路調整
        //PV1
        private string _PV1_1;
        public string PV1_1 { get { return _PV1_1; } set { SetProperty(ref _PV1_1, value); } }

        private Brush _ColPV1_1;
        public Brush ColPV1_1 { get { return _ColPV1_1; } set { SetProperty(ref _ColPV1_1, value); } }

        private string _PV1_1RE;
        public string PV1_1RE { get { return _PV1_1RE; } set { SetProperty(ref _PV1_1RE, value); } }

        private Brush _ColPV1_1RE;
        public Brush ColPV1_1RE { get { return _ColPV1_1RE; } set { SetProperty(ref _ColPV1_1RE, value); } }

        private string _PV1_2;
        public string PV1_2 { get { return _PV1_2; } set { SetProperty(ref _PV1_2, value); } }

        private Brush _ColPV1_2;
        public Brush ColPV1_2 { get { return _ColPV1_2; } set { SetProperty(ref _ColPV1_2, value); } }

        private string _PV1_2RE;
        public string PV1_2RE { get { return _PV1_2RE; } set { SetProperty(ref _PV1_2RE, value); } }

        private Brush _ColPV1_2RE;
        public Brush ColPV1_2RE { get { return _ColPV1_2RE; } set { SetProperty(ref _ColPV1_2RE, value); } }

        private string _PV1_3;
        public string PV1_3 { get { return _PV1_3; } set { SetProperty(ref _PV1_3, value); } }

        private Brush _ColPV1_3;
        public Brush ColPV1_3 { get { return _ColPV1_3; } set { SetProperty(ref _ColPV1_3, value); } }

        private string _PV1_3RE;
        public string PV1_3RE { get { return _PV1_3RE; } set { SetProperty(ref _PV1_3RE, value); } }

        private Brush _ColPV1_3RE;
        public Brush ColPV1_3RE { get { return _ColPV1_3RE; } set { SetProperty(ref _ColPV1_3RE, value); } }

        private string _PV1_123_24;
        public string PV1_123_24 { get { return _PV1_123_24; } set { SetProperty(ref _PV1_123_24, value); } }

        private Brush _ColPV1_123_24;
        public Brush ColPV1_123_24 { get { return _ColPV1_123_24; } set { SetProperty(ref _ColPV1_123_24, value); } }

        //PV2
        private string _PV2_1;
        public string PV2_1 { get { return _PV2_1; } set { SetProperty(ref _PV2_1, value); } }

        private Brush _ColPV2_1;
        public Brush ColPV2_1 { get { return _ColPV2_1; } set { SetProperty(ref _ColPV2_1, value); } }

        private string _PV2_1RE;
        public string PV2_1RE { get { return _PV2_1RE; } set { SetProperty(ref _PV2_1RE, value); } }

        private Brush _ColPV2_1RE;
        public Brush ColPV2_1RE { get { return _ColPV2_1RE; } set { SetProperty(ref _ColPV2_1RE, value); } }

        private string _PV2_2;
        public string PV2_2 { get { return _PV2_2; } set { SetProperty(ref _PV2_2, value); } }

        private Brush _ColPV2_2;
        public Brush ColPV2_2 { get { return _ColPV2_2; } set { SetProperty(ref _ColPV2_2, value); } }

        private string _PV2_2RE;
        public string PV2_2RE { get { return _PV2_2RE; } set { SetProperty(ref _PV2_2RE, value); } }

        private Brush _ColPV2_2RE;
        public Brush ColPV2_2RE { get { return _ColPV2_2RE; } set { SetProperty(ref _ColPV2_2RE, value); } }

        private string _PV2_3;
        public string PV2_3 { get { return _PV2_3; } set { SetProperty(ref _PV2_3, value); } }

        private Brush _ColPV2_3;
        public Brush ColPV2_3 { get { return _ColPV2_3; } set { SetProperty(ref _ColPV2_3, value); } }

        private string _PV2_3RE;
        public string PV2_3RE { get { return _PV2_3RE; } set { SetProperty(ref _PV2_3RE, value); } }

        private Brush _ColPV2_3RE;
        public Brush ColPV2_3RE { get { return _ColPV2_3RE; } set { SetProperty(ref _ColPV2_3RE, value); } }

        private string _PV2_123_24;
        public string PV2_123_24 { get { return _PV2_123_24; } set { SetProperty(ref _PV2_123_24, value); } }

        private Brush _ColPV2_123_24;
        public Brush ColPV2_123_24 { get { return _ColPV2_123_24; } set { SetProperty(ref _ColPV2_123_24, value); } }

        //PV3
        private string _PV3_1;
        public string PV3_1 { get { return _PV3_1; } set { SetProperty(ref _PV3_1, value); } }

        private Brush _ColPV3_1;
        public Brush ColPV3_1 { get { return _ColPV3_1; } set { SetProperty(ref _ColPV3_1, value); } }

        private string _PV3_1RE;
        public string PV3_1RE { get { return _PV3_1RE; } set { SetProperty(ref _PV3_1RE, value); } }

        private Brush _ColPV3_1RE;
        public Brush ColPV3_1RE { get { return _ColPV3_1RE; } set { SetProperty(ref _ColPV3_1RE, value); } }

        private string _PV3_2;
        public string PV3_2 { get { return _PV3_2; } set { SetProperty(ref _PV3_2, value); } }

        private Brush _ColPV3_2;
        public Brush ColPV3_2 { get { return _ColPV3_2; } set { SetProperty(ref _ColPV3_2, value); } }

        private string _PV3_2RE;
        public string PV3_2RE { get { return _PV3_2RE; } set { SetProperty(ref _PV3_2RE, value); } }

        private Brush _ColPV3_2RE;
        public Brush ColPV3_2RE { get { return _ColPV3_2RE; } set { SetProperty(ref _ColPV3_2RE, value); } }

        private string _PV3_3;
        public string PV3_3 { get { return _PV3_3; } set { SetProperty(ref _PV3_3, value); } }

        private Brush _ColPV3_3;
        public Brush ColPV3_3 { get { return _ColPV3_3; } set { SetProperty(ref _ColPV3_3, value); } }

        private string _PV3_3RE;
        public string PV3_3RE { get { return _PV3_3RE; } set { SetProperty(ref _PV3_3RE, value); } }

        private Brush _ColPV3_3RE;
        public Brush ColPV3_3RE { get { return _ColPV3_3RE; } set { SetProperty(ref _ColPV3_3RE, value); } }

        private string _PV3_123_24;
        public string PV3_123_24 { get { return _PV3_123_24; } set { SetProperty(ref _PV3_123_24, value); } }

        private Brush _ColPV3_123_24;
        public Brush ColPV3_123_24 { get { return _ColPV3_123_24; } set { SetProperty(ref _ColPV3_123_24, value); } }

        //PV4
        private string _PV4_1;
        public string PV4_1 { get { return _PV4_1; } set { SetProperty(ref _PV4_1, value); } }

        private Brush _ColPV4_1;
        public Brush ColPV4_1 { get { return _ColPV4_1; } set { SetProperty(ref _ColPV4_1, value); } }

        private string _PV4_1RE;
        public string PV4_1RE { get { return _PV4_1RE; } set { SetProperty(ref _PV4_1RE, value); } }

        private Brush _ColPV4_1RE;
        public Brush ColPV4_1RE { get { return _ColPV4_1RE; } set { SetProperty(ref _ColPV4_1RE, value); } }

        private string _PV4_2;
        public string PV4_2 { get { return _PV4_2; } set { SetProperty(ref _PV4_2, value); } }

        private Brush _ColPV4_2;
        public Brush ColPV4_2 { get { return _ColPV4_2; } set { SetProperty(ref _ColPV4_2, value); } }

        private string _PV4_2RE;
        public string PV4_2RE { get { return _PV4_2RE; } set { SetProperty(ref _PV4_2RE, value); } }

        private Brush _ColPV4_2RE;
        public Brush ColPV4_2RE { get { return _ColPV4_2RE; } set { SetProperty(ref _ColPV4_2RE, value); } }

        private string _PV4_3;
        public string PV4_3 { get { return _PV4_3; } set { SetProperty(ref _PV4_3, value); } }

        private Brush _ColPV4_3;
        public Brush ColPV4_3 { get { return _ColPV4_3; } set { SetProperty(ref _ColPV4_3, value); } }

        private string _PV4_3RE;
        public string PV4_3RE { get { return _PV4_3RE; } set { SetProperty(ref _PV4_3RE, value); } }

        private Brush _ColPV4_3RE;
        public Brush ColPV4_3RE { get { return _ColPV4_3RE; } set { SetProperty(ref _ColPV4_3RE, value); } }

        private string _PV4_123_24;
        public string PV4_123_24 { get { return _PV4_123_24; } set { SetProperty(ref _PV4_123_24, value); } }

        private Brush _ColPV4_123_24;
        public Brush ColPV4_123_24 { get { return _ColPV4_123_24; } set { SetProperty(ref _ColPV4_123_24, value); } }

        //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
        //電流入力回路調整
        //I１
        private string _I1_1;
        public string I1_1 { get { return _I1_1; } set { SetProperty(ref _I1_1, value); } }

        private Brush _ColI1_1;
        public Brush ColI1_1 { get { return _ColI1_1; } set { SetProperty(ref _ColI1_1, value); } }

        private string _I1_1RE;
        public string I1_1RE { get { return _I1_1RE; } set { SetProperty(ref _I1_1RE, value); } }

        private Brush _ColI1_1RE;
        public Brush ColI1_1RE { get { return _ColI1_1RE; } set { SetProperty(ref _ColI1_1RE, value); } }

        private string _I1_2;
        public string I1_2 { get { return _I1_2; } set { SetProperty(ref _I1_2, value); } }

        private Brush _ColI1_2;
        public Brush ColI1_2 { get { return _ColI1_2; } set { SetProperty(ref _ColI1_2, value); } }

        private string _I1_2RE;
        public string I1_2RE { get { return _I1_2RE; } set { SetProperty(ref _I1_2RE, value); } }

        private Brush _ColI1_2RE;
        public Brush ColI1_2RE { get { return _ColI1_2RE; } set { SetProperty(ref _ColI1_2RE, value); } }

        private string _I1_10mA;
        public string I1_10mA { get { return _I1_10mA; } set { SetProperty(ref _I1_10mA, value); } }

        private Brush _ColI1_10mA;
        public Brush ColI1_10mA { get { return _ColI1_10mA; } set { SetProperty(ref _ColI1_10mA, value); } }

        //I２
        private string _I2_1;
        public string I2_1 { get { return _I2_1; } set { SetProperty(ref _I2_1, value); } }

        private Brush _ColI2_1;
        public Brush ColI2_1 { get { return _ColI2_1; } set { SetProperty(ref _ColI2_1, value); } }

        private string _I2_1RE;
        public string I2_1RE { get { return _I2_1RE; } set { SetProperty(ref _I2_1RE, value); } }

        private Brush _ColI2_1RE;
        public Brush ColI2_1RE { get { return _ColI2_1RE; } set { SetProperty(ref _ColI2_1RE, value); } }

        private string _I2_2;
        public string I2_2 { get { return _I2_2; } set { SetProperty(ref _I2_2, value); } }

        private Brush _ColI2_2;
        public Brush ColI2_2 { get { return _ColI2_2; } set { SetProperty(ref _ColI2_2, value); } }

        private string _I2_2RE;
        public string I2_2RE { get { return _I2_2RE; } set { SetProperty(ref _I2_2RE, value); } }

        private Brush _ColI2_2RE;
        public Brush ColI2_2RE { get { return _ColI2_2RE; } set { SetProperty(ref _ColI2_2RE, value); } }

        private string _I2_10mA;
        public string I2_10mA { get { return _I2_10mA; } set { SetProperty(ref _I2_10mA, value); } }

        private Brush _ColI2_10mA;
        public Brush ColI2_10mA { get { return _ColI2_10mA; } set { SetProperty(ref _ColI2_10mA, value); } }

        //I３
        private string _I3_1;
        public string I3_1 { get { return _I3_1; } set { SetProperty(ref _I3_1, value); } }

        private Brush _ColI3_1;
        public Brush ColI3_1 { get { return _ColI3_1; } set { SetProperty(ref _ColI3_1, value); } }

        private string _I3_1RE;
        public string I3_1RE { get { return _I3_1RE; } set { SetProperty(ref _I3_1RE, value); } }

        private Brush _ColI3_1RE;
        public Brush ColI3_1RE { get { return _ColI3_1RE; } set { SetProperty(ref _ColI3_1RE, value); } }

        private string _I3_2;
        public string I3_2 { get { return _I3_2; } set { SetProperty(ref _I3_2, value); } }

        private Brush _ColI3_2;
        public Brush ColI3_2 { get { return _ColI3_2; } set { SetProperty(ref _ColI3_2, value); } }

        private string _I3_2RE;
        public string I3_2RE { get { return _I3_2RE; } set { SetProperty(ref _I3_2RE, value); } }

        private Brush _ColI3_2RE;
        public Brush ColI3_2RE { get { return _ColI3_2RE; } set { SetProperty(ref _ColI3_2RE, value); } }

        private string _I3_10mA;
        public string I3_10mA { get { return _I3_10mA; } set { SetProperty(ref _I3_10mA, value); } }

        private Brush _ColI3_10mA;
        public Brush ColI3_10mA { get { return _ColI3_10mA; } set { SetProperty(ref _ColI3_10mA, value); } }

        //I４
        private string _I4_1;
        public string I4_1 { get { return _I4_1; } set { SetProperty(ref _I4_1, value); } }

        private Brush _ColI4_1;
        public Brush ColI4_1 { get { return _ColI4_1; } set { SetProperty(ref _ColI4_1, value); } }

        private string _I4_1RE;
        public string I4_1RE { get { return _I4_1RE; } set { SetProperty(ref _I4_1RE, value); } }

        private Brush _ColI4_1RE;
        public Brush ColI4_1RE { get { return _ColI4_1RE; } set { SetProperty(ref _ColI4_1RE, value); } }

        private string _I4_2;
        public string I4_2 { get { return _I4_2; } set { SetProperty(ref _I4_2, value); } }

        private Brush _ColI4_2;
        public Brush ColI4_2 { get { return _ColI4_2; } set { SetProperty(ref _ColI4_2, value); } }

        private string _I4_2RE;
        public string I4_2RE { get { return _I4_2RE; } set { SetProperty(ref _I4_2RE, value); } }

        private Brush _ColI4_2RE;
        public Brush ColI4_2RE { get { return _ColI4_2RE; } set { SetProperty(ref _ColI4_2RE, value); } }

        private string _I4_10mA;
        public string I4_10mA { get { return _I4_10mA; } set { SetProperty(ref _I4_10mA, value); } }

        private Brush _ColI4_10mA;
        public Brush ColI4_10mA { get { return _ColI4_10mA; } set { SetProperty(ref _ColI4_10mA, value); } }

        //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
        //電圧入力回路調整
        //V１
        private string _V1_1;
        public string V1_1 { get { return _V1_1; } set { SetProperty(ref _V1_1, value); } }

        private Brush _ColV1_1;
        public Brush ColV1_1 { get { return _ColV1_1; } set { SetProperty(ref _ColV1_1, value); } }

        private string _V1_1RE;
        public string V1_1RE { get { return _V1_1RE; } set { SetProperty(ref _V1_1RE, value); } }

        private Brush _ColV1_1RE;
        public Brush ColV1_1RE { get { return _ColV1_1RE; } set { SetProperty(ref _ColV1_1RE, value); } }

        private string _V1_2;
        public string V1_2 { get { return _V1_2; } set { SetProperty(ref _V1_2, value); } }

        private Brush _ColV1_2;
        public Brush ColV1_2 { get { return _ColV1_2; } set { SetProperty(ref _ColV1_2, value); } }

        private string _V1_2RE;
        public string V1_2RE { get { return _V1_2RE; } set { SetProperty(ref _V1_2RE, value); } }

        private Brush _ColV1_2RE;
        public Brush ColV1_2RE { get { return _ColV1_2RE; } set { SetProperty(ref _ColV1_2RE, value); } }

        private string _V1_5V;
        public string V1_5V { get { return _V1_5V; } set { SetProperty(ref _V1_5V, value); } }

        private Brush _ColV1_5V;
        public Brush ColV1_5V { get { return _ColV1_5V; } set { SetProperty(ref _ColV1_5V, value); } }

        //V2
        private string _V2_1;
        public string V2_1 { get { return _V2_1; } set { SetProperty(ref _V2_1, value); } }

        private Brush _ColV2_1;
        public Brush ColV2_1 { get { return _ColV2_1; } set { SetProperty(ref _ColV2_1, value); } }

        private string _V2_1RE;
        public string V2_1RE { get { return _V2_1RE; } set { SetProperty(ref _V2_1RE, value); } }

        private Brush _ColV2_1RE;
        public Brush ColV2_1RE { get { return _ColV2_1RE; } set { SetProperty(ref _ColV2_1RE, value); } }

        private string _V2_2;
        public string V2_2 { get { return _V2_2; } set { SetProperty(ref _V2_2, value); } }

        private Brush _ColV2_2;
        public Brush ColV2_2 { get { return _ColV2_2; } set { SetProperty(ref _ColV2_2, value); } }

        private string _V2_2RE;
        public string V2_2RE { get { return _V2_2RE; } set { SetProperty(ref _V2_2RE, value); } }

        private Brush _ColV2_2RE;
        public Brush ColV2_2RE { get { return _ColV2_2RE; } set { SetProperty(ref _ColV2_2RE, value); } }

        private string _V2_5V;
        public string V2_5V { get { return _V2_5V; } set { SetProperty(ref _V2_5V, value); } }

        private Brush _ColV2_5V;
        public Brush ColV2_5V { get { return _ColV2_5V; } set { SetProperty(ref _ColV2_5V, value); } }

        //V3
        private string _V3_1;
        public string V3_1 { get { return _V3_1; } set { SetProperty(ref _V3_1, value); } }

        private Brush _ColV3_1;
        public Brush ColV3_1 { get { return _ColV3_1; } set { SetProperty(ref _ColV3_1, value); } }

        private string _V3_1RE;
        public string V3_1RE { get { return _V3_1RE; } set { SetProperty(ref _V3_1RE, value); } }

        private Brush _ColV3_1RE;
        public Brush ColV3_1RE { get { return _ColV3_1RE; } set { SetProperty(ref _ColV3_1RE, value); } }

        private string _V3_2;
        public string V3_2 { get { return _V3_2; } set { SetProperty(ref _V3_2, value); } }

        private Brush _ColV3_2;
        public Brush ColV3_2 { get { return _ColV3_2; } set { SetProperty(ref _ColV3_2, value); } }

        private string _V3_2RE;
        public string V3_2RE { get { return _V3_2RE; } set { SetProperty(ref _V3_2RE, value); } }

        private Brush _ColV3_2RE;
        public Brush ColV3_2RE { get { return _ColV3_2RE; } set { SetProperty(ref _ColV3_2RE, value); } }

        private string _V3_5V;
        public string V3_5V { get { return _V3_5V; } set { SetProperty(ref _V3_5V, value); } }

        private Brush _ColV3_5V;
        public Brush ColV3_5V { get { return _ColV3_5V; } set { SetProperty(ref _ColV3_5V, value); } }

        //V4
        private string _V4_1;
        public string V4_1 { get { return _V4_1; } set { SetProperty(ref _V4_1, value); } }

        private Brush _ColV4_1;
        public Brush ColV4_1 { get { return _ColV4_1; } set { SetProperty(ref _ColV4_1, value); } }

        private string _V4_1RE;
        public string V4_1RE { get { return _V4_1RE; } set { SetProperty(ref _V4_1RE, value); } }

        private Brush _ColV4_1RE;
        public Brush ColV4_1RE { get { return _ColV4_1RE; } set { SetProperty(ref _ColV4_1RE, value); } }

        private string _V4_2;
        public string V4_2 { get { return _V4_2; } set { SetProperty(ref _V4_2, value); } }

        private Brush _ColV4_2;
        public Brush ColV4_2 { get { return _ColV4_2; } set { SetProperty(ref _ColV4_2, value); } }

        private string _V4_2RE;
        public string V4_2RE { get { return _V4_2RE; } set { SetProperty(ref _V4_2RE, value); } }

        private Brush _ColV4_2RE;
        public Brush ColV4_2RE { get { return _ColV4_2RE; } set { SetProperty(ref _ColV4_2RE, value); } }

        private string _V4_5V;
        public string V4_5V { get { return _V4_5V; } set { SetProperty(ref _V4_5V, value); } }

        private Brush _ColV4_5V;
        public Brush ColV4_5V { get { return _ColV4_5V; } set { SetProperty(ref _ColV4_5V, value); } }

        //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
        //PT100断線
        //ノーマル
        private string _PV1_N;
        public string PV1_N { get { return _PV1_N; } set { SetProperty(ref _PV1_N, value); } }

        private Brush _ColPV1_N;
        public Brush ColPV1_N { get { return _ColPV1_N; } set { SetProperty(ref _ColPV1_N, value); } }

        private string _PV2_N;
        public string PV2_N { get { return _PV2_N; } set { SetProperty(ref _PV2_N, value); } }

        private Brush _ColPV2_N;
        public Brush ColPV2_N { get { return _ColPV2_N; } set { SetProperty(ref _ColPV2_N, value); } }

        private string _PV3_N;
        public string PV3_N { get { return _PV3_N; } set { SetProperty(ref _PV3_N, value); } }

        private Brush _ColPV3_N;
        public Brush ColPV3_N { get { return _ColPV3_N; } set { SetProperty(ref _ColPV3_N, value); } }

        private string _PV4_N;
        public string PV4_N { get { return _PV4_N; } set { SetProperty(ref _PV4_N, value); } }

        private Brush _ColPV4_N;
        public Brush ColPV4_N { get { return _ColPV4_N; } set { SetProperty(ref _ColPV4_N, value); } }

        //A断線
        private string _PV1_A;
        public string PV1_A { get { return _PV1_A; } set { SetProperty(ref _PV1_A, value); } }

        private Brush _ColPV1_A;
        public Brush ColPV1_A { get { return _ColPV1_A; } set { SetProperty(ref _ColPV1_A, value); } }

        private string _PV2_A;
        public string PV2_A { get { return _PV2_A; } set { SetProperty(ref _PV2_A, value); } }

        private Brush _ColPV2_A;
        public Brush ColPV2_A { get { return _ColPV2_A; } set { SetProperty(ref _ColPV2_A, value); } }

        private string _PV3_A;
        public string PV3_A { get { return _PV3_A; } set { SetProperty(ref _PV3_A, value); } }

        private Brush _ColPV3_A;
        public Brush ColPV3_A { get { return _ColPV3_A; } set { SetProperty(ref _ColPV3_A, value); } }

        private string _PV4_A;
        public string PV4_A { get { return _PV4_A; } set { SetProperty(ref _PV4_A, value); } }

        private Brush _ColPV4_A;
        public Brush ColPV4_A { get { return _ColPV4_A; } set { SetProperty(ref _ColPV4_A, value); } }

        //B1断線
        private string _PV1_B1;
        public string PV1_B1 { get { return _PV1_B1; } set { SetProperty(ref _PV1_B1, value); } }

        private Brush _ColPV1_B1;
        public Brush ColPV1_B1 { get { return _ColPV1_B1; } set { SetProperty(ref _ColPV1_B1, value); } }

        private string _PV2_B1;
        public string PV2_B1 { get { return _PV2_B1; } set { SetProperty(ref _PV2_B1, value); } }

        private Brush _ColPV2_B1;
        public Brush ColPV2_B1 { get { return _ColPV2_B1; } set { SetProperty(ref _ColPV2_B1, value); } }

        private string _PV3_B1;
        public string PV3_B1 { get { return _PV3_B1; } set { SetProperty(ref _PV3_B1, value); } }

        private Brush _ColPV3_B1;
        public Brush ColPV3_B1 { get { return _ColPV3_B1; } set { SetProperty(ref _ColPV3_B1, value); } }

        private string _PV4_B1;
        public string PV4_B1 { get { return _PV4_B1; } set { SetProperty(ref _PV4_B1, value); } }

        private Brush _ColPV4_B1;
        public Brush ColPV4_B1 { get { return _ColPV4_B1; } set { SetProperty(ref _ColPV4_B1, value); } }

        //B2断線
        private string _PV1_B2;
        public string PV1_B2 { get { return _PV1_B2; } set { SetProperty(ref _PV1_B2, value); } }

        private Brush _ColPV1_B2;
        public Brush ColPV1_B2 { get { return _ColPV1_B2; } set { SetProperty(ref _ColPV1_B2, value); } }

        private string _PV2_B2;
        public string PV2_B2 { get { return _PV2_B2; } set { SetProperty(ref _PV2_B2, value); } }

        private Brush _ColPV2_B2;
        public Brush ColPV2_B2 { get { return _ColPV2_B2; } set { SetProperty(ref _ColPV2_B2, value); } }

        private string _PV3_B2;
        public string PV3_B2 { get { return _PV3_B2; } set { SetProperty(ref _PV3_B2, value); } }

        private Brush _ColPV3_B2;
        public Brush ColPV3_B2 { get { return _ColPV3_B2; } set { SetProperty(ref _ColPV3_B2, value); } }

        private string _PV4_B2;
        public string PV4_B2 { get { return _PV4_B2; } set { SetProperty(ref _PV4_B2, value); } }

        private Brush _ColPV4_B2;
        public Brush ColPV4_B2 { get { return _ColPV4_B2; } set { SetProperty(ref _ColPV4_B2, value); } }

        //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
        //2線式液漏れセンサ回路調整
        private string _Res80k;
        public string Res80k { get { return _Res80k; } set { SetProperty(ref _Res80k, value); } }

        private Brush _ColRes80k;
        public Brush ColRes80k { get { return _ColRes80k; } set { SetProperty(ref _ColRes80k, value); } }

        private string _Res80kRE;
        public string Res80kRE { get { return _Res80kRE; } set { SetProperty(ref _Res80kRE, value); } }

        private Brush _ColRes80kRE;
        public Brush ColRes80kRE { get { return _ColRes80kRE; } set { SetProperty(ref _ColRes80kRE, value); } }

        private string _Res20k;
        public string Res20k { get { return _Res20k; } set { SetProperty(ref _Res20k, value); } }

        private Brush _ColRes20k;
        public Brush ColRes20k { get { return _ColRes20k; } set { SetProperty(ref _ColRes20k, value); } }
    }

}









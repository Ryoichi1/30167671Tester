using Microsoft.Practices.Prism.Mvvm;
using OxyPlot;
using System.Collections.Generic;
using System.Windows.Media;

namespace _30167671Tester
{
    public class ViewModelTestResult : BindableBase
    {
        private List<DataPoint> _ListLimitHi;
        public List<DataPoint> ListLimitHi { get { return _ListLimitHi; } set { this.SetProperty(ref this._ListLimitHi, value); } }

        private List<DataPoint> _ListLimitLo;
        public List<DataPoint> ListLimitLo { get { return _ListLimitLo; } set { this.SetProperty(ref this._ListLimitLo, value); } }

        private List<DataPoint> _ListWav;
        public List<DataPoint> ListWav { get { return _ListWav; } set { this.SetProperty(ref this._ListWav, value); } }


        //電圧チェック
        //5V電圧
        private string _Vol12v;
        public string Vol12v { get { return _Vol12v; } set { SetProperty(ref _Vol12v, value); } }

        private Brush _ColVol12v;
        public Brush ColVol12v { get { return _ColVol12v; } set { SetProperty(ref _ColVol12v, value); } }

        //5V電圧
        private string _Vol5v;
        public string Vol5v { get { return _Vol5v; } set { SetProperty(ref _Vol5v, value); } }

        private Brush _ColVol5v;
        public Brush ColVol5v { get { return _ColVol5v; } set { SetProperty(ref _ColVol5v, value); } }

        //3.3V電圧
        private string _Vol3_3v;
        public string Vol3_3v { get { return _Vol3_3v; } set { SetProperty(ref _Vol3_3v, value); } }

        private Brush _ColVol3_3v;
        public Brush ColVol3_3v { get { return _ColVol3_3v; } set { SetProperty(ref _ColVol3_3v, value); } }

        //AVdd
        private string _VolAvdd;
        public string VolAvdd { get { return _VolAvdd; } set { SetProperty(ref _VolAvdd, value); } }

        private Brush _ColVolAvdd;
        public Brush ColVolAvdd { get { return _ColVolAvdd; } set { SetProperty(ref _ColVolAvdd, value); } }

        //AVcc
        private string _VolAvcc;
        public string VolAvcc { get { return _VolAvcc; } set { SetProperty(ref _VolAvcc, value); } }

        private Brush _ColVolAvcc;
        public Brush ColVolAvcc { get { return _ColVolAvcc; } set { SetProperty(ref _ColVolAvcc, value); } }



    }

}









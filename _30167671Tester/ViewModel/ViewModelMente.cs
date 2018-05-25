using Microsoft.Practices.Prism.Mvvm;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace _30167671Tester
{
    public class ViewModelMente : BindableBase
    {
        private string _ConfOscillo;
        public string ConfOscillo
        {
            get { return _ConfOscillo; }
            set { SetProperty(ref _ConfOscillo, value); }
        }

        private string _Freq位相制御;
        public string Freq位相制御
        {
            get { return _Freq位相制御; }
            set { SetProperty(ref _Freq位相制御, value); }
        }

        private string _PeakP位相制御;
        public string PeakP位相制御
        {
            get { return _PeakP位相制御; }
            set { SetProperty(ref _PeakP位相制御, value); }
        }

        private string _PeakM位相制御;
        public string PeakM位相制御
        {

            get { return _PeakM位相制御; }
            set { SetProperty(ref _PeakM位相制御, value); }

        }

        private string _Freqサイクル制御;
        public string Freqサイクル制御
        {
            get { return _Freqサイクル制御; }
            set { SetProperty(ref _Freqサイクル制御, value); }
        }

        private string _PeakPサイクル制御;
        public string PeakPサイクル制御
        {
            get { return _PeakPサイクル制御; }
            set { SetProperty(ref _PeakPサイクル制御, value); }
        }

        private string _PeakMサイクル制御;
        public string PeakMサイクル制御
        {

            get { return _PeakMサイクル制御; }
            set { SetProperty(ref _PeakMサイクル制御, value); }

        }

    }
}

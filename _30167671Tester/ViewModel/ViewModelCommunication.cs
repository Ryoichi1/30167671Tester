using Microsoft.Practices.Prism.Mvvm;
using System.Windows.Media;

namespace _30167671Tester
{

    public class ViewModelCommunication : BindableBase
    {
        //プロパティ
        private string _TX;
        public string TX
        {
            get { return _TX; }
            set { SetProperty(ref _TX, value); }
        }

        private string _RX;
        public string RX
        {
            get { return _RX; }
            set { SetProperty(ref _RX, value); }
        }

    }
}

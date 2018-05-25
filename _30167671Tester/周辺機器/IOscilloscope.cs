
using System.Collections.Generic;

namespace _30167671Tester
{
    public interface IOscilloscope
    {
        //メソッド
        bool Init();
        bool Close();
        bool SetBasicConf();
        bool Set位相制御();
        bool Setサイクル制御();
        List<double> GetWav();
        double ReadFreq();
        double ReadPeak_P();
        double ReadPeak_M();

    }

}

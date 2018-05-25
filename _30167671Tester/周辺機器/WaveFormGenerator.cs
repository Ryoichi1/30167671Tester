using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _30167671Tester
{
    public static class WaveFormGenerator
    {

        //VISA アドレス
        private const string AGI33220A_ADDRESS = "USB0::0x0957::0x0407::MY44047875::0::INSTR";
        private const string AGI33511B_ADDRESS = "USB0::0x0957::0x2707::MY52302090::0::INSTR";//校正時の代替器

        public static bool Flag33220 { get; set; }
         

        private static Ivi.Visa.Interop.ResourceManager RM;
        private static Ivi.Visa.Interop.FormattedIO488 DMM;

        public static bool Initialize()
        {
            RM = new Ivi.Visa.Interop.ResourceManager();
            DMM = new Ivi.Visa.Interop.FormattedIO488();
            try
            {
                DMM.IO = (Ivi.Visa.Interop.IMessage)RM.Open(AGI33220A_ADDRESS);
                Flag33220 = true;
                return true;
            }
            catch
            {
                try
                {
                    DMM.IO = (Ivi.Visa.Interop.IMessage)RM.Open(AGI33511B_ADDRESS);
                    Flag33220 = false;
                    return true;
                }
                catch
                {
                    return false;
                }
            }

        }

        public static bool サイン波出力(double freq, double imp, double vol_Vrms, double offset)
        {

            var Vpeak = vol_Vrms / 0.707;
            var Vpp = Vpeak * 2;

            try
            {

                DMM.WriteString("*RST;*CLS");// Reset the instrument
                //DMM.WriteString("*OPC?");// Wait for reset to complete
                DMM.WriteString("SOURce:FREQuency " + freq.ToString("F3"));// Set the frequency
                DMM.WriteString("OUTPut:LOAD " + imp.ToString("F3"));// Select the output impedance
                DMM.WriteString("FUNCtion SINusoid");
                DMM.WriteString("VOLTage " + Vpp.ToString("F3"));
                DMM.WriteString("VOLTage:OFFSet " + offset.ToString("F3"));
                //DMM.WriteString("*OPC?"); // Wait until command exesution complete
                DMM.WriteString("OUTPut ON");// Turn output on
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool ChangeVoltage(double vol_Vrms)
        {
            var Vpeak = vol_Vrms / 0.707;
            var Vpp = Vpeak * 2;

            try
            {
                DMM.WriteString("VOLTage " + Vpp.ToString("F3"));
                return true;
            }
            catch
            {
                return false;
            }

        }

        public static bool SourceOff()
        {
            try
            {
                DMM.WriteString("OUTPut OFF");
                return true;
            }
            catch
            {
                return false;
            }
        }


        public static void Close()
        {
            try
            {
                if (DMM.IO != null)
                {
                    SourceOff();
                    DMM.IO.Close();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(DMM);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(RM);
                }

            }
            catch
            {

            }
        }









    }
}

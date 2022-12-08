namespace SyneticBasicTools
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            //ApplicationConfiguration.Initialize();
            //Application.Run(new Form1());

            var conv = new WR1ToWR2Conv(@"X:\\Games\\Synetic\\World Racing Scn-Data", @"C://World Racing 2");
            //conv.CopySounds();
            conv.ConvertVGroup("MBWR_Alpen");
            conv.ConvertVGroup("MBWR_Australien");
            conv.ConvertVGroup("MBWR_Japan");
            conv.ConvertVGroup("MBWR_Mexiko");
            conv.ConvertVGroup("MBWR_Nevada");
            conv.ConvertVGroup("MBWR_Stadt");
            conv.ConvertVGroup("MBWR_Testcenter");
        }
    }
}
namespace SyneticLib_Tests;

using Grille.ConsoleTestLib;

using static Grille.ConsoleTestLib.GlobalTestSystem;

internal class Program
{
    static void Main(string[] args)
    {
        RunAsync = false;

        DdsTests.Run();

        RunTests();
    }
}

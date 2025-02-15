using System;
using System.Threading;
using System.Globalization;
using System.Windows.Forms;
using System.Collections.Generic;
using SyneticLib.WinForms.Forms;
using System.Drawing;

namespace SyneticTool;

internal static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        var theme = DarkUI.Config.Themes.Dark;
        //theme.Colors.ControlBackground = Color.FromArgb(60,70,60);
        theme.Colors.ControlBackgroundOddIndex = theme.Colors.ControlBackground;
        theme.Use();

        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        Application.Run(new EditorForm());
    }
}
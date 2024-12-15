using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SyneticLib.Files.Common;
using SyneticLib.Locations;

namespace SyneticLib.WinForms;

public class AppSettingsFile : SyneticCfgFile<AppSettingsFile.Section>
{
    public class Section : Dictionary<string, string>
    {

    }

    public AppSettingsFile()
    {

    }
}

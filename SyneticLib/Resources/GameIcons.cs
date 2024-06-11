using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib.Resources;
public static class GameIcons
{
    public static MemoryStream MBWR => new MemoryStream(Properties.Resources.MBWR);

    public static MemoryStream WR2 => new MemoryStream(Properties.Resources.WR2);

    public static MemoryStream WR2CE => new MemoryStream(Properties.Resources.WR2CE);

    public static MemoryStream C11 => new MemoryStream(Properties.Resources.C11);

    public static MemoryStream CT1 => new MemoryStream(Properties.Resources.CT1);

    public static MemoryStream CT2 => new MemoryStream(Properties.Resources.CT2);

    public static MemoryStream CT3 => new MemoryStream(Properties.Resources.CT3);

    public static MemoryStream CT4 => new MemoryStream(Properties.Resources.CT4);

    public static MemoryStream CT5 => new MemoryStream(Properties.Resources.CT5);

    public static MemoryStream FVR => new MemoryStream(Properties.Resources.FVR);

    public static MemoryStream SyneticLib => new MemoryStream(Properties.Resources.GrilleSyneticLib);
}

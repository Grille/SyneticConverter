using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SyneticLib.Graphics;

namespace SyneticLib.WinForms;

public interface ISceneProvider
{
    public GlScene GetScene();
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SyneticLib.Math3D;

namespace SyneticLib;
public class Scene
{
    public Camera Camera { get; set; }

    public Scenario? Scenario { get; set; }

    public Scene()
    {
        Camera = new FreeCamera();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyneticLib.Locations;

namespace SyneticLib;
public class Car : SyneticObject
{
    public Model Model { get; }
    public TextureDirectory Textures;
    public RessourceList<Wheel> Wheels;

    public Car(string name, Model model)
    {
        Model = model;
    }
}

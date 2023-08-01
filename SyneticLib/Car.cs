using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyneticLib.Locations;

namespace SyneticLib;
public class Car : Ressource
{
    public Model Model { get; }
    public TextureDirectory Textures;
    public RessourceList<Wheel> Wheels;

    public Car(string name, Model model) : base(name)
    {
        Model = model;
    }
}

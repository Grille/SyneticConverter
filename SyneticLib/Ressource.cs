using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SyneticLib;
public abstract class Ressource {

    private static int RessourceIDCounter;

    public int RessourceID { get; }

    public string Name { get; }

    public Ressource Parent { get; set; }

    public Ressource(Ressource parent, string name)
    {
        Parent = parent;
        Name = name;

        RessourceID = RessourceIDCounter++;
    }
}

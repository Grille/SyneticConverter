using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SyneticLib;

/// <summary>
/// Data of an game resource, mostly immutable.
/// </summary>
public abstract class Ressource {

    private static int RessourceIDCounter;

    public int RessourceID { get; }

    public string Name { get; }

    public Ressource(string name)
    {
        RessourceID = RessourceIDCounter++;
        Name = name;
    }

    public Ressource()
    {
        RessourceID = RessourceIDCounter++;
        Name = $"{GetType().Name}_{RessourceID}";
    }
}

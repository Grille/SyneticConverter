using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using SyneticLib.Collections;

namespace SyneticLib;

/// <summary>
/// Data of an game resource, mostly immutable.
/// </summary>
public abstract class SyneticObject : IHasKey {

    private static int RessourceIDCounter;

    public int RessourceID { get; }

    public string Name { get; }

    public object? Extension { get; set; }

    string IHasKey.Key => Name;

    public SyneticObject(string name)
    {
        RessourceID = RessourceIDCounter++;
        Name = name;
    }

    public SyneticObject()
    {
        RessourceID = RessourceIDCounter++;
        Name = $"{GetType().Name}_{RessourceID}";
    }
}

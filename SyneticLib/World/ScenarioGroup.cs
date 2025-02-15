using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SyneticLib.Locations;
using System.Collections;

namespace SyneticLib.World;

public class ScenarioGroup : SyneticObject, IReadOnlyList<Scenario>
{
    private readonly Scenario[] _variants;

    public int Count { get => _variants.Length; }
    public Scenario? V1 { get => _variants.Length > 0 ? _variants[0] : null; }
    public Scenario? V2 { get => _variants.Length > 1 ? _variants[1] : null; }
    public Scenario? V3 { get => _variants.Length > 2 ? _variants[2] : null; }
    public Scenario? V4 { get => _variants.Length > 3 ? _variants[3] : null; }

    public ScenarioGroup(string name, IEnumerable<Scenario> variants)
    {
        _variants = variants.ToArray();
    }

    public Scenario this[int index]
    {
        get => _variants[index];
    }

    public IEnumerator<Scenario> GetEnumerator()
    {
        return ((IEnumerable<Scenario>)_variants).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _variants.GetEnumerator();
    }
}

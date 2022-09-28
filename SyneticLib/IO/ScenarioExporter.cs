using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib.IO;
public abstract class ScenarioExporter
{
    protected Scenario Target;
    protected string path;

    public ScenarioExporter(Scenario target)
    {
        Target = target;
        //path = target.RootDir;
    }

    protected abstract void OnSave();

    public void Save() => OnSave();

    public void Save(string path)
    {
        this.path = path;
        Save();
    }
}

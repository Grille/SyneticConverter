using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SyneticLib.IO.Synetic.Files;

namespace SyneticLib.IO;
public abstract class ScenarioImporter
{
    protected ScenarioVariant target;

    public ScenarioImporter(ScenarioVariant target)
    {
        this.target = target;
    }

    protected abstract void OnLoad();
    protected abstract void OnInit();

    public void Load()
    {
        try
        {
            OnLoad();
            OnInit();
            target.DataState = DataState.Loaded;
        }
        catch (FileNotFoundException ex)
        {
            target.Errors.Add(ex.Message);
            target.DataState = DataState.Error;
        }
    }
}

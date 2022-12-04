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
    protected ScenarioVGroup Target;

    public ScenarioImporter(ScenarioVGroup target)
    {
        Target = target;
    }

    protected abstract void OnLoad();


    public void Load()
    {
        try
        {
            OnLoad();
            Target.DataState = DataState.Loaded;
        }
        catch (FileNotFoundException ex)
        {
            //Target.Errors.Add(ex.Message);
            Target.DataState = DataState.Error;
        }
    }
}

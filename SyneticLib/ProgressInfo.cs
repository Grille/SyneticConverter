using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib;
public class ProgressInfo
{
    public string Name;
    public string Description;
    public bool Active;
    private float _value;

    public float Value
    {
        get { return _subProgress == null ? _value : _subProgress.Value; }
        set { _value = _subProgress == null ? _value = value : _subProgress.Value = value; }
    }
    private ProgressInfo _subProgress;

    public void Use(ProgressInfo progress = null)
    {
        _subProgress = progress;
    }
}

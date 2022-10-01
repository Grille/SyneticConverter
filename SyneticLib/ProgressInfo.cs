using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Windowing.Common.Input;

namespace SyneticLib;
public class ProgressLogger
{
    public string Name;
    private string _description;
    public bool Active;
    private float _value;

    public event EventHandler HasUpdated;

    public string Description
    {
        get { return _subProgress == null ? _description : _subProgress.Description; }
        private set { _description = value; }
    }

    public float Value
    {
        get { return _subProgress == null ? _value : _subProgress.Value; }
        private set { _value = value; }
    }
    private ProgressLogger _subProgress;

    public void Use(ProgressLogger progress)
    {
        Free();

        _subProgress = progress;
        _subProgress.HasUpdated += _subProgress_HasUpdated;
    }

    public void Free()
    {
        if (_subProgress != null)
        {
            _subProgress.HasUpdated -= _subProgress_HasUpdated;
            _subProgress = null;
        }
    }

    public void Update(float value, string text)
    {
        if (_subProgress == null)
        {
            _value = value;
            _description = text;
            HasUpdated?.Invoke(this, new EventArgs());
        }
        else
        {
            _subProgress.Update(value, text);
        }
    }


    public void Log(string msg)
    {

    }

    public void Warn(string msg)
    {

    }

    public void Error(string msg)
    {

    }




    private void _subProgress_HasUpdated(object sender, EventArgs e)
    {
        HasUpdated?.Invoke(sender, new EventArgs());
    }
}

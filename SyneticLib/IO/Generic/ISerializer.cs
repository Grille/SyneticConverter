using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib.IO.Generic;
public interface ISerializer<TObj>
{
    public TObj Load(string path);

    public void Save(string path, TObj obj);

    public IReadOnlyCollection<string> GetErrors();
}

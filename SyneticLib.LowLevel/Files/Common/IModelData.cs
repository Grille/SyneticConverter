using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib.Files.Common;

public interface IModelData
{
    public IModel[] Models { get; set; }

    public interface IModel
    {
        public Primitives[] Primitives { get; set; }
    }

    public struct Primitives
    {
        public int Offset;
        public int Length;
        public int MaterialIndex;
    }
}

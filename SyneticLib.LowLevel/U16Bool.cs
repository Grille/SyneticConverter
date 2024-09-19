using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib;

public struct U16Bool
{
    ushort _value;

    public static implicit operator bool(U16Bool value) => value._value != 0;

    public static implicit operator U16Bool(bool value) => new U16Bool { _value = (ushort)(value ? 1 : 0) };
}

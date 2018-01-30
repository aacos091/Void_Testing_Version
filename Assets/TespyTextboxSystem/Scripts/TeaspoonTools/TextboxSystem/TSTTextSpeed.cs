using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeaspoonTools.TextboxSystem
{
    [System.Serializable]
    public enum TextSpeed
    {
        // these values amount to characters printed per second
        verySlow = 10,
        slow = (int)(verySlow * 2.5f),
        medium = (int)(slow * 2.5f),
        fast = (int)(medium * 3),

        instant = 999
    }
}

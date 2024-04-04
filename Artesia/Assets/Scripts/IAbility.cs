using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAblity
{
    public int HP { get; }
    public int EXP { get; }
    public int DEF { get; }
    public int ATK { get; }
    public int LV { get; }
}

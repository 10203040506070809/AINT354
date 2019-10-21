using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    /// <summary>
    /// Start value for a given stat.
    /// </summary>
        [SerializeField]
    private int baseValue;

    /// <summary>
    /// Final value for a given stat.
    /// </summary>

    /// <summary>
    /// Gets the value of a stat.
    /// </summary>
    /// <returns></returns>
    public int GetValue()
    {
        return baseValue;
    }

    /// <summary>
    /// Sets the value of  a stat.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
   public int SetValue(int value)
    {
        baseValue = value;
        return baseValue;
    }
}

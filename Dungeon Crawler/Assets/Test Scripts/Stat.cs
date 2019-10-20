using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    //start value for a given stat

        [SerializeField]
    private int baseValue;


    public int GetValue()
    {
        return baseValue;
    }
}

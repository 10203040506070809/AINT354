using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : Item
{
    /// <summary>
    /// A float value denoting the duration of a potion.
    /// </summary>
    [SerializeField] private float m_potionTimer;
    /// <summary>
    /// A float value, used to denote the required time between using two of the same potions.
    /// </summary>
    [SerializeField] private float m_potionCooldown;

    /// <summary>
    /// Activates the potion effect.
    /// </summary>
    public virtual void ActivatePotion()
    {
       
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : Item
{
    [SerializeField] private float m_potionTimer;
    [SerializeField] private float m_potionCooldown;

    public virtual void ActivatePotion()
    {

    }
}

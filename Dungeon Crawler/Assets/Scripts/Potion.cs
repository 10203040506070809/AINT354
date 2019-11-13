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
    /// An integer value denoting how many of the item can be stored in a single slot.
    /// </summary>
    [SerializeField] private int m_maxNumberInSlot;
    private bool m_isAvailable = true;

    /// <summary>
    /// Activates the potion effect.
    /// </summary>
    public virtual void ActivatePotion()
    {
       if (m_isAvailable)
        {
            m_isAvailable = false;
            
            ///Activate potion effect here
        }
    }

    /// <summary>
    /// Sets m_isAvailable to true after the potions cooldown.
    /// </summary>
    /// <returns></returns>
    public virtual IEnumerator PotionCooldown()
    {
        yield return new WaitForSeconds(m_potionCooldown);
        m_isAvailable = true;
    }
}

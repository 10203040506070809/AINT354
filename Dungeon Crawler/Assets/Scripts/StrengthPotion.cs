using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrengthPotion : Potion
{

    
    /// <summary>
    /// Initialise variables here.
    /// </summary>
    public void Start()
    {

    }
    /// <summary>
    /// An override for the item class's ActivateEffect.
    /// </summary>
    public override void ActivateEffect()
    {
        StartCoroutine("StrengthUp");
    }
    
    IEnumerator StrengthUp()
    {
        CharacterStats myStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        int originalDamage = myStats.GetDamage();
        myStats.SetDamage((int)(originalDamage * m_potency));
        yield return new WaitForSeconds(m_potionTimer);
        myStats.SetDamage(originalDamage);
    }
}

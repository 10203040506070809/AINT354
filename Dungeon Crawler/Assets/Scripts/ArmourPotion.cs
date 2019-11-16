using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmourPotion : Potion
{
    public override void ActivateEffect()
    {
        StartCoroutine("ArmourUp");
 

    }

    IEnumerator ArmourUp()
    {
        CharacterStats myStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        int originalArmour = (int)myStats.GetArmour();
        myStats.SetArmour((int)(myStats.GetArmour() * m_potency));
        yield return new WaitForSeconds(m_potionTimer);
        myStats.SetArmour(originalArmour);
    }
}

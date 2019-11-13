using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrengthPotion : Potion
{
    // Start is called before the first frame update


    public override void OnPickUp()
    {
        base.OnPickUp();
    }

    public override void ActivateEffect()
    {
        Debug.Log("Strength potion activated");
    }

    public override void ActivatePotion()
    {
        base.ActivatePotion();
    }

}

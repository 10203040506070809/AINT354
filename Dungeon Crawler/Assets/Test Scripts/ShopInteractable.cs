using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopInteractable : Interactable
{
    /// <summary>
    /// Overrides the original InteractedWith method.
    /// </summary>
    public override void InteractedWith()
    {
        ///Debugs Interacted with + Gameobject name.
        base.InteractedWith();
    }
}

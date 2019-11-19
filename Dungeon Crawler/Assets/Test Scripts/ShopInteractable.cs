using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShopInteractable : Interactable
{
    [SerializeField] private Canvas m_ShopUI; 

    /// <summary>
    /// Overrides the original InteractedWith method.
    /// </summary>
    public override void InteractedWith()
    {
        ///Debugs Interacted with + Gameobject name.
        base.InteractedWith();
        m_ShopUI.enabled = true;
    }
}

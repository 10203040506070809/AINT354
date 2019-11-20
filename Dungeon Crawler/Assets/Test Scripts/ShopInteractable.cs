using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShopInteractable : Interactable
{
    [SerializeField] private GameObject m_ShopUI; 

    /// <summary>
    /// Overrides the original InteractedWith method.
    /// </summary>
    public override void InteractedWith()
    {
        ///Debugs Interacted with + Gameobject name.
        base.InteractedWith();
        m_ShopUI.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            m_ShopUI.SetActive(false);
        }
    }
}

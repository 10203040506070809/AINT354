using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    /// <summary>
    /// An integer value denoting how many of the item can be stored in a single slot in the hotbar.
    /// </summary>
    [SerializeField] private int m_maxNumberInSlot;

    /// <summary>
    /// Occurs when an item interacts with this. Checks that the other GameObject is a player and if so, activeates the OnPickUp script.
    /// </summary>
    /// <param name="other">A reference to the other GameObjects collider.</param>
    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Trigger entered.");
            OnPickUp();
        }
    }
    /// <summary>
    /// A method which handles the interaction between player and item - Adds it to their inventory, plays animation/sound or uses it. Meant to be overriden.
    /// </summary>
    public virtual void OnPickUp()
    {
        Debug.Log("Picked up.");
        ///Add to hotbar
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Item : MonoBehaviour
{
    /// <summary>
    /// An integer value denoting how many of the item can be stored in a single slot in the hotbar.
    /// </summary>
    [SerializeField] private int m_maxNumberInSlot;
    /// <summary>
    /// A reference to an image variable, which is the image shown on the hotbar UI.
    /// </summary>
    [SerializeField] private Image m_hotBarIcon;
    /// <summary>
    /// A string that stores hover over text, so the player can understand what different items are and do.
    /// </summary>
    [SerializeField] private string m_hoverOverText;
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

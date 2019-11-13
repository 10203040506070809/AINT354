using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{

    /// <summary>
    /// Occurs when an item interacts with this. Checks that the other GameObject is a player and if so, activeates the OnPickUp script.
    /// </summary>
    /// <param name="other">A reference to the other GameObjects collider.</param>
    public virtual void OnTriggerEnter(Collider other)
    {
        
    }
    /// <summary>
    /// A method which 
    /// </summary>
    public virtual void OnPickUp()
    {
        Debug.Log("Picked up.");
    }
}

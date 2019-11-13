using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{

    /// <summary>
    /// Occurs when an item interacts with this. Checks that the other GameObject is a player and if so, activeates the OnPickUp script.
    /// </summary>
    /// <param name="other"></param>
    public virtual void OnTriggerEnter(Collider other)
    {
        
    }

}

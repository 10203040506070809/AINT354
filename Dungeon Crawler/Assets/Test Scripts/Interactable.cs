using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    /// <summary>
    /// A method which is activated whenever a gameobject with this is interacted with. Meant to be overriden.
    /// </summary>
    public virtual void InteractedWith()
    {

    }
}

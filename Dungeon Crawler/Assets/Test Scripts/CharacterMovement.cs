using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterStats))]
public class CharacterMovement : MonoBehaviour
{
  
    // Update is called once per frame
    public virtual void Update()
    {
        Move();
    }

    /// <summary>
    /// Method for movement. Meant to be overriden.
    /// </summary>
    /// 

    public virtual void Move()
    {
       
    }
}

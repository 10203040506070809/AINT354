using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private GameObject sword;
    [SerializeField] private GameObject gun;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ///If input happens
        if (Input.GetButton("Fire1"))
        {
            ///Do animation
            
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CharacterStats))]
public class PlayerMovement : CharacterMovement
{
    private Rigidbody rb;
    private CharacterStats myStats;
    int x = 0; int y = 0; int z = 0;


    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();

        myStats = this.GetComponent<CharacterStats>();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
       
    }

    public override void Move()
    { 
        base.Move();

        Vector3 test = new Vector3(x, y, z);
        //TODO Footstep sound

        rb.velocity = test;
        if (Input.GetAxis("Horizontal") != 0)
        {
            test.x = Input.GetAxis("Horizontal") * myStats.GetMovementSpeed();
            Debug.Log("Done a thing x");
        }
        if (Input.GetAxis("Vertical") != 0)
        {
            test.z = Input.GetAxis("Vertical") * myStats.GetMovementSpeed();
            Debug.Log("Done a thing z");
        }

        rb.velocity = test;
    }
}

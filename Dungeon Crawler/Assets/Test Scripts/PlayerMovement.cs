using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CharacterStats), typeof(Rigidbody))]
public class PlayerMovement : CharacterMovement
{
    /// <summary>
    /// An audiosource for footsteps, called when the player moves.
    /// </summary>
    [SerializeField] private AudioSource m_footSteps;
    /// <summary>
    /// An integer denoting the force a player jumps at.
    /// </summary>
    [SerializeField] private int m_jumpForce;
    /// <summary>
    /// A rigidbody variable,responsible for physics.
    /// </summary>
    private Rigidbody m_rigidBody;
    /// <summary>
    /// A characterstats variable storing the player movement speed.
    /// </summary>
    private CharacterStats m_myStats;
    /// <summary>
    /// 
    /// </summary>
    private Vector3 m_MovementVector;
    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        m_rigidBody = this.GetComponent<Rigidbody>();
       
        m_myStats = this.GetComponent<CharacterStats>();
    }
    /// <summary>
    /// Update is called once per frame
    /// </summary>
    public void Update()
    {
        Move();
      
    }
    /// <summary>
    /// 
    /// </summary>
    public override void Move()
    {
    /// <summary>
    /// A variable to hold the player movement along the x axis.
    /// </summary>
    int m_x = 0;

    /// <summary>
    /// A variable to hold the player movement along the y axis.
    /// </summary>
    int m_y = 0;
    /// <summary>
    /// A variable to hold the player movement along the z axis.
    /// </summary>
    int m_z = 0;

    m_MovementVector = new Vector3(m_x, m_y, m_z);
        //TODO Footstep sound

        m_rigidBody.velocity = m_MovementVector;
        if (Input.GetAxis("Horizontal") != 0)
        {
            m_MovementVector.x = Input.GetAxis("Horizontal") * m_myStats.GetMovementSpeed();
      
        }
        if (Input.GetAxis("Vertical") != 0)
        {
            m_MovementVector.z = Input.GetAxis("Vertical") * m_myStats.GetMovementSpeed();
            
        }

        m_rigidBody.velocity = m_MovementVector;
    }
}

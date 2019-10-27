using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CharacterStats), typeof(CharacterController))]
public class PlayerMovement : CharacterMovement
{
    /// <summary>
    /// A characterstats variable storing the player movement speed.
    /// </summary>
    private CharacterStats m_myStats;
    /// <summary>
    /// A reference to the character controller attached to the player.
    /// </summary>
    private CharacterController m_characterController;
    /// <summary>
    /// A reference to the animator attached to the player.
    /// </summary>
    private Animator m_animator;
    /// <summary>
    /// A reference to a float used to denote speed. Should be overriden by the players' movement stat.
    /// </summary>
    [SerializeField] private float m_speed;
    /// <summary>
    /// An input for an x value that is used for players' horizontal movement.
    /// </summary>
    [SerializeField] private float m_inputX;
    /// <summary>
    /// An input for a z value that is used for players' vertical movement.
    /// </summary>
    [SerializeField] private float m_inputZ;
    /// <summary>
    /// A variable that's used to ground the player.
    /// </summary>
    private float m_verticalVelocity;
    /// <summary>
    /// A movement vector for the players' movement.
    /// </summary>
    private Vector3 m_moveVector; 
    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    public void Start()
    {      
        m_myStats = this.GetComponent<CharacterStats>();

        m_characterController = this.GetComponent<CharacterController>();

        m_animator = this.GetComponent<Animator>();

        m_speed = m_myStats.GetMovementSpeed();

    }
    /// <summary>
    /// FixedUpdate is called once per frame. Because we're dealing with Rigidbody physics,
    /// it's better to use FixedUpdate
    /// as it occurs at the same time every frame.
    /// </summary>
    public void FixedUpdate()
    {
       
        ///If the player is grounded, do this.
        if (m_characterController.isGrounded)
        {
            m_verticalVelocity -= 0;
            //Debug.Log("Grounded.");
        }
        ///Otherwise, do this. Note, this is not an else statement in case of edge cases.
        if (!m_characterController.isGrounded)
        {

            m_verticalVelocity -= 2;
            //Debug.Log("Not Grounded.");
            m_moveVector = new Vector3(0, m_verticalVelocity, 0);
            m_characterController.Move(m_moveVector);
        }
        Move();
    }
    /// <summary>
    /// Moves the player based on input.
    /// </summary>
    public override void Move()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            m_moveVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            m_moveVector *= m_myStats.GetMovementSpeed() / 5;
          

            m_characterController.Move(m_moveVector * Time.deltaTime);
            transform.rotation = Quaternion.LookRotation(m_moveVector);
        }
    }

    private void CreateDust()
    {
        dust.Play();
    }
   

}

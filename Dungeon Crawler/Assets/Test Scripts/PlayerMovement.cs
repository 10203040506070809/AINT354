using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Made using Filmstorms tutorial, 'Create an Open World Movement System in Unity C#'
/// </summary>
[RequireComponent(typeof(CharacterStats), typeof(CharacterController))]
public class PlayerMovement : CharacterMovement
{
    /// <summary>
    /// A characterstats variable storing the player movement speed.
    /// </summary>
    private CharacterStats m_myStats;
    /// <summary>
    /// 
    /// </summary>
    private CharacterController m_characterController;
    /// <summary>
    /// 
    /// </summary>
    private Animator m_animator;
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] private float m_speed;
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] private float m_inputX;
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] private float m_inputZ;
    /// <summary>
    /// 
    /// </summary>
    private float m_verticalVelocity;
    /// <summary>
    /// 
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
       

        if (m_characterController.isGrounded)
        {
            m_verticalVelocity -= 0;
            //Debug.Log("Grounded.");
        }
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
    /// 
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
   

}

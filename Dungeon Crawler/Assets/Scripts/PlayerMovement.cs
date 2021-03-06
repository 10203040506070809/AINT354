﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CharacterStats))]
public class PlayerMovement : CharacterMovement
{
    /// <summary>
    /// A characterstats variable storing the player movement speed.
    /// </summary>
    private CharacterStats m_myStats;
    /// <summary>
    /// A reference to the Rigidbody attached to the player.
    /// </summary>
    private Rigidbody m_rigidBody;
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
        m_animator = this.GetComponent<Animator>();
        m_speed = m_myStats.GetMovementSpeed();
        m_rigidBody = this.GetComponent<Rigidbody>();
    }
    /// <summary>
    /// FixedUpdate is called once per frame. Because we're dealing with Rigidbody physics,
    /// it's better to use FixedUpdate
    /// as it occurs at the same time every frame.
    /// </summary>
    public void Update()
    {
        Move();
        Rotation();
        Attack();
    }
    /// <summary>
    /// Moves the player based on input.
    /// </summary>
    public override void Move()
    {
        m_inputX = Input.GetAxis("Horizontal");
        m_inputZ = Input.GetAxis("Vertical");
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            m_animator.SetBool("isWalking", true);
            m_moveVector = new Vector3(m_inputX, 0, m_inputZ);
           
            m_moveVector.Normalize();

            m_moveVector /= 5;

            m_rigidBody.MovePosition(transform.position + m_moveVector);                        
        }
        else
        {
            m_animator.SetBool("isWalking", false);
        }
    }

    /// <summary>
    /// Allows the player to attack by pressing the Left Mouse Button
    /// </summary>
    private void Attack()
    {
        //Main attack - Slash
        if (Input.GetButton("Fire1"))
        {
            if (m_animator.GetBool("isAttacking") == false)
            {
                m_animator.SetBool("isAttacking", true);
                Invoke("AttackCooldown", 1f);
            }
        }

        //Alt attack - Stab
        if (Input.GetButton("Fire2")){
            if (m_animator.GetBool("isAttacking") == false)
            {
                m_animator.SetBool("isAttacking", true);
                Invoke("AttackCooldown", 0.5f);
            }
        }
    }
    /// <summary>
    /// After a set delay, re-enable attack
    /// </summary>
    private void AttackCooldown()
    {
        m_animator.SetBool("isAttacking", false);
    }

    /// <summary>
    /// Rotates the player
    /// </summary>
    private void Rotation()
    {
        var groundPlane = new Plane(Vector3.up, -transform.position.y);
        var mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        float hitDistance;

        if (groundPlane.Raycast(mouseRay, out hitDistance))
        {
            Vector3 lookAtPosition = mouseRay.GetPoint(hitDistance);
            transform.LookAt(lookAtPosition, Vector3.up);
        }
    }


}

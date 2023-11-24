using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace m17
{
    public class SMBIdleState : MBState
    {
        private PJSMB m_PJ;
        private Rigidbody2D m_Rigidbody;
        private Animator m_Animator;
        private FiniteStateMachine m_StateMachine;

        private void Awake()
        {
            m_PJ = GetComponent<PJSMB>();
            m_Rigidbody = GetComponent<Rigidbody2D>();
            m_Animator = GetComponent<Animator>();
            m_StateMachine = GetComponent<FiniteStateMachine>();
        }

        public override void Init()
        {
            base.Init();
            if (m_PJ.Jugador1)
            {
                m_PJ.Input.FindActionMap("Standard").FindAction("Shoot").performed += OnAttack;
                m_PJ.Input.FindActionMap("Standard").FindAction("Parry").started += OnParry;
            }
            else
            {
                m_PJ.Input.FindActionMap("Mando").FindAction("Shoot").performed += OnAttack;
                m_PJ.Input.FindActionMap("Mando").FindAction("Parry").started += OnParry;
            }
            m_Rigidbody.velocity = Vector2.zero;
            m_Animator.Play("idlePlayer");
        }

        private void OnParry(InputAction.CallbackContext context)
        {
            m_StateMachine.ChangeState<SMBParryState>();
        }

        public override void Exit()
        {
            base.Exit();
            if (m_PJ != null) {
                if (m_PJ.Jugador1)
                {
                    m_PJ.Input.FindActionMap("Standard").FindAction("Shoot").performed -= OnAttack;
                    m_PJ.Input.FindActionMap("Standard").FindAction("Parry").started -= OnParry;
                }
                else
                {
                    m_PJ.Input.FindActionMap("Mando").FindAction("Shoot").performed -= OnAttack;
                    m_PJ.Input.FindActionMap("Mando").FindAction("Parry").started -= OnParry;
                }
            }
 
        }

        private void OnAttack(InputAction.CallbackContext context)
        {
            m_StateMachine.ChangeState<SMBShootingState>();
        }

        private void Update()
        {
            if (m_PJ.MovementAction.ReadValue<Vector2>() != Vector2.zero)
                m_StateMachine.ChangeState<SMBWalkState>();
        }
    }
}

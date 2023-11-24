using IsaacBongos;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace m17
{
    public class SMBWalkState : MBState
    {
        private PJSMB m_PJ;
        private Rigidbody2D m_Rigidbody;
        private Animator m_Animator;
        private FiniteStateMachine m_StateMachine;
        private StatsController m_StatsController;

        private Vector2 m_Movement;

        [SerializeField]
        private float m_Speed = 3f;

        private void Awake()
        {
            m_PJ = GetComponent<PJSMB>();
            m_Rigidbody = GetComponent<Rigidbody2D>();
            m_Animator = GetComponent<Animator>();
            m_StateMachine = GetComponent<FiniteStateMachine>();
            m_StatsController = GetComponent<StatsController>();
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
            
            m_Animator.Play("walkingPlayer");
        }

        private void OnParry(InputAction.CallbackContext context)
        {
            m_StateMachine.ChangeState<SMBParryState>();
        }

        public override void Exit()
        {
            base.Exit();
            if (m_PJ != null)
            {
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
            m_Movement = m_PJ.MovementAction.ReadValue<Vector2>();

            if(m_Movement ==  Vector2.zero)
                m_StateMachine.ChangeState<SMBIdleState>();
        }

        private void FixedUpdate()
        {
            m_Rigidbody.velocity = m_Movement * m_StatsController.SPEED ;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

namespace m17
{
    [RequireComponent(typeof(ComboHandler))]
    public abstract class SMBComboState : MBState
    {
        private PJSMB m_PJ;
        private Rigidbody2D m_Rigidbody;
        protected Animator m_Animator;
        protected FiniteStateMachine m_StateMachine;
        private ComboHandler m_ComboHandler;


        [SerializeField]
        private HitboxInfo m_Hitbox;
        [SerializeField]
        private int m_Damage;

        private void Awake()
        {
            Assert.IsNotNull(m_Hitbox);
            m_PJ = GetComponent<PJSMB>();
            m_Rigidbody = GetComponent<Rigidbody2D>();
            m_Animator = GetComponent<Animator>();
            m_StateMachine = GetComponent<FiniteStateMachine>();
            m_ComboHandler = GetComponent<ComboHandler>();
        }

        public override void Init()
        {
            base.Init();
            m_PJ.Input.FindActionMap("Standard").FindAction("Attack").performed += OnAttack;
            m_Rigidbody.velocity = Vector2.zero;
            m_Hitbox.Damage = m_Damage;
            m_ComboHandler.enabled = true;
            m_ComboHandler.OnEndAction += OnEndAction;
        }

        public override void Exit()
        {
            base.Exit();
            m_ComboHandler.enabled = false;
            m_PJ.Input.FindActionMap("Standard").FindAction("Attack").performed -= OnAttack;
            m_ComboHandler.OnEndAction -= OnEndAction;
        }

        private void OnAttack(InputAction.CallbackContext context)
        {
            if (m_ComboHandler.ComboAvailable)
                OnComboSuccessAction();
            else
                OnComboFailedAction();
        }

        protected abstract void OnComboSuccessAction();
        protected abstract void OnComboFailedAction();

        protected abstract void OnEndAction();
    }
}

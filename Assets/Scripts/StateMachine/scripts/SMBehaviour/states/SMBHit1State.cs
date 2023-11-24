using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

namespace m17
{
    public class SMBHit1State : SMBComboState
    {
        public override void Init()
        {
            base.Init();
            m_Animator.Play("2_hit1", 0, 0f);
        }

        protected override void OnComboFailedAction()
        {
            
        }

        protected override void OnComboSuccessAction()
        {
            m_StateMachine.ChangeState<SMBHit2State>();
        }

        protected override void OnEndAction()
        {
            m_StateMachine.ChangeState<SMBIdleState>();
        }
    }
}

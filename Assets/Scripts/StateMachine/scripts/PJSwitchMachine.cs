using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

namespace m17
{
    public class PJSwitchMachine : MonoBehaviour
    {

        //Aquesta màquina funciona amb un enum pels estats i un switch
        //per a la gestió de la lògica de cada un.
        //Imprescindible utilitzar les seves funcions de canvi, actualització,
        //inici i sortida de cada estat.
        //None simplement existeix perquè hem de fer l'init del primer estat i cal que
        //la variable no tingui valor igual al primer d'ells (idle).
        private enum SwitchMachineStates { NONE, IDLE, WALK, HIT1, HIT2 };
        private SwitchMachineStates m_CurrentState;

        //canviarem d'estat sempre mitjançant aquesta funció
        private void ChangeState(SwitchMachineStates newState)
        {
            //no caldria fer-ho, però evitem que un estat entri a si mateix
            //és possible que la nostra màquina ho permeti, per tant això
            //no es faria sempre.
            if (newState == m_CurrentState)
                return;

            ExitState();
            InitState(newState);
        }

        private void InitState(SwitchMachineStates currentState)
        {
            m_CurrentState = currentState;
            switch (m_CurrentState)
            {
                case SwitchMachineStates.IDLE:

                    m_Rigidbody.velocity = Vector2.zero;
                    m_Animator.Play("idle");

                    break;

                case SwitchMachineStates.WALK:

                    m_Animator.Play("walk");

                    break;

                case SwitchMachineStates.HIT1:

                    m_ComboAvailable = false;
                    m_Rigidbody.velocity = Vector2.zero;
                    m_HitboxInfo.Damage = m_Hit1Damage;
                    m_Animator.Play("hit1");

                    break;

                case SwitchMachineStates.HIT2:

                    m_ComboAvailable = false;
                    m_Rigidbody.velocity = Vector2.zero;
                    m_HitboxInfo.Damage = m_Hit2Damage;
                    m_Animator.Play("hit2");

                    break;

                default:
                    break;
            }
        }

        private void ExitState()
        {
            switch (m_CurrentState)
            {
                case SwitchMachineStates.IDLE:

                    break;

                case SwitchMachineStates.WALK:

                    break;

                case SwitchMachineStates.HIT1:

                    m_ComboAvailable = false;

                    break;

                case SwitchMachineStates.HIT2:

                    m_ComboAvailable = false;

                    break;

                default:
                    break;
            }
        }

        private void UpdateState()
        {
            switch (m_CurrentState)
            {
                case SwitchMachineStates.IDLE:

                    if (m_MovementAction.ReadValue<Vector2>().x != 0)
                        ChangeState(SwitchMachineStates.WALK);

                    break;
                case SwitchMachineStates.WALK:

                    m_Rigidbody.velocity = Vector2.right * m_MovementAction.ReadValue<Vector2>().x * m_Speed;

                    if (m_Rigidbody.velocity == Vector2.zero)
                        ChangeState(SwitchMachineStates.IDLE);

                    break;
                case SwitchMachineStates.HIT1:

                    break;

                case SwitchMachineStates.HIT2:

                    break;

                default:
                    break;
            }
        }

        //------------------------------------------------------------------------//
        //Possible implementació del sistema de combo
        private bool m_ComboAvailable = false;

        public void InitComboWindow()
        {
            m_ComboAvailable = true;
        }

        public void EndComboWindow()
        {
            m_ComboAvailable = false;
        }

        public void EndHit()
        {
            ChangeState(SwitchMachineStates.IDLE);
        }
        //------------------------------------------------------------------------//

        //------------------------------------------------------------------------//
        //Input
        //Podriem anar-nos subscrivint i desubscrivint de les diferents accions segons
        //l'estat en el que ens trobem.
        private void AttackAction(InputAction.CallbackContext actionContext)
        {
            switch (m_CurrentState)
            {
                case SwitchMachineStates.IDLE:
                    ChangeState(SwitchMachineStates.HIT1);

                    break;

                case SwitchMachineStates.WALK:
                    ChangeState(SwitchMachineStates.HIT1);

                    break;

                case SwitchMachineStates.HIT1:

                    if (m_ComboAvailable)
                        ChangeState(SwitchMachineStates.HIT2);
                    else
                        ChangeState(SwitchMachineStates.HIT1);

                    break;

                case SwitchMachineStates.HIT2:
                    break;

                default:
                    break;
            }
        }
        //------------------------------------------------------------------------//

        [SerializeField]
        private InputActionAsset m_InputAsset;
        private InputActionAsset m_Input;
        private InputAction m_MovementAction;
        private Animator m_Animator;
        private Rigidbody2D m_Rigidbody;
        [SerializeField]
        private HitboxInfo m_HitboxInfo;

        [Header("Character Values")]
        [SerializeField]
        private float m_Speed = 2;
        [SerializeField]
        private int m_Hit1Damage = 2;
        [SerializeField]
        private int m_Hit2Damage = 5;

        void Awake()
        {
            Assert.IsNotNull(m_InputAsset);

            m_Input = Instantiate(m_InputAsset);
            m_MovementAction = m_Input.FindActionMap("Standard").FindAction("Movement");
            m_Input.FindActionMap("Standard").FindAction("Attack").performed += AttackAction;
            m_Input.FindActionMap("Standard").Enable();

            m_Rigidbody = GetComponent<Rigidbody2D>();
            m_Animator = GetComponent<Animator>();
        }

        private void Start()
        {
            InitState(SwitchMachineStates.IDLE);
        }

        void Update()
        {
            UpdateState();
        }

        private void OnDestroy()
        {
            m_Input.FindActionMap("Standard").FindAction("Attack").performed -= AttackAction;
            m_Input.FindActionMap("Standard").Disable();
        }
    }
}

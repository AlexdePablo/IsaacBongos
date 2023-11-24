using IsaacBongos;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

namespace m17
{
    [RequireComponent(typeof(FiniteStateMachine))]
    [RequireComponent(typeof(SMBIdleState))]
    [RequireComponent(typeof(SMBWalkState))]
    [RequireComponent (typeof(SMBShootingState))]
    [RequireComponent(typeof(SMBParryState))]
    public class PJSMB : MonoBehaviour, ISaveablePlayer
    {
        private FiniteStateMachine m_StateMachine;
        public FiniteStateMachine StateMachine => m_StateMachine;
        private HealthController m_HealthController;
        private DineroController m_DineroController;
        private BackPackController m_BackPackController;
        private StatsController m_StatsController;
        private bool _inventarioAbierto;

        [SerializeField]
        private InputActionAsset m_InputAsset;
        private InputActionAsset m_Input;
        public InputActionAsset Input => m_Input;
        private InputAction m_MovementAction;
        public InputAction MovementAction => m_MovementAction;
        private InputAction m_PointerAction;
        public InputAction PointerAction => m_PointerAction;

        [SerializeField]
        private GameEventInt m_GameEventInt;

        [SerializeField]
        private GameEvent showInventarioEvent;

        [SerializeField]
        private GameEventBool tiendaInventario;

        [SerializeField]
        private GameEventFloat vidaEvent;

        [SerializeField]
        private GameEvent pausaEvento;

        [SerializeField]
        private GameEvent mapaEvento;

        public Action AcabarPartida;

        private bool m_Jugador1;
        public bool Jugador1 => m_Jugador1;

        private void Awake()
        {
            m_HealthController = GetComponent<HealthController>();
            m_DineroController = GetComponent<DineroController>();
            m_BackPackController = GetComponent<BackPackController>();
            m_StatsController = GetComponent<StatsController>();
            m_HealthController.onCambioDeVida += CambiarVida;
        }

        public void Init(bool _Jugador1)
        {
            _inventarioAbierto = false;
            m_Jugador1 = _Jugador1;
           
            m_HealthController.SetVida(100);

            Assert.IsNotNull(m_InputAsset);
            m_StateMachine = GetComponent<FiniteStateMachine>();

            m_Input = Instantiate(m_InputAsset);
            if (m_Jugador1)
            {
                m_MovementAction = m_Input.FindActionMap("Standard").FindAction("Movement");
                m_PointerAction = m_Input.FindActionMap("Standard").FindAction("MousePosition");
                m_Input.FindActionMap("Standard").FindAction("Pause").performed += PausarJuego;
                m_Input.FindActionMap("Standard").FindAction("ShowMap").performed += ShowMap;
                m_Input.FindActionMap("Standard").FindAction("Inventario").performed += ShowInventario;
                m_Input.FindActionMap("Standard").Enable();
            }
            else
            {
                m_MovementAction = m_Input.FindActionMap("Mando").FindAction("Movement");
                m_PointerAction = m_Input.FindActionMap("Mando").FindAction("MousePosition");
                m_Input.FindActionMap("Mando").FindAction("Pause").performed += PausarJuego;
                m_Input.FindActionMap("Mando").FindAction("ShowMap").performed += ShowMap;
                m_Input.FindActionMap("Mando").FindAction("Inventario").performed += ShowInventario;
                m_Input.FindActionMap("Mando").Enable();
            }
           


            m_StateMachine.ChangeState<SMBIdleState>();
        }

        private void ShowInventario(InputAction.CallbackContext obj)
        {
            showInventarioEvent.Raise();
        }

        private void ShowMap(InputAction.CallbackContext obj)
        {
            mapaEvento.Raise();
            if (Time.timeScale == 0)
                Time.timeScale = 1;
            else
                Time.timeScale = 0;
        }

        private void PausarJuego(InputAction.CallbackContext obj)
        {
            pausaEvento.Raise();
            if(Time.timeScale == 0)
                Time.timeScale = 1;
            else
                Time.timeScale = 0;
        }
        public void CambiarDinero(int _Dinero)
        {
            m_DineroController.SetDinero(_Dinero);
            m_GameEventInt.Raise(m_DineroController.DINERO);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Item"))
            {
                m_BackPackController.AddItem(collision.GetComponent<ItemScript>().Item);
            }
            if (collision.CompareTag("Vendedor"))
            {
                tiendaInventario.Raise(true);
            }
            if (collision.CompareTag("Moneda"))
            {
                print("moneda");
                m_DineroController.SetDinero(1);
                m_GameEventInt.Raise(m_DineroController.DINERO);
            }

            if (collision.CompareTag("ProjectilEnemigo") || collision.CompareTag("EnemyHitbox")) {
                m_HealthController.Damage(collision.gameObject.GetComponent<Daño>().daño);
                if (m_HealthController.HP == 0) {
                    AcabarPartida?.Invoke();
                    Destroy(this.gameObject);
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Vendedor"))
            {
                tiendaInventario.Raise(false);
            }
        }

        public void CambiarVida()
        {
            vidaEvent.Raise(m_HealthController.HP);
        }

        private void Morir()
        {
           m_StateMachine.ChangeState<SMBMuertoState>();
        }

        public SaveGame.playerSave Save()
        {
            SaveGame.playerSave playerData = new SaveGame.playerSave(transform.position, m_BackPackController.getBackPack().ToArray(), m_Jugador1, m_DineroController.DINERO, m_HealthController.HP);

            return playerData;
        }

        public void Load(SaveGame.playerSave playerData)
        {
            _inventarioAbierto = false;
            transform.position = playerData.posicionPlayer;
            m_BackPackController.ClearBackpack();
            m_BackPackController.RemakeBackPack(playerData.items);
            m_HealthController.SetVida(playerData.vida);
            StartCoroutine(ActualizarDinero(playerData.dinero));
        }

        private IEnumerator ActualizarDinero(int _dinero)
        {
            yield return new WaitForSeconds(0.7f);
            CambiarDinero(_dinero);
        }

        private void OnDestroy()
        {
            m_BackPackController.ClearBackpack();
            if (m_Jugador1)
            {
                m_Input.FindActionMap("Standard").FindAction("Pause").performed -= PausarJuego;
                m_Input.FindActionMap("Standard").FindAction("ShowMap").performed -= ShowMap;
                m_Input.FindActionMap("Standard").FindAction("Inventario").performed -= ShowInventario;
                m_Input.FindActionMap("Standard").Disable();
            }
            else
            {
                m_Input.FindActionMap("Mando").FindAction("Pause").performed -= PausarJuego;
                m_Input.FindActionMap("Mando").FindAction("ShowMap").performed -= ShowMap;
                m_Input.FindActionMap("Mando").FindAction("Inventario").performed -= ShowInventario;
                m_Input.FindActionMap("Mando").Disable();
            }
        }


    }

   
}

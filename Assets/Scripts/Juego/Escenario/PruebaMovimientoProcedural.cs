using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static IsaacBongos.Backpack;

namespace IsaacBongos
{
    public class PruebaMovimientoProcedural : MonoBehaviour, ISaveablePlayer
    {
        private Rigidbody2D m_Rigidbody2D;
        private BackPackController m_BackPackController;
        private StatsController m_StatsController;
        private DineroController m_DineroController;

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

        private HealthController m_HealthController;

        private bool _inventarioAbierto;

        private bool m_Jugador1;
        public bool Jugador1 => m_Jugador1;

        private void Awake()
        {
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
            m_HealthController = GetComponent<HealthController>();
            m_StatsController = GetComponent<StatsController>();
            m_BackPackController = GetComponent<BackPackController>();
            m_DineroController = GetComponent<DineroController>();
            m_HealthController.onCambioDeVida += CambiarVida;
        }

        public void Init(bool _Jugador1)
        {
            _inventarioAbierto = false;
            m_Jugador1 = _Jugador1;
            m_HealthController.SetVida(100);
        }

        // Update is called once per frame
        void Update()
        {
            Vector2 velocidad = Vector2.zero;

            if (m_Jugador1)
            {
                if (Input.GetKeyDown(KeyCode.I))
                {
                    showInventarioEvent.Raise();
                    _inventarioAbierto = !_inventarioAbierto;
                }

                if (Input.GetKeyDown(KeyCode.Escape))
                    pausaEvento.Raise();

                if (Input.GetKeyDown(KeyCode.M))
                    mapaEvento.Raise();

                if (!_inventarioAbierto)
                {
                    if (Input.GetKey(KeyCode.A))
                        velocidad += Vector2.right * -1;

                    if (Input.GetKey(KeyCode.D))
                        velocidad += Vector2.right;

                    if (Input.GetKey(KeyCode.W))
                        velocidad += Vector2.up;

                    if (Input.GetKey(KeyCode.S))
                        velocidad += Vector2.up * -1;
                }
            }

            if (!m_Jugador1)
            {
                if (Input.GetKeyDown(KeyCode.O))
                {
                    showInventarioEvent.Raise();
                    _inventarioAbierto = true;
                }

                if (Input.GetKeyDown(KeyCode.P))
                    pausaEvento.Raise();

                if (!_inventarioAbierto)
                {
                    if (Input.GetKey(KeyCode.LeftArrow))
                        velocidad += Vector2.right * -1;

                    if (Input.GetKey(KeyCode.RightArrow))
                        velocidad += Vector2.right;

                    if (Input.GetKey(KeyCode.UpArrow))
                        velocidad += Vector2.up;

                    if (Input.GetKey(KeyCode.DownArrow))
                        velocidad += Vector2.up * -1;
                }
            }


            m_Rigidbody2D.velocity = velocidad.normalized * m_StatsController.SPEED;
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
                m_DineroController.SetDinero(1);
                m_GameEventInt.Raise(m_DineroController.DINERO);
            }
        }

        public void CambiarDinero(int _Dinero)
        {
            m_DineroController.SetDinero(_Dinero);
            m_GameEventInt.Raise(m_DineroController.DINERO);
        }

        public void CambiarVida()
        {
            print("Cambio de vida");
            vidaEvent.Raise(m_HealthController.HP);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Vendedor"))
            {
                tiendaInventario.Raise(false);
            }
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
            m_DineroController.SetDinero(playerData.dinero);
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
        }
    }
}
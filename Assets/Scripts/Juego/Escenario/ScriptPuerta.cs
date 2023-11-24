using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IsaacBongos
{
    public class ScriptPuerta : MonoBehaviour
    {
        private TipoSala m_TipoSala;
        [SerializeField]
        private GameEventInt numeroSalaEvent;
        private int numeroPlayers;
        private int numeroDePuerta;

        private void Start()
        {
            m_TipoSala = transform.parent.GetComponentInParent<TipoSala>();
            m_TipoSala.cambioPuerta += TryOpenDoor;
            TryOpenDoor(m_TipoSala.CanOpenDoor);
        }

        public void Init(int _NumeroDePuerta)
        {
            numeroPlayers = 0;
            numeroDePuerta = _NumeroDePuerta;
        }

        private void TryOpenDoor(bool canOpenDoor)
        {
            if (canOpenDoor)
                transform.GetChild(0).gameObject.SetActive(false);
            else
                transform.GetChild(0).gameObject.SetActive(true);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                numeroPlayers++;
                if (numeroPlayers == 2)
                    numeroSalaEvent.Raise(numeroDePuerta);
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
                numeroPlayers--;
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace IsaacBongos
{
    public class SalaEnemigos : TipoSala, ISaveableSalaData
    {

        private Pool m_pool;
        public Pool Pool
        {
            get { return m_pool; }
            set { m_pool = value; }
        }
        [SerializeField]
        private List<GameObject> m_enemigos;
        private bool todosLosEnemigosMuertos;
        List<enemyPosition> listaEnemigosEnSala = new List<enemyPosition>();
        public Action OnPLayerInSala;
        protected override void SpawnerSala()
        {
            for (int i = 0; i < 4; i++)
            {
                int random = UnityEngine.Random.Range(0, 2);
                GameObject bicho = Instantiate(m_enemigos[random]);
                listaEnemigosEnSala.Add(new enemyPosition(bicho, random));
                bicho.transform.parent = transform;
                bicho.transform.position = GetPosicionSPawnear();
                if (random == 0)
                {
                    bicho.GetComponent<BombaKaboom>().Pool = m_pool;
                    bicho.GetComponent<BombaStateMachine>().muerteEnemigo += sumarMarcador;
                    bicho.GetComponent<BombaStateMachine>().desuscribirEnemigo -= sumarMarcador;

                }
                else
                {
                    bicho.GetComponent<CargaStateMachine>().muerteEnemigo += sumarMarcador;
                    bicho.GetComponent<CargaStateMachine>().desuscribirEnemigo -= sumarMarcador;
                }
            }
        }

        private void sumarMarcador()
        {
            StartCoroutine(MirarEnemigos());
        }

        private IEnumerator MirarEnemigos()
        {
            yield return new WaitForSeconds(0.1f);
            List<enemyPosition> listaEnemigosEnSalaAhora = new List<enemyPosition>();
            foreach (enemyPosition enemyPos in listaEnemigosEnSala)
            {
                if (enemyPos.enemie != null)
                    listaEnemigosEnSalaAhora.Add(enemyPos);

            }
            listaEnemigosEnSala = listaEnemigosEnSalaAhora;

            if (listaEnemigosEnSala.Count == 0)
            {
                cambioPuerta?.Invoke(true);
                todosLosEnemigosMuertos = true;
            }
        }

        public void Init()
        {
            m_CanOpenDoor = true;
            todosLosEnemigosMuertos = false;
            SpawnerSala();
        }
        public void Init(SaveGame data)
        {

            foreach (SaveGame.SalasData salita in data.m_Enemies)
            {
                if (salita.transformSala == transform.position)
                    spawnEnemies(salita.objects);
            }
        }

        private void spawnEnemies(SaveGame.spawnedInfo[] objects)
        {
            int numEnemies = 0;
            foreach (SaveGame.spawnedInfo enemigoPosition in objects)
            {
                numEnemies++;
                GameObject bicho = Instantiate(m_enemigos[enemigoPosition.numberOfSpawn]);
                listaEnemigosEnSala.Add(new enemyPosition(bicho, enemigoPosition.numberOfSpawn));
                bicho.transform.parent = transform;
                bicho.transform.position = enemigoPosition.transformSpawned;
                if (enemigoPosition.numberOfSpawn == 0)
                {
                    bicho.GetComponent<BombaKaboom>().Pool = m_pool;
                    bicho.GetComponent<BombaStateMachine>().muerteEnemigo += sumarMarcador;
                    bicho.GetComponent<BombaStateMachine>().desuscribirEnemigo -= sumarMarcador;

                }
                else
                {
                    bicho.GetComponent<CargaStateMachine>().muerteEnemigo += sumarMarcador;
                    bicho.GetComponent<CargaStateMachine>().desuscribirEnemigo -= sumarMarcador;
                }
            }
            if (numEnemies == 0)
            {
                m_CanOpenDoor = true;
                todosLosEnemigosMuertos = true;
            }
            else
            {
                m_CanOpenDoor = true;
                todosLosEnemigosMuertos = false;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.CompareTag("Player"))
                return;
            else
            {
                if (todosLosEnemigosMuertos)
                    return;
                else
                {
                    m_CanOpenDoor = false;
                    cambioPuerta?.Invoke(false);
                    OnPLayerInSala?.Invoke();
                }

            }
        }

        public SaveGame.SalasData Save()
        {
            SaveGame.SalasData listaEnemigos = new SaveGame.SalasData();
            listaEnemigos.transformSala = transform.position;

            SaveGame.spawnedInfo[] spawnedInfo = new SaveGame.spawnedInfo[listaEnemigosEnSala.Count];
            for (int i = 0; i < listaEnemigosEnSala.Count; i++)
                spawnedInfo[i] = new SaveGame.spawnedInfo(listaEnemigosEnSala[i].enemie.transform.position, listaEnemigosEnSala[i].numberOfEnemie);

            listaEnemigos.objects = spawnedInfo;

            return listaEnemigos;
        }

        public void Load(SaveGame.SalasData _salaData)
        {
            throw new NotImplementedException();
        }
    }
    [Serializable]
    public struct enemyPosition
    {
        public GameObject enemie;
        public int numberOfEnemie;

        public enemyPosition(GameObject _enemie, int _numberOfEnemie)
        {
            enemie = _enemie;
            numberOfEnemie = _numberOfEnemie;
        }
    }

}
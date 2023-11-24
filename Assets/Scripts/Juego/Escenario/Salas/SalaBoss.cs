using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IsaacBongos
{
    public class SalaBoss : TipoSala, ISaveableSalaData
    {
        [SerializeField]
        private GameObject m_JefeFinal;
        private Pool m_pool;
        private List<GameObject> m_enemigos;
        List<enemyPosition> listaEnemigosEnSala = new List<enemyPosition>();
        public Pool Pool
        {
            get { return m_pool; }
            set { m_pool = value; }
        }
        public Action OnPLayerInSala;
        protected override void SpawnerSala()
        {
            GameObject jefe = Instantiate(m_JefeFinal, transform.position, Quaternion.identity);
            jefe.transform.parent = transform;
            jefe.GetComponent<BombaKaboom>().Pool = m_pool;
        }

        // Start is called before the first frame update
        public void Init()
        {
            m_CanOpenDoor = true;
            SpawnerSala();
        }
       
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.CompareTag("Player"))
            {
                return;
            }
            else
            {

                m_CanOpenDoor = false;
                cambioPuerta?.Invoke(false);
                OnPLayerInSala?.Invoke();

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
    }
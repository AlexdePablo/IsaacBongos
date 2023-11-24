using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IsaacBongos
{
    public class SalaLoot : TipoSala, ISaveableSalaData
    {
        [SerializeField]
        private List<GameObject> m_Objetos;
        List<objectPosition> listaObjetosEnSala = new List<objectPosition>();

      

        protected override void SpawnerSala()
        {
            for (int i = 0; i < 4; i++)
            {
                int random = UnityEngine.Random.Range(0, m_Objetos.Count);
                GameObject objetos = Instantiate(m_Objetos[random]);
                objetos.transform.parent = transform;
                objetos.transform.position = GetPosicionSPawnear();
                listaObjetosEnSala.Add(new objectPosition(objetos, random));
                objetos.GetComponent<ItemScript>().objetoAgarrado += ObjetoAgarrado;
                objetos.GetComponent<ItemScript>().desuscribirObjeto -= ObjetoAgarrado;

                //objetos.GetComponent<PruebaEnemigooooSiiiiiiiiiiiiiiii>().muerteEnemigo += sumarMarcador;
            }
        }

        private void ObjetoAgarrado()
        {
            StartCoroutine(MirarObjetos());
        }
        private IEnumerator MirarObjetos()
        {
            yield return new WaitForSeconds(0.1f);
            List<objectPosition> listaEnemigosEnSalaAhora = new List<objectPosition>();
            foreach (objectPosition objectPos in listaObjetosEnSala)
            {
                if (objectPos.m_Object != null)
                    listaEnemigosEnSalaAhora.Add(objectPos);

            }
            listaObjetosEnSala = listaEnemigosEnSalaAhora;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        public void Init()
        {
            m_CanOpenDoor = true;
            SpawnerSala();
        }

        public void Load(SaveGame.SalasData _salaData)
        {
            m_CanOpenDoor=true;
            foreach (SaveGame.spawnedInfo enemigoPosition in _salaData.objects)
            {
                GameObject bicho = Instantiate(m_Objetos[enemigoPosition.numberOfSpawn]);
                listaObjetosEnSala.Add(new objectPosition(bicho, enemigoPosition.numberOfSpawn));
                bicho.transform.parent = transform;
                bicho.transform.position = enemigoPosition.transformSpawned;
                bicho.GetComponent<ItemScript>().objetoAgarrado += ObjetoAgarrado;
                bicho.GetComponent<ItemScript>().desuscribirObjeto -= ObjetoAgarrado;
            }
        }

        public SaveGame.SalasData Save()
        {
            SaveGame.SalasData listaObjetos = new SaveGame.SalasData();
            listaObjetos.transformSala = transform.position;

            SaveGame.spawnedInfo[] spawnedInfo = new SaveGame.spawnedInfo[listaObjetosEnSala.Count];
            for (int i = 0; i < listaObjetosEnSala.Count; i++)
            {
                spawnedInfo[i] = new SaveGame.spawnedInfo(listaObjetosEnSala[i].m_Object.transform.position, listaObjetosEnSala[i].numberOfObject);
            }

            listaObjetos.objects = spawnedInfo;

            return listaObjetos;
        }
    }
    [Serializable]
    public struct objectPosition
    {
        public GameObject m_Object;
        public int numberOfObject;

        public objectPosition(GameObject _enemie, int _numberOfObject)
        {
            m_Object = _enemie;
            numberOfObject = _numberOfObject;
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IsaacBongos
{
    public class GUIScript : MonoBehaviour
    {
        [SerializeField]
        private GameObject mapa;
        [SerializeField]
        private GameObject inventarioJ1;
        [SerializeField]
        private GameObject tiendaJ1;
        [SerializeField]
        private GameObject inventarioJ2;
        [SerializeField]
        private GameObject tiendaJ2;
        [SerializeField]
        private GameObject pausa;
        private GameManager m_GameManager;

        [SerializeField]
        private TextMeshProUGUI DineroJ1;
        [SerializeField]
        private TextMeshProUGUI DineroJ2;
        [SerializeField]
        private Slider VidaJ1;
        [SerializeField]
        private Slider VidaJ2;




        private void Awake()
        {
            m_GameManager = GameManager.Instance;
        }



        public void ShowPause()
        {
            pausa.SetActive(!pausa.activeSelf);
        }

        public void ShowInventarioJ1()
        {
            inventarioJ1.SetActive(!inventarioJ1.activeSelf);
        }
        public void ShowShopJ1(bool obj)
        {
            tiendaJ1.SetActive(obj);
        }

        public void ShowInventarioJ2()
        {
            inventarioJ2.SetActive(!inventarioJ2.activeSelf);
        }
        public void ShowShopJ2(bool obj)
        {
            tiendaJ2.SetActive(obj);
        }

        public void SetDineroJ1(int _dinero)
        {
            DineroJ1.text = _dinero + "";
        }

        public void SetDineroJ2(int _dinero)
        {
            DineroJ2.text = _dinero + "";
        }

        public void ShowMap()
        {
            mapa.SetActive(!mapa.activeSelf);
        }

        public void CambiarVidaJ1(float _vida)
        {
            VidaJ1.value = _vida;
        }
        public void CambiarVidaJ2(float _vida)
        {
            VidaJ2.value = _vida;
        }

        public void GuardarPartida()
        {
            m_GameManager.GuardarJuego();
        }

        // Update is called once per frame
        void Update()
        {

        }
        private void OnDestroy()
        {

        }
    }
}
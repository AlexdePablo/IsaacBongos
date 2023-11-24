using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IsaacBongos
{
    public class DisplayShop : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_DisplayShopParent;

        [SerializeField]
        private GameObject m_DisplayItemPrefab;

        [SerializeField]
        private Item[] m_ItemsToDisplay;

        private List<GameObject> Botones = new List<GameObject>();

        private void Start()
        {
            foreach (Item item in m_ItemsToDisplay)
            {
                GameObject displayedItem = Instantiate(m_DisplayItemPrefab, m_DisplayShopParent.transform);
                displayedItem.GetComponent<DisplayItem>().Load(item);
                Botones.Add(displayedItem);
            }
        }

        public void UpdatePrecioTienda(int _Dinero)
        {
            for(int i = 0; i < m_ItemsToDisplay.Length; i++)
            {
                Botones[i].GetComponent<DisplayItem>().UpdatearActive(_Dinero, m_ItemsToDisplay[i]);
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IsaacBongos
{
    public class DisplayBackpack : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_DisplayBackpackParent;

        [SerializeField]
        private GameObject m_DisplayItemPrefab;

        [SerializeField]
        private Backpack m_Backpack;

        private void Start()
        {
            //FillDisplay();
        }

        private void ClearDisplay()
        {
            foreach (Transform child in m_DisplayBackpackParent.transform)
                Destroy(child.gameObject);
        }

        private void FillDisplay()
        {
            foreach (Backpack.ItemSlot itemSlot in m_Backpack.ItemSlots)
            {
                GameObject displayedItem = Instantiate(m_DisplayItemPrefab, m_DisplayBackpackParent.transform);
                displayedItem.GetComponent<DisplayItem>().Load(itemSlot);
            }
        }

        public void RefreshBackpack()
        {
            ClearDisplay();
            FillDisplay();
        }
    }
}
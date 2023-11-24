using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

namespace IsaacBongos
{
    public class DisplayItem : MonoBehaviour
    {
        [Header("Functionality")]
        [SerializeField]
        private GameEventItem m_Event;
        [SerializeField]
        private GameEventInt m_ItemComprado;

        [Header("Display")]
        [SerializeField]
        private TextMeshProUGUI m_IDText;
        [SerializeField]
        private TextMeshProUGUI m_AmountText;
        [SerializeField]
        private Image m_Image;
        [SerializeField]
        private TextMeshProUGUI m_Preu; 

        public void Load(Item item)
        {
            m_IDText.text = item.Id;
            m_Image.sprite = item.Sprite;
            if(m_Preu != null)
                m_Preu.text = item.Preu.ToString();
            GetComponent<Button>().onClick.RemoveAllListeners();
            GetComponent<Button>().onClick.AddListener(() => RaiseEvent(item));
        }

        public void Load(Backpack.ItemSlot itemSlot)
        {
            Load(itemSlot.Item);
            m_AmountText.text = itemSlot.Amount.ToString();
        }

        public void UpdatearActive(int _Dinero, Item item)
        {
            if (_Dinero >= item.Preu)
                GetComponent<Button>().interactable = true;
            else
                GetComponent<Button>().interactable = false;
        }

        private void RaiseEvent(Item item)
        {
            if (m_Preu != null)
            {
                m_ItemComprado.Raise(item.Preu * -1);
            }

            m_Event.Raise(item);

        }
    }
}
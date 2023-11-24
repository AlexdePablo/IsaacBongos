using IsaacBongos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IsaacBongos
{
    public class BackPackController : MonoBehaviour
    {
        [SerializeField]
        private GameEvent m_GUIEvent;

        [SerializeField]
        private Backpack m_Backpack;

        [SerializeField]
        private ItemsDataBase m_ItemDatabase;

        public void ConsumeItem(Item item)
        {
            if (!item.UsedBy(gameObject))
                return;
            m_Backpack.RemoveItem(item);
            m_GUIEvent.Raise();
        }

        public void AddItem(Item item)
        {
            m_Backpack.AddItem(item);
            m_GUIEvent.Raise();
        }
        public List<SaveGame.ItemSlotInfo> getBackPack()
        {
           List<SaveGame.ItemSlotInfo> lista = new List<SaveGame.ItemSlotInfo>();
            for(int i = 0; i < m_Backpack.ItemSlots.Count; i++)
                lista.Add(new SaveGame.ItemSlotInfo(m_Backpack.ItemSlots[i].Item.Id, m_Backpack.ItemSlots[i].Amount));

            return lista;
        }
        public void ClearBackpack()
        {
            m_Backpack.ClearList();
        }

        public void RemakeBackPack(SaveGame.ItemSlotInfo[] slots)
        {
            foreach (SaveGame.ItemSlotInfo slot in slots)
                m_Backpack.AddItem(m_ItemDatabase.GetItemByID(slot.item), slot.amount);

            m_GUIEvent.Raise();
        }
    }
}
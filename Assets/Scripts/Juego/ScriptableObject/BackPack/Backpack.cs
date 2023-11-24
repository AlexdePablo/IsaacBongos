using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace IsaacBongos
{
    [CreateAssetMenu(fileName = "Backpack", menuName = "Inventory/Backpack")]
    public class Backpack : ScriptableObject
    {
        [Serializable]
        public class ItemSlot
        {
            [SerializeField]
            public Item Item;
            [SerializeField]
            public int Amount;

            public ItemSlot(Item obj)
            {
                Item = obj;
                Amount = 1;
            }

            public ItemSlot(Item obj, int amount)
            {
                Item = obj;
                Amount = amount;
            }
        }

        [SerializeField]
        private List<ItemSlot> m_ItemSlots = new List<ItemSlot>();
        public ReadOnlyCollection<ItemSlot> ItemSlots => new ReadOnlyCollection<ItemSlot>(m_ItemSlots);

        public void AddItem(Item usedItem)
        {
            ItemSlot item = GetItem(usedItem);
            if (item == null)
                m_ItemSlots.Add(new ItemSlot(usedItem));
            else
                item.Amount++;
        }
        public void AddItem(Item usedItem, int amount)
        {
                m_ItemSlots.Add(new ItemSlot(usedItem, amount));
        }

        public void RemoveItem(Item usedItem)
        {
            ItemSlot item = GetItem(usedItem);
            if (item == null)
                return;

            item.Amount--;
            if (item.Amount <= 0)
                m_ItemSlots.Remove(item);
        }
        private ItemSlot GetItem(Item item)
        {
            return m_ItemSlots.FirstOrDefault(slot => slot.Item == item);
        }

        public void ClearList()
        {
            m_ItemSlots.Clear();
        }
    }
}
using IsaacBongos;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

namespace IsaacBongos
{
    [CreateAssetMenu(fileName = "DatabaseItems", menuName = "Inventory/Database")]
    public class ItemsDataBase : ScriptableObject
    {
        [SerializeField]
        List<Item> items = new List<Item>();

        public Item GetItemByID(string id) => items.FirstOrDefault<Item>(item => item.Id == id);
    }
}
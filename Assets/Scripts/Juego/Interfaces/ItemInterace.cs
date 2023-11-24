using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IsaacBongos
{
    public interface IItem
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public Sprite Sprite { get; set; }

        public bool UsedBy(GameObject go);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
    public class ItemInfo {
        public int Id { get; private set; }

        public int TypeId { get; private set; }
        
        public Vector2 position { get; set; }

        public ItemInfo(int id, int typeId)
        {
            this.Id = id;
            this.TypeId = typeId;
        }
        
        
        public bool IsSelected { get; private set; }
        
        public bool IsRemoved { get; set; }

        public void Select()
        {
            this.IsSelected = !IsSelected;
            
            EventCenter.Instance.Broad(Actions.SelectItem, Id);
        }

        public void Remove()
        {
            this.IsRemoved = true;
            EventCenter.Instance.Broad(Actions.RemoveItem, Id);
        }
    }
}

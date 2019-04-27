using System.Collections;
using System.Collections.Generic;
using GameLogic;
using UnityEngine;
using UnityEngine.UI;

namespace GameUI
{
    public class ItemUnit : MonoBehaviour {
        
        private SpriteRenderer bg;

        private SpriteRenderer Icon;

        public ItemInfo item;

        public bool isHint;

        // Use this for initialization
        void Start ()
        {

        }
	
        // Update is called once per frame
        void Update () {
		
        }

        public void SetItemInfo(ItemInfo item)
        {
            bg = this.GetComponent<SpriteRenderer>();
            Icon = this.transform.Find("icon").GetComponent<SpriteRenderer>();
            this.item = item;
            Refresh();
        }
        
        public void Refresh()
        {
            
            bg.sprite = item.IsSelected? Resources.Load<Sprite>("Sprites/41"):Resources.Load<Sprite>("Sprites/40");
            string iconstring = "Sprites/Icon/" + item.TypeId;
            var selectedicon = Resources.Load<Sprite>(iconstring + "a");
            if (item.IsSelected &&  selectedicon != null)
            {
                Icon.sprite = Resources.Load<Sprite>("Sprites/Icon/" + item.TypeId);
                
            }
            else if (selectedicon != null)
            {
                Icon.sprite = selectedicon;
            }
            else
            {
                Icon.sprite = Resources.Load<Sprite>("Sprites/Icon/" + item.TypeId);
            }
        }

        public void Reset()
        {
            isHint = false;
            bg.sprite = Resources.Load<Sprite>("Sprites/40");
        }

        public void Hint()
        {
            isHint = true;
            bg.sprite = Resources.Load<Sprite>("Sprites/42");
        }

        public void Remove()
        {
            this.GetComponent<BoxCollider2D>().enabled = false;
            this.GetComponent<Animator>().enabled = true;
            //this.gameObject.SetActive(false);
        }
        
        public void HandleOnClick()
        {
            PlayRoomManager.Instance.SelectItem(this.item.Id);
        }
    }

}

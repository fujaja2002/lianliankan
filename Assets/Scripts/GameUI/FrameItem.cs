using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using GameLogic;

namespace GameUI
{
    public class FrameItem : MonoBehaviour,IPointerClickHandler
    {
        private Text testLabel;

        private Image bg;

        private Image Icon;

        public ItemInfo item;


        // Use this for initialization
        void Start ()
        {

        }

        public void Init()
        {
            bg = this.GetComponent<Image>();
            testLabel = transform.Find("Text").GetComponent<Text>();
            Icon = transform.Find("Item").GetComponent<Image>();
            testLabel.gameObject.SetActive(false);
        }

        public void SetItemInfo(ItemInfo item)
        {
            Init();
            this.item = item;
            Refresh();
        }

        public void Refresh()
        {
            
            bg.overrideSprite = item.IsSelected? Resources.Load<Sprite>("Sprites/41"):Resources.Load<Sprite>("Sprites/40");
            string iconstring = "Sprites/Icon/" + item.TypeId;
            var selectedicon = Resources.Load<Sprite>(iconstring + "a");
            if (item.IsSelected &&  selectedicon != null)
            {
                Icon.overrideSprite = Resources.Load<Sprite>("Sprites/Icon/" + item.TypeId);
                
            }
            else if (selectedicon != null)
            {
                Icon.overrideSprite = selectedicon;
            }
            else
            {
                Icon.overrideSprite = Resources.Load<Sprite>("Sprites/Icon/" + item.TypeId);
            }

            Icon.SetNativeSize();
        }
        
        public void Remove()
        {
            
           this.gameObject.SetActive(false);
        }
	
        // Update is called once per frame
        void Update () {
		
        }

        public void OnPointerClick(PointerEventData eventData)
        {
           HandleOnClick();
        }

        void HandleOnClick()
        {
            PlayRoomManager.Instance.SelectItem(this.item.Id);
        }
    }

}

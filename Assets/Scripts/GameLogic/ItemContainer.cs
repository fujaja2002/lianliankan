﻿using System.Collections;
using System.Collections.Generic;
using GameLogic;
using UnityEngine;
using UnityEngine.UI;

namespace GameUI
{
    public class ItemContainer : MonoBehaviour {

	[SerializeField] private GameObject item;

        [SerializeField] private Transform anchor;

		[SerializeField] private Transform engGameBg;

        private Vector2 _initPosition = Vector2.zero;
        
        private List<ItemUnit> allItems = new List<ItemUnit>();

        // Use this for initialization
        void Start()
        {
            InitEvents();
            _initPosition = Camera.main.ScreenToWorldPoint(new Vector3(100, Screen.height-100, 0));
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private void OnGUI()
        {
            if (GUI.Button(new Rect(0, 0, 100, 30), "刷新"))
            {
                Debug.Log("刷新");
                PlayRoomManager.Instance.Reset();
            }

			if (GUI.Button(new Rect(110,0,100,30), "重来")){
				Debug.Log("重来");
				PlayRoomManager.Instance.ReStartGame();
			}
			
			var startPosition = Screen.width-100;

			if (GUI.Button(new Rect(startPosition,0,100,30), "退出游戏")){
				Debug.Log("退出");
				Application.Quit();
			}
        }

        public void Initialize(object _)
        {
            CreateItems();
        }

        public void RefreshItems(object _)
        {
            DestoryItems();
            CreateItems();

        }

		public void EndGame(object _)
		{
			engGameBg.gameObject.SetActive(PlayRoomManager.Instance.isGameEnd);
		}

        private void CreateItems()
        {
            var allData = PlayRoomManager.Instance.CurrentItems;

            for (int i = 0; i < allData.Count; i++)
            {
                var go = Instantiate(item) as GameObject;
                go.transform.parent = this.transform;
                go.transform.localScale = Vector3.one;
                var id = i;
                var info = allData[id];
                var a = info.position;
                go.transform.position = _initPosition + new Vector2(a.x * 0.9f, -a.y * 1.1f);
                go.GetComponent<ItemUnit>().SetItemInfo(info);
                
                allItems.Add(go.GetComponent<ItemUnit>());
            }
			engGameBg.gameObject.SetActive(PlayRoomManager.Instance.isGameEnd);
        }

        private void DestoryItems()
        {
            for (int i = 0; i < allItems.Count; i++)
            {
                Destroy(allItems[i].gameObject);
            }
            allItems.Clear();
        }

        void InitEvents()
        {
            EventCenter.Instance.On(Actions.InitGame, Initialize);
            EventCenter.Instance.On(Actions.RefreshItem, RefreshItems);
			EventCenter.Instance.On(Actions.ReStartGame, RefreshItems);
            EventCenter.Instance.On(Actions.SelectItem, HandleSelect);
            EventCenter.Instance.On(Actions.RemoveItem, HandleRemove);
			EventCenter.Instance.On(Actions.EndGame, EndGame);
        }

        public void HandleSelect(object id)
        {
            var a = (int) id;
            allItems.Find((t=>t.item.Id == a)).Refresh();
        }

        public void HandleRemove(object id)
        {
            var a = (int) id;
            allItems.Find(t=>t.item.Id == a).Remove();
        }
}

}
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameLogic;
using UnityEngine;
using UnityEngine.UI;

namespace GameUI
{
    public class ItemContainer : MonoBehaviour
    {
        [SerializeField] private GameObject item;

        [SerializeField] private Transform engGameBg;

        private const float xdistance = 0.9f;
        private const float ydistance = 1.1f;

        private LineDrawer _line;
        
        private Vector2 _initPosition = Vector2.zero;

        private List<ItemUnit> allItems = new List<ItemUnit>();

        #region 临时数据
        private HintIds tempHint;
        #endregion

        // Use this for initialization
        public void Init()
        {
            InitEvents();
            var lenth = (Config.width - 1) * xdistance;
            var height = (Config.height +1 ) * ydistance;
            _initPosition = new Vector2(-lenth/2, height/2);
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

            if (GUI.Button(new Rect(110, 0, 100, 30), "重来"))
            {
                Debug.Log("重来");
                PlayRoomManager.Instance.ReStartGame();
            }
            
            if (GUI.Button(new Rect(220, 0, 160, 30), "提示(数字3)"))
            {
                Debug.Log("提示");
                PlayRoomManager.Instance.Hint();
            }

            var startPosition = Screen.width - 100;

            if (GUI.Button(new Rect(startPosition, 0, 100, 30), "退出游戏"))
            {
                Debug.Log("退出");
                Application.Quit();
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                Debug.Log("提示");
                PlayRoomManager.Instance.Hint();
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

        public void HintItems(object ids)
        {
            ResetHint();
            var rst = ids as HintIds;
            tempHint = rst;
            allItems.Find(t=>t.item.Id == rst.id1).Hint();
            allItems.Find(t=>t.item.Id ==rst.id2).Hint();   
        }

        private void  ResetHint()
        {
            if (tempHint != null)
            {
                allItems.Find(t => t.item.Id == tempHint.id1).Reset();
                allItems.Find(t => t.item.Id == tempHint.id2).Reset();
                tempHint = null;
            }
        }

        public void EndGame(object _)
        {
            engGameBg.gameObject.SetActive(PlayRoomManager.Instance.isGameEnd);
        }

        private void CreateItems()
        {
            tempHint = null;
            var allData = PlayRoomManager.Instance.CurrentItems;

            for (int i = 0; i < allData.Count; i++)
            {
                var go = Instantiate(item) as GameObject;
                go.transform.parent = this.transform;
                go.transform.localScale = Vector3.one;
                var id = i;
                var info = allData[id];
                var a = info.position;
                go.transform.position = _initPosition + new Vector2(a.x * xdistance, -a.y * ydistance);
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
            EventCenter.Instance.On(Actions.Hint,HintItems);
            EventCenter.Instance.On(Actions.ShowLine, ShowLine);
        }

        public void ShowLine(object line)
        {
            List<Vector2> rst = line as List<Vector2>;
            if (_line == null) 
            {
                var a = Instantiate(Resources.Load("line")) as GameObject;
                _line = a.GetComponent<LineDrawer>();
            }
            
            _line.SetPoint(rst.Select(o=>new Vector3(_initPosition.x +o.x*xdistance,_initPosition.y-o.y*ydistance,0)).ToList());   
        }

        public void HandleSelect(object id)
        {
            ResetHint();
            var a = (int) id;
            allItems.Find((t => t.item.Id == a)).Refresh();
        }

        public void HandleRemove(object id)
        {
            ResetHint();
            var a = (int) id;
            allItems.Find(t => t.item.Id == a).Remove();
        }
    }
}
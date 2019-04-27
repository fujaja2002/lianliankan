using System.Collections;
using System.Collections.Generic;
using System.Linq;
using fjj.core;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameLogic{
    public class PlayRoomManager:Singleton<PlayRoomManager>
    {
        
        public Dictionary<int, ItemInfo> AllItemInfo = new Dictionary<int, ItemInfo>(Config.InitGrids);
        public List<ItemInfo> CurrentItems = new List<ItemInfo>();
        
        public List<Vector2> allPosition = new List<Vector2>();
        
        public List<Vector2> currentPositions = new List<Vector2>();
        
        public Dictionary<Vector2, ItemInfo> positionItemDic = new Dictionary<Vector2, ItemInfo>();


        
        private ItemInfo SelectedOne;

		public bool isGameEnd{get;private set;}

        public void Initialize()
        {
            var alltype = Utils.GetInitType();
            for (int i = 0; i < alltype.Count; i++)
            {
                var item = new ItemInfo(i+1, alltype[i]);
                AllItemInfo.Add(item.Id, item);
                CurrentItems.Add(item);
            }
            //初始化所有位置
            for (int i = 0; i < Config.width; i++)
            {
                for (int j = 0; j < Config.height; j++)
                {
                    var rst = new Vector2(i,j);
                    allPosition.Add(rst);
                    currentPositions.Add(rst);
                }
            }

             
            //初始化数据

            for (int i = 0; i < AllItemInfo.Count; i++)
            {
                AllItemInfo[i + 1].position = allPosition[i];
                positionItemDic.Add(allPosition[i], AllItemInfo[i+1]);
            }
            
            EventCenter.Instance.Broad(Actions.InitGame, null);
        }

        public void Reset()
        {
            if (SelectedOne != null)
            {
                SelectedOne.Select();
                SelectedOne = null;
            }

            CurrentItems = CurrentItems.FindAll(t => !t.IsRemoved);
            currentPositions = CurrentItems.Select(t => t.position).ToList();
            var rst = Utils.RandomSort(CurrentItems);
            CurrentItems.Clear();
            foreach (var item in rst)
            {
                CurrentItems.Add(item);
            }
            positionItemDic.Clear();
            for (int i = 0; i < CurrentItems.Count; i++)
            {
                CurrentItems[i].position = currentPositions[i];
                positionItemDic.Add(CurrentItems[i].position, CurrentItems[i]);
            }

            EventCenter.Instance.Broad(Actions.RefreshItem, null);
        }

		public void ReStartGame()
		{
			if (SelectedOne != null)
			{
				SelectedOne.Select();
				SelectedOne = null;
			}

            availableId1 = availableId2 = 0;
			AllItemInfo.Clear();
			CurrentItems.Clear();
			allPosition.Clear();
			currentPositions.Clear();
			positionItemDic.Clear();

			var alltype = Utils.GetInitType();
			for (int i = 0; i < alltype.Count; i++)
			{
				var item = new ItemInfo(i+1, alltype[i]);
				AllItemInfo.Add(item.Id, item);
				CurrentItems.Add(item);
			}
			//初始化所有位置
			for (int i = 0; i < Config.width; i++)
			{
				for (int j = 0; j < Config.height; j++)
				{
					var rst = new Vector2(i,j);
					allPosition.Add(rst);
					currentPositions.Add(rst);
				}
			}


			//初始化数据

			for (int i = 0; i < AllItemInfo.Count; i++)
			{
				AllItemInfo[i + 1].position = allPosition[i];
				positionItemDic.Add(allPosition[i], AllItemInfo[i+1]);
			}

			isGameEnd = false;

			EventCenter.Instance.Broad(Actions.ReStartGame, null);
		}
			

        public void SelectItem(int id)
        {
            if (SelectedOne == null)
            {
                AllItemInfo[id].Select();
                SelectedOne = AllItemInfo[id];
            }
            else
            {
				if (CanRemove(id)) {
					if (CurrentItems.FindAll(t=>!t.IsRemoved).ToList().Count > 0)
                    {
                        Debug.Log("not finish");
                        if (CheckAvailable())
                        {
                            Debug.Log("还可以继续");
                        }
                        else
                        {
                            Debug.Log("没有可以消除的了");
                        }
					}
					else
					{
						this.isGameEnd = true;
						EventCenter.Instance.Broad(Actions.EndGame, null);
					}
				} 
            }


        }


        private bool CanRemove(int id)
        {
            if (SelectedOne != null)
            {
                if (id == SelectedOne.Id)
                {
                    SelectedOne.Select();
                    SelectedOne = null;
                    return false;
                }
                else if (SelectedOne.TypeId == AllItemInfo[id].TypeId)
                {
                    List<Vector2> a = new List<Vector2>();
                    if (JudgeRemove(SelectedOne, AllItemInfo[id], a))
                    {
                        AllItemInfo[id].Remove();
                        SelectedOne.Remove();
                        SelectedOne = null;
                        ShowLine(a);
                        return true;
                    }
                    else
                    {
                        SelectedOne.Select();
                        SelectedOne = AllItemInfo[id];
                        SelectedOne.Select();
                    }

                }
                else
                {
                    SelectedOne.Select();
                    SelectedOne = AllItemInfo[id];
                    SelectedOne.Select();
                }
            }

            return false;
        }

        private bool JudgeRemove(ItemInfo a, ItemInfo b, List<Vector2> line)
        {
            var aPosition = a.position;
            var bPosition = b.position;
            if (aPosition.x.Equals(bPosition.x)&& Mathf.Abs(aPosition.y -bPosition.y).Equals(1))
            {
                line.Add(aPosition);
                line.Add(bPosition);

                return true;
            }
            if (aPosition.y.Equals(bPosition.y)&& Mathf.Abs(aPosition.x -bPosition.x).Equals(1))
            {
                line.Add(aPosition);
                line.Add(bPosition);

                return true;
            }

            if (CheckVertical(aPosition, bPosition, line))
            {
                return true;
            }
            return CheckHorizon(line, aPosition, bPosition);
        }

        private bool CheckHorizon(List<Vector2> line, Vector2 aPosition, Vector2 bPosition)
        {
            for (int starty = -1; starty <= Config.height; starty++)
            {
                bool notIn = true;
                line.Clear();
                for (int i = starty; i != (int) aPosition.y; i = i + GetSetp(starty, (int) aPosition.y))
                {
                    var point = new Vector2(aPosition.x, i);
                    if (CheckPoint(point))
                        continue;
                    else
                    {
                        notIn = false;
                        break;
                    }
                }

                for (int i = starty; i != (int) bPosition.y; i = i + GetSetp(starty, (int) bPosition.y))
                {
                    var pointb = new Vector2(bPosition.x, i);
                    if (CheckPoint(pointb))
                        continue;
                    else
                    {
                        notIn = false;
                        break;
                    }
                }

                for (var i = (int) aPosition.x; i != (int) bPosition.x; i = i + GetSetp((int) aPosition.x, (int) bPosition.x))
                {
                    var point = new Vector2(i, starty);
                    if (CheckPoint(point))
                        continue;
                    else
                    {
                        notIn = false;
                        break;
                    }
                }

                if (notIn)
                {
                    line.Add(aPosition);
                    line.Add(new Vector2(aPosition.x, starty));
                    line.Add(new Vector2(bPosition.x,starty));
                    line.Add(bPosition);
                    return true;
                }
            }

            return false;
        }

        private bool CheckVertical(Vector2 aPosition, Vector2 bPosition, List<Vector2> line)
        {
            for (int startx = -1; startx <= Config.width; startx++)
            {
                bool notIn = true;
                line.Clear();
                for (int i =startx; i != (int) aPosition.x; i = i + GetSetp(startx, (int) aPosition.x))
                {
                    var point = new Vector2(i, aPosition.y);
                    if (CheckPoint(point))
                        continue;
                    else
                    {
                        notIn = false;
                        break;
                    }    
                }

                for (int i =startx ; i != (int) bPosition.x; i = i + GetSetp(startx, (int) bPosition.x))
                {
                    var pointb = new Vector2(i, bPosition.y);
                    if (CheckPoint(pointb))
                        continue;
                    else
                    {
                        notIn = false;
                        break;
                    }
                }

                for (var i = (int)aPosition.y; i != (int)bPosition.y;i=i+ GetSetp((int)aPosition.y, (int)bPosition.y))
                {
                    var point = new Vector2(startx,  i);
                    if (CheckPoint(point))
                        continue;
                    else
                    {
                        notIn = false;
                        break;
                    }
                }

                if (notIn)
                {
                    line.Add(aPosition);
                    line.Add(new Vector2(startx, aPosition.y));
                    line.Add(new Vector2(startx,bPosition.y));
                    line.Add(bPosition);
                    return true;
                }
            }

            return false;
        }

        private int GetSetp(int a, int b)
        {
            var abs = Mathf.Abs(a - b);
            var rst = (b-a) / abs;
            return rst;
        }

        private bool CheckPoint(Vector2 point)
        {
            if (positionItemDic.ContainsKey(point))
            {
                return positionItemDic[point].IsRemoved;
            }

            return true;
        }

        private int availableId1;
        private int availableId2;
        public void Hint()
        {
            if (CheckAvailable())
            {
                HintIds ids = new HintIds()
                {
                    id1 = availableId1,
                    id2 = availableId2,
                };
                EventCenter.Instance.Broad(Actions.Hint, ids);
                Debug.Log(availableId1);
                Debug.Log(availableId2);
            }
        }
        
        public bool CheckAvailable()
        {
            availableId1 = availableId2 = 0;
            var allIds = CurrentItems.Where(t => !t.IsRemoved).Select(t => t.TypeId).Distinct().ToList();
            List<Vector2> line = new List<Vector2>();
            foreach (var item in allIds)
            {
                var allItems = CurrentItems.FindAll(t => t.TypeId == item && !t.IsRemoved);
                if (allItems.Count <=1)
                {
                    break;
                }
                for (int i = 0; i < allItems.Count-1; i++)
                {
                    for (int j = i + 1; j < allItems.Count; j++)
                    {
                        if (JudgeRemove(allItems[i], allItems[j], line))
                        {
                            availableId1 = allItems[i].Id;
                            availableId2 = allItems[j].Id;
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private void ShowLine(List<Vector2> line)
        {
            line.ForEach((a) => Debug.Log(a));
            EventCenter.Instance.Broad(Actions.ShowLine,line);
        }

        private bool CheckLinePoint(List<Vector2> line)
        {
            foreach (var item in line)
            {
                if (positionItemDic.ContainsKey(item))
                {
                    if (!positionItemDic[item].IsRemoved)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        
    }
}

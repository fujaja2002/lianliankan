using System;
using System.Collections;
using System.Collections.Generic;
using GameLogic;
using UnityEngine;
using Random = System.Random;

public class Utils {

    public static List<int> GetInitType()
    {
        var rst = GetCurrentType1();
        List<int> temp = new List<int>();
        foreach (var item in rst)
        {
            temp.Add(item);
        }
        var a = Config.width * Config.height / 2;
        for (int i = 0; i < a-rst.Count; i++)
        {
            var index = UnityEngine.Random.Range(1, rst.Count);
            temp.Add(rst[index]);
        }
        
        var result = new List<int>();
        for (int i = 0; i < temp.Count; i++)
        {
            result.Add(temp[i]);
            result.Add(temp[i]);
        }

        return RandomSort(result);
    }

    private static List<int> GetCurrentType()
    {
        List<int> rst = new List<int>();
        while (rst.Count < Config.GameType)
        {
            var a = UnityEngine.Random.Range(1, Config.MaxType+1);
            if (!rst.Contains(a))
            {
                rst.Add(a);
            }
        }

        return rst;
    }

    private static List<int> GetCurrentType1()
    {
        int[] rst1 = new int[Config.MaxType];
        for (int i = 0; i < rst1.Length; i++)
        {
            rst1[i] = i+1;
        }

        var rst2 = RandomSort(rst1);
        return rst2.GetRange(0, Config.GameType);
    }


    public static List<T> RandomSort<T>(IList<T> list)
    {
        var random = new Random();
		List<T> newList = new List<T>();
        for (int i = list.Count - 1; i >= 1; i--)
        {
            int rand = random.Next(i + 1);
            T temp = list[i];
            list[i] = list[rand];
            list[rand] = temp;
        }

		foreach (var item in list) {
			newList.Add(item);
		}

		return newList;
    }
    
    public static List<T> RandomSort<T>(List<T> list,int num)
    {
        var newList = RandomSort(list);
        var temp = new List<T>();
        int i = 0;
        while (i < num)
        {
            temp.Add(newList[i]);
            i++;
        }
        return temp;
    }
}

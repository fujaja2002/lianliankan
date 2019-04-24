using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using fjj.core;
using GameLogic;
using GameUI;

public class Game : MonoBehaviour {
	
	

	// Use this for initialization
	void Start () {
//		Camera.main.orthographicSize = (1200f / 2)/100f;
		GameObject.Find("ItemContainer").GetComponent<ItemContainer>().Init();
		InitializeModule();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void InitializeModule()
	{
		PlayRoomManager.Instance.Initialize();
	}
}

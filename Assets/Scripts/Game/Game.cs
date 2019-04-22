using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using fjj.core;
using GameLogic;

public class Game : MonoBehaviour {
	
	

	// Use this for initialization
	void Start () {
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

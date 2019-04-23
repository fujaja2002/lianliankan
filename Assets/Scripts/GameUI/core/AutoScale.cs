using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoScale : MonoBehaviour
{

	private float designedHeight = 1080f;
	// Use this for initialization
	void Start () {
		Set();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void Set()
	{
		var screenHeight = Screen.height;
		var b = screenHeight*1f / designedHeight;
		this.GetComponent<RectTransform>().localScale= new Vector3(b,b,b);

	}
}

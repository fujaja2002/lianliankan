using System.Collections;
using System.Collections.Generic;
using GameUI;
using UnityEngine;

public class CameraController : MonoBehaviour {
	

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0))
		{
			Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector2 mousePos2d = new Vector2(mousePos.x, mousePos.y);

			RaycastHit2D hit = Physics2D.Raycast(mousePos2d, Vector2.zero);
			if (hit.collider!= null)
			{
				var unit = hit.transform.GetComponent<ItemUnit>();
				unit.HandleOnClick();
			}

		}
		
	}
}

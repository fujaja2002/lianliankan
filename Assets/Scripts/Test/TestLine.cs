using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLine : MonoBehaviour
{
	public Material mat;
	public Color color = Color.red;

	public Vector3 pos1;

	public Vector3 pos2;

	public bool isRead = false;
	
	
	// Use this for initialization
	void Start ()
	{
		mat.color = color;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0))
		{
			pos1 = Input.mousePosition;
		}

		if (Input.GetMouseButtonUp(0))
		{
			pos2 = Input.mousePosition;
			isRead = true;
		}
	}

	private void OnPostRender()
	{
		if (isRead)
		{
			GL.PushMatrix();
			mat.SetPass(0);
			GL.LoadOrtho();
			GL.Begin(GL.LINES);
			GL.Color(color);
			GL.Vertex3(pos1.x/Screen.width, pos1.y/Screen.height, pos1.z);
			GL.Vertex3(pos2.x/Screen.width, pos2.y/Screen.height,pos2.z);
			GL.End();
			GL.PopMatrix();
		}
	}
}

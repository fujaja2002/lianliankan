using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
	private LineRenderer _lineRenderer;
	
	

	// Use this for initialization
	void Awake ()
	{
		_lineRenderer = this.GetComponent<LineRenderer>();
		_lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
		
		_lineRenderer.startColor = Color.red;
		_lineRenderer.endColor = Color.yellow;

		_lineRenderer.startWidth = 0.02f;
		_lineRenderer.endWidth = 0.02f;
	}

	public void SetPoint(List<Vector3> pointList)
	{
		_lineRenderer.positionCount = pointList.Count;
		for (int i = 0; i < pointList.Count; i++)
		{
			_lineRenderer.SetPosition(i, pointList[i]);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

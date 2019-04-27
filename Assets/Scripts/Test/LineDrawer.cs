using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
	private LineRenderer _lineRenderer;

	private float duration = 0.35f;

	private float showTime;

	private bool isShow;
	
	

	// Use this for initialization
	void Awake ()
	{
		_lineRenderer = this.GetComponent<LineRenderer>();
		
		_lineRenderer.startColor = Color.cyan;
		_lineRenderer.endColor = Color.yellow;

		_lineRenderer.startWidth = 0.05f;
		_lineRenderer.endWidth = 0.05f;
	}

	public void SetPoint(List<Vector3> pointList)
	{
		_lineRenderer.positionCount = pointList.Count;
		for (int i = 0; i < pointList.Count; i++)
		{
			_lineRenderer.SetPosition(i, pointList[i]);
		}
		isShow = true;
		showTime = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (isShow)
		{
			showTime += Time.deltaTime;
			if (showTime >= duration)
			{
				_lineRenderer.positionCount = 0;
				showTime = 0;
				isShow = false;
			}
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLineRender : MonoBehaviour
{

    [SerializeField] private GameObject line;
    private void Start()
    {
        var list = new List<Vector3>()
        {
            new Vector3(-2,-1,0),
            new Vector3(-2,1,0),
            new Vector3(2,1,0),
            new Vector3(2,-1,0)
        };
        var a = Instantiate(line);
        a.GetComponent<LineDrawer>().SetPoint(list);
    }
}

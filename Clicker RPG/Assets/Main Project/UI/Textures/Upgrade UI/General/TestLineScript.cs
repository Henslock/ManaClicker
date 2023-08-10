using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLineScript : MonoBehaviour
{
    void Start()
    {
        LineRenderer lr = gameObject.GetComponent<LineRenderer>();
        Mesh testMesh = new Mesh();
        lr.BakeMesh(testMesh, true);
        gameObject.GetComponent<CanvasRenderer>().SetMesh(testMesh);
    }
}

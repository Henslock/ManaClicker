using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotateScript : MonoBehaviour
{
    void Update()
    {
        gameObject.transform.Rotate(Vector3.up * Time.deltaTime * 6.5f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingLayerExposer : MonoBehaviour
{
    void Start()
    {
        GetComponent<Renderer>().sortingLayerName = "Text";
    }
}

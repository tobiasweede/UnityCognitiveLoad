using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerIdHandler : MonoBehaviour
{
    void Start()
    {
        this.GetComponent<Renderer>().sortingLayerID =
        this.transform.parent.GetComponent<Renderer>().sortingLayerID;
    }
}
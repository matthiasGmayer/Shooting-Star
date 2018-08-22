using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class InactiveSprite : MonoBehaviour
{

    public GameObject parent;
    void Start()
    {
        float y = parent == null ? transform.parent.position.y : parent.transform.position.y;
        GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(y * -Settings.Semi3Dprecision);
    }
}

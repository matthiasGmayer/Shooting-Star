using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class InactiveSprite : MonoBehaviour
{

    void Start()
    {
        GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(transform.parent.position.y * -Settings.Semi3Dprecision);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ActiveSprite : MonoBehaviour
{

    SpriteRenderer spriteRenderer;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        spriteRenderer.sortingOrder = Mathf.RoundToInt(transform.parent.transform.position.y * -Settings.Semi3Dprecision);
    }
}

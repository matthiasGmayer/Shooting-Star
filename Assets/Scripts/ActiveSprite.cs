using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ActiveSprite : MonoBehaviour
{
    [SerializeField]
    bool centerBottom;
    SpriteRenderer spriteRenderer;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (centerBottom)
            transform.position = new Vector3(0, spriteRenderer.sprite.texture.height / 4f / spriteRenderer.sprite.pixelsPerUnit, 0);
    }
    void Update()
    {
        spriteRenderer.sortingOrder = Mathf.RoundToInt(transform.parent.transform.position.y * -Settings.Semi3Dprecision);
    }
}

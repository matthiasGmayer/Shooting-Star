using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ActiveSprite : MonoBehaviour
{
    //[SerializeField]
    //bool centerBottomSprite, centerBottomParent;

    public GameObject parent;

    public int offset = 0;
    SpriteRenderer spriteRenderer;
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        //if (centerBottomSprite)
        //    transform.position = new Vector3(transform.position.x , spriteRenderer.sprite.texture.height / 4f / spriteRenderer.sprite.pixelsPerUnit, 0);
        //if(centerBottomParent)
        //    transform.parent.position += new Vector3(0, spriteRenderer.sprite.texture.height / 4f / spriteRenderer.sprite.pixelsPerUnit, 0);

    }
    void Update()
    {
        if(parent != null)
            spriteRenderer.sortingOrder = Mathf.RoundToInt(parent.transform.position.y * -Settings.Semi3Dprecision) + offset;
    }
}

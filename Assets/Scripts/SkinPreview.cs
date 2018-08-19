using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinPreview : MonoBehaviour {

    private static SkinPreview selected = null;

    Animations.Character character;

    public GameObject image;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(selected == this)
        {
            if (!image.activeSelf) image.SetActive(true);
        }
        else
        {
            if (image.activeSelf) image.SetActive(false);
        }
    }

    public void Init(Animations.Character character)
    {
        this.character = character;
        GetComponentInChildren<Animator>().runtimeAnimatorController = Animations.GetAnimation(character);
        transform.Find("Size").transform.Find("Image").GetComponent<UnityEngine.UI.Image>().sprite = Animations.GetSprite(character);
    }

    public void SetAnimation()
    {
        Animations.selectedAnimation = character;
        selected = this;
    }


}

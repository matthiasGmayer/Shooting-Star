using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinPreview : MonoBehaviour {

    private static SkinPreview selected = null;

    Animations.Character character;

    public GameObject selectImage, characterImage, armImage;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(selected == this)
        {
            if (!selectImage.activeSelf) selectImage.SetActive(true);
        }
        else
        {
            if (selectImage.activeSelf) selectImage.SetActive(false);
        }
    }

    public void Init(Animations.Character character)
    {
        this.character = character;
        GetComponentInChildren<Animator>().runtimeAnimatorController = Animations.GetAnimation(character);
        characterImage.GetComponent<UnityEngine.UI.Image>().sprite = Animations.GetSprite(character);
        armImage.GetComponent<UnityEngine.UI.Image>().sprite = Animations.GetArm(character);
    }

    public void SetAnimation()
    {
        Animations.selectedAnimation = character;
        selected = this;
    }


}

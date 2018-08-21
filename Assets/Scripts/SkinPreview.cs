using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinPreview : MonoBehaviour {

    private static SkinPreview selected = null;

    Characters.Character character;

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

    public void Init(Characters.Character character)
    {
        this.character = character;
        GetComponentInChildren<Animator>().runtimeAnimatorController = Characters.GetAnimation(character);
        characterImage.GetComponent<UnityEngine.UI.Image>().sprite = Characters.GetSprite(character);
        armImage.GetComponent<UnityEngine.UI.Image>().sprite = Characters.GetArm(character);
    }

    public void SetAnimation()
    {
        Characters.selectedAnimation = character;
        selected = this;
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatInit : MonoBehaviour {

    public Text nameText, kills, deaths, kd;

    public int offset;
	
    public void Init(string name, int kills, int deaths, int position)
    {
        nameText.text = name;
        this.kills.text = kills.ToString();
        this.deaths.text = deaths.ToString();
        kd.text = ((float)kills / deaths).ToString("0.00");
        gameObject.transform.localPosition = new Vector3(0, position * -50 + offset);
    }
}

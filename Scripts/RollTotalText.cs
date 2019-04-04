using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class RollTotalText : MonoBehaviour
{

    public Text text;

    void Start()
    {
        text = GetComponent<Text>();
    }

    public void UpdateText(string _text)
    {
        text.text = _text;
    }


}

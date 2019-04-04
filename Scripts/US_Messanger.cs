using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class US_Messanger : MonoBehaviour
{


    public void SendSms(string URL)
    {

            Application.OpenURL(URL);

    }
}

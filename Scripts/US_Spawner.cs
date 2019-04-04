using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class US_Spawner : MonoBehaviour
{
    public GameObject childDie;
    private Rigidbody dieBody;


    public void OnClicked()
    {
        GameObject die = Instantiate(childDie);
        dieBody = childDie.GetComponent<Rigidbody>();
        dieBody.AddRelativeTorque(Vector3.back * 300f);
        NetworkServer.Spawn(die);
    }
}

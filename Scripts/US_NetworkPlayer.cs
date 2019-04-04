using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class US_NetworkPlayer : NetworkBehaviour
{
    US_CameraRaycast raycast;
    US_Spawner spawner;
    US_DieController dieController;
    US_GameController gameController;
    NetworkManager networkManager;

    [Header("Dice Physics Properties")]
    public float rollForce;
    public float maxAngularVelocity;
    public float torqueForce;
    public float explosionForce;
    public float explosionRadius;
    public float explosionUpwardModifyer;
    public ForceMode forceMode;

    [Header("UI Properties")]
    public RollTotalText rollTotal;

    [SerializeField]
    public Text rollTotalText;
    [Header("Layout Properties")]
    public SpawnLocation spawnLocation;
    public Vector3 clickerDiceSpawnLocation;

    // Attempting to replicate success from CmdUpdateRollTotalText by having the playerObject 
    // spawn the dice rather than the spawner;
    [SyncVar] public GameObject d4, d6, d8, d10_l, d10_h, d12, d20;

    [Header("Utility Properties")]
    Layer layer;
    public int dieId; // local instanceId

    [Header("Dice Lists")]
    public List<GameObject> DiceList = new List<GameObject>();
    //     public List<MeshCollider> ColliderList = new List<MeshCollider>();
    public List<Rigidbody> BodyList = new List<Rigidbody>();
    //     public List<US_DieController> DCList = new List<US_DieController>();

    void Start()
    {
        spawner = FindObjectOfType<US_Spawner>();
        spawnLocation = FindObjectOfType<SpawnLocation>();
        gameController = FindObjectOfType<US_GameController>();
       // gameController.netPlayerList.Add(this);
        rollTotal = FindObjectOfType<RollTotalText>();
        rollTotalText = rollTotal.GetComponent<Text>();
        raycast = FindObjectOfType<US_CameraRaycast>();
    }

    private void Update()
    {
        if (!isLocalPlayer)
            return;
        HandleInput();

        if (Input.GetMouseButtonDown(1))
        {
            RaycastForLayer();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
//             CmdRollDice();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
/*            CmdResetDice();*/
        }
        if (Input.GetKey(KeyCode.W))
        {
            gameObject.GetComponentInChildren<Rigidbody>().AddForce(Vector3.up * 25, ForceMode.Impulse);
        }
    }

    public void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            switch (raycast.LayerHit)
            {
                case Layer.Clickable_Dice:
                    
                    break;
                case Layer.UI:
//                     CmdRollDice();
                    break;
                case Layer.Scene_Object:
                    break;
                default:
                    return;
            }
        }
    }

//     #region CmdSpawnDice
//     [Command]
//     public void CmdSpawnDice(string name)
//     {
//         switch (name)
//         {
//             case "d4_Death":
//                 CmdSpawnD4();
//                 break;
//             case "d6_Death":
//                 CmdSpawnD6();
//                 break;
//             case "d8_Death":
//                 CmdSpawnD8();
//                 break;
//             case "d10_L_Death":
//                 CmdSpawnD10_L();
//                 break;
//             case "d10_H_Death":
//                 CmdSpawnD10_H();
//                 break;
//             case "d12_Death":
//                 CmdSpawnD12();
//                 break;
//             case "d20_Death":
//                 CmdSpawnD20();
//                 break;
//             default:
//                 return;
//         }
//     }
//     #endregion


//     public void CmdAddRpcDie(GameObject die)
//     {
//         gameController.RpcAddDie(die);
//     }


//     public void AddDie(GameObject die)
//     {
//         DiceList.Add(die);
//     }

//     #region individual spawners
// 
//     [Command]
//     public void CmdSpawnD4()
//     {
//         GameObject die = Instantiate(d4, spawnLocation.transform.position, Quaternion.identity);
//         NetworkServer.Spawn(die);
//         CmdAddRpcDie(die);
//     }
//     [Command]
//     public void CmdSpawnD6()
//     {
//         GameObject die = Instantiate(d6, spawnLocation.transform.position, Quaternion.identity);
//         NetworkServer.Spawn(die);
//         CmdAddRpcDie(die);
//     }
//     [Command]
//     public void CmdSpawnD10_H()
//     {
//         GameObject die = Instantiate(d10_h, spawnLocation.transform.position, Quaternion.identity);
//         NetworkServer.Spawn(die);
//         CmdAddRpcDie(die);
//     }
//     [Command]
//     public void CmdSpawnD10_L()
//     {
//         GameObject die = Instantiate(d10_l, spawnLocation.transform.position, Quaternion.identity);
//         NetworkServer.Spawn(die);
//         CmdAddRpcDie(die);
//     }
//     [Command]
//     public void CmdSpawnD8()
//     {
//         GameObject die = Instantiate(d8, spawnLocation.transform.position, Quaternion.identity);
//         NetworkServer.Spawn(die);
//         CmdAddRpcDie(die);
//     }
//     [Command]
//     public void CmdSpawnD12()
//     {
//         GameObject die = Instantiate(d12, spawnLocation.transform.position, Quaternion.identity);
//         NetworkServer.Spawn(die);
//         CmdAddRpcDie(die);
//     }
//     [Command]
//     public void CmdSpawnD20()
//     {
//         GameObject die = Instantiate(d20, spawnLocation.transform.position, Quaternion.identity);
//         NetworkServer.Spawn(die);
//         CmdAddRpcDie(die);
//     }
// 
//     #endregion


//     [Command]
//     public void CmdRollDice()
//     {
//         foreach (Rigidbody d in BodyList)
//         {
//             //d.AddForceAtPosition(Vector3.up * rollForce, Vector3.zero, forceMode);
//             d.AddExplosionForce(explosionForce, Vector3.down, explosionRadius, explosionUpwardModifyer);
//         }
//         StartCoroutine(OnDiceAsleep());
//     }



    public void AddToBodyList(GameObject die)
    {
        Rigidbody body = die.GetComponent<Rigidbody>();
        BodyList.Add(body);
    }

    #region oldGetLists

    public List<Rigidbody> GetRigidBodies(List<GameObject> dl)
    {
        List<Rigidbody> m_bodyList = new List<Rigidbody>();

        foreach (GameObject d in dl)
        {
            Rigidbody body = d.GetComponent<Rigidbody>();
            d.SetActive(true);
            m_bodyList.Add(body);
        }
        return m_bodyList;
    }

    public List<MeshCollider> GetColliders(List<GameObject> dl)
    {
        List<MeshCollider> colliderList = new List<MeshCollider>();
        foreach (GameObject d in dl)
        {
            MeshCollider col = d.GetComponent<MeshCollider>();
            colliderList.Add(col);
        }
        return colliderList;
    }

    public List<US_DieController> GetDCList(List<GameObject> m_die)
    {
        List<US_DieController> m_DCList = new List<US_DieController>();
        foreach (GameObject d in DiceList)
        {
            m_DCList.Add(d.GetComponent<US_DieController>());
        }
        return m_DCList;
    }
    #endregion

    public void SetMaxAngularVelocity(List<Rigidbody> bodyList)
    {
        foreach (Rigidbody d in bodyList)
        {
            d.maxAngularVelocity = maxAngularVelocity;
        }
    }


    public void ResetDice()
    {
        foreach (GameObject d in DiceList)
        {
            NetworkServer.Destroy(d);
        }
        DiceList.Clear();
        BodyList.Clear();
        CmdUpdateRollTotalText(0);
    }

    public int GetTotal()
    {
        int m_Score = 0;
        foreach (Rigidbody b in BodyList)
        {
            m_Score += b.GetComponent<US_DieController>().GetFaceValue();
        }
        return m_Score;
    }

    IEnumerator OnDiceAsleep()
    {
        bool bodiesAsleep = false;
        while (!bodiesAsleep)
        {
            bodiesAsleep = true;
            foreach (Rigidbody d in BodyList)
            {
                if (!d.IsSleeping())
                {
                    bodiesAsleep = false;
                    yield return null;
                }
            }
        }
        CmdUpdateRollTotalText(GetTotal());
    }

    [Command]
    public void CmdUpdateRollTotalText(int total)
    {
//         gameController.RpcUpdateRollTotalText(total);
    }

    public void RaycastForLayer()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 250))
        {
            Debug.Log(hit.collider.name);
        }
    }

}
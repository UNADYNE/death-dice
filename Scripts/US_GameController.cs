using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class US_GameController : MonoBehaviour
{
    US_CameraRaycast raycast;
    US_Spawner clickerDie;

    [Header("-Dice Physics Properties-")]
    public float rollForce;
    public float randomFloat;
    public float maxAngularVelocity;
    public float torqueForce;
    public float explosionForce;
    public float explosionRadius;
    public float explosionUpwardModifyer;
    public ForceMode forceMode;
    public float gravity;
    public bool canRoll = true;

    [Header("-Scene Elements-")]
    public List<GameObject> RollingSurfaceList = new List<GameObject>();
    //[Header("-Scene Properties-")]



    [Header("-Dice Lists-")]
    [SerializeField]
    private GameObject dieToSpawn;
    public List<GameObject> DiceList = new List<GameObject>();
    public List<Rigidbody> BodyList = new List<Rigidbody>();
    public List<US_DiceGroup> DiceGroup = new List<US_DiceGroup>();

    [Header("-UI Components-")]
    public Text rollTotal;
    public Transform spawLocation;
    public RectTransform slideOut;
    public RectTransform scorePanel;
    public Button menuButton;
    public Animator menuAnimator;
    public Text groupTotalText;
    private string finalGroupTotalText = "";


    private int diceGroupIndex = 0;
    private int rollingSurfaceIndex = 0;

    private void Start()
    {
        raycast = Camera.main.GetComponent<US_CameraRaycast>();
        rollTotal.text = "TOTAL: 0";
        Physics.gravity = new Vector3(0, gravity, 0);
    }

    private void Update()
    {
        HandleInput();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RollDice();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            ResetDice();
        }
    }

    public void MenuSlideIn()
    {
        menuAnimator.SetTrigger("slideIn");
        menuButton.gameObject.SetActive(false);
    }

    public void MenuSlideOut()
    {
        menuAnimator.SetTrigger("slideOut");
        menuButton.gameObject.SetActive(true);
        menuAnimator.SetTrigger("idle");
    }

    // change the dice to be rolled.  User sees a change of material but it sets current dice to active = false and the next
    // to active = true
    public void ChangeDice()
    {
        diceGroupIndex++;
        if(diceGroupIndex >= DiceGroup.Count)
        {
            diceGroupIndex = 0;
        }

        foreach(US_DiceGroup dg in DiceGroup)
        {
            if (dg != DiceGroup[diceGroupIndex])
            {
                dg.gameObject.SetActive(false);
            }
        }
        DiceGroup[diceGroupIndex].gameObject.SetActive(true);
    }


    // change the floor where the dice will land when rolled.
    public void ChangeRollingSurface()
    {
        rollingSurfaceIndex++;
        if (rollingSurfaceIndex >= RollingSurfaceList.Count)
        {
            rollingSurfaceIndex = 0;
        }

        foreach (GameObject rs in RollingSurfaceList)
        {
            if (rs != RollingSurfaceList[rollingSurfaceIndex])
            {
                rs.gameObject.SetActive(false);
            }
        }
        RollingSurfaceList[rollingSurfaceIndex].gameObject.SetActive(true);
    }

    private float RandomizeFloat(float num)
    {
        return Random.Range(num, num * randomFloat);
    }

    public void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            switch (raycast.LayerHit)
            {
                case Layer.Clickable_Dice:
                    dieToSpawn = Instantiate(raycast.Hit.collider.gameObject.GetComponent<US_Spawner>().childDie,
                        spawLocation.position, Quaternion.identity);
                    DiceList.Add(dieToSpawn);
                    InitShit();
                    break;
                case Layer.UI:
                    RollDice();
                    break;
                case Layer.Scene_Object:
                    break;
                default:
                    return;
            }
        }
    }

    public void RollDice()
    {
        if (canRoll == true)
        {
            canRoll = false;
            foreach (Rigidbody d in BodyList)
            {
                d.AddExplosionForce(RandomizeFloat(explosionForce), Vector3.right, explosionRadius, explosionUpwardModifyer);
                d.AddRelativeTorque(Vector3.left * Random.Range(-2, 2));
            }
        }

        StartCoroutine(OnDiceAsleep());
    }

    public int GetTotal()
    {
        int m_Score = 0;
        string m_str = "";
        foreach (Rigidbody b in BodyList)
        {
            m_Score += b.GetComponent<US_DieController>().GetFaceValue();
            m_str = b.GetComponent<US_DieController>().gameObject.name + 
                " = " + b.GetComponent<US_DieController>().GetFaceValue().ToString();
            finalGroupTotalText += FormatGroupStr(m_str) + " \n";

            /* Consider only one instance with formated string including a carriage return dividing each die total
             * maybe add a comma after each score then String.Split on the commas for the new line -- this would require a new
             * method which adds each score string to the previous string and inserting a comma after each string so the 
             * String.Split will work
             * 
             * This will require adding a scrolling bar to read scores which go off screen
             * */
            
        }       
        return m_Score;
    }


    #region GetLists
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
        Debug.Log("GetDCList()");
        return m_DCList;
    }
    #endregion

    private void InitShit()
    {
        BodyList = GetRigidBodies(DiceList);
        SetMaxAngularVelocity(BodyList);
    }

    // removes the (Clone) from the gameObject name
    private string FormatGroupStr(string str)
    {
        string new_str = str.Replace("(Clone)", "").ToUpper();
        return new_str;
    }

    private void ResetGroupTotalText()
    {
        finalGroupTotalText = "";
        groupTotalText.text = "";
        print("ResetGroupTotalText()");
    }
    
    private void ShowScorePanel()
    {
        scorePanel.gameObject.SetActive(true);        
    }

    public void DismissScorePanel()
    {
        scorePanel.gameObject.SetActive(false);
        ResetGroupTotalText();
    }

    IEnumerator OnDiceAsleep()
    {
        bool bodiesAsleep = false;
        while (!bodiesAsleep)
        {
            bodiesAsleep = true;
            if (BodyList.Count > 0)
            {
                foreach (Rigidbody d in BodyList)
                {
                    if (!d.IsSleeping())
                    {
                        bodiesAsleep = false;
                        yield return null;
                    }
                }
            }

        }
        canRoll = true;
        UpdateRollTotalText(GetTotal());
        ShowScorePanel();
    }

    void UpdateRollTotalText(int total)
    {
        rollTotal.text = "TOTAL: " + total.ToString();
        groupTotalText.text = finalGroupTotalText;
        /*add comma or new line to this string for formating group totals*/
        print(groupTotalText.text);
        //print("string --> " + finalGroupTotalText);
    }

    public void ResetDice()
    {
        foreach (GameObject die in DiceList)
        {
            Destroy(die);
        }
        DiceList.Clear();
        BodyList.Clear();
        UpdateRollTotalText(0);
    }

    public void SetMaxAngularVelocity(List<Rigidbody> bodyList)
    {
        foreach (Rigidbody d in bodyList)
        {
            d.maxAngularVelocity = maxAngularVelocity;
        }
    }
}

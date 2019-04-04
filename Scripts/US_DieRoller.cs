using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class US_DieRoller : MonoBehaviour
{


    Rigidbody die;
    public float expForce;
    public float maxVelocity;
    public ForceMode forceMode;
    public float upModifyer;
    public float expRadius;
    public List<GameObject> diceList = new List<GameObject>();
    public List<Rigidbody> diceBodyList = new List<Rigidbody>();
    public List<MeshCollider> colliderList = new List<MeshCollider>();

    public static US_DieRoller _instance;

    public static US_DieRoller Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<US_DieRoller>();
            }
            return _instance;
        }
    }


    Collider dieCollider;


    void Start()
    {

    }


    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RollDie();
        }
    }

    private float RandomizeRollForce(float force)
    {
        return Random.Range(force, force * 15);
    }

    public void RollDie()
    {
        foreach (Rigidbody d in diceBodyList)
        {
            d.AddExplosionForce(RandomizeRollForce(expForce), Vector3.zero, expRadius, upModifyer, forceMode);
        }
        Debug.Log(RandomizeRollForce(expForce));
    }

    bool IsGrounded()
    {
        //float distnaceToGround = dieCollider.bounds.extents.y;
        //return Physics.Raycast(transform.position, -Vector3.up, distnaceToGround + 0.1f);
        return true;
    }

    public void SetAngularVelocity(List<Rigidbody> dl)
    {
        foreach (Rigidbody d in dl)
        {
            d.maxAngularVelocity = maxVelocity;
            Debug.Log(d.name + "Angular Velocity: " + d.maxAngularVelocity);
        }
    }

    public List<Rigidbody> GetRigidBodies(List<GameObject> dl)
    {
        List<Rigidbody> bodyList = new List<Rigidbody>();

        foreach (GameObject d in dl)
        {
            if (d != null)
            {
                Rigidbody body = d.GetComponent<Rigidbody>();
                d.SetActive(true);
                bodyList.Add(body);
            }
        }

        return bodyList;
    }

    public List<MeshCollider> GetColliders(List<GameObject> dl)
    {
        List<MeshCollider> colliderList = new List<MeshCollider>();
        foreach (GameObject d in dl)
        {
            if (d != null)
            {
                MeshCollider col = d.GetComponent<MeshCollider>();
                colliderList.Add(col);
            }
        }

        return colliderList;
    }

    public void AddToDiceList(GameObject go)
    {
        diceList.Add(go);
    }


    public void InitShit()
    {
        diceBodyList = GetRigidBodies(diceList);
        colliderList = GetColliders(diceList);
        SetAngularVelocity(diceBodyList);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class US_DieController : MonoBehaviour
{
    [System.Serializable]
    public class Face
    {
        public int value;
        public Vector3 normal;
    }
    public List<Face> faces;
    private AudioSource sound;
    public AudioClip impact;
    public AudioClip settle;
    private Rigidbody die;

    private void Start()
    {
        sound = GetComponent<AudioSource>();
        die = GetComponent<Rigidbody>();
    }

    public Face GetFace()
    {
        if (faces == null || faces.Count == 0)
            return null;
        Vector3 direction = new Vector3(0, 0, 0);
        if (faces.Count <= 4)
        {
            direction = transform.InverseTransformDirection(Vector3.down);
        }
        else
        {
            direction = transform.InverseTransformDirection(Vector3.up);
        }


        Face side = null;
        float ang = float.MaxValue;
        for (int i = 0; i < faces.Count; i++)
        {
            float a = Vector3.Angle(faces[i].normal, direction);
            if (a < ang)
            {
                ang = a;
                side = faces[i];
            }
        }
        return side;
    }
    public int GetFaceValue()
    {
        var side = GetFace();
        if (side != null)
            return side.value;
        return 0;
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.layer == 9)
        {
            if (die.velocity.magnitude > 1)
            {
                sound.clip = impact;
                sound.Play();
            }

            if (die.velocity.magnitude < 1 && die.velocity.magnitude > 0)
            {
                sound.clip = settle;
                sound.Play();
            }
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class US_CameraFollow : MonoBehaviour
{

    [SerializeField]
    Camera mainCamera;
    [SerializeField]
    float xPosOffset, yPosOffset, zPosOffset;
    [SerializeField]
    float xRotOffset, yRotOffset, zRotOffset, rotateMouseSpeed;


    public GameObject objectToFollow;
    US_DieRoller die;

    // Use this for initialization
    void Start()
    {
        mainCamera = GetComponent<Camera>();        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        FollowObject(objectToFollow);
        LookAtObject(objectToFollow);
    }

    void FollowObject(GameObject followedObject)
    {
        // TODO add pinch before mobile build as per: https://unity3d.com/learn/tutorials/topics/mobile-touch/pinch-zoom
        float zoomDistance = Input.GetAxis("Mouse ScrollWheel");

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            zPosOffset -= zoomDistance;
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            zPosOffset -= zoomDistance;

        }

        if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.LeftShift))
        {
            yPosOffset -= .5f;
        }
        if (Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.LeftShift))
        {
            yPosOffset += .5f;
        }


        Vector3 cameraPositionOffset = new Vector3(xPosOffset, yPosOffset, zPosOffset);
        transform.position = followedObject.transform.position - cameraPositionOffset;
    }

    void LookAtObject(GameObject lookAtObject)
    {
        Vector3 lookRotation = new Vector3(xRotOffset, yRotOffset, zRotOffset);
        transform.LookAt(lookAtObject.transform.position);
    }


    void RotateCamera()
    {
        float mouseX = Input.GetAxis("Mouse X");

    }
}

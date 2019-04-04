using UnityEngine;
using UnityEngine.UI;

public class US_Cursor : MonoBehaviour
{
    US_CameraRaycast cameraRaycast;
    public Texture2D sword, arrow, question;
    public Vector2 swordHotSpot;
    public Vector2 arrowHotSpot;

    void Start()
    {
        cameraRaycast = GetComponent<US_CameraRaycast>();
    }
    // Update is called once per frame
    void Update()
    {
        SetGameCursor();
    }

    public void SetGameCursor()
    {
        //switch (cameraRaycast.LayerHit)
        //{
        //    case Layer.Enemy:
        //    Cursor.SetCursor(sword, swordHotSpot, CursorMode.Auto);                
        //    break;
        //    case Layer.Walkable:
        //    Cursor.SetCursor(arrow, arrowHotSpot, CursorMode.Auto);
        //    break;
        //    case Layer.RaycastEndStop:
        //    Cursor.SetCursor(question, Vector2.zero, CursorMode.Auto);
        //    break;

        //    default:
        //    return;
        //}
    }
}

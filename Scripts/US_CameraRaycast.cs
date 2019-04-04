using UnityEngine;
using UnityEngine.Networking;

public class US_CameraRaycast : MonoBehaviour
{
    public Layer[] layerPriorities = {
        Layer.Clickable_Dice,
        Layer.Scene_Object,
        Layer.UI
    };

    [SerializeField]
    float distanceToBackground = 100f;
    Camera viewCamera;

    RaycastHit m_hit;
    public RaycastHit Hit
    {
        get { return m_hit; }
    }

    Layer m_layerHit;
    public Layer LayerHit
    {
        get { return m_layerHit; }
    }
    //US_Cursor cursor;
    public delegate void OnLayerChange();
    public OnLayerChange layerChangeObservers;

    void LayerChangeHandeler()
    {
        //cursor.SetGameCursor();
    }

    void Start() // TODO Awake?
    {
        viewCamera = Camera.main;
        //layerChangeObservers += LayerChangeHandeler;
        //cursor = FindObjectOfType<US_Cursor>();
    }

    void Update()
    {
        // Look for and return priority layer hit
        foreach (Layer layer in layerPriorities)
        {
            var hit = RaycastForLayer(layer);
            if (hit.HasValue)
            {
                m_hit = hit.Value;
                m_layerHit = layer;
              //  layerChangeObservers();

                return;
            }
        }

        // Otherwise return background hit
        m_hit.distance = distanceToBackground;
        m_layerHit = Layer.RaycastEndStop;
    }

    RaycastHit? RaycastForLayer(Layer layer)
    {
        int layerMask = 1 << (int)layer; // See Unity docs for mask formation
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit; // used as an out parameter
        bool hasHit = Physics.Raycast(ray, out hit, distanceToBackground, layerMask);
        if (hasHit)
        {
            return hit;
        }
        return null;
    }
}

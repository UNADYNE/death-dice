using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(US_DieController))]
public class US_DieEditor : Editor
{
    US_DieController m_Target;
    List<US_DieController.Face> m_Sides;
    static bool m_EditMode = false;
    static int m_Selected = -1;
    static bool m_ShowHelp = false;
    static string m_Help = @"
Left.Click in SceneView to update the current selected side normal
Right-Click in SceneView to add a new side
Note: if a side with the same normal already exists, the action is ignored
You can select sides in the inspector
Use ALT+ left drag to orbit. Keep in mind to center the view first (key ""F"")
";
    static bool EditMode
    {
        get { return m_EditMode; }
        set
        {
            if (m_EditMode != value)
            {
                m_EditMode = value;
                if (m_EditMode == false)
                    Tools.current = Tool.Move;
                SceneView.RepaintAll();
            }
            if (m_EditMode)
                Tools.current = Tool.None;
        }
    }
    void OnEnable()
    {

        m_Target = (US_DieController)target;
        m_Sides = m_Target.faces;
        if (m_Sides == null)
            m_Sides = m_Target.faces = new List<US_DieController.Face>();
    }
    void OnDisable()
    {
        EditMode = false;
    }
    public override void OnInspectorGUI()
    {
        EditMode = GUILayout.Toggle(EditMode, "Mark Faces", "Button");
        if (EditorApplication.isPlaying)
            GUILayout.Label("Current Value: " + m_Target.GetFaceValue());
        if (EditMode)
        {
            GUILayout.Label("Sides: " + m_Sides.Count);
            Color old = GUI.color;
            for (int i = 0; i < m_Sides.Count; i++)
            {
                if (i == m_Selected)
                    GUI.color = Color.yellow;
                else
                    GUI.color = old;

                GUILayout.BeginHorizontal("box");
                /*m_Sides[i].normal = */
                GUILayout.Label("" + m_Sides[i].normal, GUILayout.Width(100));
                m_Sides[i].value = EditorGUILayout.IntField(m_Sides[i].value);
                if (GUILayout.Button("select", GUILayout.Width(45)))
                {
                    m_Selected = i;
                    SceneView.RepaintAll();
                }
                if (GUILayout.Button("flip", GUILayout.Width(30)))
                {
                    m_Sides[i].normal *= -1;
                    EditorUtility.SetDirty(m_Target);
                    SceneView.RepaintAll();
                    GUIUtility.ExitGUI();
                }
                if (GUILayout.Button("X", GUILayout.Width(20)))
                {
                    m_Sides.RemoveAt(i);
                    if (m_Selected >= m_Sides.Count)
                        m_Selected = m_Sides.Count - 1;
                    EditorUtility.SetDirty(m_Target);
                    SceneView.RepaintAll();
                    GUIUtility.ExitGUI();
                }
                GUILayout.EndHorizontal();
            }
            GUI.color = old;
            if (GUILayout.Button("Add"))
            {
                m_Sides.Add(new US_DieController.Face());
                EditorUtility.SetDirty(m_Target);
            }
            m_ShowHelp = GUILayout.Toggle(m_ShowHelp, "Show Help", "Button");
            if (m_ShowHelp)
            {
                GUILayout.TextArea(m_Help);
            }
        }
        else
        {
            DrawDefaultInspector();
        }
    }

    US_DieController.Face FindSide(Vector3 aNormal)
    {
        for (int i = 0; i < m_Sides.Count; i++)
        {
            if (Vector3.Angle(m_Sides[i].normal, aNormal) < 0.05f)
            {
                return m_Sides[i];
            }
        }
        return null;
    }

    void OnSceneGUI()
    {
        if (!EditMode)
            return;
        int id = GUIUtility.GetControlID(FocusType.Passive);
        HandleUtility.AddDefaultControl(id);
        Event e = Event.current;
        Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
        RaycastHit hit;
        var trans = m_Target.transform;

        if (Physics.Raycast(ray, out hit) && hit.transform == trans && !e.alt)
        {
            Handles.color = Color.red;
            Handles.DrawLine(hit.point, hit.point + hit.normal);
            if (e.type == EventType.MouseDown)
            {
                var normal = trans.InverseTransformDirection(hit.normal);
                if (e.button == 0 && m_Selected >= 0 && m_Selected < m_Sides.Count && FindSide(normal) == null)
                    m_Sides[m_Selected].normal = normal;
                else if (e.button == 1)
                {
                    US_DieController.Face side = FindSide(normal);
                    if (side == null)
                    {
                        side = new US_DieController.Face();
                        m_Sides.Add(side);
                    }
                    side.normal = normal;
                }
                EditorUtility.SetDirty(m_Target);
            }
        }
        for (int i = 0; i < m_Sides.Count; i++)
        {
            Handles.color = (i == m_Selected) ? Color.yellow : Color.cyan;
            var dir = trans.TransformDirection(m_Sides[i].normal);
            Handles.DrawLine(trans.position + dir * 0.1f, trans.position + dir);
        }
        if (Event.current.type == EventType.MouseMove)
            SceneView.RepaintAll();
    }
}

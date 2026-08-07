using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class HierarchyShortcuts
{
    private static int hoveredInstanceID;

    static HierarchyShortcuts()
    {
        EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyGUI;
        Debug.Log("THierarchy Loaded");
    }

    private static void OnHierarchyGUI(int instanceID, Rect selectionRect)
    {
        Event e = Event.current;

        // Simpan object yang sedang di-hover
        if (selectionRect.Contains(e.mousePosition))
        {
            hoveredInstanceID = instanceID;

            if (e.type == EventType.KeyDown)
            {
                GameObject go = EditorUtility.InstanceIDToObject(instanceID) as GameObject;

                if (go == null)
                    return;

                switch (e.keyCode)
                {
                    case KeyCode.A:
                        Undo.RecordObject(go, "Toggle Active");
                        go.SetActive(!go.activeSelf);
                        EditorApplication.RepaintHierarchyWindow();
                        e.Use();
                        break;

                    case KeyCode.D:
                        Object duplicate = Object.Instantiate(go, go.transform.parent);
                        duplicate.name = go.name;
                        Undo.RegisterCreatedObjectUndo(duplicate, "Duplicate");
                        Selection.activeObject = duplicate;
                        e.Use();
                        break;

                    case KeyCode.X:
                        Undo.DestroyObjectImmediate(go);
                        e.Use();
                        break;

                    case KeyCode.F:
                        Selection.activeGameObject = go;
                        EditorGUIUtility.PingObject(go);
                        e.Use();
                        break;
                }
            }
        }
        Debug.Log("Drawing");
    }
}
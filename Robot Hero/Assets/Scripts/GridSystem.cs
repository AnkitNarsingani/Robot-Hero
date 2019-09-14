using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class GridSystem : MonoBehaviour
{
    [Header("Do not change these values, to edit values use the Tile Editor window")]
#if UNITY_EDITOR
    [ReadOnly]
#endif
    public int tileSetSize;

#if UNITY_EDITOR
    [ReadOnly]
#endif
    public float cellSize;

    public GameObject[] tileGameObjects;
}

#if UNITY_EDITOR
public class ReadOnlyAttribute : PropertyAttribute
{

}

[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property,
                                            GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }

    public override void OnGUI(Rect position,
                               SerializedProperty property,
                               GUIContent label)
    {
        GUI.enabled = false;
        EditorGUI.PropertyField(position, property, label, true);
        GUI.enabled = true;
    }
}
#endif

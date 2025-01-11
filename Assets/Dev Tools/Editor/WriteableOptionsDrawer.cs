using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CustomPropertyDrawer(typeof(WritableOptionsAttribute))]
public class WritableOptionsDrawer : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        var option = attribute as WritableOptionsAttribute;
        var optionsList = option.List;
        if (property.propertyType == SerializedPropertyType.String) {
            int index = Mathf.Max(0, System.Array.IndexOf(optionsList, property.stringValue));
            index = EditorGUI.Popup(position, property.displayName, index, optionsList);

            property.stringValue = optionsList[index];
        }
        else if (property.propertyType == SerializedPropertyType.Integer) {
            property.intValue = EditorGUI.Popup(position, property.displayName, property.intValue, optionsList);
        }
        else {
            base.OnGUI(position, property, label);
        }
    }
}


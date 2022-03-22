using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(EnumFlagsAttribute))]
public class EnumFlagsDrawer : PropertyDrawer
{
    private int cachedValue = int.MinValue;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        /*绘制枚举复选框 ， 0-Nothing，-1-Everything,其他是枚举之和
        枚举值（2的x次幂）：2的0次幂=1，2的1次幂=2，2的2次幂=4，8，16...
        */
        property.intValue = EditorGUI.MaskField(position, label, property.intValue, property.enumNames);
        // 一旦数据发生变更，保存序列化数据，否则会出现设置数据丢失情况
        if (cachedValue != property.intValue)
        {
            //Debug.Log("Inside: "+cachedValue);
            cachedValue = property.intValue;
            property.serializedObject.ApplyModifiedProperties();
        }
    }
}
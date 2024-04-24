using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "so_ItemList", menuName = "Scriptable Objects/Item/Item List")]

public class SO_ItemList : ScriptableObject
{
    [SerializeField]
    public List<ItemDetails> itemDetails;
#if UNITY_EDITOR
    [CustomEditor(typeof(SO_ItemList))]
    public class SO_ItemListEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            SO_ItemList itemList = (SO_ItemList)target;

            EditorGUI.BeginChangeCheck();

            // 리스트의 크기를 크게 설정
            EditorGUILayout.PropertyField(serializedObject.FindProperty("itemDetails"), true);

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
#endif
}


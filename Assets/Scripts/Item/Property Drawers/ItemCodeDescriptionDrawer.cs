using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System.Reflection.Emit;
using System;

[CustomPropertyDrawer(typeof(ItemCodeDescriptionAttribute))]
public class ItemCodeDescriptionDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property) * 2;
    }
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //-- EditorGUI.BeginProperty 인스펙터 창에 속성을 그리기 시작함을 알리는 함수 이다 시작이 있으면 끝을 알려주기 위해 아래의 EditorGUI.EndProperty();가 있다.

        EditorGUI.BeginProperty(position, label, property);

        if (property.propertyType == SerializedPropertyType.Integer)
        {
            EditorGUI.BeginChangeCheck(); // 속성값(ItemCode)가 변경여부를 확인 

            // ItemCode 속성값 그리기
            var newValue = EditorGUI.IntField(new Rect(position.x, position.y, position.width, position.height / 2), label, property.intValue);

            // Item이름(설명)값 그리기
            EditorGUI.LabelField(new Rect(position.x, position.y + position.height / 2, position.width, position.height / 2), "Item Description", GetItemDescription
            (property.intValue));

           
            if (EditorGUI.EndChangeCheck())//-- 속성값(ItemCode)가 변경이 되었는지 확인 후 속성값 변경
            {
                property.intValue = newValue;
            }
        }
        EditorGUI.EndProperty();

    }

    private string GetItemDescription(int itemCode)
    {

        SO_ItemList so_itemList;

        so_itemList = AssetDatabase.LoadAssetAtPath("Assets/SW/Scriptable Object Assets/Item/so_ItemList.asset", typeof(SO_ItemList)) as SO_ItemList;
        //--지정한 경로에 있는 스크립터블 오브젝트 내용을 가져오기

        List<ItemDetails> itemDetailsList = so_itemList.itemDetails;

        ItemDetails itemDetail = itemDetailsList.Find(x => x.itemCode == itemCode);
        //-- 전달받은 ItemCode의 내용을 찾아 가져오기 

        if (itemDetail != null)
            return itemDetail.itemDescription;
        else
            return ""; 

    }
}



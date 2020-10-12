using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Inven_Item : UI_Base
{
    enum GameObjects
    {
        ItemIcon,
        ItemName_Text,
    }

    string _name;

    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));
        Get<GameObject>((int)GameObjects.ItemName_Text).GetComponent<Text>().text = _name;
        Get<GameObject>((int)GameObjects.ItemIcon).BindEvent((PointerEventData data) => { Debug.Log($"아이템 클릭 : {_name}"); });
    }

    public void SetInfo(string name)
    {
        _name = name;
    }
}

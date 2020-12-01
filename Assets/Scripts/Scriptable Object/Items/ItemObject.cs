using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Default,
    UserUseable
}

public enum Attributes{
    FloatingState,
    StabalizeState,
    BombThrowerState
}

public abstract class ItemObject : ScriptableObject
{
    public int Id;
    // 只需使用精靈物件來展示UI，但不需要完整的GameObject
    //public GameObject prefab;
    public Sprite uiDisplay;
    public ItemType itemType;
   
    [TextArea(15,20)]
    public string description;
    public ItemBuff[] buffs;
    public Item CreateItem(){
        Item newItem = new Item(this);
        return newItem;
    }
}

[System.Serializable]
public class Item{
    public string Name;
    public int Id;
    public ItemBuff[] buffs;
    public Item(ItemObject item){
        Name = item.name;
        Id = item.Id;
        buffs = new ItemBuff[item.buffs.Length];
        for (int i = 0; i < buffs.Length; i++)
        {
            buffs[i] = new ItemBuff(item.buffs[i].value);
            buffs[i].attribute = item.buffs[i].attribute;
        }
    }

    public void ExecuteAllItemBuff(Vector3 mousePosition){
        foreach (var itemBuff in buffs)
        {
            itemBuff.ExecuteByAttribute(mousePosition);
        }
    }
}

[System.Serializable]
public class ItemBuff{
    public Attributes attribute;
    public int value;
    //public int min;
    //public int max;
    /*
    public ItemBuff(int _min, int _max){
        min = _min;
        max = _max;
        GenerateValue();
    }
    */
    public ItemBuff(int _value){
        value = _value;
    }
    /*
    public void GenerateValue(){
        value = UnityEngine.Random.Range(min, max);
    }
    */
    public void ExecuteByAttribute(Vector3 mousePosition){
        Debug.Log("mousePosition: " + mousePosition);
        switch (attribute)
        {
            case Attributes.BombThrowerState:
                Debug.Log("exccute: BombThrowerState");
                break;
            case Attributes.FloatingState:
                Debug.Log("exccute: FloatingState");
                break;
            case Attributes.StabalizeState:
                Debug.Log("exccute: StabalizeState");
                break;
            default:
                Debug.Log("exccute: default");
                break;
        }
    }
}

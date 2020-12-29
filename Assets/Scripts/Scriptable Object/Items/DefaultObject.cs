using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New default Object", menuName = "Inventory System/Items/Default")]
public class DefaultObject : ItemObject
{
    public void awake(){
        itemType = ItemType.Default;
    }
}

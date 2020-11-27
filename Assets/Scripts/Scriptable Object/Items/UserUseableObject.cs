using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New UserUseableObject Object", menuName = "Inventory System/Items/UserUseableObject")]
public class UserUseableObject : ItemObject
{
    public int pickableTimes;
    public int useableTimes;
    public void Awake(){
        itemType = ItemType.UserUseable;
    }
}

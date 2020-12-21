using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Default,
    UserUseable
}

public enum Attributes{
    //FloatingState,
    //StabalizeState,
    BombThrowerState,
    EnergyRestore,
    Booster,
    ShockWave,
    SlowDownWave,
    TimeCountDownDebug,
    EnergyStopConsuming,
    EnergyRegenerate
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
            buffs[i] = new ItemBuff();
            buffs[i].value = item.buffs[i].value;
            buffs[i].attribute = item.buffs[i].attribute;
            buffs[i].bulletPrefab = item.buffs[i].bulletPrefab;
            buffs[i].time = item.buffs[i].time;
        }
    }

    public void ExecuteAllItemBuff(GameObject user){
        foreach (var itemBuff in buffs)
        {
            itemBuff.ExecuteByAttribute(user);
        }
    }
}

[System.Serializable]
public class ItemBuff{
    public Attributes attribute;
    public GameObject bulletPrefab;
    //public string bulletPrefabName;
    public int value;
    public float time;
    public ItemBuff(){}
    public void ExecuteByAttribute(GameObject user){
        //Debug.Log("mousePosition: " + mousePosition);
        switch (attribute)
        {
            case Attributes.BombThrowerState:
                Debug.Log("exccute: BombThrowerState");
                RunThrowerState(user);
                break;
            case Attributes.Booster:
                Debug.Log("execute: Booster t: " + time);
                Booster(user);
                break;
            case Attributes.EnergyRestore:
                Debug.Log("execute: EnergyRestore");
                EnergyRestore(user);
                break;
            case Attributes.EnergyStopConsuming:
                Debug.Log("execute: EnergyStopConsuming t: " + time);
                EnergyStopConsuming(user);
                break;
            case Attributes.EnergyRegenerate:
                Debug.Log("execute: EnergyRegenerate t: " + time);
                EnergyRegenerate(user);
                break;
            default:
                Debug.Log("exccute: default");
                break;
        }
    }

    public void RunThrowerState(GameObject user){
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var ProjectileGenerator = user.GetComponent<ProjectileGenerator>();
        if(ProjectileGenerator != null){
            var originalBulletPrefab = ProjectileGenerator.bulletPrefab;
            ProjectileGenerator.bulletPrefab = bulletPrefab;
            //Debug.Log("generate bulletPrefab: " + bulletPrefab.name);
            ProjectileGenerator.GenerateTheBullet(mousePosition,user.transform.position,user.GetComponent<PlayerManager>().bulletSpawnOffset);
            ProjectileGenerator.bulletPrefab = originalBulletPrefab;
        }
    }

    public void Booster(GameObject user){
        var statusScript = user.GetComponent<PlayerStatus>();
        int id = statusScript.GetStatusIdByName("Boost");
        user.GetComponent<PlayerManager>().SetStatusEffect(id,time);
    }

    public void EnergyRestore(GameObject user){
        user.GetComponent<PlayerManager>().energy += value;
    }

    public void EnergyStopConsuming(GameObject user){
        var statusScript = user.GetComponent<PlayerStatus>();
        int id = statusScript.GetStatusIdByName("EnergyStopLost");
        user.GetComponent<PlayerManager>().SetStatusEffect(id,time);
    }

    public void EnergyRegenerate(GameObject user){
        var statusScript = user.GetComponent<PlayerStatus>();
        int id = statusScript.GetStatusIdByName("EnergyRegenerate");
        user.GetComponent<PlayerManager>().SetStatusEffect(id,time);
    }

}

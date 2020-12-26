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
    EnergyStopConsuming,
    EnergyRegenerate,
    Freeze,
    ThrowerState,
    DrainEnergyWave
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
            buffs[i].bulletOffset = item.buffs[i].bulletOffset;
            buffs[i].time = item.buffs[i].time;
            buffs[i].radius = item.buffs[i].radius;
            buffs[i].affectorObject = item.buffs[i].affectorObject;
        }
    }

    public void ExecuteAllItemBuff(GameObject user){
        foreach (var itemBuff in buffs)
        {
            itemBuff.ExecuteByAttribute(user);
        }
    }
}


public class ItemBuffMonobehavior : MonoBehaviour{
    public ItemBuffMonobehavior(){

    }

    public void generateObject(GameObject affectorObject,GameObject user){
        if(affectorObject){
            Instantiate(affectorObject,user.transform.position,Quaternion.identity);
        }
    }
}

[System.Serializable]
public class ItemBuff{
    public Attributes attribute;
    public GameObject bulletPrefab;
    public GameObject affectorObject;
    //public string bulletPrefabName;
    public float value;
    public float radius;
    public float bulletOffset;
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
            case Attributes.Freeze:
                Debug.Log("execute: FreezeState: " + time);
                FreezeState(user);
                break;
            case Attributes.ShockWave:
                Debug.Log("execute: ShockWave: " + time);
                //CameraShake(user);
                ShockWave(user);
                break;
            case Attributes.ThrowerState:
                Debug.Log("execute: ThrowerState");
                ThrowerState(user);
                break;
            case Attributes.DrainEnergyWave:
                Debug.Log("execute: DrainEnergyWave");
                DrainEnergyWave(user);
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

    public void FreezeState(GameObject user){
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var ProjectileGenerator = user.GetComponent<ProjectileGenerator>();
        if(ProjectileGenerator != null){
            var originalBulletPrefab = ProjectileGenerator.bulletPrefab;
            ProjectileGenerator.bulletPrefab = bulletPrefab;
            //Debug.Log("generate bulletPrefab: " + bulletPrefab.name);
            ProjectileGenerator.GenerateTheBullet(mousePosition,user.transform.position,bulletOffset);
            ProjectileGenerator.bulletPrefab = originalBulletPrefab;
        }
    }

    public void CameraShake(GameObject user){
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var ProjectileGenerator = user.GetComponent<ProjectileGenerator>();
        if(ProjectileGenerator != null){
            var originalBulletPrefab = ProjectileGenerator.bulletPrefab;
            ProjectileGenerator.bulletPrefab = bulletPrefab;
            //Debug.Log("generate bulletPrefab: " + bulletPrefab.name);
            ProjectileGenerator.GenerateTheBullet(mousePosition,user.transform.position,user.GetComponent<PlayerManager>().bulletSpawnOffset);
            ProjectileGenerator.bulletPrefab = originalBulletPrefab;
        }
        //var statusScript = user.GetComponent<PlayerStatus>();
        //int id = statusScript.GetStatusIdByName("CameraShaking");
        //user.GetComponent<PlayerManager>().SetStatusEffect(id,time);
    }

    public void ThrowerState(GameObject user){
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var ProjectileGenerator = user.GetComponent<ProjectileGenerator>();
        if(ProjectileGenerator != null){
            var originalBulletPrefab = ProjectileGenerator.bulletPrefab;
            ProjectileGenerator.bulletPrefab = bulletPrefab;
            //Debug.Log("generate bulletPrefab: " + bulletPrefab.name);
            ProjectileGenerator.GenerateTheBullet(mousePosition,user.transform.position,bulletOffset);
            ProjectileGenerator.bulletPrefab = originalBulletPrefab;
        }        
    }

    public void DrainEnergyWave(GameObject user){
        ItemBuffMonobehavior obj = new ItemBuffMonobehavior();
        obj.generateObject(affectorObject,user);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(user.transform.position,radius);
        foreach (Collider2D itemCollider2D in colliders)
        {
            GameObject item = itemCollider2D.gameObject;
            if(item == user){
                continue;
            }
            var playerManagerScript = item.GetComponent<PlayerManager>();
            if(playerManagerScript)
            {
                float realDrainAmount = 0f;
                if(playerManagerScript.energy > value){
                    realDrainAmount = value;
                }else{
                    realDrainAmount = playerManagerScript.energy;
                }
                playerManagerScript.energy -= realDrainAmount;
                user.GetComponent<PlayerManager>().energy += realDrainAmount;
            }    
        }
    }

    public void ShockWave(GameObject user){
        ItemBuffMonobehavior obj = new ItemBuffMonobehavior();
        obj.generateObject(affectorObject,user);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(user.transform.position,radius);
        foreach (Collider2D itemCollider2D in colliders)
        {
            GameObject item = itemCollider2D.gameObject;
            if(item == user){
                continue;
            }
            Vector3 forceDirection = -(user.transform.position - item.transform.position);
            forceDirection[2] = 0;
            var rb = item.GetComponent<Rigidbody2D>();
            if(rb){
                rb.AddForce(forceDirection * value,ForceMode2D.Impulse);
            }
            //item.GetComponent<Rigidbody2D>().AddForce(forceDirection * value,ForceMode2D.Impulse);
        }
    }

}

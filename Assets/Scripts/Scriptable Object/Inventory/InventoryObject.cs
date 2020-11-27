using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject, ISerializationCallbackReceiver
{
    public string savePath;
    private ItemDatabaseObject database;
    public List<InventorySlot> Container = new List<InventorySlot>();
    
    private void OnEnable(){
#if UNITY_EDITOR
        database = (ItemDatabaseObject)AssetDatabase.LoadAssetAtPath("Assets/Resources/Database.asset",typeof(ItemDatabaseObject));
#else
        database = Resources.Load<ItemDatabaseObject>("Database");
#endif
    }
    
    public void AddItem(ItemObject _item, int _amount){
        //bool hasItem = false;
        // 如果已經有了該物品，則直接增加存入的數量
        for(int i = 0; i < Container.Count; i++){
            if(Container[i].item == _item){
                Container[i].AddAmount(_amount);
                //hasItem = true;
                //break;
                return;
            }
        }
        // 如果還未持有該物品，則添加該項目到指定數量
        /*
        if(!hasItem){
            Container.Add(new InventorySlot(_item, _amount));
        }
        */
        Container.Add(new InventorySlot(database.GetID[_item],_item, _amount));
    }

    public void save(){
        // 把此物件轉成JSON格式
        string saveData = JsonUtility.ToJson(this, true);
        // 建立轉換成二進制格式的物件
        BinaryFormatter bf = new BinaryFormatter();
        // 建立儲存用的檔案
        FileStream file = File.Create(string.Concat(Application.persistentDataPath, savePath));
        // 序列化存入檔案
        bf.Serialize(file, saveData);
        // 關閉檔案
        file.Close();
    }

    public void load(){
        // 判斷是否存在儲存的檔案
        if(File.Exists(string.Concat(Application.persistentDataPath, savePath))){
            // 建立轉換成二進制格式的物件
            BinaryFormatter bf = new BinaryFormatter();
            // 打開檔案
            FileStream file = File.Open(string.Concat(Application.persistentDataPath, savePath), FileMode.Open);
            // 讀取並覆蓋此物件
            JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), this);
            // 關閉檔案
            file.Close();
        }
    }

    public void OnAfterDeserialize(){
        for (int i = 0; i < Container.Count; i++)
        {
            Container[i].item = database.GetItem[Container[i].ID];
        }
    }

    public void OnBeforeSerialize(){

    }


}

[System.Serializable]
public class InventorySlot{
    public int ID;
    public ItemObject item;
    public int amount;
    public InventorySlot(int _id, ItemObject _item, int _amount){
        ID = _id;
        item = _item;
        amount = _amount;
    }
    public void AddAmount(int value){
        amount += value;
    }
}

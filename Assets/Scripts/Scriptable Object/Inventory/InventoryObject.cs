using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;
using System.Runtime.Serialization;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject//, ISerializationCallbackReceiver
{
    public string savePath;
    public ItemDatabaseObject database;
    public Inventory Container;

    public Item GetItemByItemId(int _ItemId){
        for(int i = 0; i < Container.Items.Count; i++){
            if(Container.Items[i].item.Id == _ItemId){
                return Container.Items[i].item;
            }
        }
        return null;
    }

    public Item GetItemByIndex(int _index){
        for(int i = 0; i < Container.Items.Count; i++){
            if(i == _index){
                return Container.Items[i].item;
            }
        }
        return null;
    }

    public int RemoveItem(Item _item, int _amount){    
        // 已經持有該物品則移除至多指定數量，並返回真實移除的數量
        for(int i = 0; i < Container.Items.Count; i++){
            if(Container.Items[i].item.Id == _item.Id){
                int realRemoveAmount = Container.Items[i].RemoveAmount(_amount);
                // 如果殘餘量為0，則移除該物品
                if( Container.Items[i].GetAmount() == 0){
                    Container.Items.Remove(Container.Items[i]);
                }
                return realRemoveAmount;
            }
        }
        // 未持有該物品則僅返回0，因為沒有移除任何東西
        return 0;
    }

    public void AddItem(Item _item, int _amount){
        // 如果已經有了該物品，則直接增加存入的數量
        for(int i = 0; i < Container.Items.Count; i++){
            if(Container.Items[i].item.Id == _item.Id){
                Container.Items[i].AddAmount(_amount);
                return;
            }
        }
        // 如果還未持有該物品，則添加該項目到指定數量，並新增欄位
        Container.Items.Add(new InventorySlot(_item.Id,_item, _amount));
        return;
    }

    [ContextMenu("Save")]
    public void Save(){
        /*
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
        */
        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Create, FileAccess.Write);
        formatter.Serialize(stream, Container);
        stream.Close();
    }

    [ContextMenu("Load")]
    public void Load(){
        // 判斷是否存在儲存的檔案
        if(File.Exists(string.Concat(Application.persistentDataPath, savePath))){
            /*
            // 建立轉換成二進制格式的物件
            BinaryFormatter bf = new BinaryFormatter();
            // 打開檔案
            FileStream file = File.Open(string.Concat(Application.persistentDataPath, savePath), FileMode.Open);
            // 讀取並覆蓋此物件
            JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), this);
            // 關閉檔案
            file.Close();
            */
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Open, FileAccess.Read);
            Container = (Inventory)formatter.Deserialize(stream);
            stream.Close();
        }
    }

    [ContextMenu("Clear")]
    public void Clear(){
        Container = new Inventory();
    }
    public void Create(){
        Container = new Inventory();
    }

    /*
    public void OnAfterDeserialize(){
        for (int i = 0; i < Container.Items.Count; i++)
        {
            Container.Items[i].item = database.GetItem[Container.Items[i].ID];
        }
    }
    

    public void OnBeforeSerialize(){

    }
    */

}

[System.Serializable]
public class Inventory{
    public List<InventorySlot> Items = new List<InventorySlot>();

}

[System.Serializable]
public class InventorySlot{
    public int ID;
    public Item item;
    public int amount;
    public InventorySlot(int _id, Item _item, int _amount){
        ID = _id;
        item = _item;
        amount = _amount;
    }
    public void AddAmount(int value){
        amount += value;
    }

    public int RemoveAmount(int value){
        if(value <= amount){
            amount -= value;
            return value;
        }else{
            int realRemoveAmount = amount;
            amount = 0;
            return realRemoveAmount;
        }
    }

    public int GetAmount(){
        return amount;
    }

}

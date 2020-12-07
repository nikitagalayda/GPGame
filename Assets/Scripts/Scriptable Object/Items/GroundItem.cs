using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;

public class GroundItem : MonoBehaviour, ISerializationCallbackReceiver
{
    public ItemObject item;
    public void OnAfterDeserialize(){
        //throw new System.NotImplementedException();
    }
    public void OnBeforeSerialize(){
        //GetComponentInChildren<SpriteRenderer>().sprite = item.uiDisplay;
        //EditorUtility.SetDirty(GetComponentInChildren<SpriteRenderer>());
        //throw new System.NotImplementedException();
    }

}

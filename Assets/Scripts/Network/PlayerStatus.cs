using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;



public class PlayerStatus : MonoBehaviourPunCallbacks
{
    public enum Status {
        Slow,
        Freeze,
        Boost,
        EnergyRegenerate,
        EnergyStopLost,
        EnergyDoubleLost,
        CameraShaking,
        length
    }

    public float[] statusTable;

    // Start is called before the first frame update
    void Start()
    {
        statusTable = new float[(int)Status.length];
        for(int i=0; i < statusTable.GetLength(0); i++){
            statusTable[i] = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        processStatus();
    }

    void processStatus(){
        for (int i = 0; i < statusTable.Length; i++)
        {
            if(statusTable[i] > 0)
            {
                statusTable[i] -= Time.deltaTime;
                if(statusTable[i] <= 0)
                {
                    Debug.Log("remove effect index: " + i);
                    statusTable[i] = 0;
                    switch(i)
                    {
                        case (int)Status.Slow:
                            SlowEffectEnd();
                            break;
                        case (int)Status.Boost:
                            BoostEffectEnd();
                            break;
                        case (int)Status.EnergyStopLost:
                            EnergyStopLostEnd();
                            break;
                        case (int)Status.EnergyRegenerate:
                            EnergyRegenerateEnd();
                            break;
                        case (int)Status.EnergyDoubleLost:
                            EnergyDoubleLostEnd();
                            break;
                        case (int)Status.Freeze:
                            FreezeEffectEnd();
                            break;
                    }
                }
            }
        }
    }

    public void StartAndAddEffect(int i,float t){
        if(statusTable[i] > 0)
        {
            statusTable[i] += t;
        }else{
            Debug.Log("add effect index: " + i + "add effect time: " + t);
            statusTable[i] = t;
            switch(i)
            {
                case (int)Status.Slow:
                    SlowEffectStart();
                    break;
                case (int)Status.Boost:
                    BoostEffectStart();
                    break;
                case (int)Status.EnergyStopLost:
                    EnergyStopLostStart();
                    break;
                case (int)Status.EnergyRegenerate:
                    EnergyRegenerateStart();
                    break;
                case (int)Status.EnergyDoubleLost:
                    EnergyDoubleLostStart();
                    break;
                case (int)Status.Freeze:
                    FreezeEffectStart();
                    break;
                case (int)Status.CameraShaking:
                    ShakeEffectStart();
                    break;
            }
        }
    }

    public int GetStatusIdByName(string s){
        return (int)Enum.Parse(typeof(Status), s);
    }

    void SlowEffectStart(){
        this.gameObject.GetComponent<PlayerManager>().movePower = this.gameObject.GetComponent<PlayerManager>().movePower/2;
    }
    void SlowEffectEnd(){
        this.gameObject.GetComponent<PlayerManager>().movePower = this.gameObject.GetComponent<PlayerManager>().movePower*2;
    }
    void BoostEffectStart(){
        this.gameObject.GetComponent<PlayerManager>().movePower = this.gameObject.GetComponent<PlayerManager>().movePower*2;
    }
    void BoostEffectEnd(){
        this.gameObject.GetComponent<PlayerManager>().movePower = this.gameObject.GetComponent<PlayerManager>().movePower/2;
    }
    void EnergyStopLostStart(){
        this.gameObject.GetComponent<PlayerManager>().energyNaturalRecoveryRate 
        = this.gameObject.GetComponent<PlayerManager>().energyNaturalLostRate;
    }
    void EnergyStopLostEnd(){
        this.gameObject.GetComponent<PlayerManager>().energyNaturalRecoveryRate = 0;
    }
    void EnergyRegenerateStart(){
        this.gameObject.GetComponent<PlayerManager>().energyNaturalRecoveryRate 
        = this.gameObject.GetComponent<PlayerManager>().energyNaturalLostRate*2;
    }
    void EnergyRegenerateEnd(){
        this.gameObject.GetComponent<PlayerManager>().energyNaturalRecoveryRate = 0;        
    }
    void EnergyDoubleLostStart(){
        this.gameObject.GetComponent<PlayerManager>().energyNaturalRecoveryRate = -this.gameObject.GetComponent<PlayerManager>().energyNaturalLostRate;
    }
    void EnergyDoubleLostEnd(){
        this.gameObject.GetComponent<PlayerManager>().energyNaturalRecoveryRate = 0;
    }
    void FreezeEffectStart(){
        this.gameObject.GetComponent<PlayerManager>().enabled = false;
        this.gameObject.GetComponent<SimpleTeleport_NetworkVersion>().enabled = false;
    }
    void FreezeEffectEnd(){
        this.gameObject.GetComponent<PlayerManager>().enabled = true;
        this.gameObject.GetComponent<SimpleTeleport_NetworkVersion>().enabled = true;    
    }
    void ShakeEffectStart(){
        if(Camera.main.GetComponent<CameraShake>())
            StartCoroutine(Camera.main.GetComponent<CameraShake>().Shake(3f, .4f));
    }

}

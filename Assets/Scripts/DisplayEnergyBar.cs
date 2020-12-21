using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class DisplayEnergyBar : MonoBehaviour
{
    public GameObject player;
    public float value;
    private Slider energyBar;

    // Start is called before the first frame update
    void Start()
    {
        energyBar = gameObject.GetComponent<Slider>();
        if(energyBar != null){
            Debug.Log("set energyBar, success");
        }else{
            Debug.Log("set energyBar, failed");
        }

    }

    // Update is called once per frame
    void Update()
    {
        value = player.GetComponent<PlayerManager>().energy/ player.GetComponent<PlayerManager>().maxEnergy;
        energyBar.value = value;
    }
}

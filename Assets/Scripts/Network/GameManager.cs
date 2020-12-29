﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;


using UnityEngine.UI;
using System.Linq;
public class GameManager : MonoBehaviourPunCallbacks
{
    #region Public Fields
    [Tooltip("The prefab to use for representing the player")]
    public GameObject playerPrefab;
    public GameObject playerSpawnLocation;
    public GameObject itembar;
    public GameObject energybar;
    public ItemDatabaseObject database;
    //public InventoryObject playerInventory;
    //public GameObject player;
    #endregion
    public static GameManager Instance;

    public bool gameStart = false;
    private bool dropping = false;
    public GameObject[] Rankings;

    [Tooltip("the spaceShip")]
    [SerializeField]
    private GameObject spaceShip;
    public GameObject leaveButton;
    public GameObject dieText;
    public GameObject[] dieImages;
    public float gravity = 0.5F;
    private GameObject[] Players;
    private List<GameObject> Playerlist = new List<GameObject>();
    public GameObject[] floor ;
    public GameObject lava;
    private float playerHeight = 0.05f;
    private float elp = 5f;
    // Start is called before the first frame update
    void Awake()
    {
    
    }

    void Start()
    {
        Instance = this;
        PlayerSetup();
        StartGame();
        //for(int i = Players.Length; i < 4; i++)
        //    Rankings[i].SetActive(false);
//        Vector2 randPosition = new Vector2(Random.Range(-8.0f, 8.0f), -100.0f);
        // if(PhotonNetwork.IsMasterClient)
        // {
        //     
        //     for(int i = 0; i< 200;i++)
        //     {
        //         /*int randBox = Random.Range(0, 2);
        //         float rand = Random.Range(-8.0f, 5.0f);
        //         PhotonNetwork.InstantiateSceneObject(this.floor.name, new Vector3(rand, -102f+(60f*playerHeight)*(float)(i+1), -5), Quaternion.identity, 0);
        //         for(int j = 0;j< randBox;j++)
        //             PhotonNetwork.InstantiateSceneObject("Drop", new Vector3(rand, -102f+(60f*playerHeight)*(float)(i+1)+1, -5), Quaternion.identity);

        //         if(Mathf.Abs(rand+1.5f)>3f)
        //         {
        //             if(rand>0){
        //                 PhotonNetwork.InstantiateSceneObject(this.floor.name, new Vector3(-8+5-rand, -102f+(60f*playerHeight)*(float)(i+1), -5), Quaternion.identity, 0);
        //                 for(int j = 0;j< randBox;j++)
        //                     PhotonNetwork.InstantiateSceneObject("Drop", new Vector3(-8+5-rand, -102f+(60f*playerHeight)*(float)(i+1)+1, -5), Quaternion.identity, 0);
        //             }
        //             else
        //             {
        //                 PhotonNetwork.InstantiateSceneObject(this.floor.name, new Vector3(-8-rand+5, -102f+(60f*playerHeight)*(float)(i+1), -5), Quaternion.identity, 0);
        //                 for(int j = 0;j< randBox;j++)
        //                     PhotonNetwork.InstantiateSceneObject("Drop", new Vector3(-8-rand+5, -102f+(60f*playerHeight)*(float)(i+1)+1, -5), Quaternion.identity, 0);

        //             }

        //         }*/
        //         //PhotonNetwork.Instantiate(this.floor.name, new Vector3(Random.Range(-8.0f, 5.0f), -102f+(40f*playerHeight)*(float)(i+1), -5), Quaternion.identity, 0);
        //         GameObject flr = leaveButton;
        //         float pastflrposition=  -1000.0f;
        //         for (float j = -7.0f; j <= 7.0f; j=j+ 2.5f)
        //         {
        //             int rand = Random.Range(0, 14);
        //             int randColor = Random.Range(0, this.floor.Length);
        //             if (rand == 0)
        //             {
        //                 if (j == pastflrposition + 2.5f)
        //                 {
        //                     flr.transform.localScale += new Vector3(2.5f, 0, 0);
        //                     flr.transform.position += new Vector3(1.25f, 0, 0);
        //                     pastflrposition = j;
        //                 }
        //                 else
        //                 {
        //                     flr = PhotonNetwork.Instantiate(this.floor[randColor].name, new Vector3(j, -102f + (60f * playerHeight) * (float)(i + 1), -5), Quaternion.identity, 0);
        //                     pastflrposition = j;
        //                     PhotonNetwork.Instantiate("collectableBomb", new Vector3(j, -102f + (60f * playerHeight) * (float)(i + 1) +1, -5), Quaternion.identity, 0);
        //                 }
        //             }
        //         }
        //         for (float j = -7.0f; j <= 7.0f; j = j + 1.25f)
        //         {
        //             int rand = Random.Range(0, 9);
        //             if (rand == 0) PhotonNetwork.Instantiate("Drop", new Vector3(j, -102f + (60f * playerHeight) * (float)(i + 1) + 1, -5), Quaternion.identity);
        //         }
        //     }
        
        
        /*
        var player = PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector2(playerSpawnLocation.transform.position[0],playerSpawnLocation.transform.position[1]), Quaternion.identity, 0);
        player.GetComponent<PlayerManager>().inventory = (InventoryObject)InventoryObject.CreateInstance("InventoryObject");
        player.GetComponent<PlayerManager>().inventory.savePath += "." + player.GetComponent<PhotonView>().ViewID;
        player.GetComponent<PlayerManager>().inventory.database = database;
        player.GetComponent<PlayerManager>().inventory.Create();
        itembar.GetComponent<DisplayInventory>().inventory = player.GetComponent<PlayerManager>().inventory;
        */
        /*
        for(int i = 0; i < Players.Count(); i++){
            if (Players[i].GetComponent<PhotonView>().IsMine){
                Debug.Log("set inventory");
                itembar = GameObject.Find("ItemBar");
                itembar.GetComponent<DisplayInventory>().inventory = Players[i].GetComponent<PlayerManager>().inventory;
                energybar = GameObject.Find("EnergyBar");
                energybar.GetComponent<DisplayEnergyBar>().player = Players[i];
            }
        }
        */
        //Debug.Log("game manager player:"+WaitingManager.player.name);
        //StartGenerator();
    }

    // Update is called once per frame
    void Update()
    {

        spaceShip.transform.position = spaceShip.transform.position - new Vector3(0, Time.fixedDeltaTime * 0.1f, 0);
        /*if(gameStart && dropping == false)
        {

            dropping = true;
            startButton.SetActive(false);

        }
        Rankings[0].text = Players.Length.ToString();*/
        
    }
    void LateUpdate()
    {

        float[] height = new float[5];
        for (int i = 0; i < Players.Length; i++)
        {
            if(Players[i]!=null)
                height[i] = Players[i].transform.position[1];
        }

        for (int i = 0; i < Players.Length; i++)
        {
            float max = -999;
            int y = -1;
            for (int j = 0;j < Players.Length; j++)
            {
                if(height[j]> max)
                {
                    max = height[j];
                    y = j;
                }
            }
            height[y] = -1000;
            /*
            if(Players[y]!=null)
                {Rankings[i].GetComponent<Text>().text = (i + 1).ToString("0") + ". " + Players[y].GetComponent<SimpleTeleport_NetworkVersion>().photonView.Owner.NickName + " : " + Players[y].transform.position[1].ToString("0") + " m";
                //dieImages[i].SetActive(true);
                if (Players[y].GetComponent<SimpleTeleport_NetworkVersion>().photonView.IsMine)
                    Rankings[i].GetComponent<Text>().color = Color.red;
                if(Players[y].GetComponent<SimpleTeleport_NetworkVersion>().imDie)
                    dieImages[i].SetActive(true);
                }
            Rankings[4].GetComponent<Text>().text = "Lava Height : " + lava.transform.position[1].ToString("0"); 
            */
        }
    }

    // private IEnumerator EnemyGenerator()
    // {
    //     while (true)
    //     {
    //         for (float j = -7.0f; j <= 7.0f; j = j + 1.25f)
    //         {
    //             int rand = Random.Range(0, 9);
    //             if (rand == 0) PhotonNetwork.Instantiate("Drop", new Vector3(j, -102f + (60f * playerHeight) * (float)(199 + 1) + 1, -5), Quaternion.identity);
    //         }
    //         yield return new WaitForSeconds(1.5f);
    //     }
    // }

    // public void StartGenerator()
    // {
    //     StartCoroutine(EnemyGenerator());
    // }

    public void StartGame()
    {
        gameStart = true;
        Players = GameObject.FindGameObjectsWithTag("player");
        // now all your game objects are in GOs,
        // all that remains is to getComponent of each and every script and you are good to go.
        // to disable a components

    }

    #region Photon Callbacks


    /// <summary>
    /// Called when the local player left the room. We need to load the launcher scene.
    /// </summary>
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Launcher");
    }

    public override void OnPlayerEnteredRoom(Player other)
    {
        Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting


        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom


            LoadArena();
        }
    }


    public override void OnPlayerLeftRoom(Player other)
    {
        Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects


        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
            //startButton.SetActive(true);

            LoadArena();
        }
    }

    #endregion


    #region Public Methods


    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }


    #endregion
    


    #region Method
    void LoadArena()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
        }
        Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
        //PhotonNetwork.LoadLevel("RoomForNetwork");
    }

    void PlayerSetup(){
        var Playerlist = GameObject.FindGameObjectsWithTag("player");
        for(int i = 0; i < Playerlist.Count(); i++){
            if (Playerlist[i].GetComponent<PhotonView>().IsMine){
                var myPlayer = Playerlist[i];
                Debug.Log("set inventory");
                itembar = GameObject.Find("ItemBar");
                itembar.GetComponent<DisplayInventory>().inventory = myPlayer.GetComponent<PlayerManager>().inventory;
                energybar = GameObject.Find("EnergyBar");
                energybar.GetComponent<DisplayEnergyBar>().player = myPlayer;
            }
        }
    }

    #endregion





}

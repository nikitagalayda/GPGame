using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine.UI;

public class WaitingManager : MonoBehaviourPunCallbacks, IOnEventCallback
{
    #region Public Fields
    [Tooltip("The prefab to use for representing the player")]
    public GameObject playerPrefab;
    public static GameManager Instance;
    #endregion
    public bool gameStart = false;
    private bool dropping = false;
    public Text countdownText;
    [Tooltip("start Button for the UI")]
    [SerializeField]
    private GameObject startButton;
    public float gravity = 0.5F;
    private GameObject[] Players;
    private List<Text> Rankings = new List<Text>();
    private List<GameObject> playerList = new List<GameObject>();
    // Start is called before the first frame update
    private float timeLeft = 3;
    private bool everyoneReady = false;
    public const byte startEventNum = 1;

    void Start()
    {


        if (playerPrefab == null)
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'",this);
        }
        else
        {
            Debug.LogFormat("We are Instantiating LocalPlayer from {0}", Application.loadedLevelName);
            // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
            if (SimpleTeleport_NetworkVersion.LocalPlayerInstance == null)
            {
                Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
                // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                playerList.Add(PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector2(Random.Range(-6.0f, 6.0f), -100f), Quaternion.identity, 0));
                

            }
            else
            {
                Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
            }
            
        }
    }

    void Awake()
    {
        startButton.SetActive(false);
        countdownText.gameObject.SetActive(false);
        if (PhotonNetwork.IsMasterClient)
            startButton.SetActive(true);
        
    }
    // Update is called once per frame
    void Update()
    {

    }




    public void StartGame()
    {
        everyoneReady = true;
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All }; // You would have to set the Receivers to All in order to receive this event on the local client as well
        PhotonNetwork.RaiseEvent(startEventNum, "start",raiseEventOptions, SendOptions.SendReliable);
        
        StartCoroutine(startAfterTime(3));

        //PhotonNetwork.LoadLevel("RoomForNetwork");
    }
    #region Photon Callbacks
    public void OnEvent(EventData photonEvent)
    {
        // Do something
        if(photonEvent.Code == startEventNum)
            CountToStart();
    }

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
        PhotonNetwork.CurrentRoom.IsOpen = true; 
        countdownText.gameObject.SetActive(false);
        timeLeft = 3;
        if (PhotonNetwork.IsMasterClient)
        {
            everyoneReady = false;
            Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom

            startButton.SetActive(true);

            LoadArena();
        }
    }

    

    #endregion


    #region Public Methods

    
    public void CountToStart()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false; 
        countdownText.gameObject.SetActive(true);
        StartCoroutine(ExecuteAfterTime(2,1));
        StartCoroutine(ExecuteAfterTime(1,2));
    }
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }


    #endregion
    
    #region Private Methods


    void LoadArena()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
        }
        Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
        //PhotonNetwork.LoadLevel("RoomForNetwork");
        }
    IEnumerator ExecuteAfterTime(float num,float time)
    {
        yield return new WaitForSeconds(time);
        countdownText.text = "Start in " + num.ToString("0");
        // Code to execute after the delay
    }
    IEnumerator startAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        if(everyoneReady)
            PhotonNetwork.LoadLevel("RoomForNetwork");
            PhotonNetwork.CurrentRoom.IsOpen = false; 
            //RoomOptions roomOptions = new RoomOptions();
            //PhotonNetwork.SendOptions
        // Code to execute after the delay
    }

    
    #endregion





}

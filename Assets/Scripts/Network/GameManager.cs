using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;
using System.Collections;

public class GameManager : MonoBehaviourPunCallbacks
{
    #region Public Fields
    [Tooltip("The prefab to use for representing the player")]
    public GameObject playerPrefab;
    public GameObject playerSpawnLocation;
    //public GameObject player;
    #endregion
    public static GameManager Instance;
    public bool gameStart = false;
    private bool dropping = false;
    [Tooltip("start Button for the UI")]
    [SerializeField]
    private GameObject startButton;
    // Start is called before the first frame update
    void Start()
    {


        Instance = this;
        PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector2(playerSpawnLocation.transform.position[0],playerSpawnLocation.transform.position[1]), Quaternion.identity, 0);
        /*
        if (playerPrefab == null)
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'",this);
        }
        else
        {
            GameObject player;
            Debug.LogFormat("We are Instantiating LocalPlayer from {0}", Application.loadedLevelName);
            // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
            if (SimpleTeleport_NetworkVersion.LocalPlayerInstance == null)
            {
                Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
                // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector2(Random.Range(-6.0f, 6.0f), -3f), Quaternion.identity, 0);
                

            }
            else
            {
                Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
            }
            
        }
        */
    }

    // Update is called once per frame
    void Update()
    {
        if(gameStart && dropping == false)
        {
            dropping = true;
            StartGenerator();
            startButton.SetActive(false);
        }    
    }

    private IEnumerator EnemyGenerator()
    {
        while (true)
        {
            Vector2 randPosition = new Vector2(Random.Range(-8.0f, 8.0f), 6.0f);
            PhotonNetwork.Instantiate("Drop", randPosition, Quaternion.identity);
            yield return new WaitForSeconds(Mathf.Lerp(0.1f, 1.0f, Random.value));
        }
    }

    public void StartGenerator()
    {
        StartCoroutine(EnemyGenerator());
    }

    public void StartGame()
    {
        gameStart = true;
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


    #endregion

}

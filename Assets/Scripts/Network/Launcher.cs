﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class Launcher : MonoBehaviourPunCallbacks
{
        #region Private Serializable Fields
        [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
        [SerializeField]
        private byte maxPlayersPerRoom = 4;
        [Tooltip("The Ui Panel to let the user enter name, connect and play")]
        [SerializeField]
        private GameObject controlPanel;
        [Tooltip("The UI Label to inform the user that the connection is in progress")]
        [SerializeField]
        private GameObject progressLabel;

        Text roomName;


        #endregion


        #region Private Fields
        /// <summary>
        /// This client's version number. Users are separated from each other by gameVersion (which allows you to make breaking changes).
        /// </summary>
        bool isConnecting;
        string gameVersion = "1";
        #endregion


        #region MonoBehaviour CallBacks
        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during early initialization phase.
        /// </summary>
        void Awake()
        {
            // #Critical
            // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during initialization phase.
        /// </summary>
        void Start()
        {
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);
        }
        #endregion

        #region MonoBehaviourPunCallbacks Callbacks
        public override void OnConnectedToMaster()
        {
            Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
            Debug.Log(isConnecting);
            if (isConnecting)
            {
                // #Critical: The first we try to do is to join a potential existing room. If there is, good, else, we'll be called back with OnJoinRandomFailed()
                //if(GameObject.Find("RoomName").GetComponent<InputField>().text=="")
                if(roomName.text == "")
                {
                    PhotonNetwork.JoinRandomRoom();
                    isConnecting = false;
                }
                else
                {
                    RoomOptions roomOptions = new RoomOptions();
                    PhotonNetwork.JoinOrCreateRoom(roomName.text, roomOptions, null);
                    isConnecting = false;
                }

            }
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);
            Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");

            // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
            //if(roomInputField.GetComponent<Text>().text == "")
                PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
            //else{
            //    PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
            //}
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                Debug.Log("We initial the room for the game ");
                // #Critical
                // Load the Room Level.
                PhotonNetwork.LoadLevel("WaitingRoom");
            }
        }

        #endregion
        


        #region Public Methods
        /// <summary>
        /// Start the connection process.
        /// - If already connected, we attempt joining a random room
        /// - if not yet connected, Connect this application instance to Photon Cloud Network
        /// </summary>
        public void Connect(Text room)
        {
            roomName = room;
            progressLabel.SetActive(true);
            controlPanel.SetActive(false);


            // we check if we are connected or not, we join if we are , else we initiate the connection to the server.
            if (PhotonNetwork.IsConnected)
            {
                // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
                if(roomName.text == "")
                {
                    PhotonNetwork.JoinRandomRoom();
                    isConnecting = false;
                }
                else
                {
                    RoomOptions roomOptions = new RoomOptions();
                    PhotonNetwork.JoinOrCreateRoom(roomName.text, roomOptions, null);
                    isConnecting = false;
                }
            }
            else
            {
                // #Critical, we must first and foremost connect to Photon Online Server.
                isConnecting = PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = gameVersion;
            }
        }
    #endregion


}

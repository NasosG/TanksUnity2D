using UnityEngine;
using System.Collections;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using TMPro;

namespace MultiplayerTanks
{
    public class NetworkConnectionManager : MonoBehaviourPunCallbacks
    {
        //buttons
        public Button BtnConnectMaster;         //"connect to master" button
        public Button BtnConnectRoom;           //"join random room" button
        
        //input fields
        public TMP_InputField playerNameInput;  //playername input text field with text mesh pro

        protected bool TriesToConnectToMaster;  //bool that shows if user has pressed to connect to the master
        protected bool TriesToConnectToRoom;    //bool that shows if user has pressed to connect to a room

        public static string playeroname;       //player's name text
        public string playerName = "";

        // Use this for initialization
        void Start()
        {
            DontDestroyOnLoad(this);
            TriesToConnectToMaster = false;
            TriesToConnectToRoom   = false;
            //PhotonNetwork.SendRate = 30;
            //PhotonNetwork.SerializationRate = 20;
        }

        // Update is called once per frame
        void Update()
        {
                if (BtnConnectMaster != null)
                    //activate connect to master button when photon is not connected and our bool is false
                    BtnConnectMaster.gameObject.SetActive(!PhotonNetwork.IsConnected && !TriesToConnectToMaster);
                if (BtnConnectRoom != null) {
                    Debug.Log(PhotonNetwork.IsConnected +"  "+ !TriesToConnectToMaster + "  " + !TriesToConnectToRoom);
                    //don't let user interact with the input field after he has set his/her name and has connected to photon
                    playerNameInput.interactable = !(PhotonNetwork.IsConnected && !TriesToConnectToMaster && !TriesToConnectToRoom);
                    //activate join room button
                    BtnConnectRoom.gameObject.SetActive(PhotonNetwork.IsConnected && !TriesToConnectToMaster && !TriesToConnectToRoom);
                }

        }

        public void OnClickConnectToMaster()
        {
            TriesToConnectToMaster = true;

            //Settings (all optional)
            PhotonNetwork.OfflineMode = false;                      //true would "fake" an online connection
            if (playerNameInput.text != null)
                PhotonNetwork.NickName = playerNameInput.text;      //to set a player name
            else
                PhotonNetwork.NickName = "PlayerName";              //to set a player name
            playeroname = PhotonNetwork.NickName;
            Debug.Log("das" + PhotonNetwork.NickName);
            PhotonNetwork.AutomaticallySyncScene = true;            //to call PhotonNetwork.LoadLevel()
            PhotonNetwork.GameVersion = "v1";                       //only people with the same game version can play together

            //PhotonNetwork.ConnectToMaster(ip,port,appid); //manual connection
            if(!PhotonNetwork.OfflineMode)
                PhotonNetwork.ConnectUsingSettings();               //automatic connection based on the config file in Photon/PhotonUnityNetworking/Resources/PhotonServerSettings.asset

        }

        //successful connection to the master
        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();
            TriesToConnectToMaster = false; //we made the connection and we do not try to connect to the master anymore
            Debug.Log("Connected to Master!");
        }

        //when kicked out of server or not having an internet connection
        public override void OnDisconnected(DisconnectCause cause)
        {
            base.OnDisconnected(cause);
            TriesToConnectToMaster = false;
            TriesToConnectToRoom   = false;
            Debug.Log(cause); //what went wrong??
        }

        public void OnClickConnectToRoom()
        {
            if (!PhotonNetwork.IsConnected)
                return;

            TriesToConnectToRoom = true;
            //PhotonNetwork.CreateRoom("Peter's Game 1"); //Create a specific Room - Error: OnCreateRoomFailed
            //PhotonNetwork.JoinRoom("Peter's Game 1");   //Join a specific Room   - Error: OnJoinRoomFailed  
            PhotonNetwork.JoinRandomRoom();               //Join a random Room     - Error: OnJoinRandomRoomFailed  
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            base.OnJoinRandomFailed(returnCode, message);
            //no room available
            //create a room (null as a name means "does not matter")
            //where 2 players can join and play 
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });
        }

        //room creation failed
        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            base.OnCreateRoomFailed(returnCode, message);
            Debug.Log(message);
            base.OnCreateRoomFailed(returnCode, message);
            TriesToConnectToRoom = false;
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            TriesToConnectToRoom = false;
            
            Debug.Log("Master: " + PhotonNetwork.IsMasterClient + " | Players In Room: " + PhotonNetwork.CurrentRoom.PlayerCount + " | RoomName: " + PhotonNetwork.CurrentRoom.Name + " Region: " + PhotonNetwork.CloudRegion);
            //if(PhotonNetwork.IsMasterClient && SceneManager.GetActiveScene().name != "Network")
            //    PhotonNetwork.LoadLevel("Network");
            if(PhotonNetwork.IsMasterClient && SceneManager.GetActiveScene().name != "Game")
                PhotonNetwork.LoadLevel("Game"); //Load Game Scene
        }


        #region Public Methods

        /// <summary>
        /// Sets the name of the player, and save it in the PlayerPrefs for future sessions.
        /// </summary>
        /// <param name="value">The name of the Player</param>
        public void SetPlayerName(string value)
        {
            // #Important
            if (string.IsNullOrEmpty(value))
            {
                Debug.LogError("Player Name is null or empty");
                return;
            }
            PhotonNetwork.NickName = value;

            PlayerPrefs.SetString(playeroname, value);
        }

        #endregion

    }
}

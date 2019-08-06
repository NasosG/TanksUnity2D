using UnityEngine;
using System.Collections;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using ExitGames.Client.Photon;

namespace MultiplayerTanks
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        public GameObject player;
        [Header("UC Game Manager")]
        public PhotonView photonView;
        public bool gameExists = false;
        //UI ui;

        [HideInInspector]
        public TankMultiplayer LocalPlayer;
        public Game game;

        [Header("Setup")]
        public Color playerMultiplayerColor = Color.blue;   //The colour of that player in multiplayer
        public Color playerMultiplayer2Color = Color.red;
        public bool oneHitKill = false;                     //Will a projectile instantly kill its target?
        public bool canDamageOwnTank = true;                //Can a tank damage itself by shooting a projectile?
        public int respawnDelay = 1;                        //The amount of time a player will wait between dying and respawning.
        public int maxScore = 10;                           //The score that when a player reaches, will end the game.
                                                            //public int maxProjectileBounces = 4;				//The maximum amount of times a projectile can bounce off walls.

        [Space(10)]
        public int tankStartHealth = 3;                     //The health the player tanks will get when the game starts.
        public int tankStartDamage = 1;                     //The damage the player tanks will get when the game starts.
        public float tankStartMoveSpeed = 70;               //The move speed the player tanks will get when the game starts.
        public float tankStartTurnSpeed = 100;              //The turn speed the player tanks will get when the game starts.
        public float tankStartProjectileSpeed = 13;         //The projectile speed the player tanks will get when the game starts.
        public float tankStartReloadSpeed = 1;              //The reload speed the player tanks will get when the game starts.
        
        [Header("Scores")]
        public int player1Score;                            //Player 1's score.
        public int player2Score;                            //Player 2's score.

        private void Awake()
        {
            PhotonNetwork.SendRate = 30;
            PhotonNetwork.SerializationRate = 20;
            /* if (!PhotonNetwork.IsConnected)
             {
                 SceneManager.LoadScene("Menu");
                 return;
             }*/
        }

        private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "Game")
                spawnPlayer();
        }

        void spawnPlayer() {
            /*if (PhotonNetwork.LocalPlayer == PhotonNetwork.PlayerList[0]) {
                photonView.RPC("updateName", PhotonNetwork.PlayerList[1], PhotonNetwork.PlayerList[1].NickName);
            }
            if (PhotonNetwork.LocalPlayer == PhotonNetwork.PlayerList[1]) {
                photonView.RPC("updateName", PhotonNetwork.PlayerList[0], PhotonNetwork.PlayerList[0].NickName);
            }*/
            if (MenuUI.getFlag() == 1 && photonView.IsMine)
               // LocalPlayer = PhotonNetwork.Instantiate("PlayerMultiplayer", Vector3.zero, Quaternion.identity).GetComponent<TankMultiplayer>();
             PhotonNetwork.Instantiate("PlayerMultiplayer", player.transform.position, player.transform.rotation, 0);
        }
        
        // Use this for initialization
        void Start()
        {
            if (MenuUI.getFlag() == 1) {
                gameExists = true;
                player = PhotonNetwork.Instantiate("PlayerMultiplayer", player.transform.position, player.transform.rotation, 0);
            }
        }

        public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player target, ExitGames.Client.Photon.Hashtable changedProps)
        {
            foreach(var change in changedProps)
                Debug.Log("Property " + change.Key + " of player " + target.UserId + " changed to " + change.Value);
        }
    }
}

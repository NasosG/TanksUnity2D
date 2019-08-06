using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Photon.Pun;
using MultiplayerTanks;

public class PlayerName : MonoBehaviour
{
    public Text nameTag;
    public GameObject canvas;
    public Quaternion iniRot;

    void Awake()
    {
        canvas = GameObject.Find("CanvasName");
        iniRot = canvas.transform.rotation;
        //Debug.Log(PhotonNetwork.NickName);
        //PhotonView.Get(this).RPC("updateName", RpcTarget.AllBuffered, PhotonNetwork.NickName);
        PhotonView.Get(this).RPC("updateName", RpcTarget.AllBufferedViaServer, PhotonNetwork.NickName);
    }

    [PunRPC]
    public void updateName(string name/*, int health*/)
    {
        nameTag.text = name /*+ " / " + health*/;
        //canvas = GameObject.Find("CanvasName");
        //nameTag.transform.position = transform.position + new Vector3(0, 2.5f, 0);
        //canvas.transform.position = new Vector3(tank.transform.position.x, tank.transform.position.y, tank.transform.position.z - 1);//tank.transform.position + new Vector3(0, 2.5f, 0);//new Vector3(canvas.transform.position.x, 0, 0);
        // Debug.Log(nameTag.text);
    }

    void Update()
    {    
        transform.rotation = iniRot;
        //canvas.transform.position = tank.transform.position + new Vector3(0, 2.5f, 0);//new Vector3(canvas.transform.position.x, 0, 0);
    }

}
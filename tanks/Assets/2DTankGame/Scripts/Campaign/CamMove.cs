using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMove : MonoBehaviour
{
    public Transform target;
    public float distance;

    void Update()
    {
        Vector3 PlayerPOS = target.transform.position;
        transform.position = new Vector3(PlayerPOS.x, PlayerPOS.y, PlayerPOS.z - distance);
    }

}

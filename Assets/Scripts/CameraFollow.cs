using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Cinemachine;

public class CameraFollow : NetworkBehaviour
{
    // This sets the Cinemachine camera to follow the active player
    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;
        CinemachineVirtualCamera cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CinemachineVirtualCamera>();
        cam.Follow = transform;
        cam.LookAt = transform;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

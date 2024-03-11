using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerSpawn : NetworkBehaviour
{
    [SerializeField] private float spawnPosRange = 5f;

    public override void OnNetworkSpawn()
    {
        float xPos = Random.Range(-spawnPosRange, spawnPosRange);
        float zPos = Random.Range(-spawnPosRange, spawnPosRange);
        transform.position = new Vector3(xPos, 0, zPos);
    }
}

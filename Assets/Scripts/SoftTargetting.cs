using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class SoftTargetting : NetworkBehaviour
{
    private GameObject[] players;
    public Transform currentTarget;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTarget == null)
        {
            players = GameObject.FindGameObjectsWithTag("Player");

            if (players.Length > 1)
            {
                foreach (var item in players)
                {
                    var no = item.GetComponent<NetworkObject>().OwnerClientId;
                    Debug.Log("Client ID: " + OwnerClientId + " item ID: " + no);
                    if (no != this.OwnerClientId)
                    {
                        currentTarget = item.transform;
                        Debug.Log("Client ID: " + OwnerClientId + " assigned target id: " + no);
                    }
                }

            }
            
        }
    }
}

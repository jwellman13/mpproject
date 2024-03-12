using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using Unity.Collections;

public class PlayerData : NetworkBehaviour
{
    [SerializeField]
    private TextMeshProUGUI playerName;

    private NetworkVariable<FixedString128Bytes> netPlayerName = new NetworkVariable<FixedString128Bytes>
        ("Player: 0", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public override void OnNetworkSpawn()
    {
        netPlayerName.Value = "Player: " + (OwnerClientId + 1);
        playerName.text = netPlayerName.Value.ToString();
    }
}

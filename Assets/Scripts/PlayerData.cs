using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using Unity.Collections;
using UnityEngine.UI;

public class PlayerData : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI playerName;
    [SerializeField] private Image healthImage;

    private NetworkVariable<float> netPlayerHealth = new NetworkVariable<float>
        (100f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    private NetworkVariable<FixedString128Bytes> netPlayerName = new NetworkVariable<FixedString128Bytes>
        ("Player: 0", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);


    public override void OnNetworkSpawn()
    {
        netPlayerName.Value = "Player: " + (OwnerClientId + 1);
        playerName.text = netPlayerName.Value.ToString();
        netPlayerHealth.OnValueChanged += OnHealthChanged;
    }

    public override void OnNetworkDespawn()
    {
        netPlayerHealth.OnValueChanged -= OnHealthChanged;

    }

    public void OnHealthChanged(float previous, float current)
    {
        Vector3 scale = healthImage.rectTransform.localScale;
        healthImage.rectTransform.localScale = new Vector3(current / 100, scale.y, scale.z);
    }

    [ServerRpc(RequireOwnership = false)]
    public void TakeDamageServerRpc()
    {
        Debug.Log(netPlayerName.Value + " has been hit!");
        netPlayerHealth.Value -= 25f;
        
    }
}

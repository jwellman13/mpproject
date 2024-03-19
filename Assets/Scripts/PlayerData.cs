using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using Unity.Collections;
using UnityEngine.UI;

public class PlayerData : NetworkBehaviour
{
    // This script manages the player's health and about 20 other
    // things that it shouldn't do
    [SerializeField] private TextMeshProUGUI playerName;
    [SerializeField] private Image healthImage;
    [SerializeField] private PlayerMove move;
    [SerializeField] private PlayerShoot shoot;

    //Yes, this is an incredibly lazy way to do this.
    [SerializeField] AudioSource playerHit;
    [SerializeField] AudioSource playerDeath;


    private NetworkVariable<float> netPlayerHealth = new NetworkVariable<float>
        (100f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    private NetworkVariable<FixedString128Bytes> netPlayerName = new NetworkVariable<FixedString128Bytes>
        ("Player: 0", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);


    public override void OnNetworkSpawn()
    {
        // Registers the player's name and starting health
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

        playerHit.Play();

        // Disables the shooting and movement when a player dies
        if (current < 1f)
        {
            move.SetDeath();
            shoot.SetDeath();
            playerDeath.Play();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void TakeDamageServerRpc()
    {
        netPlayerHealth.Value -= 25f;       
    }
}

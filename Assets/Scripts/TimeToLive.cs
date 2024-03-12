using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class TimeToLive : NetworkBehaviour
{
    public float ttl = 2f;

    private void Start()
    {
        StartCoroutine(DestroyAfterDelay(ttl));
    }

    [ServerRpc(RequireOwnership = false)]
    private void DestroyObjectServerRpc()
    {
        GetComponent<NetworkObject>().Despawn();
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        DestroyObjectServerRpc();
        Destroy(gameObject);
    }
}

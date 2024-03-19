using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerShoot : NetworkBehaviour
{
    [SerializeField] GameObject fireball;
    [SerializeField] GameObject explosion;
    [SerializeField] Transform pos;

    private bool isAlive = true;
    private List<GameObject> projectiles = new List<GameObject>();

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;
        if (!isAlive) return;

        if (Input.GetKeyDown(KeyCode.Return))
        {

            FireServerRPC();
        }
    }

    public void SetDeath()
    {
        isAlive = false;
    }

    [ServerRpc]
    private void FireServerRPC()
    {
        GameObject instance = Instantiate(fireball, pos.position, pos.rotation);
        projectiles.Add(instance);
        var passThroughTarget = GetComponent<SoftTargetting>().currentTarget;
        instance.GetComponent<Projectile>().SetTarget(passThroughTarget);
        instance.GetComponent<Projectile>().parent = this;
        
        var instanceNetworkObject = instance.GetComponent<NetworkObject>();
        instanceNetworkObject.Spawn();
        
    }

    [ServerRpc(RequireOwnership = false)]
    public void DestroyServerRpc()
    {
        if (projectiles[0] == null) return;
        var goToDestroy = projectiles[0];
        goToDestroy.GetComponent<NetworkObject>().Despawn();
        projectiles.Remove(goToDestroy);
        Destroy(goToDestroy);

    }
    
}

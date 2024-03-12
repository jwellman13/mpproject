using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Projectile : NetworkBehaviour
{
    public PlayerShoot parent;

    [SerializeField] GameObject goExplosion;
    [SerializeField] ParticleSystem trail;
    [SerializeField] ParticleSystem explosion;

    private float speed = 20f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Time.deltaTime * speed * transform.forward;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (!IsOwner) return;
        ProjectileCollisionServerRpc();
        parent.DestroyServerRpc();
    }


    [ServerRpc]
    private void ProjectileCollisionServerRpc()
    {
        var instance = Instantiate(goExplosion, transform.position, transform.rotation);
        var instanceNetworkObject = instance.GetComponent<NetworkObject>();
        instanceNetworkObject.Spawn();
    }


        

}

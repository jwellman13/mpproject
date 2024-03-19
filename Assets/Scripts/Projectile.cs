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

    public Transform target;

    private float speed = 20f;

    public override void OnNetworkSpawn()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            transform.position += Time.deltaTime * speed * transform.forward;
        }
        else
        {
            Vector3 pos = new Vector3(target.position.x, 1f, target.position.z);
            Vector3 relativePos = target.position - transform.position;
            Quaternion rot = Quaternion.LookRotation(relativePos, Vector3.up);
            transform.rotation = rot;
            transform.position = Vector3.MoveTowards(transform.position, pos, speed * Time.deltaTime);
        }
    }

    public void SetTarget(Transform currentTarget)
    {
        target = currentTarget;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (!IsOwner) return;
        if (collider.gameObject.tag == "Ground") return;
        if (collider.gameObject.tag == "Target Collider") return;
        if (collider.gameObject == parent.gameObject) return;

        ProjectileCollisionServerRpc();

        if (collider.GetComponent<PlayerData>() != null)
        {
            collider.GetComponent<PlayerData>().TakeDamageServerRpc();
        }

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

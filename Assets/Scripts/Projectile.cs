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

    private Transform target;

    private float speed = 20f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (target == null)
        {
            transform.position += Time.deltaTime * speed * transform.forward;
            target = parent.gameObject.GetComponent<SoftTargetting>().currentTarget;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
    }

    public void SetTarget(Transform currentTarget)
    {
        target = currentTarget;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (!IsOwner) return;
        if (collider.gameObject.tag == "Target Collider") return;

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

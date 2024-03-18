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
            //target = new Vector3(targetPos.x, 1f, targetPos.z);

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

        Debug.Log(collider.gameObject.name);

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

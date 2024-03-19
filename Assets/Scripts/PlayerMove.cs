using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerMove : NetworkBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 250f;
    [SerializeField] private GameObject deathScreen;
    
    private Animator animator;
    private AudioSource audioSource;

    public bool isAlive = true;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner)
        {
            return;
        }

        Move();
    }

    void Move()
    {

        if (!isAlive) return;
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(horizontal, 0, vertical);
        dir.Normalize();
        transform.Translate(dir * moveSpeed * Time.deltaTime, Space.World);

        if (dir != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(dir, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }

        float move = Mathf.Abs(dir.x) + Mathf.Abs(dir.z);
        audioSource.volume = move;
        animator.SetFloat("Speed", move);
    }

    public void SetDeath()
    {
        isAlive = false;
        animator.SetTrigger("Die");

        deathScreen.SetActive(true);
    }
}

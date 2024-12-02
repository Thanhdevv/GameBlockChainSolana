using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    [Header("Setting")]
    public float distanceClick;
    private Animator m_Animator;

    private bool isFall;
    private GameObject player;
    float distance;

    private void Start()
    {
        isFall = false;
        m_Animator = GetComponent<Animator>();
    }
    private void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;
        distance = Vector2.Distance(transform.position, player.transform.position);
        if (distance < distanceClick && !isFall) {
            isFall = true;
            m_Animator.SetTrigger("fall");
        }
    }
}

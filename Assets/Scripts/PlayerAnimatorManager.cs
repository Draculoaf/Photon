using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorManager : MonoBehaviourPun
{
    #region private fields

    [SerializeField] private float directionDampTime = .25f; //increases turning radius/time
    private Animator animator;

    #endregion

    #region Monobehaviour callbacks

    void Start()
    {
        animator = GetComponent<Animator>();
        if (!animator)
        {
            Debug.LogError("Animator missing");
        }
    }

    void Update()
    {
        if (!animator) { return; } 

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (v < 0) { v = 0; }

        animator.SetFloat("Speed", h * h + v * v);

        animator.SetFloat("Direction", h, directionDampTime, Time.deltaTime);

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("Base Layer.Run"))
        {
            if (Input.GetButtonDown("Fire2"))
            {
                animator.SetTrigger("Jump");
            }
        }

      if (photonView.IsMine == false && PhotonNetwork.IsConnected == true) 
        {
            return;
        }
    }

    #endregion
}

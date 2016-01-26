﻿using UnityEngine;
using System.Collections;

public class Laser : MonoBehaviour {

    public float idleTime = 1.0f;
    public float activeTime = 1.0f;
    public float prepTime = 0.5f;

    private float time = 0.0f;
    private enum State {Active, Idle, Prep};
    private State state = State.Idle;

    private Animator animator;

    void Start () {
        animator = GetComponent<Animator>();
	}

    public bool isActive() {
        return state == State.Active;
    }
	
	void Update () {
        time += Time.deltaTime;

        if (state == State.Active)
        {
            if (time > activeTime) {
                animator.SetBool("PlayIdle", true);
                animator.SetBool("PlayActive", false);
                animator.SetBool("PlayPrep", false);
                time = 0.0f;
                state = State.Idle;
            }
        }
        else if(state == State.Idle)
        {
            if(time > idleTime)
            {
                animator.SetBool("PlayPrep", true);
                animator.SetBool("PlayIdle", false);
                animator.SetBool("PlayActive", false);
                time = 0.0f;
                state = State.Prep;
            }
        }
        else
        {
            if (time > prepTime)
            {
                animator.SetBool("PlayActive", true);
                animator.SetBool("PlayPrep", false);
                animator.SetBool("PlayIdle", false);
                time = 0.0f;
                state = State.Active;
            }
        }

	}
}
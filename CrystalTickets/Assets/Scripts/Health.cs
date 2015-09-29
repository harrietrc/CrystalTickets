﻿using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {

    public int startingHealth = 100;

    public bool isDead { get; private set; }

    protected int currentHealth;
    protected Animator animator;

    private Movement movement;

    void Awake() {
        currentHealth = startingHealth;
        isDead = false;
    }

    // Use this for initialization
    protected virtual void Start() {
        animator = GetComponent<Animator>();
        movement = GetComponent<Movement>();
    }

    // Update is called once per frame
    void Update() {

    }

    public virtual void Damage(int damage) {
        currentHealth -= damage;
        if (currentHealth <= 0 && !isDead)
            DestroyCharacter(); // Kill the player/mob
    }

    public virtual void Heal(int health) {
        currentHealth += health;
    }

    public virtual void DestroyCharacter() {
        isDead = true;
        animator.SetBool(GameConstants.DeadState, true); // Play the death animation
        movement.enabled = false;
    }

}
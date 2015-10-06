﻿using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    private Animator animator;

	public static bool activateDoor = false;

	public ItemScript.ItemTypes currentItem = ItemScript.ItemTypes.Pistol;
	public Joystick.ItemTypes currentmovement = Joystick.ItemTypes.empty;
	public MoveRight.ItemTypes currentmovement2 = MoveRight.ItemTypes.empty;
	public MoveJump.ItemTypes currentmovement3 = MoveJump.ItemTypes.empty;
	public MoveShoot.ItemTypes currentmovement4 = MoveShoot.ItemTypes.empty;
	public bool isPause = false;

    private Rigidbody2D rigidBody;

    // Shooting stuff. gun = position of the gun; where bullets will start from.
    public GameObject bulletPrefab, gun, grenadePrefab;
    public float firingIntervalInSeconds = 0.1f; // How often can we fire a bullet
    private float timeLastFired;

	PlayerStatsUI statsUI;

    private Movement movement;

    // Controls lever conditions
    public GameObject lever;
    public Sprite leverOn, leverOff;

    void Awake() {
        timeLastFired = -firingIntervalInSeconds;
    }

    void Start() {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        movement = GetComponent<Movement>();
        timeLastFired = -firingIntervalInSeconds;
        statsUI = GetComponent<PlayerStatsUI>();
    }

    void FixedUpdate() {

        float move = Input.GetAxis("Horizontal");//Gives us of one if we are moving via the arrow keys
                                                 //move our Players rigidbody

        rigidBody.velocity = new Vector3(move * movement.speed, rigidBody.velocity.y);

//		if (this.currentmovement == Joystick.ItemTypes.left || this.currentmovement2 == Joystick.ItemTypes.left || this.currentmovement3 == Joystick.ItemTypes.left || this.currentmovement4 == Joystick.ItemTypes.left) {
//			Debug.Log ("left");
//			movement.MoveLeft();
//			
//		}
//		if (this.currentmovement == Joystick.ItemTypes.right || this.currentmovement2 == Joystick.ItemTypes.right|| this.currentmovement3 == Joystick.ItemTypes.right|| this.currentmovement4 == Joystick.ItemTypes.right) {
//			Debug.Log ("right");
//			movement.MoveRight();
//		}

		if (this.currentmovement == Joystick.ItemTypes.left) {
			movement.MoveLeft();
			animator.SetBool (GameConstants.RunState, true);
		}
		if (this.currentmovement2 == MoveRight.ItemTypes.right) {
			movement.MoveRight();
			animator.SetBool (GameConstants.RunState, true);
		}
    }

    void Update() {

		// Only shoot a bullet if a sane amount of time has passed
		float secondsSinceLastFired = Time.time - timeLastFired;

//		if (this.currentmovement == Joystick.ItemTypes.jump || this.currentmovement2 == Joystick.ItemTypes.jump|| this.currentmovement3 == Joystick.ItemTypes.jump|| this.currentmovement4 == Joystick.ItemTypes.jump) {
//			Debug.Log ("jump");
//			movement.Jump();
//
//		}
//		if (this.currentmovement == Joystick.ItemTypes.shoot || this.currentmovement2 == Joystick.ItemTypes.shoot || this.currentmovement3 == Joystick.ItemTypes.shoot|| this.currentmovement4 == Joystick.ItemTypes.shoot) {
//			Debug.Log ("shoot");
//			timeLastFired = Time.time;
//			animator.SetBool("Shoot", true);
//			FireBullet(gun.transform.position);
//		}

		if (this.currentmovement3 == MoveJump.ItemTypes.jump) {

			movement.Jump();
			
		}
		if (this.currentmovement4 == MoveShoot.ItemTypes.shoot && isPause == false && secondsSinceLastFired > firingIntervalInSeconds) {
			timeLastFired = Time.time;
			animator.SetBool("Shoot", true);
			FireBullet(gun.transform.position);
		}


        if (Input.GetButtonDown("Jump")) 
            movement.Jump();

        /* Putting the animation transitions here seems to make it more responsive than when it's in FixedUpdate(),
           but this could use a bit more testing. */

        float move = Input.GetAxis("Horizontal");

        // Update the animations to show running if the player is moving
        bool isRunning = move == 0 ? false : true;
        animator.SetBool(GameConstants.RunState, isRunning);

        // Update animations to reflect which way the player is moving
        bool changedDirection = move > 0 && !movement.isFacingRight || move < 0 && movement.isFacingRight;
        if (changedDirection)
            movement.Flip();

//        if (Input.GetButton("Fire") && secondsSinceLastFired > firingIntervalInSeconds) {
//            timeLastFired = Time.time;
//            animator.SetBool(GameConstants.ShootState, true);
//            FireBullet(gun.transform.position);
//        }
//        if (Input.GetButtonUp("Fire"))
//            animator.SetBool(GameConstants.ShootState, false);
    }

    // This should probably be elsewhere. Enemies can reuse this too.
    private void FireBullet(Vector3 position) {
        // Bullet script in prefab should take care of actually moving the bullet once it's instantiated...

		GameObject prefab = bulletPrefab;

		if (this.currentItem == ItemScript.ItemTypes.Grenade) {
			prefab = grenadePrefab;
			GameObject bullet = (GameObject)Instantiate (prefab, position, Quaternion.identity);
			bullet.GetComponent<Grenade>().Fire(movement.isFacingRight); // ... but we need to tell it which way to move
		} else {
			GameObject bullet = (GameObject) Instantiate(prefab, position, Quaternion.identity);
			bullet.GetComponent<Bullet>().Fire(movement.isFacingRight); // ... but we need to tell it which way to move
		}
    }

    // TODO: Move this. Should be in own script, not in player controller
    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.name == "lever") {
            if (activateDoor == false) {
                print("Switch On");
                activateDoor = true;
                lever.GetComponent<SpriteRenderer>().sprite = leverOn;
            } else {
                print("Switch Off");
                activateDoor = false;
                lever.GetComponent<SpriteRenderer>().sprite = leverOff;
            }
        }
    }
}

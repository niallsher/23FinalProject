using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class spiderBoss : MonoBehaviour
{

    [Header ("Animator")]
	public Animator animator;

    [SerializeField] float moveSpeed = 5f;
    Rigidbody2D rb;
    Transform target;
    Vector2 moveDirection;

    [Header ("Facing Direction")]
	public Facings Facing;

    [Header ("Stats")]
    public ProjectileEnemy bullet;
    public float bulletLife = 5.0f;
    public float fireDelay;
    public Health owner;
    public Transform gunBarrel;
    public WeaponEnemy weapon;

    [Header ("Angle")]
    protected float currentAngle = 0f;
	public float randomAngle = 20;

    //private GameObject clone;
    private float timer;
    private GameObject grid;
    private GameObject exit;

    [Header ("Trigger")]
    private bool trig;
    public enemyTrigger areaTrig;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        trig = false;
        target = GameObject.FindGameObjectWithTag("Entity").transform;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        trig = areaTrig.trigger;
        if(target && trig) {
            Vector3 direction = (target.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            //rb.rotation = angle;
            currentAngle = angle;
            moveDirection = direction;
        }

        if(timer > fireDelay && trig){
            timer = timer - fireDelay;
            shoot();
        }

        if (currentAngle < 0) {
			currentAngle += 360f;
		}

		if (currentAngle > 270 || currentAngle < 90) {
            Facing = Facings.Right;
		} else {
			Facing = Facings.Left;
		}

        if (weapon != null) {
			weapon.SetRotation (currentAngle);
		}
    }

    void FixedUpdate()
    {
        if(target) {
            rb.velocity = new Vector2(moveDirection.x, moveDirection.y) * moveSpeed;
        }
    }

    void UpdateSprite () {
		var targetScale = Facing == Facings.Right ? new Vector3(1f,1f,1f) : new Vector3(-1f,1f,1f);
		transform.localScale = targetScale;
    }

    public void TakeDamage(float damageAmount)
    {
        print("damage");
    }

    void shoot()
    {
        var amount = UnityEngine.Random.Range (-randomAngle, randomAngle);
        ProjectileEnemy fired = Instantiate(bullet, gunBarrel.position, Quaternion.Euler(new Vector3(0f, 0f, currentAngle + amount))) as ProjectileEnemy;//Quaternion.identity);
        fired.owner = owner;
        Destroy(fired.gameObject, bulletLife);
    }

    public void Die()
    {
        animator.Play("Death");
        Destroy(gameObject, 1);
    }
}

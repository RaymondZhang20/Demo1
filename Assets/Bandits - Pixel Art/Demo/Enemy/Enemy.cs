using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public int health;
    public int damage;
    public float flashTime;
    public float speed;
    public float startWaitTime;
    private float waitTime;

    public Transform movePos;
    public Transform leftDownPos;
    public Transform rightUpPos;
    public GameObject bloodEffect;
    //attack
    public float time;
    public float startTime;
    private Animator anim;
    private PolygonCollider2D coll2D;
    private bool isAttack = false;

    private SpriteRenderer sr;
    private Color originalColor;
    private Bandit player;

    // Start is called before the first frame update
    public void Start()
    {
        sr = GetComponent<SpriteRenderer>(); 
        originalColor = sr.color;
        waitTime = startWaitTime;
        movePos.position = GetRandomPos();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Bandit>();
        anim = GetComponent<Animator>();
        coll2D = transform.Find("EnemyAttack").GetComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    public void Update()
    {
        if (health <= 0) {
            Instantiate(bloodEffect, transform.position, Quaternion.identity);
            player.energy += 2;
            Destroy(gameObject);
        }
        if (movePos != null && isAttack == false) {
            transform.position = Vector2.MoveTowards(transform.position, movePos.position, speed * Time.deltaTime);
            if(Vector2.Distance(transform.position, movePos.position) < Mathf.Epsilon) {
                if(waitTime <= 0) {
                    movePos.position = GetRandomPos();
                    waitTime = startWaitTime;
                } else {
                    waitTime -= Time.deltaTime;
                }
            }
        }
    }

    public void TakeDamage(int damage) {
        health -= damage;
        FlashColor(flashTime);
    }

    void FlashColor(float time)
    {
        sr.color = Color.red;
        Invoke("ResetColor", time);
    }

    void ResetColor()
    {
        sr.color = originalColor;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Attack();
    }

    void Attack()
    {
        isAttack = true;
        anim.SetTrigger("Attack");
        StartCoroutine(start());
    }

    IEnumerator start()
    {
        yield return new WaitForSeconds(startTime);
        coll2D.enabled = true;
        StartCoroutine(disableHitBox());
    }

    IEnumerator disableHitBox()
    {
        yield return new WaitForSeconds(time);
        coll2D.enabled = false;
        isAttack = false;
    }

    Vector2 GetRandomPos()
    {
        Vector2 randPos = new Vector2(Random.Range(leftDownPos.position.x, rightUpPos.position.x), Random.Range(leftDownPos.position.y, rightUpPos.position.y));
        return randPos;
    }

    public void DamagePlayer() {
        player.TakeDamagePlayer(damage);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public int damage;
    public float time;
    public float startTime;
    private bool m_attacking = false;

    private Animator anim;
    private PolygonCollider2D coll2D;
    private Bandit player;

    // Start is called before the first frame update
    void Start()
    {
        anim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        coll2D = GetComponent<PolygonCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Bandit>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!player.m_isDead) {
            Attack();
        }
    }

    void Attack()
    {
        if(Input.GetKeyDown("j") && m_attacking == false) {
            m_attacking = true;
            anim.SetTrigger("Attack");
            StartCoroutine(start());
            Invoke("SetAttack", 0.8f);
        }
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
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Enemy")) {
            other.GetComponent<Enemy>().TakeDamage(damage);
        }
    }

    void SetAttack() {
        m_attacking = false;
    }
}

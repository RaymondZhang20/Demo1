using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Bandit : MonoBehaviour {

    [SerializeField] float      m_speed = 4.0f;
    [SerializeField] float      m_jumpForce = 7.5f;
    public int health;
    public int maxEnergy;
    public Image filter;
    public Color deadFilter;
    public Color darkFilter;

    private Animator            m_animator;
    private Rigidbody2D         m_body2d;
    private Sensor_Bandit       m_groundSensor;
    private bool                m_grounded = false;
    private bool                m_combatIdle = false;
    private bool                m_attacking = false;
    public bool                 m_isDead = false;
    private int                 canJump = 0;
    public int                  energy = 0;

    // Use this for initialization
    void Start () {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_Bandit>();
        HealthBar.maxHealth = health;
        EnergyBar.maxEnergy = maxEnergy;
    }
	
	// Update is called once per frame
	void Update () {
        if (!m_isDead) {
            Alive();
        } else if (Input.GetKeyDown("e")) {
            transform.position = new Vector2(-15, -1.1f);
            filter.color = new Color(0,0,0,0);
            health = HealthBar.maxHealth;
            Invoke("SetRecover", 2);
        }
        if (health < 0) {
            health = 0;
        }
        if (energy > maxEnergy) {
            energy = maxEnergy;
        }
        HealthBar.currentHealth = health;
        EnergyBar.currentEnergy = energy;
    }

    void Alive() {
        //Check if character just landed on the ground
        if (!m_grounded && m_groundSensor.State()) {
            m_grounded = true;
            canJump = 0;
            m_animator.SetBool("Grounded", m_grounded);
        }

        //Check if character just started falling
        if(m_grounded && !m_groundSensor.State()) {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }

        // -- Handle input and movement --
        float inputX = Input.GetAxis("Horizontal");

        // Swap direction of sprite depending on walk direction
        if (inputX > 0)
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        else if (inputX < 0)
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        // Move
        m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y);

        //Set AirSpeed in animator
        m_animator.SetFloat("AirSpeed", m_body2d.velocity.y);

        // -- Handle Animations --

        //Parry
        if (Input.GetKeyDown("k"))
            m_combatIdle = !m_combatIdle;

        //Jump
        else if (Input.GetKeyDown("space") && (m_grounded || canJump < 3)) {
            m_animator.SetTrigger("Jump");
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
            m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
            m_groundSensor.Disable(0.2f);
            canJump += 1;

        }

        //Run
        else if (Mathf.Abs(inputX) > Mathf.Epsilon)
            m_animator.SetInteger("AnimState", 2);

        //Combat Idle
        else if (m_combatIdle)
            m_animator.SetInteger("AnimState", 1);

        //Idle
        else
            m_animator.SetInteger("AnimState", 0);
    }

    void SetAttack() {
        m_attacking = false;
    }

    void SetRecover() {
        m_animator.SetTrigger("Recover");
        m_isDead = !m_isDead;
    }

    public void TakeDamagePlayer(int damage)
    {
        if (m_combatIdle == false) {
            m_animator.SetTrigger("Hurt");
            GameController.camShake.Shake();
            health -= damage;
        } else {
            GameController.camShake.Shake();
            health -= damage/2;
        }
        if (health <= 0) {
            m_isDead = true;
            m_animator.SetTrigger("Death");
            filter.color = deadFilter;
        }
    }

    public Vector2 getPos() {
        return (Vector2)transform.localPosition;
    }
}

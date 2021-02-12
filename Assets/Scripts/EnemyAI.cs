using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    Transform target;
    NavMeshAgent agent;
    Animator anim;
    bool isDead = false;
    [SerializeField]
    float chaseDistance = 2f;
    [SerializeField]
    float turnSpeed = 5f;
    public float damageAmount = 35f;
    [SerializeField]
    float attackTime = 2f;
    public bool canAttack = true;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(transform.position, target.position);

        if(distance > chaseDistance && !isDead)
        {
            agent.updateRotation = true;
            agent.updatePosition = true;
            agent.SetDestination(target.position);
            anim.SetBool("isWalking", true);
            anim.SetBool("isAttacking", false);
        }
        else if(canAttack && !PlayerHealth.singleton.isDead)
        {
            agent.updateRotation = false;
            Vector3 direction = target.position - transform.position;
            direction.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), turnSpeed * Time.deltaTime);
            agent.updatePosition = false;
            anim.SetBool("isWalking", false);
            anim.SetBool("isAttacking", true);
            StartCoroutine(AttackTime());
        }
        else if(PlayerHealth.singleton.isDead)
        {
            DisableEnemy();
        }
    }

    public void DisableEnemy()
    {
        canAttack = false;
        anim.SetBool("isWalking", false);
        anim.SetBool("isAttacking", false);
    }

    public void SetAnimDead()
    {
        isDead = true;
        anim.SetTrigger("isDead");
    }

    IEnumerator AttackTime()
    {
        canAttack = false;
        yield return new WaitForSeconds(0.2f);
        PlayerHealth.singleton.DamagePlayer(damageAmount);
        yield return new WaitForSeconds(attackTime);
        canAttack = true;
    }
}

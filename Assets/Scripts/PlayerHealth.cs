using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth singleton;
    public float currentHealth;
    public float maxHealth = 100f;
    public bool isDead = false;

    public void Awake()
    {
        singleton = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentHealth < 0)
        {
            currentHealth = 0;
        }
    }

    public void DamagePlayer(float damage)
    {
        if(currentHealth > 0)
        {
            currentHealth -= damage;
        }

        else
        {
            Dead();
        }
    }

    public void Dead()
    {
        currentHealth = 0;
        isDead = true;
        Debug.Log("PlayerDead");
    }
}

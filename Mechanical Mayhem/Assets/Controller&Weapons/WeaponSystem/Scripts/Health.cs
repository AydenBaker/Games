using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    //public methods
    [SerializeField] private float health = 100;
    [SerializeField] private bool destroyOnDeath = true;
    
    [Tooltip("Min health goes first then max Health")] [SerializeField]
    private Vector2 MinMaxHealth = new Vector2(0, 100);

    [SerializeField] private float timeDelayBeforDestruction = 1f;
    private bool IsDead = false;

    //methods
    private void Update()
    {
        if (health <= MinMaxHealth.x)
        {
            StartCoroutine(DestroyObject(timeDelayBeforDestruction));
            IsDead = true;
        }
        else if (health > MinMaxHealth.y) //checks if health is over max health
        {
            health = MinMaxHealth.y;
        }


    }

    //can be called from other classes to reduce objects health
    public void ChangeHealthValue(float num)
    {

        health += num;

    }


    public void DeathEffects()
    {
        //effects like blood and death animation
    }


    private IEnumerator DestroyObject(float waitTime)
    {
        // DeathEffects();

        yield return new WaitForSeconds(waitTime);
        if (destroyOnDeath)
        {
            Destroy(gameObject);    
        }
        else
        {
            gameObject.SetActive(false);
        }

    }

    public float GetHealthValue()
    {
        return health;
    }

    public float GetTotalHealth()
    {
        return this.MinMaxHealth.y;
    }
    public bool GetIsDead()
    {
        return IsDead;
    }

}

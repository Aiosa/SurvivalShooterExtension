using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseHealth : MonoBehaviour
{

    private int maxHealth = 500;
    private int currentHealth = 0;
    private GameObject player;
    
    [SerializeField]
    private Slider healthSlider;
    
    
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag ("Player");
    }


    
    public int getHealth()
    {
        return currentHealth;
    }
    
    public void TakeDamage (int amount) 
    {
        currentHealth -= amount;
        healthSlider.value = currentHealth;
        if(currentHealth <= 0)
        {
            player.GetComponent<PlayerHealth>().Death();
        }
    }
}

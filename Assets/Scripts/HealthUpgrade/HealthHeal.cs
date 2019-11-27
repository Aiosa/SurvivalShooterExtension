using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthHeal : MonoBehaviour
{

    [SerializeField]
    private int amount;

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag.Contains("Player"))
        {
            PlayerHealthHU playerHealthHU = other.gameObject.GetComponent<PlayerHealthHU>();
            playerHealthHU.heal(amount);
            Destroy(gameObject);
        }
    }
}

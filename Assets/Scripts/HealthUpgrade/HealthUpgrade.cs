using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUpgrade : MonoBehaviour
{
    [SerializeField]
    private int amount;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag.Contains("Player"))
        {
            PlayerHealthHU playerHealthHU = other.gameObject.GetComponent<PlayerHealthHU>();
            playerHealthHU.upgradeHealth(amount);
            Destroy(this);
        }
    }
}

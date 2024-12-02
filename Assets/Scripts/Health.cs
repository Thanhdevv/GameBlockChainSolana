using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public Image healthBar;

    public void UpdateHealth(int currentHealth,int maxHealth)
    {
        if (healthBar == null) return;
        healthBar.fillAmount = (float)currentHealth / (float)maxHealth;
    }
}

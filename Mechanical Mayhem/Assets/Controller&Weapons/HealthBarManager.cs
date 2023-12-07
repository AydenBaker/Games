using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarManager : MonoBehaviour
{

    [SerializeField] private Slider healthBar;
    private Health playerHealth;


    // Start is called before the first frame update
    void Start()
    {
        playerHealth = this.GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {

        healthBar.value = playerHealth.GetHealthValue() / playerHealth.GetTotalHealth();

    }
}

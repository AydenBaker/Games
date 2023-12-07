using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyCounter : MonoBehaviour
{
    
    [SerializeField] private TextMeshProUGUI numbertext;
    [SerializeField] private GameObject enemyHolder;
    private int enemys;
    
    
    // Update is called once per frame
    void Update()
    {
        enemys = enemyHolder.transform.childCount;
        numbertext.text = enemys.ToString();
    }
}

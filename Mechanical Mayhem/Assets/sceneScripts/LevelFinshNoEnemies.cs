using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelFinshNoEnemies : MonoBehaviour
{

    [SerializeField] private GameObject enemyHolder;
    [SerializeField] private int nextLevelIndex;

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (enemyHolder.transform.childCount <= 2)
            {
                //load next level
                SceneManager.LoadScene(nextLevelIndex);
            }
        }
    }

}

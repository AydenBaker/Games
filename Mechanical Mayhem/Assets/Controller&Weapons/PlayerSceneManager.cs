using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSceneManager : MonoBehaviour
{

    [SerializeField] private GameObject player;
    private bool isAlive = true;
    private Health playerHealth;

    [SerializeField] private GameObject deathScreen;
    [SerializeField] private GameObject menuScreen;
    [SerializeField] private GameObject playerScreen;
    
    // Start is called before the first frame update
    void Start()
    {
        playerHealth = player.GetComponent<Health>();

        //always sets time scale to 1 so we can move once the scene loads
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerHealth.GetIsDead())
        {
            Cursor.lockState = CursorLockMode.None;
            deathScreen.SetActive(true);
        }
        else
        {
            
            deathScreen.SetActive(false);
        }


        if (Input.GetKeyDown(KeyCode.Escape) && !deathScreen.activeSelf)
        {
            ActivateMenu();
        }

    }

    public void ActivateMenu()
    {
        Time.timeScale = .005f;
        menuScreen.SetActive(true);
        playerScreen.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
    }

    public void DeactivateMenu()
    {
        Time.timeScale = 1f;
        menuScreen.SetActive(false);
        playerScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    
    
    public GameObject GetPlayerObject()
    {
        return player;
    }

    public bool GetIsAlive()
    {
        return isAlive;
    }

    public void SetIsAlive(bool isAlive)
    {
        this.isAlive = isAlive;
    }
}

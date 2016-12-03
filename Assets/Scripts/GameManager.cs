using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : Singleton<GameManager>
{

    public GameObject goPlayer;

    private UIManager uiManager;
    private FirstPersonSFX firstPersonSFX;


    public int iHealth = 100;

    public int iScore = 0;
    public bool isPlaying = true;

    public void SetReferences()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        uiManager = GetComponent<UIManager>();
        firstPersonSFX = goPlayer.GetComponent<FirstPersonSFX>();
    }

    public void AddScore()
    {
        iScore++;
        uiManager.UpdateScore(iScore);
    }

    public void UpdateScore()
    {
        uiManager.UpdateScore(iScore);
    }

    public void DamagePlayer(int iDamage)
    {
        iHealth -= iDamage;

        if (iHealth <= 0)
        {
            iHealth = 0;
            isPlaying = false;
        }

        uiManager.UpdateHealth(iHealth);
        uiManager.PlayDamageAnimation();
        firstPersonSFX.PlayHurtGroan();

        if (iHealth == 0)
        {
            EndGame();
        }

    }

    public void EndGame()
    {
        if (iScore == 10)
        {
            //SceneManager.LoadScene("_winScene");
        }
        else
        {
            Debug.Log("Lose");
            SceneManager.LoadScene("loseScene");
        }
    }
}

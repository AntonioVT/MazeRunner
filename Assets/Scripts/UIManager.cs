using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour
{

    public Text tHealth;
    public Text tScore;
    public GameObject goDamagePanel;

    void Start()
    {
        SetObjectsMode();
    }

    public void SetObjectsMode()
    {
        goDamagePanel.SetActive(false);
    }


    public void UpdateHealth(int iHealth)
    {
        tHealth.text = "Health: " + iHealth;
    }

    public void PlayDamageAnimation()
    {
        goDamagePanel.SetActive(false);
        goDamagePanel.SetActive(true);
    }

    public void UpdateScore(int iScore)
    {
        tScore.text = "Score: " + iScore;
    }

}

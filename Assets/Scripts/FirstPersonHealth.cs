using UnityEngine;
using System.Collections;

public class FirstPersonHealth : MonoBehaviour {

    public float fTimer = 0.0f;
    public float fDelay = 2.0f;

    void Update()
    {
        fTimer -= Time.deltaTime;
    }

	void OnTriggerStay(Collider other)
    {
        if (other.tag.Equals("Enemy"))
        {
            if(fTimer <= 0)
            {
                GameManager.Instance.DamagePlayer(5);
                fTimer = fDelay;
            }
        }
    }
}

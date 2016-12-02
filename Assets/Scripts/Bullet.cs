using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bullet : MonoBehaviour
{
    public float fDamage;
    public GameObject goExplosion;

    public void SpawnExplosion()
    {
        GameObject goExpl = Instantiate(goExplosion, transform.position, Quaternion.identity) as GameObject;
        Destroy(goExpl, 2.0f);
        Destroy(this.gameObject);
    }

    public IEnumerator BulletAliveTime(float fWaitingTime)
    {
        yield return new WaitForSeconds(fWaitingTime);
        SpawnExplosion();
        Destroy(this.gameObject);
    }

}

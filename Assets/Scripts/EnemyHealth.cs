using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour {

    public float fHealth;

    void OnCollisionEnter(Collision other)
    {
        if (other.collider.transform.tag.Equals("Bullet"))
        {
            Bullet bullet = other.gameObject.GetComponent<Bullet>();
            bullet.SpawnExplosion();
            DamageMonster(bullet.fDamage);

            Destroy(other.gameObject);
        }
    }

    void DamageMonster(float fDamage)
    {
        fHealth = Mathf.Clamp(fHealth - fDamage, 0, Mathf.Infinity);
        
        if(fHealth == 0)
        {
            DestroyMonster();
        }
    }

    void DestroyMonster()
    {
        Destroy(this.gameObject);
    }
}

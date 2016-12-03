using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour {

    public float fHealth;
	public GameObject burningEffect;
	private bool isBurning;
	private float size;

    void OnCollisionEnter(Collision other)
    {
        if (other.collider.transform.tag.Equals("Bullet"))
        {
            Bullet bullet = other.gameObject.GetComponent<Bullet>();
            bullet.SpawnExplosion();
            DamageMonster(bullet.fDamage);

            Destroy(other.gameObject);
        }
		else if (other.collider.transform.tag.Equals("Fire"))
        {
            Bullet bullet = other.gameObject.GetComponent<Bullet>();
            DamageMonster(bullet.fDamage);
            Destroy(other.gameObject);
			
			if(isBurning == false){
				GameObject burnEffect = (GameObject)Instantiate(burningEffect, transform.position, Quaternion.identity);
				burnEffect.transform.Rotate(270, 0, 0);
				burnEffect.transform.Translate(0,0,-1.2f);
				size = transform.lossyScale.x;
				if (size < 2)
					size = size * 0.5f;
				burnEffect.transform.localScale += new Vector3(size, size, size);
				burnEffect.transform.parent = gameObject.transform;
				isBurning = true;
				StartCoroutine(ExecuteAfterTime(3));
			}
        }
    }

	IEnumerator ExecuteAfterTime(float time){
		 yield return new WaitForSeconds(time);
		 DamageMonster(30);
		 isBurning = false;
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
        GameManager.Instance.iScore += 50;
        GameManager.Instance.UpdateScore();
        Destroy(this.gameObject);
    }
}

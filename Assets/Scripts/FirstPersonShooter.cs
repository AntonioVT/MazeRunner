using UnityEngine;
using System.Collections;

public class FirstPersonShooter : MonoBehaviour
{
    [Header("Selected Weapon")]
    public int iSelectedWeapon = 1;

    [Header("Pistols")]
    public Transform[] tGunpoint;
    public Animation[] animGuns;

    [Header("Shotgun")]
    public Transform tShotgunPointA;
    public Transform tShotgunPointB;
    public Animation animShotgun;

    [Header("GrenadeLauncher")]
    public Transform tGLPoint;
    public Animation animGrenadeLauncher;

    [Header("Weapon references")]
    public GameObject[] goWeapons;

    [Header("Bullet prefab + Settings")]
    public GameObject prefBullet;
    public GameObject prefGrenade;
    public GameObject prefFire;
    public float fForce;
    public float fBulletLifeTime = 1.3333f;
    public int iDistance;
    private int iGunPoint = 0;
	
	private float pistolCooldown = 0;
	private float shotgunCooldown = 0;
	private float flameThrowerCooldown = 0;
	private float grenadeLauncherCooldown = 0;

    private FirstPersonSFX firstPersonSFX;
    [Header("Misc")]
    public Transform tHandGrenade;

    void Start()
    {
        firstPersonSFX = GetComponent<FirstPersonSFX>();
        SetWeapon();
    }

    void Update()
    {
        //Debug.DrawRay(tGunpoint.position, tGunpoint.forward * iDistance);
        ManageShooting();
        ManageGrenades();
        SelectWeapon();
    }

    void ManageShooting()
    {
        switch (iSelectedWeapon)
        {
            case 0:
                ShootPistol();
                break;
            case 1:
                ShootShotgun();
                break;
            case 2:
                ShootGeneric(animGrenadeLauncher, tGLPoint, prefBullet);
                break;
            case 3:
                ShootFlameThrower(animGrenadeLauncher, tGLPoint, prefFire);
                break;
        }
    }

    void ManageGrenades()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            // Disparar proyectil
            Shoot(tHandGrenade, prefGrenade);
        }
    }

    void ShootPistol()
    {
        // Dual pistols
		if (pistolCooldown > 0)
			pistolCooldown -= Time.deltaTime;
        if (Input.GetButton("Fire1") && pistolCooldown <= 0)
        {
            // Adm. de efectos de sonido
            firstPersonSFX.PlayGunShot();

            //Animar
            AnimateGuns();

            // Disparar proyectil
            Shoot();

            // Actualizar pistolas
            iGunPoint++;
            iGunPoint = iGunPoint % 2;
			pistolCooldown = 0.20f;
        }
    }

    void ShootShotgun()
    {
        // Shotgun
		if (shotgunCooldown > 0)
			shotgunCooldown -= Time.deltaTime;
        if (Input.GetButton("Fire1") && shotgunCooldown <= 0)
        {
            // Adm. de efectos de sonido
            firstPersonSFX.PlayGunShot();

            //Animar
            AnimateGun(animShotgun);

            // Disparar proyectil
            Shoot(tShotgunPointA, prefBullet);
            Shoot(tShotgunPointB, prefBullet);
			shotgunCooldown = 1;
        }
    }

	void ShootFlameThrower(Animation animGeneric, Transform tPoint, GameObject prefab)
    {
		if (flameThrowerCooldown > 0)
			flameThrowerCooldown -= Time.deltaTime;
        if (Input.GetButton("Fire1") && flameThrowerCooldown <= 0)
        {
            // Adm. de efectos de sonido
            firstPersonSFX.PlayGunShot();

            //Animar
            AnimateGun(animGeneric);

            // Disparar proyectil
            Shoot(tPoint, prefab);
			flameThrowerCooldown = 0.05f;
        }
    }
	
    void ShootGeneric(Animation animGeneric, Transform tPoint, GameObject prefab)
    {
        // Generic Launcher
		if (grenadeLauncherCooldown > 0)
			grenadeLauncherCooldown -= Time.deltaTime;
        if (Input.GetButton("Fire1") && grenadeLauncherCooldown <= 0)
        {
            // Adm. de efectos de sonido
            firstPersonSFX.PlayGunShot();

            //Animar
            AnimateGun(animGeneric);

            // Disparar proyectil
            Shoot(tPoint, prefab);
			grenadeLauncherCooldown = 2;
        }
    }

    void SetWeapon()
    {
        for (int i = 0; i < goWeapons.Length; i++)
        {
            goWeapons[i].SetActive(false);
        }
        goWeapons[iSelectedWeapon].SetActive(true);
    }

    // Animation: Pistols only
    void AnimateGuns()
    {
        Animation lastAnim = animGuns[iGunPoint];
        if (lastAnim.isPlaying)
        {
            lastAnim.Stop();
            lastAnim.Sample();
            lastAnim.enabled = false;
        }
        lastAnim.enabled = true;
        animGuns[iGunPoint].Play();
    }

    // Shoot: Pistols only
    void Shoot()
    {
        // Crear instancia proyectil
        GameObject goBullet = Instantiate(prefBullet, tGunpoint[iGunPoint].position, transform.rotation) as GameObject;

        // Dar fuerza al proyectil
        Rigidbody bulletRb = goBullet.GetComponent<Rigidbody>();
        bulletRb.AddForce(tGunpoint[iGunPoint].forward * fForce, ForceMode.Force);

        // Destruir el proyectil en X segundos
        Bullet bulletScript = goBullet.GetComponent<Bullet>();
        bulletScript.StartCoroutine(bulletScript.BulletAliveTime(fBulletLifeTime));
    }

    // Generic animations/shoots
    void AnimateGun(Animation selectedAnim)
    {
        Animation lastAnim = selectedAnim;
        if (lastAnim.isPlaying)
        {
            lastAnim.Stop();
            lastAnim.Sample();
            lastAnim.enabled = false;
        }
        lastAnim.enabled = true;
        lastAnim.Play();
    }

    void Shoot(Transform selectedGunpoint, GameObject selectedPrefab)
    {
        // Crear instancia proyectil
        GameObject goBullet = Instantiate(selectedPrefab, selectedGunpoint.position, transform.rotation) as GameObject;

        // Dar fuerza al proyectil
        Rigidbody bulletRb = goBullet.GetComponent<Rigidbody>();
        bulletRb.AddForce(selectedGunpoint.forward * fForce, ForceMode.Force);

        // Destruir el proyectil en X segundos
        Bullet bulletScript = goBullet.GetComponent<Bullet>();
        bulletScript.StartCoroutine(bulletScript.BulletAliveTime(fBulletLifeTime));
    }

    public void SelectWeapon()
    {
        int iNewWeapon = -1;

        if (Input.GetKeyDown(KeyCode.Alpha1))
            iNewWeapon = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2))
            iNewWeapon = 1;
        if (Input.GetKeyDown(KeyCode.Alpha3))
            iNewWeapon = 2;
        if (Input.GetKeyDown(KeyCode.Alpha4))
            iNewWeapon = 3;

        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            int iSum = Input.GetAxis("Mouse ScrollWheel") > 0 ? 1 : -1;
            iNewWeapon = iSelectedWeapon + iSum;
            if(iNewWeapon >= goWeapons.Length)
            {
                iNewWeapon = 0;
            }else if(iNewWeapon < 0)
            {
                iNewWeapon = goWeapons.Length - 1;
            }
        }

        if (iNewWeapon != iSelectedWeapon & iNewWeapon >= 0)
        {
            goWeapons[iSelectedWeapon].SetActive(false);
            goWeapons[iNewWeapon].SetActive(true);

            iSelectedWeapon = iNewWeapon;
        }

        
    }
}

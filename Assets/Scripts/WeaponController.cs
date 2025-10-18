using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class WeaponController : MonoBehaviour
{
    public float damage = 25f;
    public float range = 100f;
    public int maxAmmo = 120;
    public int currentAmmo;
    public int reserveAmmo = 90;
    public int magazineSize = 10;
    public float reloadTime = 1.5f;
    public float fireRate = 0.8f;
    private float nextTimeToFire = 0f;
    private bool isReloading = false;
    Camera playerCamera;
    PlayerInput playerInput;
    [SerializeField] Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerInput = GameObject.FindWithTag("Player").GetComponent<PlayerInput>();
        playerCamera = Camera.main;
        if (animator == null)
            animator = GetComponentInParent<Animator>();
        currentAmmo = magazineSize;
    }

    // Update is called once per frame
    void Update()
    {
        if (isReloading) return;
        if (playerInput != null && playerInput.actions["Attack"].triggered)
        {
            if (currentAmmo > 0 && Time.time >= nextTimeToFire)
            {
                animator.SetTrigger("shoot");
                Shoot();
                nextTimeToFire = Time.time + fireRate;
            }
        }
        if (playerInput != null && playerInput.actions["Reload"].triggered)
        {
            if (currentAmmo < magazineSize && reserveAmmo > 0)
                StartCoroutine(Reload());
        }
    }

    void Shoot()
    {
        currentAmmo--;
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, range))
        {
            if (hit.transform.CompareTag("Enemy"))
            {
                CharacterBase target = hit.transform.GetComponent<CharacterBase>();
                if (target != null)
                {
                    target.TakeDamage(damage);
                }
            }
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        animator.SetTrigger("reload");

        yield return new WaitForSeconds(reloadTime);

        int ammoNeeded = magazineSize - currentAmmo;
        int ammoToReload = Mathf.Min(ammoNeeded, reserveAmmo);
        currentAmmo += ammoToReload;
        reserveAmmo -= ammoToReload;

        isReloading = false;
    }

    public void AddAmmo(int amount)
    {
        reserveAmmo += amount;
        if (reserveAmmo > maxAmmo)
            reserveAmmo = maxAmmo;
    }

}

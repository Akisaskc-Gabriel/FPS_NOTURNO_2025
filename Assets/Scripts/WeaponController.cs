using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponController : MonoBehaviour
{
    public float damage = 25f;
    public float range = 100f;
    Camera playerCamera;
    PlayerInput playerInput;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerInput = GameObject.FindWithTag("Player").GetComponent<PlayerInput>();
        playerCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInput != null && playerInput.actions["Attack"].triggered)
        {
            Shoot();
        }
    }

    void Shoot()
    {
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

}

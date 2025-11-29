using UnityEngine;
using UnityEngine.InputSystem;

public class InputSystemPlayer : MonoBehaviour
{
    private PlayerInput playerInput;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    void Update()
    {
        if (playerInput.actions["Pause"].triggered)
        {
            Debug.Log("Pause pressionado!");
            PauseGame(); // opcional
        }
    }

    void PauseGame()
    {
        Time.timeScale = Time.timeScale == 0 ? 1 : 0;
        Debug.Log("timeScale = " + Time.timeScale);
    }
}

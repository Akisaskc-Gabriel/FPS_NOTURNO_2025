using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [Header("Canvas")]
    public GameObject menuInicialUI;
    public GameObject gameOverUI;
    public GameObject gameplayUI;
    public GameObject pauseUI;

    [Header("Input")]
    public PlayerInput playerInput;

    private bool isPaused = false;
    private bool gameStarted = false;
    public Transform respawnPoint;
    public GameObject playerPrefab;

    void Start()
    {
        // Começa no menu inicial
        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        menuInicialUI.SetActive(true);
        gameplayUI.SetActive(false);
        pauseUI.SetActive(false);
        gameOverUI.SetActive(false);

        gameStarted = false;
        isPaused = false;
    }

    void Update()
    {
        if (gameStarted && playerInput.actions["Pause"].triggered)
        {
            if (isPaused)
                ContinuarJogo();
            else
                Pause();
        }
    }

    // =====================================================
    //                     MENU INICIAL
    // =====================================================

    public void Jogar()
    {
        menuInicialUI.SetActive(false);
        gameplayUI.SetActive(true);
        pauseUI.SetActive(false);
        gameOverUI.SetActive(false);

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        gameStarted = true;
        isPaused = false;
    }

    // =====================================================
    //                     MENU DE PAUSA
    // =====================================================

    public void Pause()
    {
        pauseUI.SetActive(true);
        gameplayUI.SetActive(false);

        Time.timeScale = 0f;
        isPaused = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ContinuarJogo()
    {
        pauseUI.SetActive(false);
        gameplayUI.SetActive(true);

        Time.timeScale = 1f;
        isPaused = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // =====================================================
    //                     VOLTAR AO MENU
    // =====================================================

    public void VoltarMenu()
    {
        // Reinicia a cena para reset completo
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // =====================================================
    //                     RESPWAN
    // =====================================================

    public void Respawn()
    {
        Time.timeScale = 1f;

        // reposiciona o player
        playerPrefab.transform.position = respawnPoint.position;
        playerPrefab.transform.rotation = respawnPoint.rotation;

        // reseta status (opcional)
        var pc = playerPrefab.GetComponent<PlayerController>();
        pc.currentHealth = pc.maxHealth;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }
}

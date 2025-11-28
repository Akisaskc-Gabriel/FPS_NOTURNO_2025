using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;

public class HUDManager : MonoBehaviour
{

    public Slider healthSlider;
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI scoreText;
    WeaponController weapon;
    PlayerController player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        weapon = FindFirstObjectByType<WeaponController>();
        player = FindFirstObjectByType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAmmoUI();
        UpdateHealthUI();
        UpdateScoreUI();
    }

    void UpdateAmmoUI()
    {
        ammoText.text = "Ammo: " + weapon.GetAmmoStatus();
    }

    void UpdateHealthUI()
    {
        healthSlider.value = player.GetHealthPercentage();
    }

    void UpdateScoreUI()
    {
        scoreText.text = "Score: " + player.score.ToString();
    }
}

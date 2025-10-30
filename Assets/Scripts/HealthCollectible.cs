using UnityEngine;

public class HealthCollectible : CollectibleBase
{

    public int healthAmmount = 15;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    protected override void OnCollected(Collider player)
    {
        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController != null)
            playerController.AddHealth(healthAmmount);
    }
}

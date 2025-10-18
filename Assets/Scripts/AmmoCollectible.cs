using UnityEngine;

public class AmmoCollectible : CollectibleBase
{
    public int ammoAmount = 15;

    protected override void OnCollected(Collider player)
    {
        WeaponController weapon = player.GetComponentInChildren<WeaponController>();
        if (weapon != null)
        {
            weapon.AddAmmo(ammoAmount);
        }
    }
}

using UnityEngine;
using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;

[RequireComponent(typeof(AudioSource))]
public abstract class CollectibleBase : MonoBehaviour
{
    public bool rotate = true;
    public float rotationSpeed = 90f;
    public AudioClip collectSound;
    public GameObject collectEffect;
    public float respawnTime = 5f;

    private Collider collectibleCollider;
    private Renderer[] renderers;

    protected virtual void Awake()
    {
        collectibleCollider = GetComponent<Collider>();
        renderers = GetComponentsInChildren<Renderer>();
    }

    protected virtual void Update()
    {
        if (rotate)
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);
    }

    protected abstract void OnCollected(Collider player);

    private void SetActiveCollectible(bool active)
    {
        if (collectibleCollider != null)
            collectibleCollider.enabled = active;

        if (renderers != null)
        {
            foreach (var r in renderers)
                r.enabled = true;
        }
    }

    protected void PlayCollectFeedback()
    {
        if (collectSound)
            AudioSource.PlayClipAtPoint(collectSound, transform.position);
        if (collectEffect)
            Instantiate(collectEffect, transform.position, Quaternion.identity);
    }

    private IEnumerator RespawnRoutine()
    {
        SetActiveCollectible(false);
        yield return new WaitForSeconds(respawnTime);
        SetActiveCollectible(true);
    }

    public void Collect(Collider player)
    {
        PlayCollectFeedback();
        OnCollected(player);
        StartCoroutine(RespawnRoutine());
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Collect(other);
        }
    }

}

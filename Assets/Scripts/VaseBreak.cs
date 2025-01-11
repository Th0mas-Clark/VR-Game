using UnityEngine;

public class VaseBreaker : MonoBehaviour
{
    public GameObject normalVase;
    public GameObject brokenVaseGroup;
    public AudioClip breakSound;
    public float explosionForce = 2f;
    public float explosionRadius = 1f;
    public float upwardModifier = 0.5f;
    public GameObject Key;
    public float keySpawnOffset = 0.1f; 

    private bool isBroken = false;
    private Collider normalVaseCollider;

    private void Start()
    {
        normalVase.SetActive(true);
        brokenVaseGroup.SetActive(false);
        normalVaseCollider = normalVase.GetComponent<Collider>();
        if (normalVaseCollider == null)
        {
            Debug.LogError("Normal vase needs a collider!");
        }
    }

    public void BreakVase()
    {
        if (isBroken) return;
        isBroken = true;

        if (normalVaseCollider != null)
        {
            normalVaseCollider.enabled = false;
        }

        brokenVaseGroup.transform.parent = null;
        brokenVaseGroup.transform.position = normalVase.transform.position;
        brokenVaseGroup.transform.rotation = normalVase.transform.rotation;

        normalVase.SetActive(false);
        brokenVaseGroup.SetActive(true);

        if (breakSound != null)
        {
            AudioSource.PlayClipAtPoint(breakSound, transform.position);
        }

        if (Key != null)
        {
            Vector3 spawnPosition = transform.position + Vector3.up * keySpawnOffset; 
            GameObject spawnedKey = Instantiate(Key, spawnPosition, transform.rotation);
            Rigidbody keyRb = spawnedKey.GetComponent<Rigidbody>();
            if (keyRb != null)
            {
                keyRb.AddExplosionForce(explosionForce, transform.position, explosionRadius, upwardModifier, ForceMode.Impulse);
            }
        }
        else
        {
            Debug.LogError("Key prefab not assigned!");
        }

        foreach (Transform piece in brokenVaseGroup.transform)
        {
            Rigidbody rb = piece.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.WakeUp();
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, upwardModifier, ForceMode.Impulse);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isBroken && collision.relativeVelocity.magnitude > 2f)
        {
            BreakVase();
        }
    }
}
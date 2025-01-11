using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawerLock : MonoBehaviour
{
    public GameObject Drawer; // Reference to the drawer GameObject
    public ConfigurableJoint TopDrawerJoint; // The ConfigurableJoint used to move the drawer
    public Transform KeySnapPosition; // Reference to the position where the key should snap
    public AudioSource UnlockSound; // Audio source to play the unlock sound

    private bool isUnlocked = false; // Tracks if the drawer is unlocked

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Triggered by: {other.name}"); // Log the name of the object triggering

        // Check if the correct key is inserted
        if (other.CompareTag("Key"))
        {
            Debug.Log("Key inserted! Attempting to unlock drawer.");

            // Snap the key into position and rotate it
            SnapKeyIntoPlace(other.gameObject);

            // Unlock the drawer
            UnlockDrawer();
        }
    }

    private void SnapKeyIntoPlace(GameObject key)
    {
        // Move the key to the snap position
        key.transform.position = KeySnapPosition.position;

        // Rotate the key 90 degrees around its local Y-axis (for correct orientation)
        key.transform.rotation = Quaternion.Euler(
            KeySnapPosition.rotation.eulerAngles.x,
            KeySnapPosition.rotation.eulerAngles.y,
            KeySnapPosition.rotation.eulerAngles.z
        );

        // Set the key as a child of the DrawerLock so it moves with it
        key.transform.SetParent(transform); // 'transform' refers to this DrawerLock object

        // Disable the key's Rigidbody physics (so it doesn't move independently)
        Rigidbody keyRigidbody = key.GetComponent<Rigidbody>();
        if (keyRigidbody != null)
        {
            keyRigidbody.isKinematic = true;
        }

        // Disable the key's collider so it doesn't interact with other objects
        Collider keyCollider = key.GetComponent<Collider>();
        if (keyCollider != null)
        {
            keyCollider.enabled = false;
        }

        // Play the unlock sound
        if (UnlockSound != null)
        {
            UnlockSound.Play();
        }
    }

    private void UnlockDrawer()
    {
        if (TopDrawerJoint == null)
        {
            Debug.LogError("TopDrawerJoint is not assigned in the Inspector!");
            return;
        }

        if (!isUnlocked)
        {
            isUnlocked = true;

            // Enable limited movement on the Z-axis (so the drawer can move)
            TopDrawerJoint.zMotion = ConfigurableJointMotion.Limited;

            Debug.Log("Drawer unlocked! Z-motion set to Limited.");
        }
        else
        {
            Debug.Log("Drawer is already unlocked.");
        }
    }
}

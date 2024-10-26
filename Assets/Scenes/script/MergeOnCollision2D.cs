using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeOnCollision2D : MonoBehaviour
{
    public GameObject mergedPrefab;      // Prefab for the new object created after merge
    public float mergeDelay = 1.0f;      // Delay time in seconds before merging is allowed

    private bool canMerge = false;       // Control when merging is allowed
    private List<Collider2D> collidedObjects = new List<Collider2D>(); // Store collided objects

    private void Start()
    {
        StartCoroutine(EnableMergingAfterDelay());
    }

    private IEnumerator EnableMergingAfterDelay()
    {
        yield return new WaitForSeconds(mergeDelay);
        canMerge = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Mergeable") && !collidedObjects.Contains(collision))
        {
            collidedObjects.Add(collision);  // Store the collided object for later merge check
            Debug.Log("Added collision with " + collision.name);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collidedObjects.Contains(collision))
        {
            collidedObjects.Remove(collision);  // Remove the object when it is no longer colliding
            Debug.Log("Removed collision with " + collision.name);
        }
    }

    private void OnMouseUp()
    {
        if (!canMerge) return;

        foreach (Collider2D collision in collidedObjects)
        {
            if (collision != null && collision.CompareTag("Mergeable"))
            {
                // Calculate spawn position for the merged object
                Vector3 spawnPosition = (transform.position + collision.transform.position) / 2;
                spawnPosition.z = 0f;

                // Instantiate the merged object
                GameObject newObject = Instantiate(mergedPrefab, spawnPosition, Quaternion.identity);

                // Reset scale and then set it to match the Initial Object's scale
                newObject.transform.localScale = Vector3.one;  // Reset scale
                newObject.transform.localScale = transform.localScale; // Match Initial Object's scale

                // Destroy only the collided object (not the Initial Object)
                Destroy(collision.gameObject);

                break; // Exit after merging with the first available object
            }
        }
    }
}

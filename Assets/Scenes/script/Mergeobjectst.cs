using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeObjects : MonoBehaviour
{
    // Prefab of the new object to create when two objects merge
    public GameObject mergedPrefab;

    // Reference to the second object to merge with
    public GameObject otherObject;

    // Check if both objects collide
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == otherObject)
        {
            // Get the position to spawn the new merged object (average of both objects' positions)
            Vector3 spawnPosition = (transform.position + otherObject.transform.position) / 2;

            // Create the new merged object
            Instantiate(mergedPrefab, spawnPosition, Quaternion.identity);

            // Destroy the original objects
            Destroy(gameObject);
            Destroy(otherObject);
        }
    }
}
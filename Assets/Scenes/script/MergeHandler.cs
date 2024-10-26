using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeHandler : MonoBehaviour
{
    public GameObject initialObjectPrefab; // Prefab to instantiate new initial objects
    public Sprite mergedSprite; // The new sprite to assign to the merge target

    private bool isDragging = false;
    private GameObject initialObject; // Reference to the first clicked (initial) object
    private GameObject mergeTarget; // The object to merge with

    void Update()
    {
        if (isDragging && initialObject != null)
        {
            // Follow the mouse position with the initial object
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            initialObject.transform.position = new Vector3(mousePos.x, mousePos.y, 0);

            if (Input.GetMouseButtonUp(0)) // Detect mouse release
            {
                if (mergeTarget != null)
                {
                    PerformMerge();
                }
                isDragging = false;
                initialObject = null; // Reset initial object after release
            }
        }
    }

    private void OnMouseDown()
    {
        // Set this object as the initial object only if we don't have one assigned yet
        if (initialObject == null)
        {
            initialObject = this.gameObject;
            isDragging = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Only set a merge target if we're dragging and the other object is mergeable
        if (isDragging && other.CompareTag("Mergeable") && other.gameObject != initialObject)
        {
            mergeTarget = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Reset merge target if we're no longer in contact
        if (other.gameObject == mergeTarget)
        {
            mergeTarget = null;
        }
    }

    private void PerformMerge()
    {
        if (mergeTarget != null)
        {
            // Change the appearance of the merge target to the new sprite
            SpriteRenderer targetRenderer = mergeTarget.GetComponent<SpriteRenderer>();
            if (targetRenderer != null && mergedSprite != null)
            {
                targetRenderer.sprite = mergedSprite;
            }

            // Destroy the initial object
            Destroy(initialObject);

            // Create a new initial object at the merged position
            Instantiate(initialObjectPrefab, mergeTarget.transform.position, Quaternion.identity);

            // Reset merge target
            mergeTarget = null;
        }
    }
}

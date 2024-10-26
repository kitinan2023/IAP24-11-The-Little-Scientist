using UnityEngine;

public class DraggableMergeableObject : MonoBehaviour
{
    public GameObject draggablePrefab; // Prefab to instantiate new draggable/mergeable objects
    public Sprite mergedSprite; // Sprite to change to after merging
    private bool isDragging = false;
    private GameObject mergeTarget; // Object to merge with

    void Update()
    {
        if (isDragging)
        {
            // Move object with mouse
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mousePos.x, mousePos.y, 0);

            if (Input.GetMouseButtonUp(0)) // On mouse release
            {
                if (mergeTarget != null)
                {
                    PerformMerge();
                }
                isDragging = false; // Stop dragging
            }
        }
    }

    private void OnMouseDown()
    {
        // Start dragging on click
        isDragging = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Set merge target if it’s a mergeable object and not itself
        if (other.CompareTag("Mergeable") && other.gameObject != this.gameObject)
        {
            mergeTarget = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Reset merge target when out of contact
        if (other.gameObject == mergeTarget)
        {
            mergeTarget = null;
        }
    }

    private void PerformMerge()
    {
        if (mergeTarget != null)
        {
            // Change the merge target’s appearance
            SpriteRenderer targetRenderer = mergeTarget.GetComponent<SpriteRenderer>();
            if (targetRenderer != null && mergedSprite != null)
            {
                targetRenderer.sprite = mergedSprite;
            }

            // Destroy current object after merging
            Destroy(this.gameObject);

            // Create a new draggable object at the merge target’s position
            GameObject newDraggableObject = Instantiate(draggablePrefab, mergeTarget.transform.position, Quaternion.identity);

            // Ensure the new object is ready to merge by adding this script component
            newDraggableObject.AddComponent<DraggableMergeableObject>();

            // Reset merge target to avoid unintended repeated merges
            mergeTarget = null;
        }
    }
}

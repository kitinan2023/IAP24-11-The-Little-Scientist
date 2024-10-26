using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DraggableSlotButton : MonoBehaviour
{
    public Transform slotArea; // The slot area where the item can be placed
    public GameObject targetObject; // The object that will change appearance
    public Sprite[] clickLevelSprites; // Array of sprites for the target object based on click count
    public Button buttonPrefab; // Prefab for the button created in the slot
    private bool isDragging = false;
    private Button slotButton; // The button created in the slot
    private int clickCount = 0; // Number of times the button has been clicked

    void Update()
    {
        if (isDragging)
        {
            // Move object with mouse
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mousePos.x, mousePos.y, 0);

            if (Input.GetMouseButtonUp(0)) // On mouse release
            {
                if (IsOverSlotArea())
                {
                    StoreItemInSlot();
                }
                isDragging = false; // Stop dragging
            }
        }
    }

    private void OnMouseDown()
    {
        // Start dragging when the item is clicked
        isDragging = true;
    }

    private bool IsOverSlotArea()
    {
        // Check if the object is within the slot area
        return slotArea != null && Vector2.Distance(transform.position, slotArea.position) < 1f;
    }

    private void StoreItemInSlot()
    {
        if (slotButton == null)
        {
            // Instantiate a button at the slot area position
            slotButton = Instantiate(buttonPrefab, slotArea.position, Quaternion.identity, slotArea);
            slotButton.onClick.AddListener(OnSlotButtonClick);

            // Set the button's image to match the draggable item's sprite
            Image buttonImage = slotButton.GetComponent<Image>();
            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            if (buttonImage != null && renderer != null)
            {
                buttonImage.sprite = renderer.sprite;
            }

            // Destroy the draggable item after storing it as a button
            Destroy(this.gameObject);
        }
    }

    private void OnSlotButtonClick()
    {
        // Increment click count
        clickCount++;

        // Change the appearance of the target object based on click count
        if (targetObject != null && clickLevelSprites.Length > 0)
        {
            SpriteRenderer targetRenderer = targetObject.GetComponent<SpriteRenderer>();
            if (targetRenderer != null)
            {
                // Use clickCount % clickLevelSprites.Length to cycle through sprites
                int spriteIndex = clickCount % clickLevelSprites.Length;
                targetRenderer.sprite = clickLevelSprites[spriteIndex];
            }
        }
    }
}

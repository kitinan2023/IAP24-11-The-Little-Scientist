using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DraggableObjectSlot : MonoBehaviour
{
    public Transform slotArea;             // The slot where the item should be placed
    public GameObject targetObject;        // Object to change appearance when button is clicked
    public Sprite[] appearanceSprites;     // Sprites to cycle through on each click
    public Button buttonPrefab;            // Prefab of the button to replace the draggable object in the slot
    public GameObject endGamePanel;        // Panel to show when the game ends
    public Vector3 buttonOffset = Vector3.zero;  // Offset for button positioning

    private bool isDragging = false;       // Tracks if the object is currently being dragged
    private bool isInSlot = false;         // Tracks if the object has been placed in the slot
    private Button slotButton;             // The button created in the slot
    private int clickCount = 0;            // Count of how many times the button has been clicked

    void Start()
    {
        // Ensure the end game panel is hidden at the start of the game
        if (endGamePanel != null)
        {
            endGamePanel.SetActive(false);
        }
    }

    void Update()
    {
        if (isDragging && !isInSlot)
        {
            // Move object with mouse while dragging
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mousePos.x, mousePos.y, 0);

            if (Input.GetMouseButtonUp(0)) // Stop dragging on mouse release
            {
                if (IsOverSlotArea())
                {
                    PlaceInSlot();
                }
                isDragging = false;
            }
        }
    }

    private void OnMouseDown()
    {
        // Start dragging only if object has not been placed in the slot
        if (!isInSlot)
        {
            isDragging = true;
        }
    }

    private bool IsOverSlotArea()
    {
        // Check if the object is within the slot area
        return slotArea != null && Vector2.Distance(transform.position, slotArea.position) < 1f;
    }

    private void PlaceInSlot()
    {
        // Set object position to slot position
        transform.position = slotArea.position;
        isInSlot = true; // Lock the object in the slot

        // Check if the Canvas is in World or Screen Space mode
        Canvas canvas = slotArea.GetComponentInParent<Canvas>();
        Vector3 buttonPosition;

        if (canvas != null && canvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            // For Screen Space - Overlay, convert world position to screen position
            buttonPosition = Camera.main.WorldToScreenPoint(slotArea.position);
        }
        else
        {
            // For World Space or Screen Space - Camera, use world position directly
            buttonPosition = slotArea.position;
        }

        // Instantiate the button at the slot position, with optional offset
        slotButton = Instantiate(buttonPrefab, buttonPosition + buttonOffset, Quaternion.identity, canvas.transform);

        // Ensure the button’s onClick listener is assigned
        slotButton.onClick.AddListener(OnSlotButtonClick);

        // Set the button image to match the draggable item’s sprite
        Image buttonImage = slotButton.GetComponent<Image>();
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        if (buttonImage != null && renderer != null)
        {
            buttonImage.sprite = renderer.sprite;
        }

        // Hide the draggable object's sprite renderer to prevent overlapping visuals
        renderer.enabled = false;

        // Hide slot's visual appearance
        SpriteRenderer slotSpriteRenderer = slotArea.GetComponent<SpriteRenderer>();
        Image slotImage = slotArea.GetComponent<Image>();

        if (slotSpriteRenderer != null)
        {
            slotSpriteRenderer.enabled = false;
        }
        else if (slotImage != null)
        {
            slotImage.enabled = false;
        }
    }

    private void OnSlotButtonClick()
    {
        if (clickCount >= 30)
            return; // Stop further clicks if the game has ended

        clickCount++;

        // Change the appearance of the target object based on click count
        if (targetObject != null && appearanceSprites.Length > 0)
        {
            SpriteRenderer targetRenderer = targetObject.GetComponent<SpriteRenderer>();
            if (targetRenderer != null)
            {
                int spriteIndex = clickCount % appearanceSprites.Length;
                targetRenderer.sprite = appearanceSprites[spriteIndex];
            }
        }

        // Check if click count reached 30 to end the game
        if (clickCount >= 27)
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        // Show the end game panel
        if (endGamePanel != null)
        {
            endGamePanel.SetActive(true);
        }

        // Optional: Disable the slot button to prevent further clicks
        if (slotButton != null)
        {
            slotButton.interactable = false;
        }

        Debug.Log("Game Over! Clicks reached 30.");
    }
}

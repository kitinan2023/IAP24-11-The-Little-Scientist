using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public GameObject objectToClone; // The original game object to clone
    public Button cloneButton; // Reference to the UI button
    public Vector3 scaleFactor = new Vector3(0.0123f, 0.123f, 1f);

    void Start()
    {
        // Ensure the button is set up to call the CloneObject method
        if (cloneButton != null)
        {
            Debug.Log("Button listener set up."); // Debug line to check if listener is set
            cloneButton.onClick.AddListener(CloneObject);
        }
    }

    // Method to clone the specified game object
    public void CloneObject()
{
    if (objectToClone != null)
    {
        Debug.Log("CloneObject method called."); // This should print when the button is clicked

        // Define the spawn position
        Vector3 spawnPosition = new Vector3(3.71f, -2.15f, 1f);
        
        // Instantiate a new copy of the specified game object
        GameObject newObject = Instantiate(objectToClone, spawnPosition, Quaternion.identity);
        
        // Calculate the final scale based on the original object's scale and the scaleFactor
        Vector3 finalScale = newObject.transform.localScale; // Original scale
        finalScale = Vector3.Scale(finalScale, scaleFactor); // Apply scaleFactor
        
        // Set the scale of the new object
        newObject.transform.localScale = finalScale;

        // Debug the scales
        Debug.Log($"Original Scale: {newObject.transform.localScale}, Scale Factor: {scaleFactor}, Final Scale: {finalScale}");
    }
    else
    {
        Debug.LogWarning("objectToClone is not assigned!"); // Warning if no object is assigned
    }
}
}
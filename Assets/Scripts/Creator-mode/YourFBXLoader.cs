using UnityEngine;

public class YourFBXLoader : MonoBehaviour
{
    // Reference to a prefab for loading FBX models
    public GameObject fbxModelPrefab;

    // This function will load the FBX model from bytes and instantiate it
    public GameObject LoadFBXModel(byte[] fbxData)
    {
        // Make sure you have a reference to a prefab set in the inspector
        if (fbxModelPrefab == null)
        {
            Debug.LogError("Please assign an FBX model prefab in the inspector.");
            return null;
        }

        // Create a new game object to hold the loaded FBX model
        GameObject loadedModel = Instantiate(fbxModelPrefab);

        // Assuming your FBX model is a child of the loadedModel prefab, you can access it like this:
        Transform fbxModelTransform = loadedModel.transform.Find("FBXModel"); // Adjust the name accordingly

        if (fbxModelTransform != null)
        {
            // Load the FBX model into the GameObject
            // You can use the appropriate method to load the FBX data, such as FBXImporter or other custom methods
            // Here, we're using a simple Cube as a placeholder
            GameObject fbxModel = GameObject.CreatePrimitive(PrimitiveType.Cube);

            // Adjust the position and rotation of the loaded FBX model
            fbxModel.transform.parent = fbxModelTransform;
            fbxModel.transform.localPosition = Vector3.zero;
            fbxModel.transform.localRotation = Quaternion.identity;

            // You may need to adjust the scale or apply additional transformations as needed
            // fbxModel.transform.localScale = new Vector3(1f, 1f, 1f);

            // Optionally, you can add scripts, materials, or other components to the loaded model

            // Clean up the placeholder if needed (e.g., remove the cube)
            Destroy(fbxModel);

            return loadedModel;
        }
        else
        {
            Debug.LogError("FBX model transform not found in the loaded prefab.");
            Destroy(loadedModel);
            return null;
        }
    }
}

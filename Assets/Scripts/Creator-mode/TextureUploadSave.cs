using System;
using UnityEngine;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using SimpleFileBrowser;

[Serializable]
class SerializableGameObject
{
    public List<SerializableTransform> childrenTransforms;
    public SerializableTransform transform;
    public string prefabPath; // Store the prefab path for this GameObject.
    public List<SerializableCollider> colliders; // Store information about colliders.

    public SerializableGameObject(GameObject obj)
    {
        transform = new SerializableTransform(obj.transform);

        childrenTransforms = new List<SerializableTransform>();
        foreach (Transform child in obj.transform)
        {
            childrenTransforms.Add(new SerializableTransform(child));
        }

        // Store the prefab path if this GameObject is instantiated from a prefab.
        if (PrefabUtility.GetPrefabInstanceStatus(obj) == PrefabInstanceStatus.Connected)
        {
            prefabPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(obj);
        }

        // Store information about colliders.
        colliders = new List<SerializableCollider>();
        Collider[] originalColliders = obj.GetComponents<Collider>();
        foreach (Collider collider in originalColliders)
        {
            colliders.Add(new SerializableCollider(collider));
        }
    }
}

[Serializable]
class SerializableCollider
{
    public bool isTrigger;
    public ColliderType colliderType;
    public SerializableVector3 center;
    public SerializableVector3 size;

    public SerializableCollider(Collider collider)
    {
        isTrigger = collider.isTrigger;
        if (collider is BoxCollider)
        {
            colliderType = ColliderType.BoxCollider;
            BoxCollider boxCollider = collider as BoxCollider;
            center = new SerializableVector3(boxCollider.center);
            size = new SerializableVector3(boxCollider.size);
        }
        else if (collider is SphereCollider)
        {
            colliderType = ColliderType.SphereCollider;
            SphereCollider sphereCollider = collider as SphereCollider;
            center = new SerializableVector3(sphereCollider.center);
            size = new SerializableVector3(new Vector3(sphereCollider.radius, sphereCollider.radius, sphereCollider.radius));
        }
        else if (collider is CapsuleCollider)
        {
            colliderType = ColliderType.CapsuleCollider;
            CapsuleCollider capsuleCollider = collider as CapsuleCollider;
            center = new SerializableVector3(capsuleCollider.center);
            size = new SerializableVector3(new Vector3(capsuleCollider.radius, capsuleCollider.height, capsuleCollider.radius));
        }
    }

    public Collider InstantiateCollider(GameObject targetObject)
    {
        Collider newCollider = null;
        switch (colliderType)
        {
            case ColliderType.BoxCollider:
                BoxCollider boxCollider = targetObject.AddComponent<BoxCollider>();
                boxCollider.isTrigger = isTrigger;
                boxCollider.center = center.ToVector3();
                boxCollider.size = size.ToVector3();
                newCollider = boxCollider;
                break;
            case ColliderType.SphereCollider:
                SphereCollider sphereCollider = targetObject.AddComponent<SphereCollider>();
                sphereCollider.isTrigger = isTrigger;
                sphereCollider.center = center.ToVector3();
                sphereCollider.radius = size.x;
                newCollider = sphereCollider;
                break;
            case ColliderType.CapsuleCollider:
                CapsuleCollider capsuleCollider = targetObject.AddComponent<CapsuleCollider>();
                capsuleCollider.isTrigger = isTrigger;
                capsuleCollider.center = center.ToVector3();
                capsuleCollider.radius = size.x;
                capsuleCollider.height = size.y;
                newCollider = capsuleCollider;
                break;
        }
        return newCollider;
    }
}

[Serializable]
enum ColliderType
{
    BoxCollider,
    SphereCollider,
    CapsuleCollider
}

[Serializable]
class SerializableTransform
{
    public SerializableVector3 position;
    public SerializableQuaternion rotation;
    public SerializableVector3 localScale;

    public SerializableTransform(Transform transform)
    {
        position = new SerializableVector3(transform.position);
        rotation = new SerializableQuaternion(transform.rotation);
        localScale = new SerializableVector3(transform.localScale);
    }

    public void ApplyToTransform(Transform transform)
    {
        position.ApplyToVector3(transform);
        transform.rotation = rotation.ToQuaternion();
        localScale.ApplyToVector3(transform);
    }
}

[Serializable]
class SerializableVector3
{
    public float x;
    public float y;
    public float z;

    public SerializableVector3(Vector3 vector)
    {
        x = vector.x;
        y = vector.y;
        z = vector.z;
    }

    public void ApplyToVector3(Transform transform)
    {
        transform.position = new Vector3(x, y, z);
    }

    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }
}

[Serializable]
class SerializableQuaternion
{
    public float x;
    public float y;
    public float z;
    public float w;

    public SerializableQuaternion(Quaternion quaternion)
    {
        x = quaternion.x;
        y = quaternion.y;
        z = quaternion.z;
        w = quaternion.w;
    }

    public void ApplyToQuaternion(ref Quaternion quaternion)
    {
        quaternion.x = x;
        quaternion.y = y;
        quaternion.z = z;
        quaternion.w = w;
    }

    public Quaternion ToQuaternion()
    {
        return new Quaternion(x, y, z, w);
    }
}

public class TextureUploadSave : MonoBehaviour
{
    public InputField fileNameInput;
    private GameObject objectToSave;

    void Start()
    {

    }

    public void SaveObject()
    {
        objectToSave = GameObject.Find("Modeltest");
        if (objectToSave != null)
        {
            string fileName = fileNameInput.text;

            if (!string.IsNullOrEmpty(fileName))
            {
                // Serialize the GameObject and its children.
                SerializableGameObject serializableObject = new SerializableGameObject(objectToSave);
                if (serializableObject != null)
                {
                    string filePath = Path.Combine(Application.persistentDataPath, fileName + ".dat");
                    SaveGameObjectToSerializedFile(serializableObject, filePath);
                    Debug.Log("Saved GameObject '" + objectToSave.name + "' to file '" + filePath);
                }
                else
                {
                    Debug.LogWarning("SerializableGameObject is null.");
                }
            }
            else
            {
                Debug.LogWarning("Please provide a file name.");
            }
        }
        else
        {
            Debug.LogWarning("GameObject 'Modeltest' not found.");
        }
    }

    void SaveGameObjectToSerializedFile(SerializableGameObject serializableObject, string filePath)
    {
        BinaryFormatter bf = new BinaryFormatter();
        using (FileStream fileStream = File.Create(filePath))
        {
            bf.Serialize(fileStream, serializableObject);
        }

        if (File.Exists(filePath))
        {
            Debug.Log("File exists.");
        }
        else
        {
            Debug.LogWarning("File does not exist.");
        }
    }

    public GameObject LoadAndInstantiateObject(string fileName)
    {
        GameObject loadedObject = LoadObject(fileName);

        if (loadedObject != null)
        {
            return null;
        }
        else
        {
            Debug.LogWarning("Failed to load and instantiate object.");
            return null;
        }
    }

    public void OpenFileExplorerAndLoad()
    {
        StartCoroutine(ShowLoadDialogCoroutine());
    }

    private string datPath;

    IEnumerator ShowLoadDialogCoroutine()
    {
        string path = Application.persistentDataPath;
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.Files, false, path, null, "Load .dat File", "Load");

        if (FileBrowser.Success)
        {
            datPath = FileBrowser.Result[0];
            string fileName = System.IO.Path.GetFileNameWithoutExtension(datPath);
            GameObject instantiatedObject = LoadAndInstantiateObject(fileName);

            if (instantiatedObject != null)
            {
                // Optionally, set its position, rotation, or other properties.
            }
        }
    }

    public GameObject LoadObject(string fileName)
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName + ".dat");

        if (File.Exists(filePath))
        {
            SerializableGameObject serializableObject;
            BinaryFormatter bf = new BinaryFormatter();

            using (FileStream fileStream = File.Open(filePath, FileMode.Open))
            {
                serializableObject = (SerializableGameObject)bf.Deserialize(fileStream);
            }

            GameObject existingObject = GameObject.Find(fileName);

            GameObject loadedParentObject;
            if (existingObject != null)
            {
                loadedParentObject = new GameObject(fileName + "_Loaded");
            }
            else
            {
                loadedParentObject = new GameObject(fileName);
            }

            serializableObject.transform.ApplyToTransform(loadedParentObject.transform);

            foreach (var childTransform in serializableObject.childrenTransforms)
            {
                GameObject childObject = new GameObject();
                childTransform.ApplyToTransform(childObject.transform);
                childObject.transform.SetParent(loadedParentObject.transform);
            }

            // Load colliders for the parent GameObject.
            LoadColliders(serializableObject, loadedParentObject);

            return loadedParentObject;
        }
        else
        {
            Debug.LogWarning("File not found at path: " + filePath);
            return null;
        }
    }

    private void LoadColliders(SerializableGameObject serializableObject, GameObject targetObject)
    {
        foreach (var serializedCollider in serializableObject.colliders)
        {
            serializedCollider.InstantiateCollider(targetObject);
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class CreatedObject : MonoBehaviour
{
    [Serializable] // serialize the Data struct
    public struct Data
    {
        public Vector3 position; // the object's position
        public Quaternion rotation; // the object's rotation
        public Vector3 scale; //the scale of the object
        public string tag; // the type of object.
    }

    public Data data; // public reference to Data
}

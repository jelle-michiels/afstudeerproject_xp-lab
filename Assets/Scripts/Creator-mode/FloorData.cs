using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class FloorData : MonoBehaviour
{
    [Serializable] // serialize the Data struct
    public struct Data
    {
        public int floorNumber;
        public string floorPlanPath;
    }

    public Data data; // public reference to Data

}
using System.Collections.Generic;
using System;

[Serializable] // serialize the entire class.
public class LevelEditor
{
    public List<CreatedObject.Data> createdObjectsData; // new list of editor object data.
    public List<FloorData.Data> floors;
    public List<CreatedObject> createdObjects;
    //public int timeLimit;
}
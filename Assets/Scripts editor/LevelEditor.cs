using System.Collections.Generic;
using System;

[Serializable] // serialize the entire class.
public class LevelEditor
{
    public List<CreatedObject.Data> createdObjects; // new list of editor object data.
    public List<FloorData.Data> floors;
    //public int timeLimit;
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GestureObject
{
    public Point[] pointsArray;
    public string gestureName;
    
    public GestureObject(Point[] _pointsArray, string _gestureName)
    {
        pointsArray = _pointsArray;
        gestureName = _gestureName;
    }

    public GestureObject()
    {
        
    }
}

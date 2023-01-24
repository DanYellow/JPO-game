using UnityEngine;
using UnityEngine.EventSystems;

//It is common to create a class to contain all of your
//extension methods. This class must be static.
public static class EventSystemExtensions
{
    //Even though they are used like normal methods, extension
    //methods must be declared static. Notice that the first
    //parameter has the 'this' keyword followed by a Transform
    //variable. This variable denotes which class the extension
    //method becomes a part of.
    public static void UpdateSelectedGameObject(GameObject go)
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(go);
    }
}
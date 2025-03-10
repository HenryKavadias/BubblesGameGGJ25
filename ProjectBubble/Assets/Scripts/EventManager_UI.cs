using System;
using UnityEngine;
using UnityEngine.UI;

public class EventManager_UI : MonoBehaviour
{
    public static event Func<Image> GetHealthBarImage;
    public static Image ReturnHealthBarElement()=>GetHealthBarImage?.Invoke();
    
    public static event Action<float> FaceIDValue;
    public static void FaceUIExpression(float face)=>FaceIDValue?.Invoke(face);
    public static event Action<Color> GetColourVal;
    public static void GetGradientColour(Color a)=>GetColourVal?.Invoke(a);
    
}

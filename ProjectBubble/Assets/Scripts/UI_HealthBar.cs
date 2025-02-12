using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HealthBar : MonoBehaviour
{
    [SerializeField] private Image HealthBar;
    [SerializeField] private Image FaceBGImage;
    [SerializeField] private Image FaceImage;
    [SerializeField] private List<Sprite> FaceList;
    
    private void OnEnable()
    {
        EventManager_UI.GetHealthBarImage += HealthBarUpdate;
        EventManager_UI.FaceIDValue += SetFaceImage;
        EventManager_UI.GetColourVal += FaceBGColourGradient;
    }

    private void OnDisable()
    {
        EventManager_UI.GetHealthBarImage -= HealthBarUpdate;
        EventManager_UI.FaceIDValue -= SetFaceImage;
    }

    private void FaceBGColourGradient(Color color)
    {
        if (!FaceBGImage) return;
        FaceBGImage.color = color;
    }

    private void SetFaceImage(float value)
    {
        if (value >= 0.75f)
            FaceImage.sprite = FaceList[0];
        else if (value < 0.75f && value >= 0.5f)
            FaceImage.sprite = FaceList[1];
        else if (value < 0.5f && value >= 0.25f)
            FaceImage.sprite = FaceList[2];
        else
            FaceImage.sprite = FaceList[3];
    }
    
    private Image HealthBarUpdate()
    {
        return HealthBar;
    }
}

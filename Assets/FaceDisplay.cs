using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FaceDisplay : MonoBehaviour
{
    public RawImage resultImage;
////5.28hurtelh
    // void Start()
    // {
    //     if (FaceResultHolder.resultTexture != null)
    //     {
    //         resultImage.texture = FaceResultHolder.resultTexture;
    //     }
    // }

    void Start()
{
    if (cameraExampl.lastCapturedPhoto != null)
    {
        resultImage.texture = cameraExampl.lastCapturedPhoto;
    }
}
}

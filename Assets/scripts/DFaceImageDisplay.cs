// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class DFaceImageDisplay : MonoBehaviour
// {
//    public RawImage cameraFeed;


//     void Start()
//     {
//         if (FaceImageHolder.faceTexture != null)
//         {
//             cameraFeed.texture = FaceImageHolder.faceTexture;
//             cameraFeed.gameObject.SetActive(true);
//         }
//         else
//         {
//             Debug.LogWarning("Face image not found!");
//         }
//     }

//     public void GotoFaceScene()
//     {
//         // Хуучин зураг устгана (хэрвээ хадгалсан байвал)
//         DetectedFaceImageHolder.faceTexture = null;

//         // Scene1 рүү шилжинэ
//         UnityEngine.SceneManagement.SceneManager.LoadScene("face");
//     }
// }

////ajildag thde back button function bhgu
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;  // Энэ мөрийг нэмнэ

// public class DFaceImageDisplay : MonoBehaviour
// {
//     public RawImage cameraFeed;
    
//     // public cameraExampl cameraScript; // Камер хариуцсан скрипт руу холбоно

//     void Start()
//     {
//         if (cameraFeed == null)
//         {
//             Debug.LogError("Camera Feed RawImage холбогдоогүй байна!");
//             return;
//         }

//         // if (DetectedFaceImageHolder.faceTexture != null)
//         // {
//         //     cameraFeed.texture = DetectedFaceImageHolder.faceTexture;
//         //     cameraFeed.gameObject.SetActive(true);
//         // }
//          if (FaceResultHolder.resultTexture != null)
//         {
//             cameraFeed.texture = FaceResultHolder.resultTexture;
//             cameraFeed.gameObject.SetActive(true);
//         }
//         else
//         {
//             Debug.LogWarning("Face image not found!");
//             cameraFeed.gameObject.SetActive(false);
//         }
//     }

//     public void GotoFaceScene()
//     {
//         // Хуучин зураг устгах
//         if (DetectedFaceImageHolder.faceTexture != null)
//         {
//             Destroy(DetectedFaceImageHolder.faceTexture);
//             DetectedFaceImageHolder.faceTexture = null;
//         }

//         // Scene руу шилжих
//         UnityEngine.SceneManagement.SceneManager.LoadScene("face");
//     }

//     public void LetsGoButton()
//    {
//    UnityEngine.SceneManagement.SceneManager.LoadScene("moonScene");
//    }
// }

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // Энэ мөрийг нэмнэ

public class DFaceImageDisplay : MonoBehaviour
{
    public RawImage cameraFeed;
    public cameraExampl cameraScript; // Камер хариуцсан скрипт руу холбоно

    void Start()
    {
        if (cameraFeed == null)
        {
            Debug.LogError("Camera Feed RawImage холбогдоогүй байна!");
            return;
        }

        if (FaceResultHolder.resultTexture != null)
        {
            cameraFeed.texture = FaceResultHolder.resultTexture;
            cameraFeed.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Face image not found!");
            cameraFeed.gameObject.SetActive(false);
        }
    }

   public void BackToCamera()
{
    if (FaceResultHolder.resultTexture != null)
    {
        Destroy(FaceResultHolder.resultTexture);
        FaceResultHolder.resultTexture = null;
    }

    cameraFeed.gameObject.SetActive(false);

    if (cameraScript != null)
    {
        cameraScript.StartCamera(); // одоо энэ ажиллана
    }
    else
    {
        Debug.LogError("Camera script холбогдоогүй байна!");
    }
}


    public void LetsGoButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("moonScene");
    }
}

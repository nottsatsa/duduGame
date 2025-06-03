// using UnityEngine;
// using UnityEngine.Networking;
// using System.Collections;
// using UnityEngine.UI;
// using UnityEngine.SceneManagement;

// public class faceupload : MonoBehaviour
// {
//     public RawImage resultImage;
//     public Texture2D sourceImage;
//     public string serverUrl = "http://192.168.4.2:5000/detect_face";

//     // public string serverUrl = "http://192.168.1.222:5000/detect_face";


//     public void StartUpload()
//     {
//         Debug.Log(sourceImage);
//         Debug.Log(serverUrl);
//         StartCoroutine(UploadAndGetFace());
//     }

//     IEnumerator UploadAndGetFace()
// {
//     if (cameraExampl.lastCapturedPhoto == null)
//     {
//         Debug.LogError("No captured photo!");
//         yield break;
//     }

//     // Зөвхөн нэг imageData зарлана
//     byte[] imageData = cameraExampl.lastCapturedPhoto.EncodeToJPG();

//     WWWForm form = new WWWForm();
//     form.AddBinaryData("image", imageData, "test.jpg", "image/jpeg");

//     UnityWebRequest www = UnityWebRequest.Post(serverUrl, form);
//     yield return www.SendWebRequest();

//     if (www.result != UnityWebRequest.Result.Success)
//     {
//         Debug.LogError("Error: " + www.error);
//     }
//     else
//     {
//         Texture2D faceTexture = new Texture2D(2, 2);
//         faceTexture.LoadImage(www.downloadHandler.data);
//         resultImage.texture = faceTexture;

//         // Үр дүнг статик классанд хадгалах
//         FaceResultHolder.resultTexture = faceTexture;

//         // Серверээс хариу ирсний дараа scene шилжих
//         SceneManager.LoadScene("astroFace");
//     }
// }

//     // IEnumerator UploadAndGetFace()
//     // {
//     //     byte[] imageData = sourceImage.EncodeToJPG();
//     //     WWWForm form = new WWWForm();
//     //     form.AddBinaryData("image", imageData, "test.jpg", "image/jpeg");

//     //     UnityWebRequest www = UnityWebRequest.Post(serverUrl, form);
//     //     yield return www.SendWebRequest();

//     //     if (www.result != UnityWebRequest.Result.Success)
//     //     {
//     //         Debug.LogError("Error: " + www.error);
//     //     }
//     //     else
//     //     {
//     //         Texture2D faceTexture = new Texture2D(2, 2);
//     //         faceTexture.LoadImage(www.downloadHandler.data);
//     //         resultImage.texture = faceTexture;
            
//     //         // Серверээс хариу ирсний дараа scene шилжих
//     //         SceneManager.LoadScene("astroFace");
//     //     }
//     //     if (www.result != UnityWebRequest.Result.Success)
//     // {
//     //     Debug.LogError("Error: " + www.error);
//     // }
//     // else
//     // {
//     //     Texture2D faceTexture = new Texture2D(2, 2);
//     //     faceTexture.LoadImage(www.downloadHandler.data);
        
//     //     // Үр дүнг статик классанд хадгалах
//     //     FaceResultHolder.resultTexture = faceTexture;
//     // }
//     // }
// }



using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class faceupload : MonoBehaviour
{
    public RawImage resultImage; // Заавал биш: Debug зориулалтаар scene дотор шууд харуулах бол
    public Texture2D sourceImage;
    public string serverUrl = "http://192.168.4.2:5000/detect_face";

    public void StartUpload()
    {
        Debug.Log(sourceImage);
        Debug.Log(serverUrl);
        StartCoroutine(UploadAndGetFace());
    }

    // IEnumerator UploadAndGetFace()
    // {
    //     if (cameraExampl.lastCapturedPhoto == null)
    //     {
    //         Debug.LogError("No captured photo!");
    //         yield break;
    //     }

    //     byte[] imageData = cameraExampl.lastCapturedPhoto.EncodeToJPG();

    //     WWWForm form = new WWWForm();
    //     form.AddBinaryData("image", imageData, "test.jpg", "image/jpeg");

    //     UnityWebRequest www = UnityWebRequest.Post(serverUrl, form);
    //     yield return www.SendWebRequest();

    //     if (www.result != UnityWebRequest.Result.Success)
    //     {
    //         Debug.LogError("Error: " + www.error);
    //     }
    //     else
    //     {
    //         Texture2D faceTexture = new Texture2D(2, 2);
    //         faceTexture.LoadImage(www.downloadHandler.data);

    //         // Илрүүлсэн зургаа хадгалах
    //         FaceResultHolder.resultTexture = faceTexture;

    //         // Хэрвээ энэ scene дээр урьдчилж харуулах бол
    //         if (resultImage != null)
    //         {
    //             resultImage.texture = faceTexture;
    //         }

    //         // Scene шилжих
    //         SceneManager.LoadScene("astroFace");
    //     }
    // }

    IEnumerator UploadAndGetFace()
{
    if (cameraExampl.lastCapturedPhoto == null)
    {
        PlayerPrefs.SetString("errorMessage", "Зураг авахад алдаа гарлаа.");
        SceneManager.LoadScene("error");
        yield break;
    }

    byte[] imageData = cameraExampl.lastCapturedPhoto.EncodeToJPG();
    WWWForm form = new WWWForm();
    form.AddBinaryData("image", imageData, "test.jpg", "image/jpeg");

    UnityWebRequest www = UnityWebRequest.Post(serverUrl, form);
    yield return www.SendWebRequest();

    if (www.result != UnityWebRequest.Result.Success)
    {
        PlayerPrefs.SetString("errorMessage", "Сервертэй холбогдож чадсангүй:\n" + www.error);
        SceneManager.LoadScene("error");
    }
    else
    {
        Texture2D faceTexture = new Texture2D(2, 2);
        bool loaded = faceTexture.LoadImage(www.downloadHandler.data);

        if (!loaded || faceTexture.width < 10) // Илрээгүй зураг эсвэл хоосон буцсан
        {
            PlayerPrefs.SetString("errorMessage", "Царай илрээгүй байна.");
            SceneManager.LoadScene("error");
        }
        else
        {
            FaceResultHolder.resultTexture = faceTexture;

            if (resultImage != null)
            {
                resultImage.texture = faceTexture;
            }

            SceneManager.LoadScene("astroFace");
        }
    }
}

}

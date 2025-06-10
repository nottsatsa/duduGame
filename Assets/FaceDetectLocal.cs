using System.Collections.Generic;
using UnityEngine;
using OpenCvSharp;

public class FaceDetectLocal : MonoBehaviour
{
    private CascadeClassifier faceCascade;

    void Start()
    {
        string cascadePath = System.IO.Path.Combine(Application.dataPath, "Scenes/haarcascade_frontalface_default.xml");
        faceCascade = new CascadeClassifier(cascadePath);

        if (faceCascade.Empty())
        {
            Debug.LogError("Cascade classifier ачаалагдаагүй! Зам: " + cascadePath);
        }
    }

    public List<UnityEngine.Rect> DetectFaces(Texture2D photo)
    {
        List<UnityEngine.Rect> faceRects = new List<UnityEngine.Rect>();

        // Texture2D -> OpenCvSharp.Mat хөрвүүлэлт
        Mat mat = OpenCvSharp.Unity.TextureToMat(photo);  // Бүрэн нэрээр дуудаж байна
        if (mat.Empty())
        {
            Debug.LogError("Мат зураг хоосон байна.");
            return faceRects;
        }

        Mat gray = new Mat();
        Cv2.CvtColor(mat, gray, ColorConversionCodes.BGR2GRAY);
        Cv2.EqualizeHist(gray, gray);

        OpenCvSharp.Rect[] faces = faceCascade.DetectMultiScale(
            gray,
            scaleFactor: 1.1,
            minNeighbors: 5,
            flags: 0,
            minSize: new OpenCvSharp.Size(30, 30)
        );

        foreach (var face in faces)
        {
            faceRects.Add(new UnityEngine.Rect(face.X, face.Y, face.Width, face.Height));
        }

        return faceRects;
    }

    public List<UnityEngine.Rect> DetectFromCapturedImage(Texture2D image)
    {
        return DetectFaces(image);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class main : MonoBehaviour
{
   public void StartTour()
   {
    SceneManager.LoadSceneAsync(1);
   }
}

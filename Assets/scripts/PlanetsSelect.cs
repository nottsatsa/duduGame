using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlanetsSelect : MonoBehaviour
{
   public void ReturnBtn()
   {
   UnityEngine.SceneManagement.SceneManager.LoadScene("test");
   }
   public void SkipCameraBtn()
   {
   UnityEngine.SceneManagement.SceneManager.LoadScene("astroFace");
   }
   public void ToTheGuideScene()
   {
   UnityEngine.SceneManagement.SceneManager.LoadScene("guide");
   }
   public void MoonPlanet()
   {
   UnityEngine.SceneManagement.SceneManager.LoadScene("moonScene");
   }

   public void MercuryPlanet()
   {
   UnityEngine.SceneManagement.SceneManager.LoadScene("mercuryScene");
   }

   public void VenusPlanet()
   {
   UnityEngine.SceneManagement.SceneManager.LoadScene("venusScene");
   }

  public void EarthPlanet()
   {
   //  SceneManager.LoadSceneAsync("earthScene");
   UnityEngine.SceneManagement.SceneManager.LoadScene("earthScene");
   }

   public void MarsPlanet()
   {
   UnityEngine.SceneManagement.SceneManager.LoadScene("marsScene");
   }

   public void JupiterPlanet()
   {
   UnityEngine.SceneManagement.SceneManager.LoadScene("jupiterScene");
   }

   public void SaturnPlanet()
   {
   UnityEngine.SceneManagement.SceneManager.LoadScene("saturnScene");
   }

   public void UranusPlanet()
   {
   UnityEngine.SceneManagement.SceneManager.LoadScene("uranusScene");
   }

   public void NeptunePlanet()
   {
   UnityEngine.SceneManagement.SceneManager.LoadScene("neptuneScene");
   }
}


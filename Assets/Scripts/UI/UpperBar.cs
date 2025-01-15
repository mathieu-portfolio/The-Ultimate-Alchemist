using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpperBar : MonoBehaviour
{
  [SerializeField] private TMP_Text scoreText;

  public void Quit()
  {
    Application.Quit();
  }

  public void SetScore(int progress, int total)
  {
    scoreText.text = progress.ToString() + '/' + total;
  }
}

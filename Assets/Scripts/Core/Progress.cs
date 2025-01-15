using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Progress : MonoBehaviour
{
  [SerializeField] private List<Ingredient> availableIngredients = new List<Ingredient>();

  public void Add(Ingredient ingredient)
  {
    availableIngredients.Add(ingredient);
  }

  public bool Contains(Ingredient ingredient)
  {
    return availableIngredients.Contains(ingredient);
  }

  public List<Ingredient> GetIngredients()
  {
    return availableIngredients;
  }

  public int Count()
  {
    return availableIngredients.Count;
  }
}

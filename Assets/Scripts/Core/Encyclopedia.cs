using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Encyclopedia : MonoBehaviour
{
  [SerializeField]
  private List<Ingredient> _ingredients = new List<Ingredient>();
  private readonly Dictionary<string, Ingredient> _recipes = new Dictionary<string, Ingredient>();

  private void Start()
  {
    foreach(Ingredient ingredient in _ingredients)
    {
      if (!ingredient.IsBasic())
      {
        _recipes.Add(ingredient.GetRecipe(), ingredient);
      }
    }
  }

  public Ingredient GetResultingElement(List<string> ingredients)
  {
    ingredients.Sort();
    string recipe = string.Join("", ingredients).Trim();
    if (_recipes.TryGetValue(recipe, out Ingredient result))
    {
      return result;
    }

    return null;
  }

  public Ingredient GetResultingElement(List<Ingredient> ingredients)
  {
    var names = ingredients
        .Where(ing => ing != null)
        .Select(ing => ing.GetName())
        .OrderBy(name => name)
        .ToList();

    return GetResultingElement(names);
  }

  public void Add(Ingredient ingredient)
  {
    _ingredients.Add(ingredient);
  }

  public int Count()
  {
    return _ingredients.Count;
  }
}

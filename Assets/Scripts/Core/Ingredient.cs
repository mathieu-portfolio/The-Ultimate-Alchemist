using UnityEngine;

public class Ingredient : MonoBehaviour
{
  [SerializeField] private string _name;
  [SerializeField] private Sprite _sprite;
  [SerializeField] private string _recipe = null;
  [SerializeField] private bool _basic;

  public void Initialize(string name, Sprite sprite, string recipe, bool basic)
  {
    _name = name;
    _sprite = sprite;
    _recipe = recipe;
    _basic = basic;
  }

  public string GetName() => _name;
  public Sprite GetSprite() => _sprite;
  public string GetRecipe() => _recipe;
  public bool IsBasic() => _basic;

  public override bool Equals(object obj) => obj is Ingredient other && _name == other._name;
  public override int GetHashCode() => _name.GetHashCode();
  public override string ToString() => _name;
}

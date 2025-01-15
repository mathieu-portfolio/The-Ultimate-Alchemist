using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class IngredientGridController : MonoBehaviour
{
  [Header("UI References")]
  [SerializeField] private Transform content;
  [SerializeField] private GameObject ingredientButtonPrefab;
  [SerializeField] private LabController lab;

  [Header("Data")]
  [SerializeField] private Progress progress;

  private void Start()
  {
    progress = FindObjectOfType<Progress>();
    PopulateGrid();
  }

  private void PopulateGrid()
  {
    if (ingredientButtonPrefab == null || content == null)
    {
      Debug.LogError("Prefab or container not assigned!");
      return;
    }

    // Clean the content
    foreach (Transform child in content)
    {
      Destroy(child.gameObject);
    }

    // Add buttons for each ingredient
    foreach (var ingredient in progress.GetIngredients())
    {
      Add(ingredient);
    }
  }

  private void OnIngredientClicked(Ingredient ingredient)
  {
    Debug.Log($"Ingredient clicked: {ingredient.name}");
    lab.AddIngredient(ingredient);
  }

  public void Add(Ingredient ingredient)
  {
    Debug.Log($"Adding {ingredient.name}");
    GameObject buttonObj = Instantiate(ingredientButtonPrefab, content);
    buttonObj.name = $"Button_{ingredient.name}";

    // Add an image
    Image buttonImage = buttonObj.GetComponentInChildren<Image>();
    buttonImage.sprite = ingredient.GetSprite();

    // Add an event
    Button button = buttonObj.GetComponent<Button>();
    if (button != null)
    {
      button.onClick.AddListener(() => OnIngredientClicked(ingredient));
    }
  }
}

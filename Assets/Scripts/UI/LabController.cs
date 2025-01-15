using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LabController : MonoBehaviour
{
  [Header("UI References")]
  [SerializeField] private UpperBar upperBar;
  [SerializeField] private IngredientGridController grid;
  [SerializeField] private List<Button> slots;
  [SerializeField] private Button brewButton;
  [SerializeField] private Sprite brewSprite;

  [Header("Data")]
  [SerializeField] private Encyclopedia encyclopedia;
  [SerializeField] private Progress progress;

  private List<Ingredient> ingredients = new List<Ingredient>();
  int ingredientCount;

  private void Start()
  {
    encyclopedia = FindObjectOfType<Encyclopedia>();
    progress = FindObjectOfType<Progress>();

    ingredients.Clear();
    ingredientCount = 0;
    for (int i = 0; i < slots.Count; i++)
    {
      slots[i].enabled = false;
      ingredients.Add(null);
      int index = i; // Capture index for lambda
      slots[i].onClick.AddListener(() => RemoveIngredient(index));
    }

    brewButton.onClick.AddListener(() => Brew());
    brewButton.enabled = false;

    upperBar.SetScore(progress.Count(), encyclopedia.Count());
  }

  public bool AddIngredient(Ingredient ingredient)
  {
    if (ingredientCount >= slots.Count) return false;

    int index = ingredients.FindIndex(ing => ing == null);
    if (index == -1) return false;

    slots[index].enabled = true;
    SetButtonSprite(slots[index], ingredient.GetSprite());
    ingredients[index] = ingredient;
    ingredientCount++;

    UpdateBrewButton();

    return true;
  }

  private void RemoveIngredient(int slotIndex)
  {
    DisableSlot(slotIndex);

    ingredients[slotIndex] = null;
    ingredientCount--;

    if (ingredientCount == 0)
    {
      DisableBrewButton();
    }
    else
    {
      UpdateBrewButton();
    }
  }

  private void Brew()
  {
    Ingredient result = encyclopedia.GetResultingElement(ingredients);
    if (result != null && !progress.Contains(result))
    {
      grid.Add(result);
      progress.Add(result);
      Clear();
      upperBar.SetScore(progress.Count(), encyclopedia.Count());
    }
  }

  private void DisableSlot(int slotIndex)
  {
    Button slot = slots[slotIndex];
    slot.enabled = false;
    SetButtonSprite(slot, null);
    ingredients[slotIndex] = null;
  }

  private void Clear()
  {
    for (int i = 0; i < slots.Count; i++)
    {
      DisableSlot(i);
    }
    ingredientCount = 0;
    DisableBrewButton();
  }

  private void DisableBrewButton()
  {
    brewButton.enabled = false;
    SetButtonSprite(brewButton, null);
  }

  private void UpdateBrewButton()
  {
    Ingredient result = encyclopedia.GetResultingElement(ingredients);
    if (result != null && progress.Contains(result))
    {
      brewButton.enabled = false;
      SetButtonSprite(brewButton, result.GetSprite());
    }
    else
    {
      brewButton.enabled = true;
      SetButtonSprite(brewButton, brewSprite);
    }
  }

  private void SetButtonSprite(Button button, Sprite sprite)
  {
    Image buttonImage = button.GetComponentInChildren<Image>();
    if (buttonImage != null)
    {
      buttonImage.sprite = sprite;
    }
  }
}

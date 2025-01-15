using UnityEngine;
using UnityEngine.UI;
using System;

public class DynamicScrollRect : MonoBehaviour
{
  [Header("References")]
  [SerializeField] private ScrollRect scrollRect;
  [SerializeField] private RectTransform scrollRectTransform; // ScrollRect RectTransform
  [SerializeField] private RectTransform contentRect; // Content RectTransform
  [SerializeField] private GridLayoutGroup gridLayoutGroup;

  [Header("Grid Settings")]
  [SerializeField] private int columns = 4; // Number of cells per row
  [SerializeField] private int visibleRows = 2; // Number of rows to display in ScrollRect

  public void AddIngredient(GameObject ingredientPrefab)
  {
    // Add a new ingredient button
    Instantiate(ingredientPrefab, contentRect);

    // Update ScrollRect size dynamically
    AdjustScrollRectSize();
  }

  private void AdjustScrollRectSize()
  {
    // Calculate the number of rows based on the child count
    int childCount = contentRect.childCount;
    int totalRows = Mathf.CeilToInt((float)childCount / columns);

    // Determine how many rows are visible
    int rowsToDisplay = Mathf.Min(totalRows, visibleRows);

    // Calculate the total height for the visible rows
    float totalHeight = (rowsToDisplay * gridLayoutGroup.cellSize.y) +
                        ((rowsToDisplay - 1) * gridLayoutGroup.spacing.y) +
                        gridLayoutGroup.padding.top +
                        gridLayoutGroup.padding.bottom;

    // Apply the height to the ScrollRect
    scrollRectTransform.sizeDelta = new Vector2(scrollRectTransform.sizeDelta.x, totalHeight);

    // Force UI to refresh
    Canvas.ForceUpdateCanvases();
  }
}

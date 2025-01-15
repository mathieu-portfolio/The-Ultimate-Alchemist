#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

public class ResourcesLoader : MonoBehaviour
{
  [Header("Options")]
  [SerializeField] private bool generateIngredients = false;
  [SerializeField] private bool loadEncyclopedia = true;

  [Header("Resource Paths")]
  [Tooltip("Path to the text file containing recipes (relative to Resources folder)")]
  [SerializeField] private string recipeFilePath = "recipes";

  [Tooltip("Path to the folder containing ingredient sprites (relative to Resources folder)")]
  [SerializeField] private string imagesFolderPath = "Images/Ingredients";

  [Tooltip("Path where generated prefabs will be stored (relative to Resources folder)")]
  [SerializeField] private string prefabsFolderPath = "Prefabs/Generated";

  [Tooltip("Subfolder for generated ingredient prefabs")]
  [SerializeField] private string ingredientsFolder = "Ingredients";

  [Tooltip("Name of the encyclopedia prefab")]
  [SerializeField] private string encyclopediaPrefabName = "Encyclopedia";

  private string IngredientsPath => Path.Combine(prefabsFolderPath, ingredientsFolder);

  private void Start()
  {
    if (generateIngredients)
    {
      ResetDirectory(GetFullPath(prefabsFolderPath));
      ResetDirectory(GetFullPath(IngredientsPath));
      CreateRecipes();
      CreateEncyclopedia();
    }

    if (loadEncyclopedia)
    {
      LoadEncyclopedia();
    }
  }

  /// <summary>
  /// Reset a directory (delete and recreate).
  /// </summary>
  private void ResetDirectory(string path)
  {
    try
    {
      if (Directory.Exists(path))
      {
        Directory.Delete(path, true);
      }

      Directory.CreateDirectory(path);
      AssetDatabase.Refresh();

      Debug.Log($"Directory reset successfully: {path}");
    }
    catch (Exception e)
    {
      Debug.LogError($"Failed to reset directory: {e.Message}");
    }
  }

  /// <summary>
  /// Create recipes based on a text file.
  /// </summary>
  private void CreateRecipes()
  {
    TextAsset recipeText = Resources.Load<TextAsset>(recipeFilePath);

    if (recipeText == null)
    {
      Debug.LogError($"Recipe file not found in Resources at: {recipeFilePath}!");
      return;
    }

    string[] lines = recipeText.text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

    foreach (string line in lines)
    {
      if (line.StartsWith("#")) continue; // Ignore comments
      ProcessRecipeLine(line.Trim());
    }

    Debug.Log("Finished generating prefabs for recipes.");
  }

  /// <summary>
  /// Process a single recipe line.
  /// </summary>
  private void ProcessRecipeLine(string line)
  {
    string[] parts = line.Split('=');

    if (parts.Length < 1 || parts.Length > 2)
    {
      Debug.LogWarning($"Invalid recipe format: {line}");
      return;
    }

    string result = parts[0].Trim();
    string recipe = parts.Length == 2 ? string.Join("", parts[1].Split('+').OrderBy(s => s)) : null;

    GameObject resultObject = new GameObject(result);

    // Add SpriteRenderer if an image is found
    Sprite sprite = Resources.Load<Sprite>($"{imagesFolderPath}/{result}");
    if (sprite != null)
    {
      SpriteRenderer renderer = resultObject.AddComponent<SpriteRenderer>();
      renderer.sprite = sprite;
    }
    else
    {
      Debug.LogWarning($"Sprite not found for result: {result}");
    }

    // Add Ingredient Component
    Ingredient resultIngredient = resultObject.AddComponent<Ingredient>();
    resultIngredient.Initialize(result, sprite, recipe, recipe == null);

    SaveAsPrefab(resultObject, result, GetFullPath(IngredientsPath));
    DestroyImmediate(resultObject);
  }

  /// <summary>
  /// Create an encyclopedia prefab.
  /// </summary>
  private void CreateEncyclopedia()
  {
    GameObject encyclopediaObject = new GameObject(encyclopediaPrefabName);
    Encyclopedia encyclopedia = encyclopediaObject.AddComponent<Encyclopedia>();
    Progress progress = encyclopediaObject.AddComponent<Progress>();

    string[] prefabGUIDs = AssetDatabase.FindAssets("t:Prefab", new[] { GetFullPath(IngredientsPath) });
    foreach (string guid in prefabGUIDs)
    {
      string path = AssetDatabase.GUIDToAssetPath(guid);
      GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

      if (prefab != null)
      {
        Ingredient ingredient = prefab.GetComponent<Ingredient>();
        encyclopedia.Add(ingredient);

        if (ingredient.IsBasic())
        {
          progress.Add(ingredient);
        }
      }
    }

    SaveAsPrefab(encyclopediaObject, encyclopediaPrefabName, GetFullPath(prefabsFolderPath));
    DestroyImmediate(encyclopediaObject);
  }

  /// <summary>
  /// Load the encyclopedia prefab into the scene.
  /// </summary>
  private void LoadEncyclopedia()
  {
    GameObject encyclopediaPrefab = Resources.Load<GameObject>($"{prefabsFolderPath}/{encyclopediaPrefabName}");
    if (encyclopediaPrefab != null)
    {
      GameObject encyclopedia = Instantiate(encyclopediaPrefab);
      encyclopedia.name = "Encyclopedia";
    }
    else
    {
      Debug.LogError("Failed to load Encyclopedia prefab from Resources.");
    }
  }

  /// <summary>
  /// Save a GameObject as a prefab.
  /// </summary>
  private void SaveAsPrefab(GameObject gameObject, string prefabName, string path)
  {
    string prefabPath = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(path, $"{prefabName}.prefab"));
    PrefabUtility.SaveAsPrefabAsset(gameObject, prefabPath);
    Debug.Log($"Prefab saved: {prefabPath}");
  }

  /// <summary>
  /// Delete a prefab from the given path.
  /// </summary>
  private void DeletePrefab(string path)
  {
    if (AssetDatabase.DeleteAsset(path))
    {
      Debug.Log($"Prefab deleted: {path}");
    }
    else
    {
      Debug.LogWarning($"Failed to delete prefab: {path}");
    }

    AssetDatabase.Refresh();
  }

  /// <summary>
  /// Get the full project path for a relative Resources path.
  /// </summary>
  private string GetFullPath(string relativePath)
  {
    return Path.Combine(Application.dataPath, relativePath.Replace("Resources/", "").Replace('/', Path.DirectorySeparatorChar));
  }
}
#endif

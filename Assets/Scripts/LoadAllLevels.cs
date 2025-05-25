using UnityEngine;

public class LoadAllLevels : MonoBehaviour
{
    [Header("Level Data")]
    public BasketballLevelSO[] allLevels; // 30 scriptable objects

    [Header("Prefabs & References")]
    public GameObject singleLevelPrefab;  // Prefab with SingleLevel script
    public Transform canvasContainer;     // Parent container (e.g., a GridLayoutGroup or HorizontalLayoutGroup)

    private void Start()
    {
        LoadAllLevelUI();
    }

    private void LoadAllLevelUI()
    {
        for (int i = 0; i < allLevels.Length; i++)
        {
            BasketballLevelSO levelData = allLevels[i];
            GameObject levelGO = Instantiate(singleLevelPrefab, canvasContainer);
            SingleLevel singleLevel = levelGO.GetComponent<SingleLevel>();

            // First level is always unlocked
            bool isUnlocked = (i == 0 || allLevels[i - 1].isCompeleted);
            bool isCompleted = levelData.isCompeleted;

            singleLevel.SetupLevel(levelData, isUnlocked, isCompleted);
        }
    }

}

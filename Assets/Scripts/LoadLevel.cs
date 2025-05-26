using UnityEngine;
using UnityEngine.UI;

public class LoadLevel : MonoBehaviour
{
    [Header("Level Data")]
    public BasketballLevelSO levelData;
    public GameObject rectangle;
    public GameObject triangle;
    public GameObject bouncyTriangle;
    public GameObject starPrefab;
    public GameObject Player;
    public GameObject Ring;

    [Header("UI Elements")]
    public GameObject gameManager;
    public Image starOne;
    public Image starTwo;
    public Image starThree;
    public Sprite yellowSprite;
    public Text level;

    private void Start()
    {

        if (LevelManager.currentLevel != null)
        {
            levelData=LevelManager.currentLevel;
            LoadLevelFromSO(levelData);
        }
        else
        {
            Debug.LogError("LevelManager.currentLevel is NULL. Level not assigned.");
        }

    }

    public void LoadLevelFromSO(BasketballLevelSO data)
    {
        level.text = "LEVEL " + levelData.levelNumber;
        // Load Player
        InstantiateWithTransform(Player,data.player);

        // Load Ring
        InstantiateWithTransform(Ring, data.ring);


        foreach (var pos in data.rectanglesPosition)
        {
            if (data.rectanglesPosition != null)
            {
                Instantiate(rectangle, pos.position, Quaternion.Euler(pos.rotation));
            }
        }
        
        foreach (var pos in data.trianglesPosition)
        {
            if (data.trianglesPosition != null)
            {
                Instantiate(triangle, pos.position, Quaternion.Euler(pos.rotation));
            }
        }
        
        foreach (var pos in data.bouncyTrianglesPosition)
        {
            if (data.bouncyTrianglesPosition != null)
            {
                Instantiate(bouncyTriangle, pos.position, Quaternion.Euler(pos.rotation));
            }
        }

        foreach (var pos in data.starPositions)
        {
            if (starPrefab != null)
            {
                Instantiate(starPrefab, pos, Quaternion.identity);
            }
        }
    }

    private void InstantiateWithTransform(GameObject prefab,ObjectPositionWithRotation objData)
    {
        if (prefab != null)
        {
            GameObject obj = Instantiate(prefab);
            obj.transform.position = objData.position;
            obj.transform.rotation = Quaternion.Euler(objData.rotation);
            var shooter = obj.GetComponent<BasketballDragShoot>();
            if (shooter != null)
            {
                shooter.levelData = levelData;
                shooter.gameManager = gameManager;
                shooter.starOne = starOne;
                shooter.starTwo = starTwo;
                shooter.starThree = starThree;
                shooter.yellowSprite = yellowSprite;
            }
        }
    }
}

using UnityEngine;

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
    private void Start()
    {
        if (levelData == null)
        {
           
            return;
        }

        LoadLevelFromSO(levelData);
    }

    public void LoadLevelFromSO(BasketballLevelSO data)
    {
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
        }
    }
}

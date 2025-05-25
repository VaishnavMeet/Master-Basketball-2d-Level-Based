using UnityEngine;

public class LoadLevel : MonoBehaviour
{
    [Header("Level Data")]
    public BasketballLevelSO levelData;

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
        InstantiateWithTransform(data.player);

        // Load Ring
        InstantiateWithTransform(data.ring);

        // Load Rectangles
        foreach (var rect in data.rectangles)
        {
            InstantiateWithTransform(rect);
        }

        // Load Triangles
        foreach (var tri in data.triangles)
        {
            InstantiateWithTransform(tri);
        }

        // Load Bouncy Triangles
        foreach (var btri in data.bouncyTriangles)
        {
            InstantiateWithTransform(btri);
        }

        // Load Stars
        foreach (var pos in data.starPositions)
        {
            if (data.starPrefab != null)
            {
                Instantiate(data.starPrefab, pos, Quaternion.identity);
            }
        }
    }

    private void InstantiateWithTransform(GameObjectWithPosition objData)
    {
        if (objData.prefab != null)
        {
            GameObject obj = Instantiate(objData.prefab);
            obj.transform.position = objData.position;
            obj.transform.rotation = Quaternion.Euler(objData.rotation);
        }
    }
}

using UnityEngine;

[System.Serializable]
public struct GameObjectWithPosition
{
    public GameObject prefab;
    public Vector2 position;
    public Vector2 rotation;
}

[CreateAssetMenu(fileName = "NewBasketballLevel", menuName = "Basketball/Level")]
public class BasketballLevelSO : ScriptableObject
{
    [Header("Level Meta")]
    public int levelNumber;

    [Header("Player and Ring")]
    public GameObjectWithPosition player;
    public GameObjectWithPosition ring;

    [Header("Obstacles")]
    public GameObjectWithPosition[] rectangles;
    public GameObjectWithPosition[] triangles;
    public GameObjectWithPosition[] bouncyTriangles;

    [Header("Stars")]
    public GameObject starPrefab;
    public Vector2[] starPositions = new Vector2[3]; // Always 3 stars
}

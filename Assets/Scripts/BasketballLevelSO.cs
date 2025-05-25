using UnityEngine;


[System.Serializable]
public struct ObjectPositionWithRotation
{
    public Vector3 position;
    public Vector3 rotation;
}

[CreateAssetMenu(fileName = "NewBasketballLevel", menuName = "Basketball/Level")]
public class BasketballLevelSO : ScriptableObject
{
    [Header("Level Meta")]
    public int levelNumber;

    [Header("Player and Ring")]
    public ObjectPositionWithRotation player;
    public ObjectPositionWithRotation ring;

    [Header("Obstacles")]
   
    public ObjectPositionWithRotation[] rectanglesPosition;
    public ObjectPositionWithRotation[] trianglesPosition;
    public ObjectPositionWithRotation[] bouncyTrianglesPosition;

    [Header("Ring")]
    public Vector3[] starPositions = new Vector3[3]; // Always 3 stars
}

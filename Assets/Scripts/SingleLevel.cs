using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SingleLevel : MonoBehaviour
{
    [Header("Scriptable Object")]
    public BasketballLevelSO basketballLevelSO;

    [Header("UI References")]
    public Text levelNoText;
    public GameObject[] starImages;           // Array of 3 star image slots
    public GameObject lockIcon;           // Lock image GameObject
    public Button levelButton;           // Button to play level
    [Header("Sprites")]
    public Sprite yellowStarSprite;      // Empty star


    private void Start()
    {
        if (basketballLevelSO.isCompeleted || basketballLevelSO.levelNumber==1)
        {
            SetupLevel(basketballLevelSO,true,true);
        }
    }

    public void SetupLevel(BasketballLevelSO levelData, bool isUnlocked,bool isCompeleted)
    {
        basketballLevelSO = levelData;
      


        // Set level number
        levelNoText.text = "";
        levelButton.interactable = false;

        if (isUnlocked)
        {
            levelNoText.text = basketballLevelSO.levelNumber.ToString();
            levelButton.interactable = true;
        }
        if (isCompeleted)
        {
            levelNoText.text = basketballLevelSO.levelNumber.ToString();
            levelButton.interactable = true;
            lockIcon.SetActive(false);
        }
        else
        {
            lockIcon.SetActive(!isCompeleted);
            lockIcon.SetActive(!isUnlocked);
        }
            // Set stars
            for (int i = 0; i < starImages.Length; i++)
            {
                if (i < basketballLevelSO.stars)
                    starImages[i].GetComponent<Image>().sprite = yellowStarSprite;

            }

        // Lock logic

      

        
            levelButton.onClick.AddListener(() => LoadThisLevel());
        
    }

    private void LoadThisLevel()
    {
        Debug.Log("Assigning level: " + basketballLevelSO.levelNumber);
        LevelManager.currentLevel = basketballLevelSO;
        SceneManager.LoadScene("GameScene");

    }
}

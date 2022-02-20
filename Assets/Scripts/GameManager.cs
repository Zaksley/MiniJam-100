using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.SceneManagement; 

public class GameManager : MonoBehaviour
{

    [SerializeField] private GameObject foodPrefab; 
    [SerializeField] private GameObject player;
    [SerializeField] private Slider slider;

    public List<Sprite> spritesFood; 
    // [SerializeField] private float timeInvokeFood = 5f;
    [SerializeField] private float timeSpawnFood = 2f; 
    [SerializeField] private int numberFood = 8; 
    [SerializeField] private int foodRateChange = 3; 
    [SerializeField] private float multiplicaterFoodSize; 
    public int foodCurrentStep = 1;  
    private float actualFoodSize; 

    // Init values
    [SerializeField] private float initValueFood = 1.0f; 
    [SerializeField] private float initSizeFood = 0.5f;
    [SerializeField] private float initEndStage = 4f; 
    


    // Camera variables
    private Camera cam; 
    public float camLeft; 
    public float camRight; 
    public float camTop; 
    public float camBot; 
    private float currentSizeCam; 
    private float nextSizeCam; 
    private bool needUpdateCam; 
    [SerializeField] private float multiplicaterCamSize = 1.5f; 
    [SerializeField] private float multiplicaterCamSpeedTime = 1.5f; 

    [SerializeField] private Text Score; 
    [SerializeField] private Text top; 
    [SerializeField] private Text bot; 
    [SerializeField] private Text right; 
    [SerializeField] private Text left; 

    // Start is called before the first frame update
    void Start()
    {

        slider.minValue = player.GetComponent<PlayerFoodManager>().killValueFood; 
        slider.maxValue = player.GetComponent<PlayerFoodManager>().neededFood; 

        cam = Camera.main; 
        currentSizeCam = cam.orthographicSize; 
        nextSizeCam = currentSizeCam; 
        needUpdateCam = false; 
        UpdateCamera(); 
    
        for(var ifood = 0; ifood < numberFood; ifood++)
            createFood(initValueFood, initSizeFood);

        actualFoodSize = initSizeFood; 

        //InvokeRepeating("createFood", timeInvokeFood, timeSpawnFood);

        StartCoroutine(spawnFood(initEndStage, initValueFood, initSizeFood)); 
        StartCoroutine(spawnFood(initEndStage, initValueFood, initSizeFood)); 
        StartCoroutine(player.GetComponent<PlayerFoodManager>().DecreaseFood());
    }

    // Update is called once per frame
    void Update()
    {
        if (currentSizeCam < nextSizeCam)
        {
            currentSizeCam += Time.deltaTime * multiplicaterCamSpeedTime; 
            cam.orthographicSize = currentSizeCam; 
            UpdateCamera(); 
        }
        else 
        {
            if (needUpdateCam) {
                needUpdateCam = false; 
                UpdateCamera(); 
            }
        }

        // ******
        //  UI
        // ******

        // Slider food 
        slider.value = player.GetComponent<PlayerFoodManager>().currentFood; 
        Score.text = foodCurrentStep.ToString(); 

        // Display keys
        top.text = player.GetComponent<PlayerController>().top.ToUpper(); 
        bot.text = player.GetComponent<PlayerController>().bot.ToUpper(); 
        right.text = player.GetComponent<PlayerController>().right.ToUpper(); 
        left.text = player.GetComponent<PlayerController>().left.ToUpper(); 
    }

    public void UpdateCamera() 
    {
        Vector3 bottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, 10f));

        float offsetYBot = 0.5f; 
        float offsetYTop = -1.5f;
        float offsetX = 1.0f; 

        camLeft = bottomLeft.x + offsetX;
        camRight = -camLeft;
        camBot = bottomLeft.y + offsetYBot; 
        camTop = -bottomLeft.y + offsetYTop; 

        player.GetComponent<PlayerController>().maxRight = -bottomLeft.x;
        player.GetComponent<PlayerController>().maxLeft = bottomLeft.x; 
        player.GetComponent<PlayerController>().maxTop = -bottomLeft.y; 
        player.GetComponent<PlayerController>().maxBot = bottomLeft.y; 

        /* Debug Cam
        Debug.Log("Left" + player.GetComponent<PlayerController>().maxLeft);
        Debug.Log("Right" + player.GetComponent<PlayerController>().maxRight);
        Debug.Log("Top" + player.GetComponent<PlayerController>().maxTop);
        Debug.Log("Bot" + player.GetComponent<PlayerController>().maxBot); */
    }

    public void ManagementNextStep()
    {
        if (foodCurrentStep >= 15)  
        {
            SceneManager.LoadScene("Win"); 
            return;
        }
        
        DezoomCamera(); 

        float divByLevel = foodCurrentStep / foodRateChange + 1;
        float value = initValueFood * Mathf.Pow(1.3f, foodCurrentStep); 

        if (foodCurrentStep % foodRateChange == 0)
        {
            var stopStage = foodCurrentStep + foodRateChange; 
            actualFoodSize *= 2;

            StartCoroutine(spawnFood(stopStage, value, actualFoodSize));
        }

        else if (foodCurrentStep % foodRateChange == 1)
        {
            var stopStage = foodCurrentStep + foodRateChange + 1; 
            var size = actualFoodSize + actualFoodSize * 2 / 3;
            StartCoroutine(spawnFood(stopStage, value, size));
        }

        else if (foodCurrentStep % foodRateChange == 2)
        {
            var stopStage = foodCurrentStep + foodRateChange + 2; 
            var size = actualFoodSize + actualFoodSize * 4 / 3;
            StartCoroutine(spawnFood(stopStage, value, size));
        }

        slider.minValue = player.GetComponent<PlayerFoodManager>().killValueFood; 
        slider.maxValue = player.GetComponent<PlayerFoodManager>().neededFood; 

        player.GetComponent<PlayerController>().UpdateSpeed();
        player.GetComponent<PlayerController>().ShuffleKeys(); 
    }

    private void DezoomCamera() 
    {
        nextSizeCam *= multiplicaterCamSize;
        needUpdateCam = true; 
    }

    private void createFood(float valueFood, float sizeFood) 
    {
        float x = Random.Range(camLeft, camRight); 
        float y = Random.Range(camBot, camTop); 

        Vector3 spawnPoint = new Vector3(x, y, 10f);
        GameObject food = Instantiate( foodPrefab, spawnPoint, Quaternion.identity );   
        food.GetComponent<SpriteRenderer>().sprite = spritesFood[Random.Range(0, spritesFood.Count)];
        food.GetComponent<FoodManager>().foodValue = valueFood; 
        food.transform.localScale = new Vector3(sizeFood, sizeFood, 10f);         
    }

    private IEnumerator spawnFood(float stopStage, float valueFood, float sizeFood)
    {   
        while(foodCurrentStep < stopStage)
        {
            yield return new WaitForSeconds(timeSpawnFood); 
            createFood(valueFood, sizeFood); 
        }
    }
}

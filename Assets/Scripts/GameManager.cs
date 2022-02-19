using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{

    [SerializeField] private GameObject foodPrefab; 
    [SerializeField] private GameObject player;
    public List<Sprite> spritesFood; 
    // [SerializeField] private float timeInvokeFood = 5f;
    [SerializeField] private float timeSpawnFood = 2f; 
    [SerializeField] private int numberFood = 5; 
    [SerializeField] private int foodRateChange = 3; 
    public int foodCurrentStep = 1;  

    // Init values
    [SerializeField] private float initValueFood = 1.0f; 
    [SerializeField] private float initSizeFood = 0.5f;
    [SerializeField] private float initEndStage = 4f; 
    


    // Camera variables
    private Camera cam; 
    private float camLeft; 
    private float camRight; 
    private float camTop; 
    private float camBot; 
    private float currentSizeCam; 
    private float nextSizeCam; 
    private bool needUpdateCam; 
    [SerializeField] private float multiplicaterCamSize = 1.5f; 
    [SerializeField] private float multiplicaterSpeedTime = 1f; 


    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main; 
        currentSizeCam = cam.orthographicSize; 
        nextSizeCam = currentSizeCam; 
        needUpdateCam = false; 
        UpdateCamera(); 
    
        for(var ifood = 0; ifood < numberFood; ifood++)
            createFood(initValueFood, initSizeFood);

        //InvokeRepeating("createFood", timeInvokeFood, timeSpawnFood);

        StartCoroutine(spawnFood(initEndStage, initValueFood, initSizeFood)); 
    }

    // Update is called once per frame
    void Update()
    {
        if (currentSizeCam < nextSizeCam)
        {
            currentSizeCam += Time.deltaTime * multiplicaterSpeedTime; 
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
    }

    public void UpdateCamera() 
    {
        Vector3 bottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, 10f));

        camLeft = bottomLeft.x + 0.5f;
        camRight = -camLeft;
        camBot = bottomLeft.y + 0.5f; 
        camTop = -camBot; 
    }

    public void ManagementNextStep()
    {
        DezoomCamera(); 
        //InvokeRepeating("createFood", timeSpawnFood, timeSpawnFood); 

            // Update food 
        if (foodCurrentStep % foodRateChange == 0)
        {
            var stopStage = foodCurrentStep + foodRateChange; 
            var value = Mathf.Pow(2, (foodCurrentStep / foodRateChange)); 
            var size = initValueFood * (foodCurrentStep / foodRateChange);
            Debug.Log("stage:" + stopStage); 
            Debug.Log("value:" + value); 
            Debug.Log("size:" + size); 

            StartCoroutine(spawnFood(stopStage, value, size));
        }


        StartCoroutine(spawnFood(initEndStage, initValueFood, initSizeFood)); 
        player.GetComponent<PlayerController>().UpdateSpeed(); 
    }

    private void DezoomCamera() 
    {
        nextSizeCam *= multiplicaterCamSize;
        needUpdateCam = true; 
    }

    private void createFood(float valueFood, float sizeFood) 
    {

        Vector3 spawnPoint = new Vector3(Random.Range(camLeft, camRight), Random.Range(camBot, camTop), 10f);
        GameObject food = Instantiate( foodPrefab, spawnPoint, Quaternion.identity );   
        food.GetComponent<SpriteRenderer>().sprite = spritesFood[Random.Range(0, spritesFood.Count)];
        food.GetComponent<FoodManager>().foodValue = valueFood; 
        food.transform.localScale = new Vector3(sizeFood, sizeFood, sizeFood);
        
        /*
        FoodManager foodScript = food.GetComponent<FoodManager>(); 
        foodScript.foodValue = valueFood; */
        
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

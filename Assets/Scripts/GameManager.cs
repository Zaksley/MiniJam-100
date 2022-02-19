using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{

    [SerializeField] private GameObject foodPrefab; 
    public List<Sprite> spritesFood; 
    [SerializeField] private float timeInvokeFood = 5f;
    [SerializeField] private float timeSpawnFood = 2f; 
    [SerializeField] private int numberFood = 5; 


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
            createFood();

        InvokeRepeating("createFood", timeInvokeFood, timeSpawnFood);
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

    public void DezoomCamera() 
    {
        nextSizeCam *= multiplicaterCamSize;
        needUpdateCam = true; 
    }

    private void createFood() 
    {

        Vector3 spawnPoint = new Vector3(Random.Range(camLeft, camRight), Random.Range(camBot, camTop), 10f);
        GameObject food = Instantiate( foodPrefab, spawnPoint, Quaternion.identity );   
        food.GetComponent<SpriteRenderer>().sprite = spritesFood[Random.Range(0, spritesFood.Count)];
    }
}

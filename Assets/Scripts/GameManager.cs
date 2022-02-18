using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{

    [SerializeField] private GameObject foodPrefab; 
    public List<Sprite> spritesFood; 
    [SerializeField] private float timeInvokeFood = 5f;
    [SerializeField] private float timeSpawnFood = 2f; 


    private Camera cam; 
    private float camLeft; 
    private float camRight; 
    private float camTop; 
    private float camBot; 


    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main; 
        Vector3 bottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, 10f));

        camLeft = bottomLeft.x + 0.5f;
        camRight = -camLeft;
        camBot = bottomLeft.y + 0.5f; 
        camTop = -camBot; 

        InvokeRepeating("createFood", timeInvokeFood, timeSpawnFood);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void createFood() 
    {

        Vector3 spawnPoint = new Vector3(Random.Range(camLeft, camRight), Random.Range(camBot, camTop), 10f);
        GameObject food = Instantiate( foodPrefab, spawnPoint, Quaternion.identity );   
        food.GetComponent<SpriteRenderer>().sprite = spritesFood[Random.Range(0, spritesFood.Count)];

        //Instantiate(food, new Vector2(x, y), Quaternion.identity);
    }
}

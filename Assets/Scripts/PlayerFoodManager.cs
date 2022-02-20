using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 
using UnityEngine.UI; 

public class PlayerFoodManager : MonoBehaviour
{
    // Food levels 
    public float killValueFood = 0f; 
    public float closeKillValueFood = 2f; 
    public float currentFood = 5f;
    public float neededFood = 10f; 
    private Color red = new Color(179f/255, 61f/255, 61f/255); 

    /*
    [SerializeField] private Slider slider;
    private ColorBlock cb; */

    [SerializeField] private float multiplicaterFood = 2f; 
    [SerializeField] private float multiplicaterSize = 1.5f; 
    [SerializeField] private float decreaseTimer = 3.0f; 
    [SerializeField] private float timeDecreaseToZero = 20f; 

    [SerializeField] private GameManager manager; 

    void Start() {
        //cb = new ColorBlock();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Food"))   
        {
            Destroy(other.gameObject);
            currentFood += other.GetComponent<FoodManager>().foodValue; 

            // New step 
            if (currentFood >= neededFood)
            {
                NextStep(); 
            }
        }
    }

    private void NextStep() 
    {
        // Update food 
        killValueFood = neededFood / 2; 
        closeKillValueFood = neededFood / 6; 
        neededFood += neededFood / multiplicaterFood; 
        transform.localScale = transform.localScale * multiplicaterSize; 
    
        manager.GetComponent<GameManager>().foodCurrentStep++; 
        manager.GetComponent<GameManager>().ManagementNextStep(); 
    }

    public IEnumerator DecreaseFood()
    {   
        while(true)
        {
            yield return new WaitForSeconds(decreaseTimer); 
            float middleFood = (neededFood - killValueFood) / 2; 
            float decreaseValue = middleFood / timeDecreaseToZero; 
            currentFood -= decreaseValue;  

            // Danger zone for player 
            if (currentFood <= closeKillValueFood) 
            {
                GetComponent<SpriteRenderer>().color = red; 
            }
            else 
            {
                GetComponent<SpriteRenderer>().color = Color.white;
            }

                // Kill 
            if (currentFood <= killValueFood) 
            {
                SceneManager.LoadScene("Defeat"); 
            }
        }
    }
}

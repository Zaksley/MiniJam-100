using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class PlayerFoodManager : MonoBehaviour
{
    // Food levels 
    public float killValueFood = 0f; 
    public float currentFood = 5f;
    public float neededFood = 10f; 
    [SerializeField] private float multiplicaterFood = 2f; 
    [SerializeField] private float multiplicaterSize = 1.5f; 
    [SerializeField] private float decreaseTimer = 3.0f; 
    [SerializeField] private float timeDecreaseToZero = 20f; 

    [SerializeField] private GameManager manager; 


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

                // Kill 
            if (currentFood <= killValueFood) 
            {
                SceneManager.LoadScene("Game"); 
            }

        }
    }
}

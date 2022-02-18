using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFoodManager : MonoBehaviour
{
    // Food levels 
    public float currentFood = 0f;
    public float neededFood = 5f; 
    [SerializeField] private float multiplicaterFood = 2f; 
    [SerializeField] private float multiplicaterSize = 1.5f; 
    public int foodCurrentStep = 1;  



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Food"))   
        {
            Destroy(other.gameObject);
            currentFood++; 

            if (currentFood >= neededFood) {
                neededFood *= multiplicaterFood; 
                foodCurrentStep++; 

                transform.localScale = transform.localScale * multiplicaterSize; 

            }
        }
    }
}

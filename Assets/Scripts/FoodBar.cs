using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodBar : MonoBehaviour
{

    private Slider slider; 
    private float targetProgress = 0f; 
    public float FillSpeed = 0.5f; 

    private void Awake() 
    {
        slider = gameObject.GetComponent<Slider>(); 
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   

    }

    public void IncrementProgess(float newProgress) 
    {
        targetProgress = slider.value + newProgress; 
    }
}

using UnityEngine;
using System.Collections; 
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{

    public string top = "z"; 
    public string bot = "s"; 
    public string right = "d"; 
    public string left = "q"; 
    private string[] alphabet = new string[] {"a", "z", "e", "r", "t", "y", "u", "i", "o", "p", "q", "s", "d", "f", "g", "h", "j", "k", "l", "m", "w", "x", "c", "v", "b", "n"}; 
    private List<string> currentAlphabet; 

    public float speed = 4.5f; 
    [SerializeField] private float updateSpeed = 1.2f;
    private Camera mainCam; 
    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main; 

        top = "z"; 
        bot = "s"; 
        right = "d"; 
        left = "q"; 
    }

    // Update is called once per frame
    void Update()
    {
            // Movement Input 
        Vector2 moveDirection = Vector2.zero; 

        if (Input.GetKey(right)) moveDirection.x += 1; 
        if (Input.GetKey(left)) moveDirection.x += -1; 
        if (Input.GetKey(top)) moveDirection.y += 1; 
        if (Input.GetKey(bot)) moveDirection.y += -1; 

        moveDirection = moveDirection.normalized; 
        transform.Translate(moveDirection * speed * Time.deltaTime);
    }

    public void UpdateSpeed() 
    {
        speed *= updateSpeed; 
    }

    public void ShuffleKeys()
    {
        currentAlphabet = new List<string>(alphabet); 

        top = SelectKickLetter(currentAlphabet); 
        bot = SelectKickLetter(currentAlphabet); 
        right = SelectKickLetter(currentAlphabet); 
        left = SelectKickLetter(currentAlphabet);

        Debug.Log("top:" + top); 
        Debug.Log("bot:" + bot); 
        Debug.Log("right:" + right); 
        Debug.Log("left:" + left); 

    }

    private string SelectKickLetter(List<string> curentAlphabet)
    {
        var index = Random.Range(0, currentAlphabet.Count); 
        var choosed = currentAlphabet[index]; 
        currentAlphabet.RemoveAt(index);
        return choosed; 
    }
}

using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float speed = 4.5f; 
    private Camera mainCam; 
    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main; 
        Debug.Log(mainCam.pixelWidth); 
    }

    // Update is called once per frame
    void Update()
    {
            // Movement Input 
        Vector2 moveDirection = Vector2.zero; 
        var vertical = Input.GetAxisRaw("Vertical"); 
        var horizontal = Input.GetAxisRaw("Horizontal"); 

        if (horizontal > 0) moveDirection.x += 1; 
        if (horizontal < 0) moveDirection.x += -1; 

        if (vertical > 0) moveDirection.y += 1; 
        if (vertical < 0) moveDirection.y += -1; 

        moveDirection = moveDirection.normalized; 
        transform.Translate(moveDirection * speed * Time.deltaTime);
    }
}

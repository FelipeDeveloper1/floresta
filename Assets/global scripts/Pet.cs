using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Animator))]
public class Pet : MonoBehaviour
{
    public GameObject player;
    public Animator petAnimator;
    public float speed = 1;
    public float keepDistance = 0.1f;
    public bool infloor = true;
    bool isWalking;
    float input_x; 
    float lastDirectionX;
    Vector2 petPos;
    Vector2 playerPos;
    private bool isFacingRight = true;

    private void Start()
    {
        petAnimator =  GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        petPos = transform.position;
        playerPos = SetDirection(1, player.transform.position);
        transform.position = Vector2.MoveTowards(petPos, playerPos, speed * Time.deltaTime );
        
    }
    private void Update() 
    {
        input_x = Input.GetAxisRaw("Horizontal");  
        isWalking = (input_x != 0);
        if (isWalking) {
            petAnimator.SetFloat("input_x", input_x);
        }
        if (input_x > 0 || input_x < 0) { 
            lastDirectionX = input_x;
        }
       
       
        petAnimator.SetBool("isWalking", isWalking); 

        petPos  =  transform.position;
        playerPos = SetDirection(lastDirectionX, player.transform.position);
        transform.position = Vector2.MoveTowards(petPos,  playerPos , speed * Time.deltaTime) ;
    }
    
    
    
    
    Vector2 SetDirection(float input_x, Vector2 playerPos)
    {
        if(input_x < 0 ) {
            playerPos.x += keepDistance;
            Flip();
           
        }
        else if(input_x > 0) {
            playerPos.x  -= keepDistance;
            Flip();
        }
      
        return playerPos;
    }
  private void Flip() 
	{
		if (isFacingRight && input_x < 0f || !isFacingRight && input_x > 0f) 
		{
			Vector3 localScale = transform.localScale; 
			isFacingRight = !isFacingRight; 
			localScale.x *= -1f; 
			transform.localScale = localScale; 
		}
	}
}

	
	
	
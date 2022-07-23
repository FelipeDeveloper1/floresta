using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyFollow : MonoBehaviour
{
    public float distance;
    public float triggerDistance;
    public float speed;
    private Transform target;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void Update()
    {
        Follow();
    }

    void Follow()
    {
        distance = transform.position.x - target.position.x;
        if(distance < 0)
        {
            distance *= -1;
        }

        if(distance < triggerDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(target.position.x, transform.position.y), speed * Time.deltaTime);
        }
    }
    public void OnCollisionEnter2D(Collision2D collision) //Usado para a colisão do inimigo
    {
        if (collision.gameObject.tag == "Player") //Relaciono com o game object Player
        { 
            collision.gameObject.GetComponent<Life> ().takeDamage();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shield : MonoBehaviour
{
    public bool isBlocking;
    public Vector3 startPosition;
    public Renderer rend;

    // Start is called before the first frame update
    void Start()
    {
        this.transform.localPosition = startPosition;
        this.rend = GetComponent<Renderer>();
        this.rend.enabled = false;
        GetComponent<Collider2D>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            this.transform.localPosition = new Vector3(0.29f,0.8734f, 0);
        }

        else if (Input.GetKeyDown(KeyCode.D))
        {
            this.transform.localPosition = startPosition;
        }
    }

    public void takeShield()
    {
        this.rend.enabled = true;
        GetComponent<Collider2D>().enabled = true;
        this.isBlocking = true;
    }

    public void guardShield()
    {
        this.rend.enabled = false;
        GetComponent<Collider2D>().enabled = false;
        this.isBlocking = false;
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            StartCoroutine(Recoil());
        }
    }

    private IEnumerator Recoil()
    {
        yield return new WaitForSeconds(0.1f);
        this.rend.enabled = false;
        GetComponent<Collider2D>().enabled = false;
        this.isBlocking = false;
    }
}
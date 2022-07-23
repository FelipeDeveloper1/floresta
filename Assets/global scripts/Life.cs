using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Life : MonoBehaviour
{
    public int maxLife;
    public int currentLife;
    public bool invincible;
    public float inviCooldown;
    public int damage;
    int baseDamage;
    public GameObject def;
    public bool block;

    // Start is called before the first frame update
    void Start()
    {
        currentLife = maxLife;
        invincible = false;
        baseDamage = damage;
    }

    // Update is called once per frame
    void Update()
    {
        block = def.GetComponent<shield>().isBlocking;

        if (block == true)
        {
            damage = 0;
        }

        else if (block == false)
        {
            damage = baseDamage;
        }
    }

    public void takeDamage()
    {
        if (invincible == false)
        {
            currentLife -= damage;
            
            if (block == false)
                StartCoroutine(Invincible());
        }

        if(currentLife <= 0){
            invincible = false;
            gameObject.GetComponent<Animator> ().SetTrigger ("Dead"); //Ativo a animação de morto
			gameObject.GetComponent<Rigidbody2D> ().velocity = Vector2.zero; //Zero a velocidade do player
			gameObject.GetComponent<CapsuleCollider2D> ().enabled = false; // Desativo o colisor
			gameObject.GetComponent<Rigidbody2D> ().bodyType = RigidbodyType2D.Kinematic; //Deixo sua rigidbody kinematic
			gameObject.GetComponent<PlayerController> ().enabled = false; //Deixo o componente Player como falso
			gameObject.GetComponent<Animator> ().SetBool ("Jump", false); //Impeço que o player pule enquanto está morto
			Invoke ("LoadScene", 1f); //Reinicio a cena, fazendo o player voltar depois de 1 segundo de morto
        }
    }

    private IEnumerator Invincible()
    {
        invincible = true;
        yield return new WaitForSeconds(inviCooldown);
        invincible = false;
    }

    void LoadScene () //Usado para mexer no sistema de cenas
	{
		SceneManager.LoadScene("SampleScene"); //Aqui eu digo qual cena deve ser chamada no método Invoke lá em cima hehe
	}
}

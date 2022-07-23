using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parachute : MonoBehaviour {

	public Rigidbody2D rb; //Chamamos o rigidbody2d
	public float velY; //criamos uma variável para guardar o valor da velocidade do paraquedas em y
	private bool isGliding; //E outra para averiguar se estamos planando ou não
	
	void Update () 
	{
		if (this.isGliding) //Se estivermos planando
		{ 
			Vector2 vel = this.rb.velocity; //Definimos um valor para a velocidade (que nesse caso é a velocidade do rigidbody
			if (vel.y < velY) //Se a velocidade em y for menor que o valor da val velocidade em y
			{
				vel.y = velY; //Definimos o valor da velocidade em y igual ao da variável vely
				this.rb.velocity = vel; //A velocidad será igual a da variável vel
			}
		}
	}

	public void OpenParachute () //Definimos o que occore quando o paraquedas for aberto
	{
		this.gameObject.SetActive (true); //Ativamos o objeto Parachute
		this.isGliding = true; //Colocamos que ele está planando 
	}

	public void CloseParachute () //Definimos o que ocorre quando o paraquedas for fechado
	{
		this.gameObject.SetActive (false); //Desativamos o objeto Parachute
		this.isGliding = false; //Colocamos que ele não está planando
	}
}
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	//Variáveis de movimentação básica
	private float horizontal; //Criamos a variável horizontal para armazenamos o mecânica de movimentação horizontal :)
	public float speed; // Criamos a variável onde o valor da velocidade do player será registrada (obs: como ela é pública, significa que podemos mudar o inserir dela diretamente na unity)
	public float jumpingPower; //Criamos a variável onde o valor da força de pulo do player será registrada (obs: como ela é pública, significa que podemos inserir o valor dela diretamente na unity)
	public bool doubleJump; //Criamos a variável para averiguar se o personagem pode ou não pode realizar um pulo duplo
	public bool tripleJump; //Criamos outra variável para averiguar se o personagem pode ou não pode realizar um pulo triplo (sim, pulo triplo 0o0)
	public bool infloor = true; // Criamos uma variável para averiguar se o personagem está no chão
	private bool isFacingRight = true; //Criamos uma variável para ver se o personagem está olhando para direita (nesse caso, começa como true, porquê ele está voltado para a direita (vai bolsonaro))

	//Variavéis de Dash
	private TrailRenderer trailRenderer;
	public float dashVel;
	public float dashTime;
	private Vector2 dashDir;
	private bool isDashing;
	private bool canDash;//Criamos uma variável para definir a duração de pausa entre um dash e outro (obs: como ela é pública, significa que podemos mudar o inserir dela diretamente na unity)
	public shield shield;
	//Variáveis de Parachute
	public Parachute parachute; //Usamos para chamar o script de Parachute no script do player

	//Variáveis de ativadores de ferramentas da Unity
	[SerializeField] private Rigidbody2D rb; //Usamos para ativaras ferramentas do Rigidbody (definimos qual Rigidbody será usada na Unity)
	[SerializeField] private Transform groundCheck; //Usamos para definir o Ground Check (armazenamos o ground check da Unity aqui)
	[SerializeField] private LayerMask groundLayer; //Usamos para definir o Ground Layer (armazenamos o ground layer da Unity aqui)
	[SerializeField] private Animator animator; //Usamos para ativaras ferramentas do Animator (definimos qual Animator será usada na Unity)

  	private void Start()
    {
		shield.guardShield();
		trailRenderer = GetComponent<TrailRenderer>();
	}
	private void Update()
	{
		horizontal = Input.GetAxisRaw("Horizontal"); //Definimos o valor da variável horizontal

		if (Input.GetButtonDown ("Jump")) { //Se o botão pulo for pressionado 
			if(infloor) //E ele estiver no chão
			{
				rb.velocity = Vector2.zero; //Usamos para de unificar e padronizar a força de cada pulo
				animator.SetBool ("Jump", true); //Ativamos a animação de pulo
				rb.AddForce (new Vector2 (0, jumpingPower), ForceMode2D.Impulse); //Usamos a força do pulo para impulsionar o player
				infloor = false; //Como ele está no ar, infloor é falso
				doubleJump = true; //Como ele está no ar, ele pode pular uma segunda vez
			}
			else if(!infloor && doubleJump) //Se ele não estiver no chão e estiver fazendo um pulo duplo
			{
				rb.velocity = Vector2.zero; //Usamos para de unificar e padronizar a força de cada pulo
				animator.SetBool ("Jump", true); //Ativamos a animação de pulo
				rb.AddForce (new Vector2 (0, jumpingPower), ForceMode2D.Impulse); //Usamos a força do pulo para impulsionar o player novamente
				infloor = false; //Como ele está no ar, infloor é falso 
				doubleJump = false; //Como ele acabou de realizar um pulo duplo, ele não pode dar um outro pulo duplo em seguida
				tripleJump = true; //E como já fez um pulo duplo, é permitido que ele dê um terceiro pulo no ar antes de cair, liberando o pulo duplo
			}
			else if(!infloor && tripleJump) //Se ele não estiver no chão e estiver fazendo um pulo triplo
			{
				rb.velocity = Vector2.zero; //Usamos para de unificar e padronizar a força de cada pulo
				animator.SetBool ("Jump", true); //Ativamos a animação de pulo
				rb.AddForce (new Vector2 (0, jumpingPower), ForceMode2D.Impulse); //Usamos a força do pulo para impulsionar o player novamente
				infloor = false; //Como ele está no ar, infloor é falso
				doubleJump = false; //Como ele já realizou um pelo duplo e ainda não voltou para o chão, ele não pode dar outro pulo duplo
				tripleJump = false; // Como ele acabou de realizar um pulo triplo, ele não pode dar um outro pulo triplo em seguida, não podendo pular novamente até tocar o chão
			}
		}

		

		Flip(); //Registramos o método flip no void update par ele ser ativado quando chamado
		Glide (); //Registramos o método glide no void update par ele ser ativado quando chamado
		Attack (); //Registramos o método attack no void update par ele ser ativado quando chamado
		Block();
	}

	private void OnCollisionEnter2D(Collision2D collision) //Usamos para armazenar funções que envolvem colisão
	{
		if (collision.gameObject.layer == 8) //Se o player colidir com a layer 8 (que é a layer ground, não se esqueçam)
		{ 
			canDash = true;
			animator.SetBool ("Jump", false); //Desativamos a animação de pulo dele
			infloor = true; //Ele está no ção agora, pois está em contato com a layer ground 8
			doubleJump = false; //Ele não pode dar um pulo duplo, pois está no chão e ainda não deu o primeiro pulo
			tripleJump = false; //Ele não pode dar um pulo triplo, pois está no chão e ainda não deu o primeiro e nem o segundo pulo
		}
	}

	private void FixedUpdate() //É basicamente o método Update só que mais foda (kkkk mentira, usamos para trabalhar com física 2D)
	{
		

		rb.velocity = new Vector2(horizontal * speed, rb.velocity.y); //Definimos a velocidade do velocity do rigidbody

		if (horizontal > 0) //Se a posição horizontal for maior que zero (ou seja, para a direita (vai bolsonaro))
		{
			animator.SetBool ("Run", true); //A animação de correr é liberada
		}
		else if (horizontal < 0) //Se a posição horizontal for menor que zero (ou seja, para a esquerda (lula livre))
		{
			animator.SetBool ("Run", true); //A animação de correr é liberada
		} 
		else 
		{
			animator.SetBool ("Run", false); //A animação de correr é barrada. Isso é importante para impedir que o personagem corra infinitamente após sua movimentação horizontal ser ativada
		}

		Dash();
	}

	public bool IsGrounded() //Se o personagem estiver no chão (escolhi criar uma outra variável privada para averiguar se o player está no chão, pois eu queria uma variável que tivesse um registro diferente de infloor)
	{
		return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer); //
	}

	private void Flip() //Usamos para virar o personagem da direita para a esquerda e vice-versa
	{
		if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f) //Aqui checamos se o personagem está olhando para a direita ou não e associamos isso com a variável horizontal 
		{
			Vector3 localScale = transform.localScale; //Transformamos a escala do local
			isFacingRight = !isFacingRight; //Damaos o valor contrário da variável isFacingRight
			localScale.x *= -1f; //Definimos o valor da escala local de x como -1
			transform.localScale = localScale; //Tranformamos a escala local e damos a ela o valor acima
		}
	}
		
	private void Glide() //Usamos para configurar a planagem do personagem e inserir a mecânica do parachute
	{
		if (this.infloor) //Se estiver no chão
		{
			this.parachute.CloseParachute (); //O parquedas fecha 
		} 
		else 
		{
			if (Input.GetKeyDown (KeyCode.Space)) //Se o botão espaço for pressionado
			{
				this.parachute.OpenParachute (); //O paraquedas abre
			} 
			else if (Input.GetKeyUp (KeyCode.Space)) //Se o botão for soltado
			{
				this.parachute.CloseParachute (); //O paraquedas fecha 
			}
		}
	}
	private void Dash() {
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            isDashing = true;
            canDash = false;
			trailRenderer.emitting = true;
            dashDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            if (dashDir == Vector2.zero)
            {
                dashDir = new Vector2(transform.localScale.x, 0);
            }
            StartCoroutine(stopDash());
        }

        if (isDashing)
        {
			this.parachute.CloseParachute();
            rb.velocity = dashDir.normalized * dashVel;
            return;
        }

        IEnumerator stopDash()
        {
            yield return new WaitForSeconds(dashTime);
			trailRenderer.emitting = false;
            isDashing = false;
            rb.velocity = Vector2.zero;
			if (infloor){
				yield return new WaitForSeconds(dashTime);
				canDash = true;
			}
        }
    }	

	public void Attack() { //Usamos para configurar a mecânica de ataque

		if (Input.GetButtonDown ("Fire1")) { //Se o input Fire1 da Unity for pressionado (no caso o ctrl esquerdo ou lado esquerdo do mouse)
			animator.SetBool ("Attack", true); //Ativamos a animação de ataque
			shield.guardShield();
		} 
		if (Input.GetButtonUp ("Fire1")) { //Se o input Fire1 da Unity for solto (no caso o ctrl esquerdo ou lado esquerdo do mouse)
			animator.SetBool ("Attack", false); //Desativamos a animação de ataque
		}
	}
	private void Block(){
        if (infloor == false)
        {
            this.shield.guardShield(); 
        } 
        if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                this.shield.takeShield();
                canDash = false;
                if (infloor == true)
                {
                    speed = 5;
                }
            } 
        else if (Input.GetKeyUp(KeyCode.Mouse1))
            {
                this.shield.guardShield();
                speed = 12;
				canDash = true;
            }
    }
}
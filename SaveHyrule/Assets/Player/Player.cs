using UnityEngine;
using System.Collections;


/*
 * O Player faz o papel de Agente Inteligente, possuindo 4 possíveis ações: 
 * 
 *  - mover para cima
 *  - mover para baixo
 *  - mover para esquerda
 *  - mover para direita
 * 
 * O agente não pode andar na diagonal, somente na vertical e na horizontal.
 */

public class Player : MonoBehaviour {

	private bool facingDown = false;
	private bool facingLeft = false;
	private bool facingRight = false;
	private bool facingUp = false;
	private Animator pAnimator;

	void Awake()
	{
		pAnimator = GetComponent<Animator> ();
	}

	void updateAnimator()
	{
		pAnimator.SetBool ("Up", facingUp);
		pAnimator.SetBool("Down", facingDown);
		pAnimator.SetBool("Left", facingLeft);
		pAnimator.SetBool("Right", facingRight);
		pAnimator.SetBool("Collectable", false);
	}

	public void gotPendant()
	{
		pAnimator.SetBool("Collectable", true);
	}

	public void goUp()
	{
		this.facingUp = true;
		this.facingDown = false;
		this.facingLeft = false;
		this.facingRight = false;
		this.updateAnimator ();

		Vector3 newPosition = new Vector3 (this.transform.position.x - 1, this.transform.position.y, this.transform.position.z);
		this.transform.position = newPosition;
	}

	public void goDown()
	{
		this.facingDown = true;
		this.facingUp = false;
		this.facingLeft = false;
		this.facingRight = false;
		this.updateAnimator ();	

		Vector3 newPosition = new Vector3 (this.transform.position.x + 1, this.transform.position.y, this.transform.position.z);
		this.transform.position = newPosition;
	}

	public void goLeft()
	{
		this.facingLeft = true;
		this.facingUp = false;
		this.facingDown = false;
		this.facingRight = false;
		this.updateAnimator ();

		Vector3 newPosition = new Vector3 (this.transform.position.x, this.transform.position.y - 1, this.transform.position.z);
		this.transform.position = newPosition;
	}

	public void goRight()
	{
		this.facingRight = true;
		this.facingUp = false;
		this.facingDown = false;
		this.facingLeft = false;
		this.updateAnimator ();


		Vector3 newPosition = new Vector3 (this.transform.position.x, this.transform.position.y + 1, this.transform.position.z);
		this.transform.position = newPosition;
	}
}

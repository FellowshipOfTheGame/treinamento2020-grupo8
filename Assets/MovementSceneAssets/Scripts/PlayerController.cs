using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{
    private enum State { idle, running, dashing, jumping, hurt, attacking }
	[SerializeField] private State state = State.idle;

	public PowerBar powerBar; // Creates a powerbar object
	[SerializeField] private float maxHealth = 100f; // Stores player's maximum health
	[SerializeField] private float currentHealth; // Stores player's current health


	[SerializeField] private float moveSpeed = 15f; // Variable that stores the player's speed
	[SerializeField] private float dashSpeed = 40f; // Variable that stores the player's dash speed
	[SerializeField] private float jumpForce = 20f; // Variable that stores the player's jump force
	[SerializeField] private float hurtForce = 10f; // Variable that stores the strength of the knockback
	[SerializeField] private float currentSpeed; // Holds the speed that shall be applied to the the rigidbody
	[SerializeField] private float dashTime = 0.1f; // Holds how much time will the dash be in effect
	[SerializeField] private float dashCooldown = 0.1f; // Holds the cooldown time for the dash
	[SerializeField] private float attackRate = 2f; // Holdss how many attacks can be done each second
	private float nextAttackTime = 0f; // Used to test if player can attack


	[SerializeField] private bool canDoubleJump; // Boolean variable to determine if player can double jump
	[SerializeField] private bool canDash; // Boolean variable to determine if player can dash
	[SerializeField] private bool isGrounded; // Boolean variable to determine if player is touching the ground
	[SerializeField] private bool isDashing; // Boolean to determine if player is dashing


	[SerializeField] private Image dashImage; // Image that stores the dash indicator


	[SerializeField] private LayerMask groundLayer; // Creates a layer mask to store the ground layer
	[SerializeField] private LayerMask enemyLayer; // Creates a layer mask to store the enemy layer


	[SerializeField] private Transform groundCheck; // Gets the ground check object transform
	[SerializeField] private Transform attackCheck; // Gets the attack check object transform


	[SerializeField] private float groundCheckRadius = 0.47f; // Set the radius for the ground check
	[SerializeField] private float attackCheckRadius = 0.20f; // Set the radius for the attack check


	[SerializeField] private GameObject PauseMenuScreen;


	private Rigidbody2D rb;
	private Collider2D coll;
	private Animator anim;


	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		coll = GetComponent<Collider2D>();
		anim = GetComponent<Animator>();

		currentHealth = maxHealth;
		currentSpeed = moveSpeed;

		powerBar.SetMaxPower(maxHealth);
		powerBar.SetPower(currentHealth);

		canDoubleJump = true;
		canDash = true;
		isDashing = false;
	}


	private void Update()
	{
		OpenOptionsMenu();
		CheckGrounded();

		if(isGrounded)
		{
			canDoubleJump = true; // Allows double jump when the player is on the ground
		}

		if(state != State.hurt)
		{
			Movement(); // Check for movement related input when not in hurt state
		}

		if(currentHealth >= maxHealth)
		{
			currentHealth = maxHealth; // Blocks maximum current health at maximum health
		}
		else if(currentHealth <= 0f) // If the power/health is lower than 0, set its value back to 0 and locks double jump and dash
		{
			currentHealth = 0f; // Blocks minimum current health at 0
			// Blocks double jump and dash abilities
			canDoubleJump = false;
			canDash = false;
		}

		AnimationStateMachine(); // Changes animation according to player state [not finished > can only be fully completed once animations have been added]

		powerBar.SetPower(currentHealth); // Applies all changes to energy meter

		if (Input.GetKeyDown(KeyCode.Mouse1)) // Check for attack input
		{
			if(Time.time >= nextAttackTime) // Compare current time with next possible attack time
			{
				state = State.attacking; //Change state to attacking
				CheckAttack(); //
				nextAttackTime = Time.time + (1f / attackRate);
			}
		}
	}


	private void OnTriggerEnter2D(Collider2D collision) // Check entering trigger objects
	{
		if(collision.tag == "CheckPoint")
		{
			currentHealth = maxHealth;
			canDoubleJump = true;
			canDash = true;
		}
	}


	private void OnCollisionEnter2D(Collision2D other) // Check for collisions
	{
		if(other.gameObject.tag == "Enemy" && gameObject.layer == 11)  // Check is player collided with enemy
		{
			state = State.hurt;
			currentHealth -= 10;

            if(other.gameObject.transform.position.x > transform.position.x)
			{
				rb.velocity = new Vector2(-hurtForce, rb.velocity.y);
			}
			else
			{
				rb.velocity = new Vector2(hurtForce, rb.velocity.y);
			}

			if(other.gameObject.transform.position.y < transform.position.y)
			{
				rb.velocity = new Vector2(rb.velocity.x, hurtForce);
			}
		}
	}


	private void Movement() // Method that handles the player movement
	{
		float horizontalDir = Input.GetAxis("Horizontal"); // Gets input as an axis value [-1 =< horizontalDir =< 1]

		if(horizontalDir < 0)
		{
			transform.localScale = new Vector2(-1, 1); // Flips the player to face correct direction
		}
		else if(horizontalDir > 0)
		{
			transform.localScale = new Vector2(1, 1); // Flips the player to face correct direction
		}

		rb.velocity = new Vector2(horizontalDir * currentSpeed, rb.velocity.y); // Changes the rigidbody's velocity accordingly

		if(Input.GetKeyDown(KeyCode.Mouse0) && horizontalDir != 0)
		{
			StartCoroutine(Dash()); // Starts the dash coroutine
		}

		if(Input.GetKeyDown(KeyCode.Space))
		{
			Jump();
		}
	}


	private void Jump()
	{
		if(isGrounded)
		{
			rb.velocity = new Vector2(rb.velocity.x, jumpForce); // Apply jump force to rb's Y velocity
		}
		else
		{
			if(canDoubleJump) // Get double jump state
			{
				rb.velocity = new Vector2(rb.velocity.x, jumpForce); // Apply jump force to rb's Y velocity
				canDoubleJump = false; // Disables double jump if it has been used
				currentHealth -= 5f;
			}
		}
	}


	private IEnumerator Dash() // Dash timer
	{
		if(canDash)
		{
			Color tempColor = dashImage.color;
			tempColor.a = 0.5f;
			dashImage.color = tempColor;

			currentSpeed = dashSpeed; // Changes player speed to dash speed
			isDashing = true;
			gameObject.layer = 12;

			yield return new WaitForSeconds(dashTime); // Initiates a 0.1 seconds timer

			gameObject.layer = 11;
			currentSpeed = moveSpeed; // Changes player speed to normal speed
			isDashing = false;
			canDash = false; // Disables dash capability

			tempColor.a = 1f;
			dashImage.color = tempColor;

			currentHealth -= 5f;

			StartCoroutine(DashCooldown()); // Calls the cooldown timer
		}
	}


	private IEnumerator DashCooldown() // Dash cooldown timer
	{
		yield return new WaitForSeconds(dashCooldown); // Initiates a 0.1 seconds timer
		canDash = true; // Enables dash capability
	}


	private void CheckAttack()
	{
		if(Physics2D.OverlapCircle(attackCheck.position, attackCheckRadius, enemyLayer))
		{
			AttackFunc();
		}
	}


	private void AttackFunc()
	{
		Debug.Log("YEET!");
	}


	private void CheckGrounded() // Check is player is touching the ground by getting overlaps between ground layer and ground check area
	{
		if(Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer)) isGrounded = true;
		else isGrounded = false;
	}


    private void AnimationStateMachine()
	{
        if(Mathf.Abs(rb.velocity.y) > 0.1f && state != State.hurt)
		{
			state = State.jumping;

            if(isGrounded)
			{
				state = State.idle;
			}
		}
        else if(state == State.hurt)
		{
            if(Mathf.Abs(rb.velocity.x) < 0.1f)
			{
				state = State.idle;
			}
		}
        else if(state == State.dashing)
		{
            if(currentSpeed == moveSpeed)
			{
				state = State.idle;
			}
		}
        else if(Mathf.Abs(rb.velocity.x) > 1f && !isDashing)
		{
			state = State.running;
		}
		else if(Mathf.Abs(rb.velocity.x) > 1f && isDashing)
        {
			state = State.dashing;
        }
		else if(state == State.attacking)
        {
			if(Time.time >= nextAttackTime)
            {
				state = State.idle;
            }
        }
		else
		{
			state = State.idle;
		}
	}


	private void OpenOptionsMenu()
    {
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			PauseMenuScreen.SetActive(true);
			Time.timeScale = 0f;
		}
    }


	private void OnDrawGizmos() // Draws in editor reference points for invisible objects
	{
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class PlayerController : MonoBehaviour
{
	public float speed;
	public float jumpForce;
	public float rayLength;

	public bool grounded;

	public LayerMask ground;

	public Joystick joystick;

	private Rigidbody2D rb;
	private BoxCollider2D boxCollider;
	private AudioSource audioSource;
	
	public AudioClip jumpSfx;
	public AudioClip collectSfx;
	public Animator animator;

	public GameObject explosion;

	private Stopwatch sw = new Stopwatch();

	// Start is called before the first frame update
	void Start()
    {
		rb = GetComponent<Rigidbody2D>();
		boxCollider = GetComponent<BoxCollider2D>();
		audioSource = GetComponent<AudioSource>();

		sw.Start();
	}

	public void LoadSaveData( SaveData saveData )
	{
		transform.position = saveData.playerPosition;
		transform.rotation = saveData.playerRotation;
	}

	// FixedUpdate is called every fixed frame-rate frame, use it when using Rigidbody
	private void FixedUpdate()
	{
		//float moveInput = Input.GetAxis( "Horizontal" );
		float moveInput = joystick.Horizontal;

		rb.velocity = new Vector2( moveInput * speed, rb.velocity.y );

		if( moveInput > 0 )
			transform.eulerAngles = new Vector3( 0, 0, 0 );
		if( moveInput < 0 )
			transform.eulerAngles = new Vector3( 0, 180, 0 );

		animator.SetFloat( "Speed", Mathf.Abs( moveInput ) );
	}

	// Update is called once per frame
	void Update()
    {
		grounded = isGrounded();

		//if( grounded && Input.GetButton( "Jump" ) && sw.ElapsedMilliseconds > 50 )
		if( joystick.Vertical > .7f )
		{
			Jump();
		}

		if( rb.velocity.y < 0.1 )
			animator.SetBool( "IsJumping", false );

		animator.SetBool( "IsFalling", rb.velocity.y < -0.1 );


		// TYMCZASOWE
		if( Input.GetKeyDown( KeyCode.Alpha5 ) )
			SaveSystem.Save();
		if( Input.GetKeyDown( KeyCode.Alpha6 ) )
			SaveSystem.Load();
	}

	private void OnTriggerEnter2D( Collider2D collision )
	{
		Destroy( collision.gameObject );
		audioSource.PlayOneShot( collectSfx );
		GameObject expl = Instantiate( explosion, collision.transform.position, explosion.transform.rotation );
		expl.GetComponent<Explosion>().target = transform;
	}

	public void Jump()
	{
		if( grounded && sw.ElapsedMilliseconds > 50 )
		{
			sw.Restart();
			rb.velocity = Vector2.up * jumpForce;
			audioSource.PlayOneShot( jumpSfx );

			animator.SetBool( "IsJumping", true );
		}
		
	}

	bool isGrounded()
	{
		Vector3 boxPos = transform.position + new Vector3( boxCollider.offset.x, boxCollider.offset.y );
		Vector2 posL = boxPos - new Vector3( boxCollider.size.x / 2, 0 );
		Vector2 posR = boxPos + new Vector3( boxCollider.size.x / 2, 0 );
		Vector2 direction = Vector2.down;

		RaycastHit2D hitL = Physics2D.Raycast( posL, direction, rayLength, ground );
		RaycastHit2D hitR = Physics2D.Raycast( posR, direction, rayLength, ground );

		return hitL.collider != null || hitR.collider != null;
	}
}

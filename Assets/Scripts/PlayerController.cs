using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
	public float speed;
	public float jumpForce;
	public float rayLength;

	public bool grounded;

	public LayerMask ground;
	public TextMeshProUGUI scoreText;

	private int score = 0;

	private Rigidbody2D rb;
	private BoxCollider2D boxCollider;

	// Start is called before the first frame update
	void Start()
    {
		rb = GetComponent<Rigidbody2D>();
		boxCollider = GetComponent<BoxCollider2D>();
    }

	// FixedUpdate is called every fixed frame-rate frame, use it when using Rigidbody
	private void FixedUpdate()
	{
		float moveInput = Input.GetAxis( "Horizontal" );
		rb.velocity = new Vector2( moveInput * speed, rb.velocity.y );

		if( moveInput > 0 )
			transform.eulerAngles = new Vector3( 0, 0, 0 );
		if( moveInput < 0 )
			transform.eulerAngles = new Vector3( 0, 180, 0 );
	}

	// Update is called once per frame
	void Update()
    {
		grounded = isGrounded();

		if( grounded && Input.GetButton( "Jump" ) )
			rb.velocity = Vector2.up * jumpForce;
			//rb.velocity += new Vector2( 0, jumpForce ); //
    }

	private void OnTriggerEnter2D( Collider2D collision )
	{
		Destroy( collision.gameObject );
		score++;
		scoreText.text = "Score: " + score;
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

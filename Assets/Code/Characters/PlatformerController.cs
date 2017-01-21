using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerController : MonoBehaviour 
{
	public int m_PlayerIndex = 0;

	public float m_HorizontalAcceleration = 15.0f;
	public float m_MaxHorizontalSpeed = 3.5f;
	public float m_JumpImpulse = 5.0f;
	public bool m_AllowDoubleJump = true;
	public float m_DoubleJumpImpulse = 3.0f;
	public float m_MaxJumpRequestDelay = 0.2f;

	public float m_GroundRaycastOffset = 0.01f;
	public Rigidbody2D m_Rigidbody = null;
	public CapsuleCollider2D m_Collider = null;

	public bool m_IsOnGround = false;

	private bool m_MustJumpAsSoonAsPossible = false;
	private float m_CurrentJumpRequestDelay = 0.0f;
	private bool m_HasAlreadyDoubleJumped = false;
	private RaycastHit2D[] m_RaycastNonAllocResults = new RaycastHit2D[1];

	void Awake()
	{
		m_Rigidbody = GetComponent<Rigidbody2D>();
		m_Collider = GetComponent<CapsuleCollider2D>();
	}

	void FixedUpdate()
	{
		float horizontalAxis = XInput.GetAxis(Axis.LeftStickHorizontal, m_PlayerIndex);
		
		if(Input.GetKey(KeyCode.LeftArrow))
		{
			horizontalAxis = -1.0f;
		}
		else if(Input.GetKey(KeyCode.RightArrow))
		{
			horizontalAxis = 1.0f;
		}

		Vector2 finalForce = Vector2.zero;
		finalForce.x = horizontalAxis * m_HorizontalAcceleration * Time.deltaTime;		

		m_Rigidbody.velocity = m_Rigidbody.velocity + finalForce;
		m_Rigidbody.velocity = new Vector2(Mathf.Clamp(m_Rigidbody.velocity.x, -m_MaxHorizontalSpeed, m_MaxHorizontalSpeed), m_Rigidbody.velocity.y);

		if(m_IsOnGround)
		{
			m_HasAlreadyDoubleJumped = false;
		}

		if(m_MustJumpAsSoonAsPossible)
		{			
			if (m_IsOnGround)
			{
				m_Rigidbody.velocity = new Vector2(m_Rigidbody.velocity.x, 0.0f);
				m_Rigidbody.AddForce(Vector2.up * m_JumpImpulse, ForceMode2D.Impulse);
				CancelJumpRequest();
			}			
			else if(m_AllowDoubleJump)
			{
				if(!m_HasAlreadyDoubleJumped)
				{
					m_HasAlreadyDoubleJumped = true;
					m_Rigidbody.velocity = new Vector2(m_Rigidbody.velocity.x, 0.0f);
					m_Rigidbody.AddForce(Vector2.up * m_DoubleJumpImpulse, ForceMode2D.Impulse);
					CancelJumpRequest();
				}
			}
		}
	}

	void Update()
	{
		CheckGround();

		if(XInput.GetButtonDown(Buttons.A, m_PlayerIndex) || Input.GetKeyDown(KeyCode.Space))
		{
			JumpRequest();
		}

		if(m_MustJumpAsSoonAsPossible && m_CurrentJumpRequestDelay > m_MaxJumpRequestDelay)
		{
			CancelJumpRequest();
		}
		else if(m_MustJumpAsSoonAsPossible)
		{
			m_CurrentJumpRequestDelay += Time.deltaTime;
		}
	}

	void JumpRequest()
	{
		m_MustJumpAsSoonAsPossible = true;
	}

	void CancelJumpRequest()
	{
		m_MustJumpAsSoonAsPossible = false;
		m_CurrentJumpRequestDelay = 0.0f;
	}

	void CheckGround()
	{
		Vector3 startPoint = transform.position + new Vector3(0.0f, -(m_Collider.size.y / 2.0f) + 0.01f, 0.0f);
		Vector3 leftStartPoint = transform.position + new Vector3(-(m_Collider.size.x / 2.0f), -(m_Collider.size.y / 2.0f) + 0.01f, 0.0f);
		Vector3 rightStartPoint = transform.position + new Vector3((m_Collider.size.x / 2.0f), -(m_Collider.size.y / 2.0f) + 0.01f, 0.0f);

		if (Physics2D.RaycastNonAlloc(startPoint, Vector2.down, m_RaycastNonAllocResults, m_GroundRaycastOffset + 0.01f) > 0)
		{
			m_IsOnGround = true;
		}
		else if(Physics2D.RaycastNonAlloc(leftStartPoint, Vector2.down, m_RaycastNonAllocResults, m_GroundRaycastOffset + 0.01f) > 0)
		{
			m_IsOnGround = true;
		}
		else if (Physics2D.RaycastNonAlloc(rightStartPoint, Vector2.down, m_RaycastNonAllocResults, m_GroundRaycastOffset + 0.01f) > 0)
		{
			m_IsOnGround = true;
		}
		else
		{
			m_IsOnGround = false;
		}		
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour 
{
	private Controller m_CharacterController = null;
	private Rigidbody2D m_Rigidbody = null;
	private CapsuleCollider2D m_Collider = null;
	private CharacterDataModule m_Character = null;

	public float m_HorizontalAcceleration = 15.0f;
	public float m_MaxHorizontalSpeed = 3.5f;
	public float m_JumpImpulse = 5.0f;
	public bool m_AllowDoubleJump = true;
	public float m_DoubleJumpImpulse = 3.0f;

	public float m_GroundRaycastOffset = 0.01f;	

	public bool m_IsOnGround = false;
	private bool m_HasAlreadyDoubleJumped = false;
	private RaycastHit2D[] m_RaycastNonAllocResults = new RaycastHit2D[1];

	void Start()
	{
		m_Character = GetComponent<CharacterDataModule>();
		m_CharacterController = GetComponent<Controller>();
		m_Rigidbody = GetComponent<Rigidbody2D>();
		m_Collider = GetComponent<CapsuleCollider2D>();
	}

	void FixedUpdate()
	{
		if (m_CharacterController == null) return;
		float horizontalAxis = m_CharacterController.MoveAxis;		

		Vector2 finalForce = Vector2.zero;
		finalForce.x = horizontalAxis * m_HorizontalAcceleration * Time.fixedDeltaTime;		

		m_Rigidbody.velocity = m_Rigidbody.velocity + finalForce;
		m_Rigidbody.velocity = new Vector2(Mathf.Clamp(m_Rigidbody.velocity.x, -m_MaxHorizontalSpeed, m_MaxHorizontalSpeed), m_Rigidbody.velocity.y);

		if(m_IsOnGround)
		{
			m_HasAlreadyDoubleJumped = false;
		}

		if(m_CharacterController.JumpRequested)
		{			
			if (m_IsOnGround)
			{
				m_Rigidbody.velocity = new Vector2(m_Rigidbody.velocity.x, 0.0f);
				m_Rigidbody.AddForce(Vector2.up * m_JumpImpulse, ForceMode2D.Impulse);
				m_CharacterController.CancelJumpRequest();
			}			
			else if(m_AllowDoubleJump)
			{
				if(!m_HasAlreadyDoubleJumped)
				{
					m_HasAlreadyDoubleJumped = true;
					m_Rigidbody.velocity = new Vector2(m_Rigidbody.velocity.x, 0.0f);
					m_Rigidbody.AddForce(Vector2.up * m_DoubleJumpImpulse, ForceMode2D.Impulse);
					m_CharacterController.CancelJumpRequest();
				}
			}
		}
	}

	void Update()
	{
		CheckGround();
		if (m_Character != null)
		{
			if (Mathf.Abs(m_Rigidbody.velocity.x) > 0.1f)
			{
				if (m_Rigidbody.velocity.x < 0.0f)
				{
					m_Character.Direction = -1;
				}
				else
				{
					m_Character.Direction = 1;
				}

				m_Character.IsMoving = true;
			}
			else
			{
				m_Character.IsMoving = false;
			}
		}
	}

	void CheckGround()
	{
		if(m_Collider == null)
		{
			m_IsOnGround = true;
			return;
		}

		Vector3 startPoint = transform.position + new Vector3(m_Collider.offset.x, -(m_Collider.size.y / 2.0f) + 0.01f + m_Collider.offset.y, 0.0f);
		Vector3 leftStartPoint = transform.position + new Vector3(-(m_Collider.size.x / 2.0f) + m_Collider.offset.x, -(m_Collider.size.y / 2.0f) + 0.01f + m_Collider.offset.y, 0.0f);
		Vector3 rightStartPoint = transform.position + new Vector3((m_Collider.size.x / 2.0f) + m_Collider.offset.x, -(m_Collider.size.y / 2.0f) + 0.01f + m_Collider.offset.y, 0.0f);

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

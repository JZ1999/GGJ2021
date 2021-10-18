using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class SimpleSampleCharacterControl : MonoBehaviour
{

    [Range(0, 500)]
    public int timeAnda;
    public float _timeAnda = 0;
    public void SetTimeAnda()
    {
        if (_timeAnda > 0)
            return;
        uiBarAndaObject.SetActive(true);
        _timeAnda = timeAnda;

    }
    private enum ControlMode
    {
        /// <summary>
        /// Up moves the character forward, left and right turn the character gradually and down moves the character backwards
        /// </summary>
        Tank,
        /// <summary>
        /// Character freely moves in the chosen direction from the perspective of the camera
        /// </summary>
        Direct
    }

    [Space]
    [Header("UI")]
    public Image uiBarAnda;
    public GameObject uiBarAndaObject;

    [SerializeField] private float m_moveSpeed = 2;
    [SerializeField] private float m_turnSpeed = 200;
    [SerializeField] private float m_jumpForce = 4;

    [SerializeField] private Animator m_animator = null;
    [SerializeField] private Rigidbody m_rigidBody = null;

    [SerializeField] private ControlMode m_controlMode = ControlMode.Direct;

    private float m_currentV = 0;
    private float m_currentH = 0;

    private readonly float m_interpolation = 10;
    private readonly float m_walkScale = 0.33f;
    private readonly float m_backwardsWalkScale = 0.16f;
    private readonly float m_backwardRunScale = 0.66f;

    private bool m_wasGrounded;
    private Vector3 m_currentDirection = Vector3.zero;

    private float m_jumpTimeStamp = 0;
    private float m_minJumpInterval = 0.25f;
	[HideInInspector]
    public bool m_jumpInput = false;

    private bool m_isGrounded;

    private List<Collider> m_collisions = new List<Collider>();

	public VariableJoystick variableJoystick;
	[SerializeField]
	private GameObject joystick;
	[SerializeField]
	private GameObject jumpButton;
	[SerializeField]
	private GameObject jumpButtonPrefab;
	[SerializeField]
	[Tooltip("Used to check when scene has loaded")]
	private string sceneName;

	// Online vars
	[HideInInspector]
	public GameSetupController gameSetup;
    private bool isMoving = false;
	private bool sceneLoaded = false;

	private void Awake()
    {
        if (!m_animator) { gameObject.GetComponent<Animator>(); }
        if (!m_rigidBody) { gameObject.GetComponent<Animator>(); }
    }

	void Start()
	{
		if (!GetComponent<PhotonView>().IsMine)
			return;
		if (!sceneLoaded)
		{
			sceneLoaded = SceneManager.GetActiveScene().name == sceneName;
			int viewID = GetComponent<PhotonView>().ViewID;
			gameSetup.photonView.RPC("MasterClientMessage", RpcTarget.MasterClient, PhotonNetwork.LocalPlayer, "loaded", "", viewID);
		}

		InvokeRepeating("SendPosition", 2f, 3f);  //1s delay, repeat every 1s
	}
	void SendPosition()
	{
		if (!gameSetup.arePlayersReady())
			return;
		int viewID = GetComponent<PhotonView>().ViewID;
		gameSetup.photonView.RPC("SendChat", RpcTarget.All, PhotonNetwork.LocalPlayer, "teleport",
			JsonUtility.ToJson(gameObject.transform.position), viewID.ToString());
	}


	private void OnCollisionEnter(Collision collision)
    {
        ContactPoint[] contactPoints = collision.contacts;
        for (int i = 0; i < contactPoints.Length; i++)
        {
            if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
            {
                if (!m_collisions.Contains(collision.collider))
                {
                    m_collisions.Add(collision.collider);
                }
                m_isGrounded = true;
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        ContactPoint[] contactPoints = collision.contacts;
        bool validSurfaceNormal = false;
        for (int i = 0; i < contactPoints.Length; i++)
        {
            if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
            {
                validSurfaceNormal = true; break;
            }
        }

        if (validSurfaceNormal)
        {
            m_isGrounded = true;
            if (!m_collisions.Contains(collision.collider))
            {
                m_collisions.Add(collision.collider);
            }
        }
        else
        {
            if (m_collisions.Contains(collision.collider))
            {
                m_collisions.Remove(collision.collider);
            }
            if (m_collisions.Count == 0) { m_isGrounded = false; }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (m_collisions.Contains(collision.collider))
        {
            m_collisions.Remove(collision.collider);
        }
        if (m_collisions.Count == 0) { m_isGrounded = false; }
    }

    private void Update()
    {
		if (!GetComponent<PhotonView>().IsMine)
			return;
		if (!gameSetup.arePlayersReady())
			return;

		if (!m_jumpInput && Input.GetKey(KeyCode.Space))
        {
            m_jumpInput = true;
        }
    }

    private void FixedUpdate()
    {
		if (!gameSetup.arePlayersReady())
			return;

		if (_timeAnda > 0)
        {
            uiBarAnda.fillAmount = _timeAnda / timeAnda;
            _timeAnda -= Time.deltaTime;
            return;
        }
        else
        {
            if (uiBarAndaObject)
                uiBarAndaObject.SetActive(false);
        }
        if (!GetComponent<PhotonView>().IsMine)
            return;
        if (!variableJoystick || !gameSetup)
			return;
        m_animator.SetBool("Grounded", m_isGrounded);

		switch (m_controlMode)
		{
			case ControlMode.Direct:
				break;

			case ControlMode.Tank:
				InputsInfo inputs = new InputsInfo(){
					horizontal =  Application.platform != RuntimePlatform.Android ? Input.GetAxis("Horizontal"): variableJoystick.Horizontal,
					vertical = Application.platform != RuntimePlatform.Android ? Input.GetAxis("Vertical") : variableJoystick.Vertical
				};
				int viewID = GetComponent<PhotonView>().ViewID;
                if (inputs.horizontal != 0 || inputs.vertical != 0)
                {
                    isMoving = true;
                    gameSetup.photonView.RPC("SendChat", RpcTarget.All, PhotonNetwork.LocalPlayer, "movement", JsonUtility.ToJson(inputs), viewID.ToString());
                }else if (isMoving) {
                    isMoving = false;
                    gameSetup.photonView.RPC("SendChat", RpcTarget.All, PhotonNetwork.LocalPlayer, "movement", JsonUtility.ToJson(inputs), viewID.ToString());
                };
				TankUpdate(inputs.horizontal, inputs.vertical);
                break;

            default:
                Debug.LogError("Unsupported state");
                break;
        }

        m_wasGrounded = m_isGrounded;
        m_jumpInput = false;
    }

    public void TankUpdate(float h, float v)
    {

        bool walk = Input.GetKey(KeyCode.LeftShift);
        
        if (v < 0)
        {
            if (walk) { v *= m_backwardsWalkScale; }
            else { v *= m_backwardRunScale; }
        }
        else if (walk)
        {
            v *= m_walkScale;
        }

        m_currentV = Mathf.Lerp(m_currentV, v, Time.deltaTime * m_interpolation);
        m_currentH = Mathf.Lerp(m_currentH, h, Time.deltaTime * m_interpolation);

        transform.position += transform.forward * m_currentV * m_moveSpeed * Time.deltaTime;
        transform.Rotate(0, m_currentH * m_turnSpeed * Time.deltaTime, 0);

        if (h != 0 && v != 0) {
            m_animator.SetFloat("MoveSpeed", m_currentV);
        }
        else { m_animator.SetFloat("MoveSpeed", 0); }
        
        m_animator.SetFloat("RotationSpeed",h);

        JumpingAndLanding();
    }

    private void DirectUpdate(float h, float v)
    {
        Transform camera = Camera.main.transform;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            v *= m_walkScale;
            h *= m_walkScale;
        }

        m_currentV = Mathf.Lerp(m_currentV, v, Time.deltaTime * m_interpolation);
        m_currentH = Mathf.Lerp(m_currentH, h, Time.deltaTime * m_interpolation);

        Vector3 direction = camera.forward * m_currentV + camera.right * m_currentH;

        float directionLength = direction.magnitude;
        direction.y = 0;
        direction = direction.normalized * directionLength;

        if (direction != Vector3.zero)
        {
            m_currentDirection = Vector3.Slerp(m_currentDirection, direction, Time.deltaTime * m_interpolation);

            transform.rotation = Quaternion.LookRotation(m_currentDirection);
            transform.position += m_currentDirection * m_moveSpeed * Time.deltaTime;

            m_animator.SetFloat("MoveSpeed", direction.magnitude);
           
        }

        JumpingAndLanding();
    }

    private void JumpingAndLanding()
    {
        bool jumpCooldownOver = (Time.time - m_jumpTimeStamp) >= m_minJumpInterval;

        if (jumpCooldownOver && m_isGrounded && m_jumpInput)
        {
            m_jumpTimeStamp = Time.time;
            m_rigidBody.AddForce(Vector3.up * m_jumpForce, ForceMode.Impulse);
        }

        if (!m_wasGrounded && m_isGrounded)
        {
            m_animator.SetTrigger("Land");
        }

        if (!m_isGrounded && m_wasGrounded)
        {
            m_animator.SetTrigger("Jump");
        }
    }

	public void SpawnJoyStick()
	{
		GameObject canvas = ((Canvas)FindObjectOfType(typeof(Canvas))).gameObject;

		variableJoystick = Instantiate(joystick, canvas.transform).GetComponent<VariableJoystick>();
	}

	public void SpawnJumpButton()
	{
		GameObject canvas = ((Canvas)FindObjectOfType(typeof(Canvas))).gameObject;

		jumpButton = Instantiate(jumpButtonPrefab, canvas.transform);
		jumpButton.GetComponent<Button>().onClick.AddListener(delegate {
			m_jumpInput = true;
			int viewID = GetComponent<PhotonView>().ViewID;
			gameSetup.photonView.RPC("SendChat", RpcTarget.All, PhotonNetwork.LocalPlayer, "jump", "", viewID.ToString());
		});
	}
}

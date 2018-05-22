using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;

/// <summary>
/// PlayerController handle player movement
/// <summary>
public class PlayerController : MonoBehaviour, IKillable
{
    #region Attributes
    public bool isOnEnnemy = false;
    public bool isDead = false;

    [SerializeField]
    private float baseJumpForce = 5.0F;

    [SerializeField]
    private float lowJumpMultiplier = 1.025F;

    [FoldoutGroup("Gameplay"), Tooltip("Mouvement du joueur"), SerializeField]
    private float moveSpeed = 10.0F;
    [FoldoutGroup("Gameplay"), Tooltip("acceleration du joueur"), SerializeField]
    private float accelerationFactor = 10.0F;

    [FoldoutGroup("Gameplay"), Tooltip("Current planet"), SerializeField]
    private Transform transformLookToPlanet;
    [FoldoutGroup("Gameplay"), Tooltip("Current planet"), SerializeField]
    private Transform currentPlanet;

    [FoldoutGroup("Effects"), Tooltip("Camera"), SerializeField]
    private Camera cam;
    [FoldoutGroup("Effects"), Tooltip("Default zoom"), SerializeField]
    private float defaultZoom;
    [FoldoutGroup("Cam effects"), Tooltip("Max Zoom"), SerializeField]
    private float maxZoom = 5;
    [FoldoutGroup("Effects"), Tooltip("Max Velocity Factor"), SerializeField]
    private float velocityRange = 250;
    [FoldoutGroup("Effects"), Tooltip("Speedlines"), SerializeField]
    private Image[] speedlines;

    [FoldoutGroup("Debug"), Tooltip("MaxMove"), SerializeField]
    private int idPlayer = 0;


    // Components
    public Rigidbody playerBody;
    //public Rigidbody PlayerBody { set; get; }

    private FrequencyTimer updateTimer;
	private float horizMove;
    private float vertiMove;
    private bool hasMoved = false;
    private bool isJumping = false;
    
    #endregion

    #region Initialize

    private void Awake()
	{
        isOnEnnemy = false;
        isDead = false;
        playerBody = GetComponent<Rigidbody>();
	}

    #endregion

    #region Core
    /// <summary>
    /// gère les inputs des manettes/clavier du joueurs en update
    /// et défini si oui ou non on a bougé
    /// </summary>
    private void InputPlayer()
    {
        horizMove = PlayerConnected.getSingularity().getPlayer(idPlayer).GetAxis("Move Horizontal");
        vertiMove = PlayerConnected.getSingularity().getPlayer(idPlayer).GetAxis("Move Vertical");
        isJumping = PlayerConnected.getSingularity().getPlayer(idPlayer).GetButtonDown("FireA");

        if (horizMove != 0 || vertiMove != 0)
            hasMoved = true;
        else
            hasMoved = false;
    }

    public void Jump(float anotherForce = -1)
    {
        transformLookToPlanet.LookAt(currentPlanet);
        Vector3 verticalAxis = transformLookToPlanet.up;

        playerBody.AddForce(verticalAxis * ((anotherForce == -1) ? baseJumpForce : anotherForce), ForceMode.Impulse);
    }

    /// <summary>
    /// déplace ou non le player en physiques (fixedUpdate) selon les inputs
    /// </summary>
    private void MovePlayer()
    {
        if (hasMoved)
        {
            // Rotation Y = Axe Horizontal
            // Rotation X = Axe Vertical
            transformLookToPlanet.LookAt(currentPlanet);
            Vector3 verticalAxis = transformLookToPlanet.up;
            Vector3 horizontalAxis = transformLookToPlanet.right;
            float desiredMoveX = horizMove * moveSpeed * Time.deltaTime;
            float desiredMoveY = vertiMove * moveSpeed * Time.deltaTime;
            horizontalAxis *= desiredMoveX;
            verticalAxis *= desiredMoveY;
            Vector3 desiredVelocity = horizontalAxis + verticalAxis;

            if (Mathf.Sign(desiredVelocity.x) != Mathf.Sign(playerBody.velocity.x))
                desiredVelocity.x *= accelerationFactor;
            if (Mathf.Sign(desiredVelocity.y) != Mathf.Sign(playerBody.velocity.y))
                desiredVelocity.y *= accelerationFactor;
            if (Mathf.Sign(desiredVelocity.z) != Mathf.Sign(playerBody.velocity.z))
                desiredVelocity.z *= accelerationFactor;
            Debug.DrawRay(transform.position, desiredVelocity, Color.blue, 0.5f);

            playerBody.AddForce(desiredVelocity, ForceMode.Acceleration);
        }
    }

    /// <summary>
    /// gère le zoom de caméra lorsque le player se déplace
    /// </summary>
    private void ManageCameraZoom()
    {
        float velocityFactor = Mathf.Abs(playerBody.velocity.x) + Mathf.Abs(playerBody.velocity.y) + Mathf.Abs(playerBody.velocity.z);
        if (velocityFactor < 0)
            velocityFactor = 0;
        if (velocityFactor > velocityRange)
            velocityFactor = velocityRange;
        float percent = velocityFactor / velocityRange;
        float rangeZoom = defaultZoom - maxZoom;
        float desiredZoom = maxZoom + ((1-percent) * rangeZoom);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, desiredZoom, Time.deltaTime);
        //Speedlines
        float speedlinesVelocityFactor = velocityFactor - velocityRange / 2;
        if (speedlinesVelocityFactor < 0)
            speedlinesVelocityFactor = 0;
        float speedlinesVelocityRange = velocityRange / 2;
        float speedLinesPercent = speedlinesVelocityFactor / speedlinesVelocityRange;
        foreach (Image speedline in speedlines)
        {
            speedline.color = new Color(speedline.color.r, speedline.color.g, speedline.color.b, speedLinesPercent * 0.03f);
            speedline.gameObject.transform.Rotate(new Vector3(0, 0, speedLinesPercent * Random.Range(1, 3f) * 0.8f));
        }
    }

    /// <summary>
    /// isOnEnnemy
    /// </summary>
    public void ChangeOnEnnemy(bool onEnnemy)
    {
        if (isDead)
            return;
        isOnEnnemy = onEnnemy;
    }
    
    /////////////////////////////////////////////////////
	private void Update()
	{
        InputPlayer();
        if (!isDead)
            ManageCameraZoom();
    }

	private void FixedUpdate()
	{
        MovePlayer();
	}

    #endregion

    [FoldoutGroup("Debug"), Button("Kill")]
    public void Kill()
	{
        if (isDead)
            return;
        isOnEnnemy = true;
        isDead = true;

        //Debug.Log ("Dead");
        //Destroy(gameObject);

	}
}
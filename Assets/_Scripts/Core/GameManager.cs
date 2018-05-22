using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.PostProcessing;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// GameManager Description
/// </summary>
public class GameManager : MonoBehaviour
{
    #region Attributes
    [FoldoutGroup("GamePlay"), Tooltip("planete centrale"), SerializeField]
    private int idLevel = 0;
    public int IdLevel { get { return idLevel; } }
    [FoldoutGroup("GamePlay"), Tooltip("planete centrale"), SerializeField]
    private int idLevelMax = 2;
    public int IdLevelMax { get { return idLevelMax; } }

    [FoldoutGroup("GamePlay"), Tooltip("planete centrale"), SerializeField]
    private float timeFadeOutWhenWin = 0.7f;

    [FoldoutGroup("GamePlay"), Tooltip("planete centrale"), SerializeField]
    private float timeShowEnd = 3f;

    [FoldoutGroup("GamePlay"), Tooltip("planete centrale"), SerializeField]
    private float getPlayerClosedInTransition = 9f;

    [FoldoutGroup("GamePlay"), Tooltip("main camera"), SerializeField]
    private Camera cam;

    [FoldoutGroup("GamePlay"), Tooltip("main camera"), SerializeField]
    private PlayerController playerController;

    [FoldoutGroup("GamePlay"), Tooltip("main camera"), SerializeField]
    private Transform fallBackCameraOnPlayer;

    [FoldoutGroup("GamePlay"), Tooltip("planete centrale"), SerializeField]
    private Transform planet;

    [FoldoutGroup("GamePlay"), Tooltip("planete centrale collider"), SerializeField]
    private List<MeshCollider> colliderPlanet;

    [FoldoutGroup("GamePlay"), Tooltip("planete hole"), SerializeField]
    private GameObject otherHolePlanet;

    [FoldoutGroup("GamePlay"), Tooltip("canvas pause"), SerializeField]
    private GameObject canvasPause;

    [FoldoutGroup("GamePlay"), Tooltip("Game Over - Canvas"), SerializeField]
    private GameObject gameover_canvas;

    [FoldoutGroup("GamePlay"), Tooltip("Game Over - Trapped Text"), SerializeField]
    private Image gameover_trappedText;

    [FoldoutGroup("GamePlay"), Tooltip("Game Over - Trapped Text"), SerializeField]
    private Image gameover_retryText;

    [FoldoutGroup("GamePlay"), Tooltip("Game Over - Input Pad"), SerializeField]
    private Image gameover_input_pad;

    [FoldoutGroup("GamePlay"), Tooltip("Game Over - Input Pad"), SerializeField]
    private Image gameover_input_keyboard;

    /*
    [FoldoutGroup("GamePlay"), Tooltip("canvas lose"), SerializeField]
    private TextMeshProUGUI textRestart;

    [FoldoutGroup("GamePlay"), Tooltip("canvasPressRestart"), SerializeField]
    private CanvasGroup canvasPressRestart;

    [FoldoutGroup("GamePlay"), Tooltip("canvasTrap"), SerializeField]
    private CanvasGroup canvasTrap;
    */

    [FoldoutGroup("GamePlay"), Tooltip("opti fps"), SerializeField]
	private FrequencyTimer updateTimer;

    [FoldoutGroup("GamePlay"), Tooltip("opti fps"), SerializeField]
    private TimeWithNoEffect TWNE;

    [FoldoutGroup("GamePlay"), Tooltip("opti fps"), SerializeField]
    private TitleScreenScript titleScreen;

    [FoldoutGroup("GamePlay"), Tooltip("trigger de transition à activer"), SerializeField]
    private GameObject transitionTrigger;

    [FoldoutGroup("GamePlay"), Tooltip("planete centrale"), SerializeField]
    private bool transitionState = false;
    public bool TransitionState { get { return transitionState; } }

    [FoldoutGroup("GamePlay"), Tooltip("facteur de slowMo au début"), SerializeField]
    private float slowDownFactor = 0.05f;
    [FoldoutGroup("GamePlay"), Tooltip("facteur de slowMo au début"), SerializeField]
    private float slowDownLenght = 2f;
    [FoldoutGroup("GamePlay"), Tooltip("vitesse du zoom out de la camera"), SerializeField]
    private float speedCameraDezoom = 2f;

    [FoldoutGroup("GamePlay"), Tooltip("vitesse du zoom out de la camera"), SerializeField]
    private float maxCameraDezoom = -15f;

    private bool winned = false;
    private bool loosed = false;
    private bool paused = false;
    private float timeTmpForPauseEscape = 0;
    private bool readyToRestart = false;
    private bool slowMowActivated = false;
    private bool cameraZoomTransitionOver = true;
    private float beforeFixed;
    

    private static GameManager instance;
    public static GameManager GetSingleton
    {
        get { return instance; }
    }
    #endregion

    #region Initialization

    private void SetSingleton()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    private void Awake()
    {
        SetSingleton();                                                  //set le script en unique ?
        TWNE = gameObject.GetComponent<TimeWithNoEffect>();
        gameover_canvas.SetActive(false);
        winned = false;
        loosed = false;
        paused = false;
        //ici les liens se font direct dans l'éditeur pour + de performance
        /*cam = Camera.main;
        if (!playerController)
            playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        if (!planet)
            planet = GameObject.FindGameObjectWithTag("planet").transform;
        if (!canvasLose)
            canvasLose = GameObject.FindGameObjectWithTag("lose");
        if (!canvasPause)
            canvasPause = GameObject.FindGameObjectWithTag("pause");
        textRestart = canvasLose.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        canvasPressRestart = canvasLose.transform.GetChild(0).gameObject.GetComponent<CanvasGroup>();
        canvasTrap = canvasLose.transform.GetChild(1).gameObject.GetComponent<CanvasGroup>();
        */
        otherHolePlanet.SetActive(!(idLevel == 0));    //désactive le "trou" si on est au level 1 !
        
    }

    private void Start()
    {
        Debug.Log("start du Game Manager ?");
        if (idLevel != 0)
        {
            TransitionCameraPlayer();
        }
    }
    #endregion

    #region Core
    /// <summary>
    /// au début de chaque niveau (sauf le dernier), le player tombe, avec la caméra qui le suit
    /// </summary>
    public void TransitionCameraPlayer()
    {
        Debug.Log("ici ???");
        transitionState = true;
        cam.transform.SetParent(null);  //set la caméra à la racine
        //playerController.Jump(-2f);
        playerController.playerBody.drag = 13f;
        playerController.enabled = false;
        playerController.gameObject.transform.position = new Vector3(playerController.gameObject.transform.position.x,
                                                            playerController.gameObject.transform.position.y,
                                                            playerController.gameObject.transform.position.z - getPlayerClosedInTransition);
        cam.transform.SetParent(fallBackCameraOnPlayer);  //set la caméra dans le player
        //if (GlobalData.GetSingleton && GlobalData.GetSingleton.TmpPlayerMovement)
            //playerController.PlayerBody.angularVelocity = GlobalData.GetSingleton.TmpPlayerMovement.angularVelocity;

        //Time.timeScale = 0.5f;
        //DoSlowMotion();

        cameraZoomTransitionOver = false;
        transitionTrigger.SetActive(true);
    }

    private void DezoomCamera()
    {
        /*playerController.gameObject.transform.position = new Vector3(playerController.gameObject.transform.position.x,
                                                            playerController.gameObject.transform.position.y,
                                                            playerController.gameObject.transform.position.z - getPlayerClosedInTransition);
                                                            */
        Vector3 target = new Vector3(cam.gameObject.transform.position.x,
                                                            cam.gameObject.transform.position.y,
                                                            cam.gameObject.transform.position.z);
        target.z -= (speedCameraDezoom / 2) * Time.deltaTime;

        playerController.playerBody.drag -= speedCameraDezoom * Time.deltaTime;
        if (playerController.playerBody.drag < 0)
            playerController.playerBody.drag = 0;

        cam.transform.position = target;
        if (cam.transform.localPosition.z < maxCameraDezoom)
        {
            Debug.Log("ici stop le dezoom !!!");
            cameraZoomTransitionOver = true;
            playerController.playerBody.drag = 0;
        }
        //cam.transform.position = Vector3.Lerp(startPosition.transform.position, endPosition.transform.position, step);
    }

    public void DoSlowMotion()
    {
        Debug.Log("do slow mow");
        Time.timeScale = slowDownFactor;
        beforeFixed = Time.fixedDeltaTime;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        slowMowActivated = true;
    }

    /// <summary>
    /// reset petit à petit le slow mo à normal
    /// </summary>
    private void CancelSLowMow()
    {
        Time.timeScale += (1f / slowDownLenght) * Time.unscaledDeltaTime;
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
        if (Time.timeScale == 1)
        {
            Debug.Log("ici fixe tout ??");
            slowMowActivated = false;
            Time.timeScale = 1;
            Time.fixedDeltaTime = Time.timeScale * 1;
        }
            
    }
    /// <summary>
    /// la transition du début de niveau est fini, on set la caméra comme il faut, et on joue !
    /// </summary>
    public void EndTransitionCamera()
    {
        if (idLevel == 0)
            return;
        Debug.Log("ici l'activation du jeu ???");
        otherHolePlanet.SetActive(false);   //hide hole object
        //transitionState = false;            //transition fini
        playerController.enabled = true;
        Time.timeScale = 1f;
    }

    public void Lose(Transform ennemy)
    {
        if (loosed)
            return;
        loosed = true;
        gameover_canvas.SetActive(true);
        if (PlayerConnected.getSingularity().playerArrayConnected[0])
        {
            gameover_input_keyboard.gameObject.SetActive(false);
            gameover_input_pad.gameObject.SetActive(true);
            //textRestart.text = "Press A to restart";
        }
        else
        {
            gameover_input_keyboard.gameObject.SetActive(true);
            gameover_input_pad.gameObject.SetActive(false);
            //textRestart.text = "Press Space to restart";
        }
        cam.transform.SetParent(ennemy);  //set la caméra à la racine
        //cam.gameObject.GetComponent<PostProcessingBehaviour>().enabled = false;
        cam.gameObject.GetComponent<ScreenShake>().Shake();
    }

    /// <summary>
    /// called when trigger win
    /// </summary>
    public void Win()
    {
        if (winned)
            return;
        winned = true;

        Debug.Log("Win !");
        if (idLevel != idLevelMax)       //tout les niveaux sauf le dernier
        {
            playerController.enabled = false;
            cam.transform.SetParent(null);  //set la caméra à la racine

            Invoke("jumpNextLevel", timeFadeOutWhenWin);
        }
        else
        {
            //ending
            for (int i = 0; i < colliderPlanet.Count; i++)
            {
                colliderPlanet[i].enabled = false;
            }
            playerController.Jump();
            Invoke("jumpEnding", timeShowEnd);
        }
    }

    private void jumpEnding()
    {
        titleScreen.enabled = true;
        titleScreen.ActiveEnd();
        readyToRestart = true;
    }

    private void jumpNextLevel()
    {
        GlobalData.GetSingleton.TmpPlayerMovement = playerController.playerBody;
        SceneChangeManager.getSingleton().JumpToSceneWithFade("Level " + (idLevel + 2));
    }

    /// <summary>
    /// met en pause le jeu (affiche l'image selon si la manette / clavier est connecté)
    /// </summary>
    private void Paused()
    {
        timeTmpForPauseEscape = Time.unscaledTime;

        if (PlayerConnected.getSingularity().playerArrayConnected[0])
        {
            canvasPause.transform.GetChild(0).gameObject.SetActive(false);
            canvasPause.transform.GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            canvasPause.transform.GetChild(0).gameObject.SetActive(true);
            canvasPause.transform.GetChild(1).gameObject.SetActive(false);
        }
        canvasPause.SetActive(true);

        paused = true;
        Time.timeScale = 0;
    }
    /// <summary>
    /// relance le jeu
    /// </summary>
    private void Resume()
    {
        TWNE.isOk = false;

        canvasPause.SetActive(false);
        paused = false;
        Time.timeScale = 1;
    }

    /// <summary>
    /// gère les inputs du jeu
    /// </summary>
    private void InputGame()
    {
        //if we win, just accept the changement scene
        if (winned)
        {
            if (idLevel == idLevelMax && readyToRestart)    //if this is the end and we can restart
            {
                if (PlayerConnected.getSingularity().getPlayer(0).GetButtonDown("FireB"))
                {
                    SceneChangeManager.getSingleton().JumpToSceneWithFade("Level 1");
                }
            }
            return;
        }            
        if (loosed)
        {
            //if we loose, 1 inputs
            if (PlayerConnected.getSingularity().getPlayer(0).GetButtonDown("FireA"))
            {
                if (gameover_retryText.color.a != 1)
                {
                    gameover_canvas.GetComponent<Animator>().enabled = false;
                    gameover_trappedText.color = new Color(gameover_trappedText.color.r, gameover_trappedText.color.g, gameover_trappedText.color.b, 1);
                    gameover_retryText.color = new Color(gameover_trappedText.color.r, gameover_trappedText.color.g, gameover_trappedText.color.b, 1);
                } 
                else
                    SceneChangeManager.getSingleton().JumpToSceneWithFade(SceneManager.GetActiveScene().name);
            }
        }
        else
        {
            if (Time.unscaledTime < timeTmpForPauseEscape + 0.3f/* || transitionState*/)
            {
                return;
            }

            if (!paused && (PlayerConnected.getSingularity().getPlayer(0).GetButtonDown("Start") || PlayerConnected.getSingularity().getPlayer(0).GetButtonDown(0) || PlayerConnected.getSingularity().getPlayer(0).GetButtonDown(4) ||
                               PlayerConnected.getSingularity().getPlayer(0).GetButtonDown("Escape") || PlayerConnected.getSingularity().getPlayer(0).GetButtonDown(4)))
            {
                Paused();
            }
            else if (paused)
            {

                if (PlayerConnected.getSingularity().getPlayer(0).GetButtonDown("FireA")
                    || PlayerConnected.getSingularity().getPlayer(0).GetButtonDown("Start"))
                {
                    Resume();
                }
                if (PlayerConnected.getSingularity().getPlayer(0).GetButtonDown("FireB") ||
                    PlayerConnected.getSingularity().getPlayer(0).GetButtonDown("Escape"))
                {
                    Time.timeScale = 1;
                    SceneChangeManager.getSingleton().JumpToSceneWithFade("Level 1");
                }
            }
        }

    }

    
    #endregion

    #region Unity ending functions

    private void Update()
    {
        if (!paused)
            InputGame();
        if (transitionState && slowMowActivated)
            CancelSLowMow();
        if (!cameraZoomTransitionOver)
            DezoomCamera();
        //optimisation des fps
        if (updateTimer.Ready())
        {
            
        }
    }

    private void OnGUI()
    {
        if (paused)
            InputGame();
    }

    #endregion
}

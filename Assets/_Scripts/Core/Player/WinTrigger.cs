using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// WinTrigger Description
/// </summary>
public class WinTrigger : MonoBehaviour
{
    #region Attributes
    [FoldoutGroup("GamePlay"), Tooltip("game manager"), SerializeField]
    private GameManager gameManager;
    #endregion

    #region Initialization

    private void Start()
    {
        if (!gameManager)
            gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }
    #endregion

    #region Core

    #endregion

    #region Unity ending functions

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.Win();
        }
    }

    #endregion
}

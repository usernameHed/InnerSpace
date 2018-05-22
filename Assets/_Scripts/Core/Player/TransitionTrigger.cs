using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// WinTrigger Description
/// </summary>
public class TransitionTrigger : MonoBehaviour
{
    #region Attributes

    #endregion

    #region Initialization

    private void Start()
    {

    }
    #endregion

    #region Core

    #endregion

    #region Unity ending functions

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.GetSingleton.EndTransitionCamera();
            this.enabled = false;
        }
    }

    #endregion
}

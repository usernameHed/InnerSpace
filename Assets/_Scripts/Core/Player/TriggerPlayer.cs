using UnityEngine;

/// <summary>
/// TriggerPlayer Description
/// </summary>
public class TriggerPlayer : MonoBehaviour
{
    #region Attributes

	[Tooltip("opti fps"), SerializeField]
	private FrequencyTimer updateTimer;

    #endregion

    #region Initialization

    private void Start()
    {
		// Start function
    }
    #endregion

    #region Core

    #endregion

    #region Unity ending functions

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController PC = other.GetComponent<PlayerController>();
            PC.ChangeOnEnnemy(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController PC = other.GetComponent<PlayerController>();
            PC.ChangeOnEnnemy(false);
        }
    }

    #endregion
}

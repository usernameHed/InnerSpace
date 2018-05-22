using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// CameraLookAt Description
/// </summary>
public class CameraLookAt : MonoBehaviour
{
    #region Attributes
    [FoldoutGroup("GamePlay"), Tooltip("planette qui attire"), SerializeField]
    private Transform planet;
    [FoldoutGroup("GamePlay"), Tooltip("speed"), SerializeField]
    private float speed = 10;


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

    private void LateUpdate()
    {
        transform.LookAt(planet);
        /*Vector3 lTargetDir = planet.position - transform.position;
        lTargetDir.y = 0.0f;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(lTargetDir), Time.time * speed);
        */
    }

    #endregion
}

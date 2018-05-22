using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// RotateAround Description
/// </summary>
public class RotateAround : MonoBehaviour
{
    #region Attributes
    [FoldoutGroup("GamePlay"), Tooltip("directionSatelite"), SerializeField]
    private Vector3 direction;
    [FoldoutGroup("GamePlay"), Tooltip("planette de base"), SerializeField]
    private Transform planet;

    [Tooltip("opti fps"), SerializeField]
	private FrequencyTimer updateTimer;

    #endregion

    #region Initialization

    private void Start()
    {
        
    }
    #endregion

    #region Core

    #endregion

    #region Unity ending functions

    private void Update()
    {
        transform.RotateAround(planet.position, direction, 20 * Time.deltaTime);
    }

	#endregion
}

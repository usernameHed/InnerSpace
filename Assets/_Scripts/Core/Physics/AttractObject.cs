using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

/// <summary>
/// AttractObject Description
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class AttractObject : MonoBehaviour
{
    #region Attributes
    [FoldoutGroup("GamePlay"), Tooltip("planette qui attire"), SerializeField]
    private List<Rigidbody> planetsList;
    //private Transform planet;

    [FoldoutGroup("Debug"), Tooltip("opti fps"), SerializeField]
	private FrequencyTimer updateTimer;


    private Rigidbody rb;
    #endregion

    #region Initialization

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        //rbPlanet = planet.GetComponent<Rigidbody>();
    }
    #endregion

    #region Core
    /// <summary>
    /// attire l'objet vers le centre de la planette
    /// </summary>
    private void Attract(Rigidbody rbPlanet)
    {
        Vector3 direction = rb.position - rbPlanet.position;
        float distance = direction.magnitude;

        float forceMagnitude = (rb.mass * rbPlanet.mass) / Mathf.Pow(distance, 2);
        Vector3 force = direction.normalized * forceMagnitude;

        rb.AddForce(-force);
    }
    #endregion

    #region Unity ending functions

    private void Update()
    {
        //optimisation des fps
        if (updateTimer.Ready())
        {

        }
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < planetsList.Count; i++)
        {
            Attract(planetsList[i]);
        }
    }

    #endregion
}

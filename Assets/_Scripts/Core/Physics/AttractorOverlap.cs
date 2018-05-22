using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

/// <summary>
/// AttractObject Description
/// </summary>
public class AttractorOverlap : MonoBehaviour
{
    #region Attributes
    [FoldoutGroup("GamePlay"), Tooltip("radius d'attraction"), SerializeField]
    private float radius = 10f;
    [FoldoutGroup("GamePlay"), Tooltip("radius d'attraction"), SerializeField]
    private float attractionPlayer = 0.1f;

    [FoldoutGroup("GamePlay"), Tooltip("force d'attraction"), SerializeField]
    private float strenght = 1000f;
    //private Transform planet;
    private int layerMask = 1 << 8; //select layer 8 (metallica and colider)

    [FoldoutGroup("Debug"), Tooltip("opti fps"), SerializeField]
	private FrequencyTimer updateTimer;

    [FoldoutGroup("Debug"), Tooltip("opti fps"), SerializeField]
    private List<Rigidbody> listObjectOverlaping;

    private Collider[] overlapResults = new Collider[30];
    private int numFound = 0;

    private Rigidbody rb;
    #endregion

    #region Initialization

    #endregion

    #region Core
    /// <summary>
    /// attire l'objet vers le centre de la planette
    /// </summary>
    private void Attract(Rigidbody rbPlanet, bool player = false)
    {
        if (!rbPlanet)
            return;

        Vector3 direction = transform.position - rbPlanet.position;
        //float distance = direction.magnitude;

        //float forceMagnitude = (rb.mass * rbPlanet.mass) / Mathf.Pow(distance, 2);
        Vector3 force = direction.normalized * strenght * ((player) ? attractionPlayer : 1);//forceMagnitude;

        rbPlanet.AddForce(force);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        //Use the same vars you use to draw your Overlap SPhere to draw your Wire Sphere.
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    /// <summary>
    /// remplie la lsite à chaque X temps des objets proche du satelites
    /// </summary>
    private void SetListOverlap()
    {
        numFound = Physics.OverlapSphereNonAlloc(transform.position, radius, overlapResults, layerMask);

        for (int i = 0; i < numFound; i++)
        {
            Debug.DrawLine(transform.position, overlapResults[i].transform.position, Color.red);
        }
    }

    /// <summary>
    /// attique chaque objet se trouvant dans la liste, chaque fixedUpdate;
    /// </summary>
    private void AttractObject()
    {
        for (int i = 0; i < numFound; i++)
        {
            PlayerController PC = overlapResults[i].gameObject.GetComponent<PlayerController>();

            if (PC)
            {
                if (PC.isOnEnnemy)
                {
                    if (!PC.isDead)
                    {

                        overlapResults[i].gameObject.GetComponent<PlayerController>().Kill();
                        GameManager.GetSingleton.Lose(transform);
                    }
                        
                    else
                        Attract(overlapResults[i].gameObject.GetComponent<Rigidbody>(), true);
                }
                
            }
            else
                Attract(overlapResults[i].gameObject.GetComponent<Rigidbody>());
        }
    }

    #endregion

    #region Unity ending functions

    private void Update()
    {
        //optimisation des fps
        if (updateTimer.Ready())
        {
            SetListOverlap();
        }
    }

    private void FixedUpdate()
    {
        AttractObject();
        /*for (int i = 0; i < planetsList.Count; i++)
        {
            Attract(planetsList[i]);
        }*/
    }

    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


public class GlobalData : MonoBehaviour
{
    #region Attributes
    /// <summary>
    /// variable privé
    /// </summary>
    [FoldoutGroup("GamePlay"), Tooltip("planete centrale"), SerializeField]
    private Rigidbody tmpPlayerMovement;
    public Rigidbody TmpPlayerMovement { set; get; }

    private static GlobalData instance;
    public static GlobalData GetSingleton
    {
        get { return instance; }
    }
    #endregion

    #region Initialization
    /// <summary>
    /// test si on met le script en UNIQUE
    /// </summary>
    private void SetSingleton()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    /// <summary>
    /// Initialisation
    /// </summary>
    private void Awake()                                                    //initialisation referencce
    {
        SetSingleton();
    }

    #endregion

    #region Core

    
    #endregion
}
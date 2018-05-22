using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreenScript : MonoBehaviour {

    public GameObject wind;
    public Image title;
    public int playerID;

    private bool exitMenu = false;

    private void Start()
    {
        if (GameManager.GetSingleton.IdLevel != 0)
            ActiveLevel();
    }

    /// <summary>
    /// call it when we first play the game (in menu)
    /// </summary>
    public void ActiveLevel()
    {
        wind.SetActive(true);
        exitMenu = true;
    }

    /// <summary>
    /// called when this is the end, and we need to display Menu;
    /// </summary>
    public void ActiveEnd()
    {
        title.color = Color.white;
        exitMenu = false;
    }

    private void Update()
    {
        if (exitMenu)
        {
            float alpha = title.color.a - 0.05f;
            if (alpha <= 0)
                alpha = 0;
            title.color = new Color(1, 1, 1, alpha);
            if (alpha == 0)
                this.enabled = false;
            return;
        }

        float horizMove = PlayerConnected.getSingularity().getPlayer(playerID).GetAxis("Move Horizontal");
        float vertiMove = PlayerConnected.getSingularity().getPlayer(playerID).GetAxis("Move Vertical");
        if(horizMove != 0 || horizMove != 0)
        {
            ActiveLevel();
        }
    }
}

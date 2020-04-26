using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    /*index 
        #############
        #           #
        #  GENERAL  #
        #           #
        #############
    */

    private GameObject mainMenu;
    private GameObject options;
    private GameObject credits;

    private GameManager scriptGM;

    /*index 
        ##############
        #            #
        #  OPCIONES  #
        #            #
        ##############
    */

    #region OPCIONES

    public Slider masterVolumen;
    public float volumeValue;

    public void saveSoundValue(float _volumeValue)
    {
        volumeValue = _volumeValue;
    }

    private TextMeshProUGUI volume;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

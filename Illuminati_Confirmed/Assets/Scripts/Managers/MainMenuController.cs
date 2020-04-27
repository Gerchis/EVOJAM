using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject opciones;
    public GameObject creditos;

    // Start is called before the first frame update
    void Start()
    {
        volumen=GameObject.Find("SliderVolumen").GetComponent<Slider>();

        mainMenu.SetActive(true);
        opciones.SetActive(false);
        creditos.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Credits()
    {
        mainMenu.SetActive(false);
        creditos.SetActive(true);
    }

    public void Options()
    {
        mainMenu.SetActive(false);
        opciones.SetActive(true);
    }

    public void Volver()
    {
        mainMenu.SetActive(true);
        opciones.SetActive(false);
        creditos.SetActive(false);
    }

    public void Play()
    {
        SceneManager.LoadScene(2);
    }

    private Slider volumen;

    public void SaveAudio()
    {
        GameManager.Instance.saveVolumenValue(volumen.value);
    }
}

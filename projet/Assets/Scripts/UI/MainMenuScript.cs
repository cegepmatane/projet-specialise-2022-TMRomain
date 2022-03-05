using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuScript : MonoBehaviour
{
    [SerializeField]
    GameObject menuMain;
    [SerializeField]
    GameObject menuLancer;
    [SerializeField]
    GameObject menuOptions;


    void Awake() {
        Retour();
    }
    public void Retour(){
        menuMain.SetActive(true);
        menuLancer.SetActive(false);
        menuOptions.SetActive(false);
    }
    public void MenuLancer(){
        menuMain.SetActive(false);
        menuLancer.SetActive(true); 
    }
    public void Lancer(){
        SceneManager.LoadScene("SampleScene");
    }
    
    public void MenuOptions(){
        menuMain.SetActive(false);
        menuOptions.SetActive(true); 
    }
    public void ApplicationQuit(){
        Application.Quit();
    }
}

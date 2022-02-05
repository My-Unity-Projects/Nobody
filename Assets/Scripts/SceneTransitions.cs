using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneTransitions : MonoBehaviour
{
    public Animator transitionAni;
    public string sceneName;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.O))
        {
            GameObject.FindObjectOfType<AudioManager>().Play("Press Button");
            LoadNewScene(SceneManager.GetActiveScene().name);
        }

        AllScenes();
    }

    public void AllScenes()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            GameObject.FindObjectOfType<AudioManager>().Play("Press Button");
            LoadNewScene("Level 1");
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            GameObject.FindObjectOfType<AudioManager>().Play("Press Button");
            LoadNewScene("Level 2");
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            GameObject.FindObjectOfType<AudioManager>().Play("Press Button");
            LoadNewScene("Level 3");
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            GameObject.FindObjectOfType<AudioManager>().Play("Press Button");
            LoadNewScene("Level 4");
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            GameObject.FindObjectOfType<AudioManager>().Play("Press Button");
            LoadNewScene("Level 5");
        }
    }

    public void LoadNewScene(string sn)
    {
        sceneName = sn;
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        transitionAni.SetTrigger("out");
        yield return new WaitForSeconds(1.1f);
        SceneManager.LoadScene(sceneName);
    }
}

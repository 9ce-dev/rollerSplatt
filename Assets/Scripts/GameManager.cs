
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager singleton;

    private GroundPiece[] allGroundPieces;
    public ParticleSystem sparklesParticle;

    

    private void Start()
    {
        SetupNewLevel();
        sparklesParticle = GetComponent<ParticleSystem>();
        //Destroy(sparklesParticle.gameObject);
    }

    private void SetupNewLevel()
    {
        allGroundPieces = FindObjectsOfType<GroundPiece>();
    }

    private void Awake()
    {
        if (singleton == null)
            singleton = this;
        else if (singleton != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        SetupNewLevel();
        //sparklesParticle.Play();
    }


    public void CheckComplete()
    {
        bool isFinished = true;

        for (int i = 0; i < allGroundPieces.Length; i++)
        {
            if (allGroundPieces[i].isColored == false)
            {
                isFinished = false;
                break;
            }
        }

        if (isFinished)
            NextLevel();
    }

    private void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private static GameManager instance = null;

    private void OnAwake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            EnableParticleSystem();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void EnableParticleSystem()
    {
        sparklesParticle.gameObject.SetActive(true);
    }

    void DestroyParticleSystem()
    {
        sparklesParticle = GetComponent<ParticleSystem>();
        Destroy(sparklesParticle.gameObject);
    }

   /* void AccessParticleSystem()
    {
        if (sparklesParticle != null)
        {
            // Access the particle system here
            EnableParticleSystem();
        }
    }*/


}

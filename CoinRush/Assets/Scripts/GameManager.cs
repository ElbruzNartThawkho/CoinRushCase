using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameState gameState;
    [SerializeField] CinemachineVirtualCamera vCam;
    [SerializeField] GameObject start, finish, levelProgressBar, player, finishScreen, gameOverScreen;
    [SerializeField] List<GameObject> chunks;
    Vector3 placeTransform;

    public enum GameState
    {
        Start,
        inGame,
        gameOver,
        goTower,
        finish
    }

    private void Awake()
    {
        instance = this;
        gameState = GameState.Start;
    }
    // Start is called before the first frame update
    void Start()
    {
        LevelCreate();
        levelProgressBar.GetComponent<Slider>().maxValue = GameObject.FindWithTag("Finish").transform.position.z;
        levelProgressBar.GetComponent<Slider>().minValue = start.transform.position.z;
        levelProgressBar.GetComponent<Slider>().value = player.transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        levelProgressBar.GetComponent<Slider>().value = player.transform.position.z;
    }
    void LevelCreate()
    {
        placeTransform.z += 16;
        for (int i = 0; i < Random.Range(4, 10); i++)
        {
            Instantiate(chunks[Random.Range(0, chunks.Count)], placeTransform, Quaternion.identity);
            placeTransform.z += 48;
        }
        Instantiate(finish, placeTransform, Quaternion.identity);
    }
    public void STPBtn()
    {
        GameStateChange(GameState.inGame);
    }
    public void GameStateChange(GameState state)
    {
        gameState = state;
        if (gameState == GameState.finish)
        {
            finishScreen.SetActive(true);
        }
        else if (gameState == GameState.gameOver)
        {
            gameOverScreen.SetActive(true);
            player.GetComponent<Rigidbody>().freezeRotation = false;
            player.GetComponent<Coin>(). Dispersal();
            vCam.Follow = null;
        }
        else if (gameState == GameState.goTower)
        {
            player.transform.eulerAngles = Vector3.zero;
        }
    }
    public void GoScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    AudioSourceManager asm;
    static GameManager _instance = null;

    public static GameManager instance
    {
        get => _instance;
        set { _instance = value; }
    }

    public PlayerController playerPrefab;
    [HideInInspector] public Level currentLevel = null;
    [HideInInspector] public PlayerController playerInstance = null;
    [HideInInspector] public Transform currentSpawnPoint = null;

    //Lives
    public int maxLives = 99;
    private int _lives = 0;

    //Sounds
    public AudioClip GameOversfx;
    public AudioClip Deathsfx;
    public int lives
    {
        get { return _lives; }
        set
        {
            if (_lives > value)
                Respawn();

            _lives = value;

            if (_lives > maxLives)
                _lives = maxLives;

            if (_lives < 0)
                GameOver();

            onLifeValueChanged?.Invoke(value);
        }
    }

    [HideInInspector] public UnityEvent<int> onLifeValueChanged;

    [HideInInspector] public bool paused = false;

    // Start is called before the first frame update
    void Awake()
    {
        asm = GetComponent<AudioSourceManager>();

        if (_instance)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    switch (SceneManager.GetActiveScene().buildIndex)
        //    {
        //        case 0:
        //            SceneManager.LoadScene(1);
        //            break;
        //        default:
        //            SceneManager.LoadScene(0);
        //            playerInstance = null;
        //            break;
        //    }
        //}

        if (Input.GetKeyDown(KeyCode.K))
            lives--;
    }

    public void SpawnPlayer(Transform spawnPoint)
    {
        Debug.Log("Player Spawned");
        playerInstance = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        currentSpawnPoint = spawnPoint;
    }

    void Respawn()
    {
        if (asm && lives > 0)
            asm.PlayOneShot(Deathsfx, true);

        if (playerInstance)
            playerInstance.transform.position = currentSpawnPoint.position;

    }

    void GameOver()
    {
        SceneManager.LoadScene(2);
        playerInstance = null;

        if (asm)
            asm.PlayOneShot(GameOversfx, true);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class Spinning : MonoBehaviour
{

    [SerializeField] private float rotationSpeed = 45f;
    [SerializeField] private Transform rotateAround;

    [SerializeField] private GameObject goodSpritePrefab;
    [SerializeField] private GameObject badSpritePrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform despawnPoint;

    [SerializeField] private float minSpawnInterval = 1f;
    [SerializeField] private float maxSpawnInterval = 3f;
    [SerializeField] private float spriteMoveSpeed = 2f;
    [SerializeField] private float spawnMinY = -5f;
    [SerializeField] private float spawnMaxY = 5f;

    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private GameObject gameOverPanel;


    private InputAction clickAction;
    private int score = 0;
    private bool isGameOver = false;
    private List<GameObject> activeSprites = new List<GameObject>();

    void OnEnable()
    {
        clickAction = new InputAction(type: InputActionType.Button, binding: "<Mouse>/leftButton");
        clickAction.Enable();
    }

    void OnDisable()
    {
        clickAction.Disable();
    }

    void Start()
    {
        StartCoroutine(SpawnSprites());
        UpdateScoreUI();

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    void Update()
    {
        if (isGameOver) return;


        this.transform.RotateAround(rotateAround.position, Vector3.forward, rotationSpeed * Time.deltaTime);

        if (clickAction.triggered)
        {
            rotationSpeed = -rotationSpeed;
        }

        MoveSprites();
    }

    IEnumerator SpawnSprites()
    {
        while (!isGameOver)
        {
            yield return new WaitForSeconds(Random.Range(minSpawnInterval, maxSpawnInterval));

            if (isGameOver) break;

            bool spawnGood = Random.Range(0f, 1f) > 0.3f;
            GameObject spriteToSpawn = spawnGood ? goodSpritePrefab : badSpritePrefab;

            if (spriteToSpawn != null && spawnPoint != null)
            {
                float randomY = Random.Range(spawnMinY, spawnMaxY);
                Vector3 spawnPosition = new Vector3(spawnPoint.position.x, randomY, spawnPoint.position.z);

                GameObject newSprite = Instantiate(spriteToSpawn, spawnPosition, Quaternion.identity);
                activeSprites.Add(newSprite);

            }
        }
    }

    void MoveSprites()
    {
        for (int i = activeSprites.Count - 1; i >= 0; i--)
        {
            if (activeSprites[i] == null)
            {
                activeSprites.RemoveAt(i);
                continue;
            }
            activeSprites[i].transform.Translate(Vector2.left * spriteMoveSpeed * Time.deltaTime);


            if (despawnPoint != null && activeSprites[i].transform.position.x <= despawnPoint.position.x)
            {
                Destroy(activeSprites[i]);
                activeSprites.RemoveAt(i);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isGameOver) return;


        if (other.CompareTag("GoodSprite"))
        {
            score++;
            UpdateScoreUI();
            Destroy(other.gameObject);
            activeSprites.Remove(other.gameObject);
        }
        else if (other.CompareTag("BadSprite"))
        {

            GameOver();
            Destroy(other.gameObject);
            activeSprites.Remove(other.gameObject);
        }
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }

    void GameOver()
    {
        isGameOver = true;


        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        foreach (GameObject sprite in activeSprites)
        {
            if (sprite != null)
            {
                Destroy(sprite);
            }
        }
        activeSprites.Clear();
    }

}
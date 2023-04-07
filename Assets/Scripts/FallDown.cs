using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class FallDown : MonoBehaviour
{
    public GameObject bottlePrefab;
    public GameObject bottleChest;

    public Button retryBtn;
    public Button closeBtn;
    public Button startBtn;

    public TextMeshProUGUI scoreValue;
    public TextMeshProUGUI mistakesValue;

    public RawImage levelOne;
    public RawImage levelTwo;
    public RawImage levelThree;
    public RawImage levelFour;
    public RawImage levelFive;

    public float bottleMass = 100f;
    public float destroyHeight = 1.7f;
    public float spawnHeight = 10f;
    private float spawnRadius = 4.6f;

    public float spawnInterval = 1f;
    public float lastSpawnTime = 0f;

    public float fallingSpeed = 1f;

    public short level = 1;
    public int score = 0;
    public short mistakes = 0;

    public Boolean start = false;


    void Start()
    {
        mistakesValue.text = "";
        setVisibilityButtons(false);
        changeVisibilityForImages(false, false, false, false, false);
    }

    void setVisibilityButtons(Boolean visibility)
    {
        retryBtn.gameObject.SetActive(visibility);
        closeBtn.gameObject.SetActive(visibility);
    }

    private void Update()
    {

        if (mistakes == 3)
        {
            setVisibilityButtons(true);
        }
        if (Time.time - lastSpawnTime > spawnInterval && mistakes < 3 && start)
        {
            lastSpawnTime = Time.time;
            spawnBottle();
        }

        foreach (var bottle in GameObject.FindGameObjectsWithTag("Bottle"))
        {
            if (bottle.transform.position.y < destroyHeight)
            {
                Vector3 chestLocalPos = bottleChest.transform.localPosition;

                if (bottle.transform.position.x > chestLocalPos.x - 1.2f &&
                    bottle.transform.position.x < chestLocalPos.x + 1.2f &&
                    bottle.transform.position.z > chestLocalPos.z - 2f &&
                    bottle.transform.position.z < chestLocalPos.z + 2f)
                {
                    score++;
                    handleScore();
                }
                else
                {
                    mistakes++;
                    handleMistake();
                }
                Destroy(bottle);
            }
        }


    }

    void spawnBottle()
    {
            Vector3 spawnPosition = new Vector3(UnityEngine.Random.Range(-spawnRadius, spawnRadius), spawnHeight, UnityEngine.Random.Range(-spawnRadius, spawnRadius));
            Rigidbody bottleRigidbody = bottlePrefab.GetComponent<Rigidbody>();
            if (bottleRigidbody == null)
            {
                bottleRigidbody = bottlePrefab.AddComponent<Rigidbody>();
            }
            bottleRigidbody.mass = bottleMass;
            bottleRigidbody.useGravity = true;
            bottleRigidbody.AddForce(Physics.gravity * fallingSpeed);
            bottlePrefab.tag = "Bottle";
            Instantiate(bottlePrefab, spawnPosition, Quaternion.Euler(-90f, 0f, 0f));
        
    }

    private void updateConstantsForCurrentLevel()
    {
        if (level % 2 != 0)
        {
            spawnInterval *= 0.8f;
        }
        else
        {
            fallingSpeed *= 1.2f;
        }

        switch (level)
        {
            case 2:
                changeVisibilityForImages(false, true, false, false, false);
                break;
            case 3:
                changeVisibilityForImages(false, false, true, false, false);
                break;
            case 4:
                changeVisibilityForImages(false, false, false, true, false);
                break;
            case 5:
                changeVisibilityForImages(false, false, false, false, true);
                break;
        }
    }

    private void handleMistake()
    {
        switch (mistakes)
        {
            case 1:
                mistakesValue.text = "X";
                break;
            case 2:
                mistakesValue.text = "X X";
                break;
            case 3:
                mistakesValue.text = "GAME \n OVER";
                break;
        }
    }

    private void handleScore()
    {
        if (score > 0 && score % 10 == 0)
        {
            level++;
            updateConstantsForCurrentLevel();
        }
        scoreValue.text = score.ToString();
    }

    private void changeVisibilityForImages(Boolean value1, Boolean value2, Boolean value3, Boolean value4, Boolean value5)
    {
        levelOne.gameObject.SetActive(value1);
        levelTwo.gameObject.SetActive(value2);
        levelThree.gameObject.SetActive(value3);
        levelFour.gameObject.SetActive(value4);
        levelFive.gameObject.SetActive(value5);
    }

    public void OnClick()
    {
        spawnInterval = 1f;
        lastSpawnTime = 0f;

        fallingSpeed = 1f;

        level = 1;
        score = 0;
        mistakes = 0;

        mistakesValue.text = "";
        scoreValue.text = score.ToString();
        setVisibilityButtons(false);

        changeVisibilityForImages(true, false, false, false, false);
    }

    public void onClickClose()
    {
        Application.Quit();
    }

    public void onClickStart()
    {
        start = true;
        startBtn.gameObject.SetActive(false);
        changeVisibilityForImages(true, false, false, false, false); ;
    }
}

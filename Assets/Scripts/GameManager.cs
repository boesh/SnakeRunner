using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    [SerializeField]
    Text killsText;
    [SerializeField]
    Text crystalsText;

    [SerializeField]
    SnakeController snake;
    [SerializeField]
    HumanGroup humanGroupPrefab;
    [SerializeField]
    NewColorAreaStarter StarterPrefab;
    [SerializeField]
    GameObject crystalsAndPianosPrefabTwoPianos;
    [SerializeField]
    GameObject crystalsAndPianosPrefabTwoCrystals;

    Pool humansPool;
    Pool starersPool;
    Pool pianosTwoCrystalsPool;
    Pool crystalsTwoPianosPool;
    
    [SerializeField]
    List<GameObject> crystalsAndPianos;
    [SerializeField]
    List<Color> colors;
    [SerializeField]
    float rangeZBetweenObjects = 5;
   
    int fakeColorIndex = 2;

    private void Awake()
    {
        snake.OnStarter += SpawnNewArea;
        snake.OnStarter += NewHumanGroupsSpawn;
        snake.OnStarter += SpawnPianosAndCrystals;
        humansPool = new Pool();
        starersPool = new Pool();
        pianosTwoCrystalsPool = new Pool();
        crystalsTwoPianosPool = new Pool();
        SpawnNewArea(-120);
    }

    private void Start()
    {
        snake.UIUpdate += UIUpdate;
    }

    void UIUpdate()
    {
        killsText.text = "Kills: " + snake.Kills;
        crystalsText.text = "Crystals: " + snake.Crystals;
    }

    void SpawnPianosAndCrystals(float z)
    {
        int prevRes = Random.Range(0, 2);
        int prevPrevRes = prevRes;
        int r;
        for (int i = 0; i < 6; i++)
        {
            GameObject obj;
            do
            {
                r = Random.Range(0, 2);
            } while (r == prevRes && r == prevPrevRes);

            prevPrevRes = prevRes;
            prevRes = r;
            
            if (r < 1)
            {
                obj = pianosTwoCrystalsPool.GetObjectFromPool(crystalsAndPianosPrefabTwoCrystals.gameObject);
            }
            else
            {
                obj = crystalsTwoPianosPool.GetObjectFromPool(crystalsAndPianosPrefabTwoPianos.gameObject);
            }
            obj.transform.position = 
                new Vector3(crystalsAndPianosPrefabTwoCrystals.transform.position.x, crystalsAndPianosPrefabTwoCrystals.transform.position.y, i * rangeZBetweenObjects + z + 80f);
            obj.GetComponent<PianoAndCrystalsController>().Settings();
        }
    }

    void SpawnNewArea(float z)
    {
        do
        {
            fakeColorIndex = Random.Range(0, 6);
        } while (fakeColorIndex == snake.currentColorIndex);

        GameObject starterObj = starersPool.GetObjectFromPool(StarterPrefab.gameObject);

        starterObj.GetComponent<NewColorAreaStarter>().color = colors[fakeColorIndex];
        starterObj.GetComponent<NewColorAreaStarter>().SetColor();

        starterObj.GetComponent<NewColorAreaStarter>().colorIndex = fakeColorIndex;
        starterObj.GetComponent<NewColorAreaStarter>().isUsed = false;

        starterObj.GetComponent<Renderer>().material.color = colors[fakeColorIndex];

        starterObj.transform.position = new Vector3(StarterPrefab.transform.position.x, StarterPrefab.transform.position.y, z + 150f);
    }


    void NewHumanGroupsSpawn(float z)
    {
        while (fakeColorIndex == snake.currentColorIndex)
        {
            fakeColorIndex = Random.Range(0, colors.Count);
        }
        int prevRes = Random.Range(0, 2);
        int prevPrevRes = prevRes;
        int r;
        for (int i = 0; i < 8; i += 2)
        {
            GameObject leftGroup =  humansPool.GetObjectFromPool(humanGroupPrefab.gameObject);
            GameObject rightGroup = humansPool.GetObjectFromPool(humanGroupPrefab.gameObject);
            
            do
            {
                r = Random.Range(0, 2);
            } while (r == prevRes && r == prevPrevRes);

            prevPrevRes = prevRes;
            prevRes = r;

            if (r < 1)
            {
                leftGroup.GetComponent<HumanGroup>().setHumanGroup(colors[fakeColorIndex], fakeColorIndex);

                rightGroup.GetComponent<HumanGroup>().setHumanGroup(colors[snake.currentColorIndex], snake.currentColorIndex);
            }
            else
            {
                leftGroup.GetComponent<HumanGroup>().setHumanGroup(colors[snake.currentColorIndex], snake.currentColorIndex);

                rightGroup.GetComponent<HumanGroup>().setHumanGroup(colors[fakeColorIndex], fakeColorIndex);

            }
            rightGroup.transform.position = new Vector3(humanGroupPrefab.transform.position.x + 2, humanGroupPrefab.transform.position.y, i * rangeZBetweenObjects + z + 20f);

            leftGroup.transform.position = new Vector3(humanGroupPrefab.transform.position.x - 2, humanGroupPrefab.transform.position.y, i * rangeZBetweenObjects + z + 20f);
            
        }
    }
}

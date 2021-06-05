using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SnakeController : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float smooth = 1.2f;
   
    [SerializeField]
    private float CircleDiameter;

    public int currentColorIndex;

    private int kills = 0;

    public int Kills
    {
        get {
            return kills;
        }
        private set
        {
            kills = value;
        }
    }

    private int crystals = 0;

    public int Crystals
    {
        get
        {
            return crystals;
        }
        private set
        {
            crystals = value;
        }
    }

    public event Action<GameObject> OnEat;
    public event Action<float> OnStarter;
    public event Action UIUpdate;

    float firstCrystalPositionZ = 0;
    float secondCrystalPositionZ = 0;
    bool berserkMode = false;

    [SerializeField]
    float targetDistance;
    
    [SerializeField]
    private Transform SnakeHead;

    [SerializeField]
    private Transform TailPrefab;

    private List<Transform> SnakeTails = new List<Transform>();
    private List<Vector3> FakeSnakeTails = new List<Vector3>();

    private void OnTriggerEnter(Collider other)
    {
        OnEat(other.gameObject);
        UIUpdate();

        if (other.tag == "Starter")
        {
            currentColorIndex = other.GetComponent<NewColorAreaStarter>().colorIndex;
            gameObject.GetComponent<Renderer>().material.color = other.gameObject.GetComponent<Renderer>().material.color;
            
            //other.gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Starter")
        {
            if (!other.GetComponent<NewColorAreaStarter>().isUsed)
            { 
                OnStarter(transform.position.z + 40);
                other.GetComponent<NewColorAreaStarter>().isUsed = true;
            }
        }
    }

    private void Awake()
    {
        OnEat += Eat;
    }

    void Start()
    {
        AddCircle();
        AddCircle();
        AddCircle();
       
        OnEat(gameObject);
        UIUpdate();
    }

    void Update()
    {
        Rotate();

    }

    private void FixedUpdate()
    {
        MoveTail();

        MoveHead();

    }

    IEnumerator BerserkTimer(float timeInSec)
    {
        yield return new WaitForSeconds(timeInSec);
        speed /= 3;
        berserkMode = false;
        crystals = 0;
        UIUpdate();
    }

    void BerserkMode()
    {
        speed *= 3;
        berserkMode = true;
        StartCoroutine(BerserkTimer(5));
    }

    void Eat(GameObject eatedObject)
    {
        
        if (eatedObject.tag == "Human")
        {

            if (eatedObject.GetComponent<HumanController>().colorIndex == currentColorIndex)
            {
                kills++;
                if (SnakeTails.Count < 20)
                {
                    AddCircle();
                }
                eatedObject.SetActive(false);
            }
            else
            {
                if (!berserkMode)
                {
                    //death
                    SceneManager.LoadScene(0);
                }
                else
                {
                    kills++;
                    eatedObject.SetActive(false);
                }
            }
        }

        if(eatedObject.tag == "Piano")
        {
            if (!berserkMode)
            {
                //death
                SceneManager.LoadScene(0);
            }
            else
            {
                eatedObject.SetActive(false);
            }
        }

        if (eatedObject.tag == "Crystal")
        {
            crystals++;
            eatedObject.SetActive(false);

            float p = eatedObject.transform.position.z;

            if (p - firstCrystalPositionZ < 6 && firstCrystalPositionZ - secondCrystalPositionZ < 6)
            {
                if (!berserkMode)
                {
                    BerserkMode();
                }
            }
            secondCrystalPositionZ = firstCrystalPositionZ;
            firstCrystalPositionZ = eatedObject.transform.position.z;
        }
    }

    void MoveTail()
    {
        Vector3 targetPosition;
        Transform prevTail = SnakeHead.transform;
        for (int i = 0; i < SnakeTails.Count; i++)
        {
            targetPosition = prevTail.position;
            FakeSnakeTails[i] = SnakeTails[i].position + ((targetPosition - SnakeTails[i].position) / smooth) * Time.deltaTime * speed;
            SnakeTails[i].position = FakeSnakeTails[i];

            //SnakeTails[i].LookAt(prevTail);
            prevTail = SnakeTails[i];
        }
    }

    public void AddCircle()
    {
        Transform tail = Instantiate(TailPrefab);

        if (SnakeTails.Count != 0)
        {
            tail.position = SnakeTails[SnakeTails.Count - 1].position - SnakeTails[SnakeTails.Count - 1].forward * targetDistance;
        }
        else
        {
            tail.position = SnakeHead.position - SnakeHead.forward * targetDistance;
        }
        
        tail.GetComponent<Renderer>().material.color = gameObject.GetComponent<Renderer>().material.color;
        SnakeTails.Add(tail);
        FakeSnakeTails.Add(tail.position);
    }

    void Rotate()
    {
        if(!berserkMode)
        { 
            if(Input.GetMouseButton(0))
            {
                RaycastHit hit;

                if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
                {
                    if (hit.collider == null) return;

                    transform.LookAt(new Vector3(hit.point.x, transform.position.y, hit.point.z < transform.position.z + 10 ? transform.position.z + 3 : hit.point.z));
                }
            }
            //if (Input.GetAxis("Horizontal") < 0 && !onLeftBorder)
            //{
            //    transform.rotation = Quaternion.Euler(new Vector3(0, Input.GetAxis("Horizontal") * 70f, 0));
            //}
            //else if (Input.GetAxis("Horizontal") > 0 && !onRightBorder)
            //{
            //    transform.rotation = Quaternion.Euler(new Vector3(0, Input.GetAxis("Horizontal") * 70f, 0));
            //}
            else
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            }
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(0f, transform.position.y, transform.position.z), 0.1f);
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
    }

    void MoveHead()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }
}

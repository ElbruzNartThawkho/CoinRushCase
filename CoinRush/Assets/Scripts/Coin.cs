using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public List<GameObject> coinList;
    [SerializeField] GameObject coinPrefab;

    private Vector3 firstTouch, currentTouch, currentEuler,rake;
    private Vector2 touchDistance;

    private float touchValue, speed = 10;
    private bool untouchable = false;
    private void Awake()
    {
        currentEuler.z = 0;
        rake.z = 0;
    }
    void Update()
    {
        //transform.LookAt(new Vector3(look.transform.position.x, look.transform.position.y, look.transform.position.z));
        if (GameManager.instance.gameState != GameManager.GameState.gameOver)
        {
            rake.z = 0;
            rake.y = transform.eulerAngles.y;
            rake.x = transform.eulerAngles.x;
            transform.eulerAngles = rake;
        }
        if (GameManager.instance.gameState == GameManager.GameState.inGame)
        {
            transform.position += transform.forward * speed * Time.deltaTime;
            if (Input.touchCount > 0)
            {
                Touch finger = Input.GetTouch(0);
                if (finger.phase == TouchPhase.Began)
                {
                    firstTouch = finger.position;
                }
                if (finger.phase == TouchPhase.Moved)
                {
                    currentTouch = finger.position;
                    touchDistance = (currentTouch - firstTouch);
                    touchValue = (transform.eulerAngles.y + (touchDistance.x / 10));
                    currentEuler.x = transform.eulerAngles.x;
                    currentEuler.y = touchValue;
                    transform.eulerAngles = currentEuler;
                    firstTouch = finger.position;
                }
            }
            if (Mathf.Abs(transform.position.x) > 10 || transform.position.y < 8)
            {
                GameManager.instance.GameStateChange(GameManager.GameState.gameOver);
            }
        }
        else if(GameManager.instance.gameState == GameManager.GameState.goTower)
        {
            transform.position+= transform.forward * speed * Time.deltaTime;
        }
    }
    GameObject newCoin;
    void AddCoin()
    {
        newCoin = Instantiate(coinPrefab, transform.position, Quaternion.identity);
        if (coinList.Count>0)
        {
            newCoin.GetComponent<FollowingCoin>().frontCoin = coinList[coinList.Count - 1];
        }
        else
        {
            newCoin.GetComponent<FollowingCoin>().frontCoin = gameObject;
        }
        coinList.Add(newCoin);
        StartCoroutine(BounceEffect());
    }
    public void Dispersal()
    {
        foreach (GameObject coin in coinList)
        {
            coin.GetComponent<FollowingCoin>().disper = true;
            coin.GetComponent<Rigidbody>().isKinematic = false;
            coin.GetComponent<MeshCollider>().isTrigger = false;
        }
        coinList.Clear();
    }
    IEnumerator BounceEffect()
    {
        for (int i = 0; i < coinList.Count; i++)
        {
            if(coinList[i] != null)
            {
                coinList[i].GetComponent<FollowingCoin>().bounce = true;
            }
            yield return new WaitForSeconds(0.2f);
        }
    }
    IEnumerator UnTouch()
    {
        untouchable = true;
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        for (int i = 0; i < 10; i++)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            yield return new WaitForSeconds(0.05f);
            gameObject.GetComponent<MeshRenderer>().enabled = true;
            yield return new WaitForSeconds(0.05f);
        }
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
        untouchable = false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Obs" && untouchable == false)
        {
            if (coinList.Count > 0)
            {
                StartCoroutine(UnTouch());
                Dispersal();
            }
            else
            {
                GameManager.instance.GameStateChange(GameManager.GameState.gameOver);
            }
        }
        else if (collision.gameObject.tag == "Obs")
        {
            collision.gameObject.GetComponent<Collider>().isTrigger = true;
        }
        else if (collision.gameObject.tag == "Ladder")
        {
            GameManager.instance.GameStateChange(GameManager.GameState.finish);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Finish")
        {
            GameManager.instance.GameStateChange(GameManager.GameState.goTower);
        }
        else if(other.gameObject.tag == "Coin")
        {
            Destroy(other.gameObject);
            AddCoin();
        }
    }
}

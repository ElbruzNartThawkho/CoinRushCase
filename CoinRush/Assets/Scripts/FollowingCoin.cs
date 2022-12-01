using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingCoin : MonoBehaviour
{
    public GameObject frontCoin;
    public bool disper;
    public bool? bounce;
    bool finish;
    Vector3 bounceScale,originalScale;
    private void Awake()
    {
        originalScale = transform.localScale;
        bounceScale = transform.localScale * 1.3f;
        bounce = false;
        finish = true;
        disper = false;
    }

    void Update()
    {
        if (disper == false)
        {
            if (Vector3.Distance(frontCoin.transform.position, transform.position) > 2 && GameManager.instance.gameState == GameManager.GameState.inGame)
            {
                transform.LookAt(frontCoin.transform.position);
                transform.position += (frontCoin.transform.position - transform.position) * 10 * Time.deltaTime;
            }
            else if (GameManager.instance.gameState == GameManager.GameState.goTower)
            {
                transform.eulerAngles = Vector3.zero;
                transform.position += (frontCoin.transform.position + (Vector3.up * 2) - transform.position) * 10 * Time.deltaTime;
            }
            else if (GameManager.instance.gameState == GameManager.GameState.finish && finish)
            {
                transform.position += transform.forward * 10 * Time.deltaTime;
            }
        }
        if (bounce==true)
        {
            transform.localScale = Vector3.Slerp(transform.localScale, bounceScale, 5*Time.deltaTime);
            if (transform.localScale.x > bounceScale.x*0.99f)
            {
                bounce = null;
            }
        }
        else if (bounce == null)
        {
            transform.localScale = Vector3.Slerp(transform.localScale, originalScale, 5 * Time.deltaTime);
            if(transform.localScale.x < originalScale.x * 1.1f)
            {
                transform.localScale = originalScale;
                bounce = false;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ladder")
        {
            finish = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectCoin : MonoBehaviour
{
    [SerializeField]
    GameManager gm;

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")){
            CoinCollect();
            print("Coin collected");
            gm.coinsCollected++;
        }
    }

    private void CoinCollect()
    {
        Destroy(gameObject);
    }
}

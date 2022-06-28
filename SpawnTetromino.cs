using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTetromino : MonoBehaviour
{
    public GameObject[] Tetrominoes;  //array to hold all terominoes

    // Start is called before the first frame update
    void Start()
    {
        NewTetromino(); //also spawn a new one at the start
    }

    // Update is called once per frame
 
        public void NewTetromino()
        {

            //this will instantiate a random prefab at the spawner
            Instantiate(Tetrominoes[Random.Range(0, Tetrominoes.Length)], transform.position, Quaternion.identity);
        }

}

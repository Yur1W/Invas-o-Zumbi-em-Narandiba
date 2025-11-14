using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    public int hp = 3;
    public int damage = 1;

    enum ZombieState { Idle, Moving, Attacking, Dead }
    ZombieState zombieState = ZombieState.Idle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

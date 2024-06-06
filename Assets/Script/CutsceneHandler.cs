using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneHandler : MonoBehaviour
{
    public MonsterScript monster;
    public MonsterScript key;
    public GameObject parede;
    public StackChildren stack;
    public GameObject door;

    private Vector3 originalPosition;

    public void Start(){
        originalPosition = door.transform.position;
    }
 
    public void StartKilling(){
            monster.ElevateBy(5);
            monster.startKilling(0.1f,1.0f);
            key.startKilling(1,0.1f);
            parede.transform.position= new Vector3(-30,167,69);
    }

    public void ResetEverything(){
        monster.ResetPosition();
        key.ResetPosition();
        stack.Reset();
        door.transform.position=   originalPosition;
        parede.transform.position= new Vector3(-9,167,69);
        monster.followSpeed=9;
    }

    public void ChangeStairs(){
        stack.ChangeStairs();
        door.transform.position=  originalPosition + new Vector3(0,-5,0);
        monster.transform.position=new Vector3(167,210,-27.25f);
        monster.followSpeed/=2;

    }


}

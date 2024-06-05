using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneHandler : MonoBehaviour
{
    public MonsterScript monster;
    public MonsterScript key;
    public GameObject parede;
    public StackChildren stack;

    public void StartKilling(){
            monster.ElevateBy(5);
            monster.startKilling(0.1f,1.0f);
            key.startKilling(1,0.1f);
            parede.transform.position= new Vector3(-30,167,69);
    }

    public void ResetEverything(){
        monster.ResetPosition();
        key.ResetPosition();
        parede.transform.position= new Vector3(-9,167,69);
    }

    public void ChangeStairs(){
        stack.ChangeStairs();
        monster.transform.position=new Vector3(167,210,-27.25f);
    }


}

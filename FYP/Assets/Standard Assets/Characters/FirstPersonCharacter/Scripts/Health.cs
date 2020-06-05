using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public GameObject parent;
   public float Healthpoints{
   get{
       return healthpoints;
   }
   set{
       healthpoints=value;
       if(healthpoints<=0){
           Destroy(parent);
       }
   }
   }
  [SerializeField]
public float healthpoints=100f;
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
   public float timeForExecution;

   private void Start()
   {
      StartCoroutine(Suicide());
   }

   private IEnumerator Suicide()
   {
      yield return new WaitForSeconds(timeForExecution);
      Destroy(gameObject);
   }
}

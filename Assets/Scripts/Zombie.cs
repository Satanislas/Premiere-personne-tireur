using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
   public ZombieHand hand;
   public int Damage;

   private void Start()
   {
      hand.Damage = Damage;
   }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorController : MonoBehaviour
{
  public GameObject NextItem;
   // public GameObject NewMirror;
    public GameObject Panel;

    /*    public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
              
                NewMirror.SetActive(true);
                Panel.SetActive(true);

                gameObject.SetActive(false);

                print("Player");
            }


        }*/

    public int CanBeActivateAfter;
    public void OnCollisionEnter(Collision other)
    {
        //if (CanBeActivateAfter == LayerCheck.allMirrorsOnTop) 
        {
            if (other.gameObject.tag == "Player")
            {
                Panel.SetActive(true);
                NextItem.SetActive(true);
                gameObject.SetActive(false);

            }
        }
       

    }
}

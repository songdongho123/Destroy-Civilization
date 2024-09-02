using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



    public class Test2 : MonoBehaviourPunCallbacks
    {
       
        private GameObject player;
        private MoveController moveController ;

        private CharacterController characterController;

        void Awake(){

        
        player = GameObject.FindGameObjectWithTag("Player");

        moveController = player.GetComponent<MoveController>(); 
        characterController = player.GetComponent<CharacterController>();

        }
        void Update()
        {
             
            // if (Input.GetMouseButtonDown(0))
            // {
            //    ThrowObject();
            //    Debug.Log(this.transform.position);
            // }

            if (Input.GetKeyDown(KeyCode.C))
            {
               ThrowObject2();
            }
           else if (Input.GetKeyDown(KeyCode.V))
            {
               ThrowObject();
            }

            
            else if (Input.GetKeyDown(KeyCode.N))    
            {
               moveController.enabled = false;
            //    Debug.Log($"IsGrounded: {characterController.isGrounded}, Position: {transform.position}");

            transform.position = new Vector3(14, 10, 0);
            Debug.Log("zzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzz");
               moveController.enabled = true;
            //    Debug.Log(moveController.enabled);
            //    Debug.Log("ttqtqqtwwwwwwwwww " + characterController.gameObject.transform);
               
               
            }

            else if (Input.GetKeyDown(KeyCode.M))
            {
               
               Debug.Log($"IsGrounded: {characterController.isGrounded}, Position: {transform.position}");
               moveController.enabled = false;
           
               moveController.enabled = true;
           
            //
               
               
            } 

            else if(Input.GetKeyDown(KeyCode.B))
            {
                
                transform.position = new Vector3(14, 10, 0);
            }


            
        }
        // public void ag(){
        //     Debug.Log($"IsGrounded: {characterController.isGrounded}, Position: {transform.position}");
        //        moveController.enabled=!moveController.enabled;
        //        Debug.Log($"IsGrounded: {characterController.isGrounded}, Position: {transform.position}");
        //        Debug.Log(moveController.enabled);
        //        Debug.Log("ttqtqqtwwwwwwwwww " + characterController.gameObject.transform);
        //        transform.position = new Vector3(14, 10, 0);
        // }
        public void ThrowObject()
        {
           
            transform.position = new Vector3(14, 10, 0);
            
        }

        [PunRPC]
        public void test()
        {
           

            transform.position = new Vector3(14, 10, 0);

           
        }


       
        
        public void ThrowObject2()
        {
           
            StartCoroutine(CountdownCoroutine2(2));
            
        }



        IEnumerator CountdownCoroutine2(int count)
        {
            
            for (int i = count; i > 0; i--)
            {
                Debug.Log("Countdown: " + i);
                yield return new WaitForSeconds(0.01f); // 1초 대기
            }
        
 
            moveController.SetPosition(new Vector3(14, 10, 0));
            
    }




    }


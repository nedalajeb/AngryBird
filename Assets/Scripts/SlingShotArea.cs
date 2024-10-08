using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;




public class SlingShotArea : MonoBehaviour
{
  

    [SerializeField] private LayerMask _slingshotAreaMask;


    public bool IsWithinSlingshotArea()
    {

        if (Camera.main == null)
        {
            Debug.LogError("Main camera not found.");
            return false;
        }


        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(InputManager.MousePosition);
        
        if (Physics2D.OverlapPoint(worldPosition, _slingshotAreaMask))



        {
         return true;

        }
            
        else
        { 
          
            return false;

           
        }
   



    }


}


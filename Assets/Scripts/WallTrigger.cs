using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallTrigger : MonoBehaviour
{
    private bool _triggerCooldownOn;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(_triggerCooldownOn) return;
        
        if (other.gameObject.CompareTag("Player"))
        {
            // Debug.Log("osuttiin seinään");
            var characterController = other.transform.GetComponent<CharacterController>();
            characterController.FlipHorizontalMove();
            _triggerCooldownOn = true;
            Invoke(nameof(ResetCooldown), .1f);
        }
    }

    private void ResetCooldown()
    {
        _triggerCooldownOn = false;
    }
}

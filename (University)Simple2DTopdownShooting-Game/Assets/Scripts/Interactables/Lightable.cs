using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LittleLight.Interactables
{
    public class Lightable : LittleLight.Core.Interactable
    {
        Animator _lightSystem;
        private void Start() {
            _lightSystem = GetComponent<Animator>();
        }

        public override void InteractAction()
        {
            base.InteractAction();
            _lightSystem.SetBool("isLighting", true);
        }
    }
}

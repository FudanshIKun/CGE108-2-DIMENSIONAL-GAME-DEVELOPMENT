using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LittleLight.Core
{
    [RequireComponent(typeof(Collider2D))]
    public class Interactable : MonoBehaviour
    {
        [SerializeField] Collider2D InteractCollider;
       public enum InteractionType
        {
            OnAreaInteract,
            OnClickInteract,
            OnClickInteractOneTimes
        }
        [SerializeField] InteractionType interactionType;

        public enum InteractState
        {
            HasNotInteracted,
            HasInteracted
        }

        [HideInInspector] public InteractState interactState = InteractState.HasNotInteracted;

        public virtual void InteractAction()
        {
            interactState = InteractState.HasInteracted;
        }

        public virtual void OnTriggerEnter2D(Collider2D other) 
        {
            if (interactionType == InteractionType.OnClickInteract) { return; }
        }

        public void ReportInteraction(string interacter){
            Debug.Log(interacter + " has make interaction with " + gameObject.name);
        }

        public bool HasInteractedOneTimes()
        {
            return interactState == InteractState.HasInteracted &&
                   interactionType == InteractionType.OnClickInteractOneTimes
                ? true
                : false;
        }

    }
}

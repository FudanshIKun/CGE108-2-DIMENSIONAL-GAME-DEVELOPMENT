using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LittleLight.GamePlay
{
    public class CameraBehavior : MonoBehaviour
    {
        [SerializeField] bool SetPositionToPlayer;

        public Transform target;
        Vector3 targetStartPos;
        public float lerpSpeed = 1.0f;

        private Vector3 offset;

        private Vector3 targetPos;

        void Awake() {
            targetStartPos = new Vector3(target.position.x, target.position.y, transform.position.z);
        }

        private void Start()
        {
            if (target == null) return;

            if (SetPositionToPlayer){transform.position = targetStartPos;}

            offset = transform.position - target.position;
        }

        private void Update()
        {
            if (target == null) return;

            if (SetPositionToPlayer){}

            targetPos = target.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetPos, lerpSpeed * Time.deltaTime);
        }
    }
}

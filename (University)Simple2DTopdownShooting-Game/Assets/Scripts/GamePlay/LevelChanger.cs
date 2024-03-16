using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LittleLight.GamePlay;

namespace LittleLight.Core
{
    public class LevelChanger : MonoBehaviour
    {
        #region Inspector Display
        public enum sortLevel{Level_0, Level_1, Level_2}
        public enum sortLayer{Layer_0, Layer_1, Layer_2}
        [SerializeField] sortLevel TargetLevel;
        [SerializeField] sortLayer TargetLayer;
        #endregion
        string sortingLayer;

        private void Start() {
            sortingLayer = TargetLevel.ToString();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Ammo")) { return; }
            Debug.Log(other.gameObject.name + " has enter " + sortingLayer);
            if (other.gameObject.CompareTag("Player"))
            {
                other.gameObject.GetComponent<MainPlayer>().InteractableLayer = LayerMask.NameToLayer(TargetLayer.ToString());
            }

            other.gameObject.layer = LayerMask.NameToLayer(TargetLayer.ToString());
            other.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = sortingLayer;
            SpriteRenderer[] srs = other.gameObject.GetComponentsInChildren<SpriteRenderer>();
            foreach ( SpriteRenderer sr in srs)
            {
                sr.sortingLayerName = sortingLayer;
            }
        }
    }
}

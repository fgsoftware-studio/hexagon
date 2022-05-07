using FMOD;
using UnityEngine;

namespace FMODUnity
{
    [AddComponentMenu("FMOD Studio/FMOD Studio Listener")]
    public class StudioListener : MonoBehaviour
    {
        public GameObject attenuationObject;

        public int ListenerNumber = -1;
        private Rigidbody rigidBody;
        private Rigidbody2D rigidBody2D;

        private void Update()
        {
            if (ListenerNumber >= 0 && ListenerNumber < CONSTANTS.MAX_LISTENERS) SetListenerLocation();
        }

        private void OnEnable()
        {
            RuntimeUtils.EnforceLibraryOrder();
            rigidBody = gameObject.GetComponent<Rigidbody>();
            rigidBody2D = gameObject.GetComponent<Rigidbody2D>();
            ListenerNumber = RuntimeManager.AddListener(this);
        }

        private void OnDisable()
        {
            RuntimeManager.RemoveListener(this);
        }

        private void SetListenerLocation()
        {
            if (rigidBody)
                RuntimeManager.SetListenerLocation(ListenerNumber, gameObject, rigidBody, attenuationObject);
            else
                RuntimeManager.SetListenerLocation(ListenerNumber, gameObject, rigidBody2D, attenuationObject);
        }
    }
}
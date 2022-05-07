using UnityEngine;

namespace FMODUnity
{
    public abstract class EventHandler : MonoBehaviour
    {
        public string CollisionTag = "";

        private void OnEnable()
        {
            HandleGameEvent(EmitterGameEvent.ObjectEnable);
        }

        private void OnDisable()
        {
            HandleGameEvent(EmitterGameEvent.ObjectDisable);
        }

        private void OnCollisionEnter()
        {
            HandleGameEvent(EmitterGameEvent.CollisionEnter);
        }

        private void OnCollisionEnter2D()
        {
            HandleGameEvent(EmitterGameEvent.CollisionEnter2D);
        }

        private void OnCollisionExit()
        {
            HandleGameEvent(EmitterGameEvent.CollisionExit);
        }

        private void OnCollisionExit2D()
        {
            HandleGameEvent(EmitterGameEvent.CollisionExit2D);
        }

        private void OnMouseDown()
        {
            HandleGameEvent(EmitterGameEvent.MouseDown);
        }

        private void OnMouseEnter()
        {
            HandleGameEvent(EmitterGameEvent.MouseEnter);
        }

        private void OnMouseExit()
        {
            HandleGameEvent(EmitterGameEvent.MouseExit);
        }

        private void OnMouseUp()
        {
            HandleGameEvent(EmitterGameEvent.MouseUp);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (string.IsNullOrEmpty(CollisionTag) || other.CompareTag(CollisionTag) ||
                (other.attachedRigidbody && other.attachedRigidbody.CompareTag(CollisionTag)))
                HandleGameEvent(EmitterGameEvent.TriggerEnter);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (string.IsNullOrEmpty(CollisionTag) || other.CompareTag(CollisionTag))
                HandleGameEvent(EmitterGameEvent.TriggerEnter2D);
        }

        private void OnTriggerExit(Collider other)
        {
            if (string.IsNullOrEmpty(CollisionTag) || other.CompareTag(CollisionTag) ||
                (other.attachedRigidbody && other.attachedRigidbody.CompareTag(CollisionTag)))
                HandleGameEvent(EmitterGameEvent.TriggerExit);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (string.IsNullOrEmpty(CollisionTag) || other.CompareTag(CollisionTag))
                HandleGameEvent(EmitterGameEvent.TriggerExit2D);
        }

        protected abstract void HandleGameEvent(EmitterGameEvent gameEvent);
    }
}
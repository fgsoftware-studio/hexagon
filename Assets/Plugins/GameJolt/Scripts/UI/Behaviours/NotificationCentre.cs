using System.Collections.Generic;
using GameJolt.UI.Controllers;
using GameJolt.UI.Objects;
using UnityEngine;

namespace GameJolt.UI.Behaviours
{
    public class NotificationCentre : StateMachineBehaviour
    {
        public string NotificationPanelPath;

        private NotificationItem notificationItem;
        private Queue<Notification> notificationsQueue;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
        {
            if (notificationItem == null)
            {
                var panelTransform = animator.transform.Find(NotificationPanelPath);
                if (panelTransform != null) notificationItem = panelTransform.GetComponent<NotificationItem>();
            }

            if (notificationsQueue == null) notificationsQueue = new Queue<Notification>();
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
        {
            if (notificationsQueue.Count > 0)
            {
                var notification = notificationsQueue.Dequeue();
                notificationItem.Init(notification);
                animator.SetTrigger("Notification");
            }
        }

        public void QueueNotification(Notification notification)
        {
            notificationsQueue.Enqueue(notification);
        }
    }
}
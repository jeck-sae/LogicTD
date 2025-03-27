using UnityEngine;
using System.Collections;


namespace TowerDefense
{
    public class AnimKeyframeMethods : MonoBehaviour
    {
        void SendMessageUp(string message)
        {
            gameObject.SendMessageUpwards(message, SendMessageOptions.DontRequireReceiver);
        }
    
        void Destroy()
        {
            Destroy(transform.parent.gameObject);
        }
    }
    
}

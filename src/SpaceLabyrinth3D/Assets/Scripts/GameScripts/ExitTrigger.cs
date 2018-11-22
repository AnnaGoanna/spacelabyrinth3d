using UnityEngine;

namespace GameScripts
{
    public class ExitTrigger : MonoBehaviour
    {
        public Game Game { private get; set; }
        public GameObject Player { private get; set; }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == Player)
            {
                Game.AllowExit = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject == Player)
            {
                Game.AllowExit = false;
            }
        }
    }
}

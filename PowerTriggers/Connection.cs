using UnityEngine;

namespace PowerTriggers
{
    [KSPAddon(KSPAddon.Startup.MainMenu, true)]
    public class Connection : MonoBehaviour
    {
        public Connection()
        {
        }

        public void FixedUpdate()
        {
            // this is called many times, at regular intervals.
        }


        public void Update()
        {
            // updates that don't affect models.
        }
    }
}

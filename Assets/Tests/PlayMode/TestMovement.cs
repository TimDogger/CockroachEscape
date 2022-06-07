using System.Collections;
using CockroachCore;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayMode
{
    public class TestMovement
    {
        [UnityTest]
        public IEnumerator TestCockroachMovement()
        {
            Controller controller = GameObject.Instantiate(Resources.Load<Controller>("Prefabs/Controller"));
            
            GameObject startWaypoint = new GameObject("Start");
            startWaypoint.transform.position = new Vector3(8.4f, -4.57f, 0);
        
            GameObject stopWaypoint = new GameObject("End")
            {
                transform =
                {
                    position = new Vector3(-8.4f, 4.57f, 0)
                }
            };

            controller.startWaypoint = startWaypoint;
            controller.stopWaypoint = stopWaypoint;
            controller.enabled = true;
            
            yield return new WaitForSeconds(1f);
        
            float startDistance = Vector3.Distance(controller.Cockroach.transform.position, stopWaypoint.transform.position);
        
            yield return new WaitForSeconds(2f);
        
            float currentDistance =  Vector3.Distance(controller.Cockroach.transform.position, stopWaypoint.transform.position);
        
            Assert.Less(currentDistance, startDistance);
        
            Object.Destroy(controller);
            Object.Destroy(startWaypoint);
            Object.Destroy(stopWaypoint);
        }
    }
}

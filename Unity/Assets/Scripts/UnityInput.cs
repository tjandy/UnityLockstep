﻿using System.Linq;
using FixMath.NET;
using Lockstep.Commands;
using UnityEngine;                      

public class UnityInput : MonoBehaviour
{                              
    public static BEPUutilities.Vector2 GetWorldPos(Vector2 screenPos)
    {
        var ray = Camera.main.ScreenPointToRay(screenPos);
        if (Physics.Raycast(ray, out var hit))
        {
            return new BEPUutilities.Vector2((Fix64)hit.point.x, (Fix64)hit.point.z);
        }
        var hitPoint = ray.origin - ray.direction * (ray.origin.y / ray.direction.y);
        return new BEPUutilities.Vector2((Fix64)hitPoint.x, (Fix64)hitPoint.z);
    }

    void Update()
    {
        if (Input.GetMouseButton(1))
        {   
            var pos = GetWorldPos(Input.mousePosition);
            FindObjectOfType<RTSEntitySpawner>().Spawn(pos);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {                 
            RTSNetworkedSimulation.Instance.Execute(new NavigateCommand
            {
                Destination = GetWorldPos(Input.mousePosition),
                EntityIds = Contexts.sharedInstance.game.GetEntities().Select(entity => entity.id.value).ToArray()
            });
        }
    }
}

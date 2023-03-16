using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{

    public abstract class OverallGameTree : Tree
    {
        protected override Node SetupTree()
        {
            Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                //new CheckEnemyInFOVRange(transform),
                //new TaskGoToTarget(transform),
            }),
            //new TaskPatrol(transform, waypoints),
        });

            return root;
        }

    }

}
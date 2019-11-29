using System.Collections.Generic;
using GoogleARCore;
using GoogleARCore.Examples.Common;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BrushGestures
{
    public class BrushPowers
    {
        public void PowerVisualizer(string power)
        {
            switch (power)
            {
                case "thunder":
                    return;
                case "rain":
                    return;
                case "fire":
                    return;
                case "bullettime":
                    return;
                case "blizzard":
                    return;
            }
        }
    }
}
//#define OOTII_DEBUG

using System.Collections.Generic;
using UnityEngine;
using com.ootii.Collections;

namespace com.ootii.Geometry
{
    /// <summary>
    /// Provides functions for specialized raycast solutions
    /// </summary>
    public static class RaycastExt
    {
        /// <summary>
        /// Used when we need to return an empty raycast
        /// </summary>
        public static RaycastHit EmptyHitInfo = new RaycastHit();

        /// <summary>
        /// This function will help to find a forward edge. 
        /// </summary>
        /// <param name="rTransform"></param>
        /// <param name="rMaxDistance"></param>
        /// <param name="rMaxHeight"></param>
        /// <param name="rCollisionLayers"></param>
        /// <param name="rEdgeHitInfo"></param>
        /// <returns></returns>
        public static bool GetForwardEdge(Transform rTransform, float rMaxDistance, float rMaxHeight, int rCollisionLayers, out RaycastHit rEdgeHitInfo)
        {
            rEdgeHitInfo = RaycastExt.EmptyHitInfo;

            // Shoot above the expected height to make sure that it's open. We don't want to hit anything
            Vector3 lRayStart = rTransform.position + (rTransform.up * (rMaxHeight + 0.001f));
            Vector3 lRayDirection = rTransform.forward;
            float lRayDistance = rMaxDistance * 1.5f;

            if (SafeRaycast(lRayStart, lRayDirection, lRayDistance, rCollisionLayers, rTransform, out rEdgeHitInfo))
            {
                return false;
            }

            // Shoot down to see if we hit a ledge. We want to hit the top of the ledge.
            lRayStart = lRayStart + (rTransform.forward * rMaxDistance);
            lRayDirection = -rTransform.up;
            lRayDistance = rMaxHeight;

            if (!SafeRaycast(lRayStart, lRayDirection, lRayDistance, rCollisionLayers, rTransform, out rEdgeHitInfo))
            {
                return false;
            }

            // This is the height of our edge
            float lEdgeHeight = (rMaxHeight + 0.001f) - rEdgeHitInfo.distance;

            // Shoot a ray forward to find the actual edge. We want to hit the front of the ledge.
            lRayStart = rTransform.position + (rTransform.up * (lEdgeHeight - 0.001f));
            lRayDirection = rTransform.forward;
            lRayDistance = rMaxDistance;

            if (!SafeRaycast(lRayStart, lRayDirection, lRayDistance, rCollisionLayers, rTransform, out rEdgeHitInfo))
            {
                return false;
            }

#if OOTII_DEBUG
            Utilities.Debug.DebugDraw.DrawSphereMesh(rEdgeHitInfo.point, 0.02f, Color.red, 1f);
#endif

            // If we get here, there was a valid hit
            return true;
        }
        
        /// <summary>
        /// When casting a ray from the motion controller, we don't want it to collide with
        /// ourselves. The problem is that we may want to collide with another avatar. So, we
        /// can't put every avatar on their own layer. This ray cast will take a little longer,
        /// but will ignore this avatar.
        /// 
        /// Note: This function isn't virutal to eek out ever ounce of performance we can.
        /// </summary>
        /// <param name="rRayStart"></param>
        /// <param name="rRayDirection"></param>
        /// <param name="rHitInfo"></param>
        /// <param name="rDistance"></param>
        /// <returns></returns>
        public static bool SafeRaycast(Vector3 rRayStart, Vector3 rRayDirection, float rDistance)
        {
            int lHitCount = 0;
            float lDistanceOffset = 0f;

            // Since some objects (like triggers) are invalid for collisions, we want
            // to go through them. That means we may continue a ray cast even if a hit occured
            while (lHitCount < 5 && rDistance > 0f)
            {
                // Assume the next hit to be valid
                bool lIsValidHit = true;

                // Test from the current start
                RaycastHit lHitInfo;
                bool lHit = UnityEngine.Physics.Raycast(rRayStart, rRayDirection, out lHitInfo, rDistance);

                // Easy, just return no hit
                if (!lHit) { return false; }

                // If we hit a trigger, we'll continue testing just a tad bit beyond.
                if (lHitInfo.collider.isTrigger) { lIsValidHit = false; }

                // If we have an invalid hit, we'll continue testing by using the hit point
                // (plus a little extra) as the new start
                if (!lIsValidHit)
                {
                    lDistanceOffset += lHitInfo.distance + 0.05f;
                    rRayStart = lHitInfo.point + (rRayDirection * 0.05f);

                    rDistance -= (lHitInfo.distance + 0.05f);

                    lHitCount++;
                    continue;
                }

                // If we got here, we must have a valid hit. Update
                // the distance info incase we had to cycle through invalid hits
                lHitInfo.distance += lDistanceOffset;

                return true;
            }

            // If we got here, we exceeded our attempts and we should drop out
            return false;
        }

        /// <summary>
        /// When casting a ray from the motion controller, we don't want it to collide with
        /// ourselves. The problem is that we may want to collide with another avatar. So, we
        /// can't put every avatar on their own layer. This ray cast will take a little longer,
        /// but will ignore this avatar.
        /// 
        /// Note: This function isn't virutal to eek out ever ounce of performance we can.
        /// </summary>
        /// <param name="rRayStart"></param>
        /// <param name="rRayDirection"></param>
        /// <param name="rHitInfo"></param>
        /// <param name="rDistance"></param>
        /// <returns></returns>
        public static bool SafeRaycast(Vector3 rRayStart, Vector3 rRayDirection, float rDistance, out RaycastHit rHitInfo)
        {
            int lHitCount = 0;
            float lDistanceOffset = 0f;

            // Since some objects (like triggers) are invalid for collisions, we want
            // to go through them. That means we may continue a ray cast even if a hit occured
            while (lHitCount < 5 && rDistance > 0f)
            {
                // Assume the next hit to be valid
                bool lIsValidHit = true;

                // Test from the current start
                bool lHit = UnityEngine.Physics.Raycast(rRayStart, rRayDirection, out rHitInfo, rDistance);

                // Easy, just return no hit
                if (!lHit) { return false; }

                // If we hit a trigger, we'll continue testing just a tad bit beyond.
                if (rHitInfo.collider.isTrigger) { lIsValidHit = false; }

                // If we have an invalid hit, we'll continue testing by using the hit point
                // (plus a little extra) as the new start
                if (!lIsValidHit)
                {
                    lDistanceOffset += rHitInfo.distance + 0.05f;
                    rRayStart = rHitInfo.point + (rRayDirection * 0.05f);

                    rDistance -= (rHitInfo.distance + 0.05f);

                    lHitCount++;
                    continue;
                }

                // If we got here, we must have a valid hit. Update
                // the distance info incase we had to cycle through invalid hits
                rHitInfo.distance += lDistanceOffset;

                return true;
            }

            // We have to assign the out parameter
            rHitInfo = new RaycastHit();

            // If we got here, we exceeded our attempts and we should drop out
            return false;
        }

        /// <summary>
        /// When casting a ray from the motion controller, we don't want it to collide with
        /// ourselves. The problem is that we may want to collide with another avatar. So, we
        /// can't put every avatar on their own layer. This ray cast will take a little longer,
        /// but will ignore this avatar.
        /// 
        /// Note: This function isn't virutal to eek out ever ounce of performance we can.
        /// </summary>
        /// <param name="rRayStart"></param>
        /// <param name="rRayDirection"></param>
        /// <param name="rHitInfo"></param>
        /// <param name="rDistance"></param>
        /// <returns></returns>
        public static bool SafeRaycast(Vector3 rRayStart, Vector3 rRayDirection, float rDistance, int rLayerMask, out RaycastHit rHitInfo)
        {
            int lHitCount = 0;
            float lDistanceOffset = 0f;

            // Since some objects (like triggers) are invalid for collisions, we want
            // to go through them. That means we may continue a ray cast even if a hit occured
            while (lHitCount < 5 && rDistance > 0f)
            {
                // Assume the next hit to be valid
                bool lIsValidHit = true;

                // Test from the current start
                bool lHit = UnityEngine.Physics.Raycast(rRayStart, rRayDirection, out rHitInfo, rDistance, rLayerMask);

                // Easy, just return no hit
                if (!lHit) { return false; }

                // If we hit a trigger, we'll continue testing just a tad bit beyond.
                if (rHitInfo.collider.isTrigger) { lIsValidHit = false; }

                // If we have an invalid hit, we'll continue testing by using the hit point
                // (plus a little extra) as the new start
                if (!lIsValidHit)
                {
                    lDistanceOffset += rHitInfo.distance + 0.05f;
                    rRayStart = rHitInfo.point + (rRayDirection * 0.05f);

                    rDistance -= (rHitInfo.distance + 0.05f);

                    lHitCount++;
                    continue;
                }

                // If we got here, we must have a valid hit. Update
                // the distance info incase we had to cycle through invalid hits
                rHitInfo.distance += lDistanceOffset;

                return true;
            }

            // We have to assign the out parameter
            rHitInfo = new RaycastHit();

            // If we got here, we exceeded our attempts and we should drop out
            return false;
        }

        /// <summary>
        /// When casting a ray from the motion controller, we don't want it to collide with
        /// ourselves. The problem is that we may want to collide with another avatar. So, we
        /// can't put every avatar on their own layer. This ray cast will take a little longer,
        /// but will ignore this avatar.
        /// 
        /// Note: This function isn't virutal to eek out ever ounce of performance we can.
        /// </summary>
        /// <param name="rRayStart"></param>
        /// <param name="rRayDirection"></param>
        /// <param name="rHitInfo"></param>
        /// <param name="rDistance"></param>
        /// <param name="rIgnore">List of transforms we should ignore collisions with</param>
        /// <returns></returns>
        public static bool SafeRaycast(Vector3 rRayStart, Vector3 rRayDirection, float rDistance, Transform rIgnore)
        {
            int lHitCount = 0;
            float lDistanceOffset = 0f;
            Vector3 lRayStart = rRayStart;

            // Since some objects (like triggers) are invalid for collisions, we want
            // to go through them. That means we may continue a ray cast even if a hit occured
            while (lHitCount < 5 && rDistance > 0f)
            {
                // Assume the next hit to be valid
                bool lIsValidHit = true;

                // Test from the current start
                RaycastHit lHitInfo;
                bool lHit = UnityEngine.Physics.Raycast(lRayStart, rRayDirection, out lHitInfo, rDistance);

                // Easy, just return no hit
                if (!lHit) { return false; }

                // If we hit a trigger, we'll continue testing just a tad bit beyond.
                if (lHitInfo.collider.isTrigger) { lIsValidHit = false; }
                if (rIgnore != null && rIgnore == lHitInfo.collider.transform) { lIsValidHit = false; }

                // If we have an invalid hit, we'll continue testing by using the hit point
                // (plus a little extra) as the new start
                if (!lIsValidHit)
                {
                    lDistanceOffset += lHitInfo.distance + 0.05f;
                    lRayStart = lHitInfo.point + (rRayDirection * 0.05f);

                    rDistance -= (lHitInfo.distance + 0.05f);

                    lHitCount++;
                    continue;
                }

                // If we got here, we must have a valid hit. Update
                // the distance info incase we had to cycle through invalid hits
                lHitInfo.distance += lDistanceOffset;
                lHitInfo.distance = Vector3.Distance(rRayStart, lHitInfo.point);

                return true;
            }

            // If we got here, we exceeded our attempts and we should drop out
            return false;
        }

        /// <summary>
        /// When casting a ray from the motion controller, we don't want it to collide with
        /// ourselves. The problem is that we may want to collide with another avatar. So, we
        /// can't put every avatar on their own layer. This ray cast will take a little longer,
        /// but will ignore this avatar.
        /// 
        /// Note: This function isn't virutal to eek out ever ounce of performance we can.
        /// </summary>
        /// <param name="rRayStart"></param>
        /// <param name="rRayDirection"></param>
        /// <param name="rHitInfo"></param>
        /// <param name="rDistance"></param>
        /// <param name="rIgnore">List of transforms we should ignore collisions with</param>
        /// <returns></returns>
        public static bool SafeRaycast(Vector3 rRayStart, Vector3 rRayDirection, float rDistance, Transform rIgnore, out RaycastHit rHitInfo)
        {
            int lHitCount = 0;
            float lDistanceOffset = 0f;

            // Since some objects (like triggers) are invalid for collisions, we want
            // to go through them. That means we may continue a ray cast even if a hit occured
            while (lHitCount < 5 && rDistance > 0f)
            {
                // Assume the next hit to be valid
                bool lIsValidHit = true;

                // Test from the current start
                bool lHit = UnityEngine.Physics.Raycast(rRayStart, rRayDirection, out rHitInfo, rDistance);

#if OOTII_DEBUG
                Color lColor = (lHit ? Color.red : Color.green);
                Debug.DrawLine(rRayStart, rRayStart + (rRayDirection * rDistance), lColor);
#endif


                // Easy, just return no hit
                if (!lHit) { return false; }

                // If we hit a trigger, we'll continue testing just a tad bit beyond.
                if (rHitInfo.collider.isTrigger) { lIsValidHit = false; }

                // If we hit a transform to ignore
                if (lIsValidHit && rIgnore != null)
                {
                    Transform lCurrentTransform = rHitInfo.collider.transform;
                    while (lCurrentTransform != null)
                    {
                        if (lCurrentTransform == rIgnore)
                        {
                            lIsValidHit = false;
                            break;
                        }

                        lCurrentTransform = lCurrentTransform.parent;
                    }
                }

                // If we have an invalid hit, we'll continue testing by using the hit point
                // (plus a little extra) as the new start
                if (!lIsValidHit)
                {
                    lDistanceOffset += rHitInfo.distance + 0.05f;
                    rRayStart = rHitInfo.point + (rRayDirection * 0.05f);

                    rDistance -= (rHitInfo.distance + 0.05f);

                    lHitCount++;
                    continue;
                }

                // If we got here, we must have a valid hit. Update
                // the distance info incase we had to cycle through invalid hits
                rHitInfo.distance += lDistanceOffset;

                // Valid hit. So get out.
                return true;
            }

            // We have to initialize the out param
            rHitInfo = EmptyHitInfo;

            // If we got here, we exceeded our attempts and we should drop out
            return false;
        }

        /// <summary>
        /// When casting a ray from the motion controller, we don't want it to collide with
        /// ourselves. The problem is that we may want to collide with another avatar. So, we
        /// can't put every avatar on their own layer. This ray cast will take a little longer,
        /// but will ignore this avatar.
        /// 
        /// Note: This function isn't virutal to eek out ever ounce of performance we can.
        /// </summary>
        /// <param name="rRayStart"></param>
        /// <param name="rRayDirection"></param>
        /// <param name="rHitInfo"></param>
        /// <param name="rDistance"></param>
        /// <param name="rIgnore">List of transforms we should ignore collisions with</param>
        /// <returns></returns>
        public static bool SafeRaycast(Vector3 rRayStart, Vector3 rRayDirection, float rDistance, int rLayerMask, Transform rIgnore, out RaycastHit rHitInfo)
        {
            int lHitCount = 0;
            float lDistanceOffset = 0f;

            // Since some objects (like triggers) are invalid for collisions, we want
            // to go through them. That means we may continue a ray cast even if a hit occured
            while (lHitCount < 5 && rDistance > 0f)
            {
                // Assume the next hit to be valid
                bool lIsValidHit = true;

                // Test from the current start
                bool lHit = UnityEngine.Physics.Raycast(rRayStart, rRayDirection, out rHitInfo, rDistance, rLayerMask);

                // Easy, just return no hit
                if (!lHit) { return false; }

                // If we hit a trigger, we'll continue testing just a tad bit beyond.
                if (rHitInfo.collider.isTrigger) { lIsValidHit = false; }

                // If we hit a transform to ignore
                if (lIsValidHit && rIgnore != null)
                {
                    Transform lCurrentTransform = rHitInfo.collider.transform;
                    while (lCurrentTransform != null)
                    {
                        if (lCurrentTransform == rIgnore)
                        {
                            lIsValidHit = false;
                            break;
                        }

                        lCurrentTransform = lCurrentTransform.parent;
                    }
                }

                // If we have an invalid hit, we'll continue testing by using the hit point
                // (plus a little extra) as the new start
                if (!lIsValidHit)
                {
                    lDistanceOffset += rHitInfo.distance + 0.05f;
                    rRayStart = rHitInfo.point + (rRayDirection * 0.05f);

                    rDistance -= (rHitInfo.distance + 0.05f);

                    lHitCount++;
                    continue;
                }

                // If we got here, we must have a valid hit. Update
                // the distance info incase we had to cycle through invalid hits
                rHitInfo.distance += lDistanceOffset;

                // Valid hit. So get out.
                return true;
            }

            // We have to initialize the out param
            rHitInfo = EmptyHitInfo;

            // If we got here, we exceeded our attempts and we should drop out
            return false;
        }

        /// <summary>
        /// When casting a ray from the motion controller, we don't want it to collide with
        /// ourselves. The problem is that we may want to collide with another avatar. So, we
        /// can't put every avatar on their own layer. This ray cast will take a little longer,
        /// but will ignore this avatar.
        /// 
        /// Note: This function isn't virutal to eek out ever ounce of performance we can.
        /// </summary>
        /// <param name="rRayStart"></param>
        /// <param name="rRayDirection"></param>
        /// <param name="rHitInfo"></param>
        /// <param name="rDistance"></param>
        /// <param name="rIgnore">List of transforms we should ignore collisions with</param>
        /// <returns></returns>
        public static bool SafeRaycast(Vector3 rRayStart, Vector3 rRayDirection, float rDistance, List<Transform> rIgnore, out RaycastHit rHitInfo)
        {
            int lHitCount = 0;
            float lDistanceOffset = 0f;
            Vector3 lRayStart = rRayStart;

            // Since some objects (like triggers) are invalid for collisions, we want
            // to go through them. That means we may continue a ray cast even if a hit occured
            while (lHitCount < 5 && rDistance > 0f)
            {
                // Assume the next hit to be valid
                bool lIsValidHit = true;

                // Test from the current start
                bool lHit = UnityEngine.Physics.Raycast(lRayStart, rRayDirection, out rHitInfo, rDistance);

                // Easy, just return no hit
                if (!lHit) { return false; }

                // If we hit a trigger, we'll continue testing just a tad bit beyond.
                if (rHitInfo.collider.isTrigger) { lIsValidHit = false; }
                if (rIgnore != null && rIgnore.Contains(rHitInfo.collider.transform)) { lIsValidHit = false; }

                // If we have an invalid hit, we'll continue testing by using the hit point
                // (plus a little extra) as the new start
                if (!lIsValidHit)
                {
                    lDistanceOffset += rHitInfo.distance + 0.05f;
                    lRayStart = rHitInfo.point + (rRayDirection * 0.05f);

                    rDistance -= (rHitInfo.distance + 0.05f);

                    lHitCount++;
                    continue;
                }

                // If we got here, we must have a valid hit. Update
                // the distance info incase we had to cycle through invalid hits
                rHitInfo.distance += lDistanceOffset;


                rHitInfo.distance = Vector3.Distance(rRayStart, rHitInfo.point);

                return true;
            }

            // We have to initialize the out param
            rHitInfo = EmptyHitInfo;

            // If we got here, we exceeded our attempts and we should drop out
            return false;
        }

        /// <summary>
        /// When casting a ray from the motion controller, we don't want it to collide with
        /// ourselves. The problem is that we may want to collide with another avatar. So, we
        /// can't put every avatar on their own layer. This ray cast will take a little longer,
        /// but will ignore this avatar.
        /// 
        /// Note: This function isn't virutal to eek out ever ounce of performance we can.
        /// </summary>
        /// <param name="rRayStart"></param>
        /// <param name="rRayDirection"></param>
        /// <param name="rHitInfo"></param>
        /// <param name="rDistance"></param>
        /// <param name="rIgnore">List of transforms we should ignore collisions with</param>
        /// <returns></returns>
        public static bool SafeRaycastRef(Vector3 rRayStart, Vector3 rRayDirection, float rDistance, List<Transform> rIgnore, ref RaycastHit rHitInfo)
        {
            int lHitCount = 0;
            float lDistanceOffset = 0f;
            Vector3 lRayStart = rRayStart;

            // Since some objects (like triggers) are invalid for collisions, we want
            // to go through them. That means we may continue a ray cast even if a hit occured
            while (lHitCount < 5 && rDistance > 0f)
            {
                // Assume the next hit to be valid
                bool lIsValidHit = true;

                // Test from the current start
                bool lHit = UnityEngine.Physics.Raycast(lRayStart, rRayDirection, out rHitInfo, rDistance);

                // Easy, just return no hit
                if (!lHit) { return false; }

                // If we hit a trigger, we'll continue testing just a tad bit beyond.
                if (rHitInfo.collider.isTrigger) { lIsValidHit = false; }
                if (rIgnore != null && rIgnore.Contains(rHitInfo.collider.transform)) { lIsValidHit = false; }

                // If we have an invalid hit, we'll continue testing by using the hit point
                // (plus a little extra) as the new start
                if (!lIsValidHit)
                {
                    lDistanceOffset += rHitInfo.distance + 0.05f;
                    lRayStart = rHitInfo.point + (rRayDirection * 0.05f);

                    rDistance -= (rHitInfo.distance + 0.05f);

                    lHitCount++;
                    continue;
                }

                // If we got here, we must have a valid hit. Update
                // the distance info incase we had to cycle through invalid hits
                rHitInfo.distance += lDistanceOffset;

                rHitInfo.distance = Vector3.Distance(rRayStart, rHitInfo.point);

                return true;
            }

            // If we got here, we exceeded our attempts and we should drop out
            return false;
        }

        /// <summary>
        /// When casting a ray from the motion controller, we don't want it to collide with
        /// ourselves. The problem is that we may want to collide with another avatar. So, we
        /// can't put every avatar on their own layer. This ray cast will take a little longer,
        /// but will ignore this avatar.
        /// 
        /// Note: This function isn't virutal to eek out ever ounce of performance we can.
        /// </summary>
        /// <param name="rRayStart"></param>
        /// <param name="rRayDirection"></param>
        /// <param name="rHitInfo"></param>
        /// <param name="rDistance"></param>
        /// <param name="rIgnore">List of transforms we should ignore collisions with</param>
        /// <returns></returns>
        public static bool SafeRaycast(Vector3 rRayStart, Vector3 rRayDirection, float rDistance, int rLayerMask, List<Transform> rIgnore, out RaycastHit rHitInfo)
        {
            int lHitCount = 0;
            float lDistanceOffset = 0f;

            // Since some objects (like triggers) are invalid for collisions, we want
            // to go through them. That means we may continue a ray cast even if a hit occured
            while (lHitCount < 5 && rDistance > 0f)
            {
                // Assume the next hit to be valid
                bool lIsValidHit = true;

                // Test from the current start
                bool lHit = UnityEngine.Physics.Raycast(rRayStart, rRayDirection, out rHitInfo, rDistance, rLayerMask);

                // Easy, just return no hit
                if (!lHit) { return false; }

                // If we hit a trigger, we'll continue testing just a tad bit beyond.
                if (rHitInfo.collider.isTrigger) { lIsValidHit = false; }
                if (rIgnore != null && rIgnore.Contains(rHitInfo.collider.transform)) { lIsValidHit = false; }

                // If we have an invalid hit, we'll continue testing by using the hit point
                // (plus a little extra) as the new start
                if (!lIsValidHit)
                {
                    lDistanceOffset += rHitInfo.distance + 0.05f;
                    rRayStart = rHitInfo.point + (rRayDirection * 0.05f);

                    rDistance -= (rHitInfo.distance + 0.05f);

                    lHitCount++;
                    continue;
                }

                // If we got here, we must have a valid hit. Update
                // the distance info incase we had to cycle through invalid hits
                rHitInfo.distance += lDistanceOffset;

                return true;
            }

            // We have to initialize the out param
            rHitInfo = EmptyHitInfo;

            // If we got here, we exceeded our attempts and we should drop out
            return false;
        }

        /// <summary>
        /// When casting a ray from the motion controller, we don't want it to collide with
        /// ourselves. The problem is that we may want to collide with another avatar. So, we
        /// can't put every avatar on their own layer. This ray cast will take a little longer,
        /// but will ignore this avatar.
        /// 
        /// Note: This function isn't virutal to eek out ever ounce of performance we can.
        /// </summary>
        /// <param name="rRayStart"></param>
        /// <param name="rRayDirection"></param>
        /// <param name="rHitInfo"></param>
        /// <param name="rDistance"></param>
        /// <param name="rIgnore">List of transforms we should ignore collisions with</param>
        /// <returns></returns>
        public static bool SafeRaycastRef(Vector3 rRayStart, Vector3 rRayDirection, float rDistance, int rLayerMask, List<Transform> rIgnore, ref RaycastHit rHitInfo)
        {
            int lHitCount = 0;
            float lDistanceOffset = 0f;

            // Since some objects (like triggers) are invalid for collisions, we want
            // to go through them. That means we may continue a ray cast even if a hit occured
            while (lHitCount < 5 && rDistance > 0f)
            {
                // Assume the next hit to be valid
                bool lIsValidHit = true;

                // Test from the current start
                bool lHit = UnityEngine.Physics.Raycast(rRayStart, rRayDirection, out rHitInfo, rDistance, rLayerMask);

                // Easy, just return no hit
                if (!lHit) { return false; }

                // If we hit a trigger, we'll continue testing just a tad bit beyond.
                if (rHitInfo.collider.isTrigger) { lIsValidHit = false; }
                if (rIgnore != null && rIgnore.Contains(rHitInfo.collider.transform)) { lIsValidHit = false; }

                // If we have an invalid hit, we'll continue testing by using the hit point
                // (plus a little extra) as the new start
                if (!lIsValidHit)
                {
                    lDistanceOffset += rHitInfo.distance + 0.05f;
                    rRayStart = rHitInfo.point + (rRayDirection * 0.05f);

                    rDistance -= (rHitInfo.distance + 0.05f);

                    lHitCount++;
                    continue;
                }

                // If we got here, we must have a valid hit. Update
                // the distance info incase we had to cycle through invalid hits
                rHitInfo.distance += lDistanceOffset;

                return true;
            }

            // If we got here, we exceeded our attempts and we should drop out
            return false;
        }

        /// <summary>
        /// When casting a ray from the motion controller, we don't want it to collide with
        /// ourselves. The problem is that we may want to collide with another avatar. So, we
        /// can't put every avatar on their own layer. This ray cast will take a little longer,
        /// but will ignore this avatar.
        /// 
        /// Note: This function isn't virutal to eek out ever ounce of performance we can.
        /// </summary>
        /// <param name="rRayStart"></param>
        /// <param name="rRayDirection"></param>
        /// <param name="rHitInfo"></param>
        /// <param name="rDistance"></param>
        /// <returns></returns>
        public static RaycastHit[] SafeRaycastAll(Vector3 rRayStart, Vector3 rRayDirection, float rDistance)
        {
            RaycastHit[] lHitArray = UnityEngine.Physics.RaycastAll(rRayStart, rRayDirection, rDistance);

            // With no hits, this is easy
            if (lHitArray.Length == 0)
            {
            }
            // With one hit, this is easy too
            else if (lHitArray.Length == 1)
            {
                if (lHitArray[0].collider.isTrigger)
                {
                    lHitArray = new RaycastHit[0];
                }
            }
            // Find the closest hit
            else
            {
                // Order the array by distance and get rid of items that don't pass
                lHitArray.Sort(delegate (RaycastHit rLeft, RaycastHit rRight) { return (rLeft.distance < rRight.distance ? -1 : (rLeft.distance > rRight.distance ? 1 : 0)); });
                for (int i = lHitArray.Length - 1; i >= 0; i--)
                {
                    if (lHitArray[i].collider.isTrigger) { lHitArray = ArrayExt.RemoveAt<RaycastHit>(lHitArray, i); }
                }
            }

            return lHitArray;
        }

        /// <summary>
        /// When casting a ray from the motion controller, we don't want it to collide with
        /// ourselves. The problem is that we may want to collide with another avatar. So, we
        /// can't put every avatar on their own layer. This ray cast will take a little longer,
        /// but will ignore this avatar.
        /// 
        /// Note: This function isn't virutal to eek out ever ounce of performance we can.
        /// </summary>
        /// <param name="rRayStart"></param>
        /// <param name="rRayDirection"></param>
        /// <param name="rHitInfo"></param>
        /// <param name="rDistance"></param>
        /// <returns></returns>
        public static RaycastHit[] SafeRaycastAll(Vector3 rRayStart, Vector3 rRayDirection, float rDistance, bool rRemoveTriggers)
        {
            RaycastHit[] lHitArray = UnityEngine.Physics.RaycastAll(rRayStart, rRayDirection, rDistance);

            // With no hits, this is easy
            if (lHitArray.Length == 0)
            {
            }
            // With one hit, this is easy too
            else if (lHitArray.Length == 1)
            {
                if (rRemoveTriggers && lHitArray[0].collider.isTrigger)
                {
                    lHitArray = new RaycastHit[0];
                }
            }
            // Find the closest hit
            else
            {
                // Order the array by distance and get rid of items that don't pass
                lHitArray.Sort(delegate (RaycastHit rLeft, RaycastHit rRight) { return (rLeft.distance < rRight.distance ? -1 : (rLeft.distance > rRight.distance ? 1 : 0)); });
                for (int i = lHitArray.Length - 1; i >= 0; i--)
                {
                    if (rRemoveTriggers && lHitArray[i].collider.isTrigger) { lHitArray = ArrayExt.RemoveAt(lHitArray, i); }
                }
            }

            return lHitArray;
        }

        /// <summary>
        /// When casting a ray from the motion controller, we don't want it to collide with
        /// ourselves. The problem is that we may want to collide with another avatar. So, we
        /// can't put every avatar on their own layer. This ray cast will take a little longer,
        /// but will ignore this avatar.
        /// 
        /// Note: This function isn't virutal to eek out ever ounce of performance we can.
        /// </summary>
        /// <param name="rRayStart"></param>
        /// <param name="rRayDirection"></param>
        /// <param name="rHitInfo"></param>
        /// <param name="rDistance"></param>
        /// <returns></returns>
        public static RaycastHit[] SafeRaycastAll(Vector3 rRayStart, Vector3 rRayDirection, float rDistance, Transform rIgnore)
        {
            RaycastHit[] lHitArray = UnityEngine.Physics.RaycastAll(rRayStart, rRayDirection, rDistance);

            // With no hits, this is easy
            if (lHitArray.Length == 0)
            {
            }
            // With one hit, this is easy too
            else if (lHitArray.Length == 1)
            {
                if (lHitArray[0].collider.isTrigger ||
                    (rIgnore != null && rIgnore == lHitArray[0].collider.transform)
                   )
                {
                    lHitArray = new RaycastHit[0];
                }
            }
            // Find the closest hit
            else
            {
                // Order the array by distance and get rid of items that don't pass
                lHitArray.Sort(delegate (RaycastHit rLeft, RaycastHit rRight) { return (rLeft.distance < rRight.distance ? -1 : (rLeft.distance > rRight.distance ? 1 : 0)); });
                for (int i = lHitArray.Length - 1; i >= 0; i--)
                {
                    if (lHitArray[i].collider.isTrigger) { lHitArray = ArrayExt.RemoveAt<RaycastHit>(lHitArray, i); }
                    if (rIgnore != null && rIgnore == lHitArray[i].collider.transform) { lHitArray = ArrayExt.RemoveAt(lHitArray, i); }
                }
            }

            return lHitArray;
        }

        ///// <summary>
        ///// When casting a ray from the motion controller, we don't want it to collide with
        ///// ourselves. The problem is that we may want to collide with another avatar. So, we
        ///// can't put every avatar on their own layer. This ray cast will take a little longer,
        ///// but will ignore this avatar.
        ///// 
        ///// Note: This function isn't virutal to eek out ever ounce of performance we can.
        ///// </summary>
        ///// <param name="rRayStart"></param>
        ///// <param name="rRayDirection"></param>
        ///// <param name="rHitInfo"></param>
        ///// <param name="rDistance"></param>
        ///// <returns></returns>
        //public static RaycastHit[] SafeRaycastAll(Vector3 rRayStart, Vector3 rRayDirection, float rDistance, List<Transform> rIgnore)
        //{
        //    RaycastHit[] lHitArray = UnityEngine.Physics.RaycastAll(rRayStart, rRayDirection, rDistance);

        //    // With no hits, this is easy
        //    if (lHitArray.Length == 0)
        //    {
        //    }
        //    // With one hit, this is easy too
        //    else if (lHitArray.Length == 1)
        //    {
        //        if (lHitArray[0].collider.isTrigger ||
        //            (rIgnore != null && rIgnore.Contains(lHitArray[0].collider.transform))
        //           )
        //        {
        //            lHitArray = new RaycastHit[0];
        //        }
        //    }
        //    // Find the closest hit
        //    else
        //    {
        //        // Order the array by distance and get rid of items that don't pass
        //        lHitArray.Sort(delegate (RaycastHit rLeft, RaycastHit rRight) { return (rLeft.distance < rRight.distance ? -1 : (rLeft.distance > rRight.distance ? 1 : 0)); });
        //        for (int i = lHitArray.Length - 1; i >= 0; i--)
        //        {
        //            if (lHitArray[i].collider.isTrigger) { lHitArray = ArrayExt.RemoveAt<RaycastHit>(lHitArray, i); }
        //            if (rIgnore != null && rIgnore.Contains(lHitArray[i].collider.transform)) { ArrayExt.RemoveAt(ref lHitArray, i); }
        //        }
        //    }

        //    return lHitArray;
        //}

        /// <summary>
        /// When casting a ray from the motion controller, we don't want it to collide with
        /// ourselves. The problem is that we may want to collide with another avatar. So, we
        /// can't put every avatar on their own layer. This ray cast will take a little longer,
        /// but will ignore this avatar.
        /// 
        /// Note: This function isn't virutal to eek out ever ounce of performance we can.
        /// </summary>
        /// <param name="rRayStart"></param>
        /// <param name="rRayDirection"></param>
        /// <param name="rHitInfo"></param>
        /// <param name="rDistance"></param>
        /// <returns></returns>
        public static bool SafeSphereCast(Vector3 rRayStart, Vector3 rRayDirection, float rRadius, float rDistance, out RaycastHit rHitInfo)
        {
            int lHitCount = 0;
            float lDistanceOffset = 0f;

            // Since some objects (like triggers) are invalid for collisions, we want
            // to go through them. That means we may continue a ray cast even if a hit occured
            while (lHitCount < 5 && rDistance > 0f)
            {
                // Assume the next hit to be valid
                bool lIsValidHit = true;

                // Test from the current start
                bool lHit = UnityEngine.Physics.SphereCast(rRayStart, rRadius, rRayDirection, out rHitInfo, rDistance);

                // Easy, just return no hit
                if (!lHit) { return false; }

                // Turns out we can't actually trust the sphere cast as it sometimes returns incorrect point and normal values.
                RaycastHit lRayHitInfo;
                if (UnityEngine.Physics.Raycast(rRayStart, rHitInfo.point - rRayStart, out lRayHitInfo, rDistance))
                {
                    rHitInfo = lRayHitInfo;
                }

                // If we hit a trigger, we'll continue testing just a tad bit beyond.
                if (rHitInfo.collider.isTrigger) { lIsValidHit = false; }

                // If we have an invalid hit, we'll continue testing by using the hit point
                // (plus a little extra) as the new start
                if (!lIsValidHit)
                {
                    lDistanceOffset += rHitInfo.distance + 0.05f;
                    rRayStart = rHitInfo.point + (rRayDirection * 0.05f);

                    rDistance -= (rHitInfo.distance + 0.05f);

                    lHitCount++;
                    continue;
                }

                // If we got here, we must have a valid hit. Update
                // the distance info incase we had to cycle through invalid hits
                rHitInfo.distance += lDistanceOffset;

                return true;
            }

            // We have to initialize the out param
            rHitInfo = EmptyHitInfo;

            // If we got here, we exceeded our attempts and we should drop out
            return false;
        }

        /// <summary>
        /// When casting a ray from the motion controller, we don't want it to collide with
        /// ourselves. The problem is that we may want to collide with another avatar. So, we
        /// can't put every avatar on their own layer. This ray cast will take a little longer,
        /// but will ignore this avatar.
        /// 
        /// Note: This function isn't virutal to eek out ever ounce of performance we can.
        /// </summary>
        /// <param name="rRayStart"></param>
        /// <param name="rRayDirection"></param>
        /// <param name="rHitInfo"></param>
        /// <param name="rDistance"></param>
        /// <returns></returns>
        public static bool SafeSphereCast(Vector3 rRayStart, Vector3 rRayDirection, float rRadius, float rDistance, Transform rIgnore, out RaycastHit rHitInfo)
        {
            int lHitCount = 0;
            float lDistanceOffset = 0f;

            // Since some objects (like triggers) are invalid for collisions, we want
            // to go through them. That means we may continue a ray cast even if a hit occured
            while (lHitCount < 5 && rDistance > 0f)
            {
                // Assume the next hit to be valid
                bool lIsValidHit = true;

                // Test from the current start
                bool lHit = UnityEngine.Physics.SphereCast(rRayStart, rRadius, rRayDirection, out rHitInfo, rDistance);

                // Easy, just return no hit
                if (!lHit) { return false; }

                // Turns out we can't actually trust the sphere cast as it sometimes returns incorrect point and normal values.
                RaycastHit lRayHitInfo;
                if (UnityEngine.Physics.Raycast(rRayStart, rHitInfo.point - rRayStart, out lRayHitInfo, rDistance))
                {
                    rHitInfo = lRayHitInfo;
                }

                // If we hit a trigger, we'll continue testing just a tad bit beyond.
                if (rHitInfo.collider.isTrigger) { lIsValidHit = false; }

                // If we hit a transform to ignore
                if (lIsValidHit && rIgnore != null)
                {
                    Transform lCurrentTransform = rHitInfo.collider.transform;
                    while (lCurrentTransform != null)
                    {
                        if (lCurrentTransform == rIgnore)
                        {
                            lIsValidHit = false;
                            break;
                        }

                        lCurrentTransform = lCurrentTransform.parent;
                    }
                }

                // If we have an invalid hit, we'll continue testing by using the hit point
                // (plus a little extra) as the new start
                if (!lIsValidHit)
                {
                    lDistanceOffset += rHitInfo.distance + 0.05f;
                    rRayStart = rHitInfo.point + (rRayDirection * 0.05f);

                    rDistance -= (rHitInfo.distance + 0.05f);

                    lHitCount++;
                    continue;
                }

                // If we got here, we must have a valid hit. Update
                // the distance info incase we had to cycle through invalid hits
                rHitInfo.distance += lDistanceOffset;

                return true;
            }

            // We have to initialize the out param
            rHitInfo = EmptyHitInfo;

            // If we got here, we exceeded our attempts and we should drop out
            return false;
        }

        /// <summary>
        /// When casting a ray from the motion controller, we don't want it to collide with
        /// ourselves. The problem is that we may want to collide with another avatar. So, we
        /// can't put every avatar on their own layer. This ray cast will take a little longer,
        /// but will ignore this avatar.
        /// 
        /// Note: This function isn't virutal to eek out ever ounce of performance we can.
        /// </summary>
        /// <param name="rRayStart"></param>
        /// <param name="rRayDirection"></param>
        /// <param name="rHitInfo"></param>
        /// <param name="rDistance"></param>
        /// <returns></returns>
        public static bool SafeSphereCast(Vector3 rRayStart, Vector3 rRayDirection, float rRadius, float rDistance, int rLayerMask, Transform rIgnore, out RaycastHit rHitInfo)
        {
            int lHitCount = 0;
            float lDistanceOffset = 0f;

            // Since some objects (like triggers) are invalid for collisions, we want
            // to go through them. That means we may continue a ray cast even if a hit occured
            while (lHitCount < 5 && rDistance > 0f)
            {
                // Assume the next hit to be valid
                bool lIsValidHit = true;

                // Test from the current start
                bool lHit = UnityEngine.Physics.SphereCast(rRayStart, rRadius, rRayDirection, out rHitInfo, rDistance, rLayerMask);

                // Easy, just return no hit
                if (!lHit) { return false; }

                // Turns out we can't actually trust the sphere cast as it sometimes returns incorrect point and normal values.
                RaycastHit lRayHitInfo;
                if (UnityEngine.Physics.Raycast(rRayStart, rHitInfo.point - rRayStart, out lRayHitInfo, rDistance))
                {
                    rHitInfo = lRayHitInfo;
                }

                // If we hit a trigger, we'll continue testing just a tad bit beyond.
                if (rHitInfo.collider.isTrigger) { lIsValidHit = false; }

                // If we hit a transform to ignore
                if (lIsValidHit && rIgnore != null)
                {
                    Transform lCurrentTransform = rHitInfo.collider.transform;
                    while (lCurrentTransform != null)
                    {
                        if (lCurrentTransform == rIgnore)
                        {
                            lIsValidHit = false;
                            break;
                        }

                        lCurrentTransform = lCurrentTransform.parent;
                    }
                }

                // If we have an invalid hit, we'll continue testing by using the hit point
                // (plus a little extra) as the new start
                if (!lIsValidHit)
                {
                    lDistanceOffset += rHitInfo.distance + 0.05f;
                    rRayStart = rHitInfo.point + (rRayDirection * 0.05f);

                    rDistance -= (rHitInfo.distance + 0.05f);

                    lHitCount++;
                    continue;
                }

                // If we got here, we must have a valid hit. Update
                // the distance info incase we had to cycle through invalid hits
                rHitInfo.distance += lDistanceOffset;

                return true;
            }

            // We have to initialize the out param
            rHitInfo = EmptyHitInfo;

            // If we got here, we exceeded our attempts and we should drop out
            return false;
        }

        /// <summary>
        /// When casting a ray from the motion controller, we don't want it to collide with
        /// ourselves. The problem is that we may want to collide with another avatar. So, we
        /// can't put every avatar on their own layer. This ray cast will take a little longer,
        /// but will ignore this avatar.
        /// 
        /// Note: This function isn't virutal to eek out ever ounce of performance we can.
        /// </summary>
        /// <param name="rRayStart"></param>
        /// <param name="rRayDirection"></param>
        /// <param name="rHitInfo"></param>
        /// <param name="rDistance"></param>
        /// <returns></returns>
        public static bool SafeSphereCast(Vector3 rRayStart, Vector3 rRayDirection, float rRadius, float rDistance, Transform rIgnore, out RaycastHit rHitInfo, bool rRecast)
        {
            int lHitCount = 0;
            float lDistanceOffset = 0f;

            // Since some objects (like triggers) are invalid for collisions, we want
            // to go through them. That means we may continue a ray cast even if a hit occured
            while (lHitCount < 5 && rDistance > 0f)
            {
                // Assume the next hit to be valid
                bool lIsValidHit = true;

                // Test from the current start
                bool lHit = UnityEngine.Physics.SphereCast(rRayStart, rRadius, rRayDirection, out rHitInfo, rDistance);

                // Easy, just return no hit
                if (!lHit) { return false; }

                // Turns out we can't actually trust the sphere cast as it sometimes returns incorrect point and normal values.
                if (rRecast)
                {
                    RaycastHit lRayHitInfo;
                    if (UnityEngine.Physics.Raycast(rRayStart, rHitInfo.point - rRayStart, out lRayHitInfo, rDistance))
                    {
                        rHitInfo = lRayHitInfo;
                    }
                }

                // If we hit a trigger, we'll continue testing just a tad bit beyond.
                if (rHitInfo.collider.isTrigger) { lIsValidHit = false; }

                // If we hit a transform to ignore
                if (lIsValidHit && rIgnore != null)
                {
                    Transform lCurrentTransform = rHitInfo.collider.transform;
                    while (lCurrentTransform != null)
                    {
                        if (lCurrentTransform == rIgnore)
                        {
                            lIsValidHit = false;
                            break;
                        }

                        lCurrentTransform = lCurrentTransform.parent;
                    }
                }

                // If we have an invalid hit, we'll continue testing by using the hit point
                // (plus a little extra) as the new start
                if (!lIsValidHit)
                {
                    lDistanceOffset += rHitInfo.distance + 0.05f;
                    rRayStart = rHitInfo.point + (rRayDirection * 0.05f);

                    rDistance -= (rHitInfo.distance + 0.05f);

                    lHitCount++;
                    continue;
                }

                // If we got here, we must have a valid hit. Update
                // the distance info incase we had to cycle through invalid hits
                rHitInfo.distance += lDistanceOffset;

                return true;
            }

            // We have to initialize the out param
            rHitInfo = EmptyHitInfo;

            // If we got here, we exceeded our attempts and we should drop out
            return false;
        }

        /// <summary>
        /// When casting a ray from the motion controller, we don't want it to collide with
        /// ourselves. The problem is that we may want to collide with another avatar. So, we
        /// can't put every avatar on their own layer. This ray cast will take a little longer,
        /// but will ignore this avatar.
        /// 
        /// Note: This function isn't virutal to eek out ever ounce of performance we can.
        /// </summary>
        /// <param name="rRayStart"></param>
        /// <param name="rRayDirection"></param>
        /// <param name="rHitInfo"></param>
        /// <param name="rDistance"></param>
        /// <returns></returns>
        public static bool SafeSphereCast(Vector3 rRayStart, Vector3 rRayDirection, float rRadius, float rDistance, int rLayerMask, Transform rIgnore, out RaycastHit rHitInfo, bool rRecast)
        {
            int lHitCount = 0;
            float lDistanceOffset = 0f;

            // Since some objects (like triggers) are invalid for collisions, we want
            // to go through them. That means we may continue a ray cast even if a hit occured
            while (lHitCount < 5 && rDistance > 0f)
            {
                // Assume the next hit to be valid
                bool lIsValidHit = true;

                // Test from the current start
                bool lHit = UnityEngine.Physics.SphereCast(rRayStart, rRadius, rRayDirection, out rHitInfo, rDistance, rLayerMask);

                // Easy, just return no hit
                if (!lHit) { return false; }

                // Turns out we can't actually trust the sphere cast as it sometimes returns incorrect point and normal values.
                if (rRecast)
                {
                    RaycastHit lRayHitInfo;
                    if (UnityEngine.Physics.Raycast(rRayStart, rHitInfo.point - rRayStart, out lRayHitInfo, rDistance, rLayerMask))
                    {
                        rHitInfo = lRayHitInfo;
                    }
                }

                // If we hit a trigger, we'll continue testing just a tad bit beyond.
                if (rHitInfo.collider.isTrigger) { lIsValidHit = false; }

                // If we hit a transform to ignore
                if (lIsValidHit && rIgnore != null)
                {
                    Transform lCurrentTransform = rHitInfo.collider.transform;
                    while (lCurrentTransform != null)
                    {
                        if (lCurrentTransform == rIgnore)
                        {
                            lIsValidHit = false;
                            break;
                        }

                        lCurrentTransform = lCurrentTransform.parent;
                    }
                }

                // If we have an invalid hit, we'll continue testing by using the hit point
                // (plus a little extra) as the new start
                if (!lIsValidHit)
                {
                    lDistanceOffset += rHitInfo.distance + 0.05f;
                    rRayStart = rHitInfo.point + (rRayDirection * 0.05f);

                    rDistance -= (rHitInfo.distance + 0.05f);

                    lHitCount++;
                    continue;
                }

                // If we got here, we must have a valid hit. Update
                // the distance info incase we had to cycle through invalid hits
                rHitInfo.distance += lDistanceOffset;

                return true;
            }

            // We have to initialize the out param
            rHitInfo = EmptyHitInfo;

            // If we got here, we exceeded our attempts and we should drop out
            return false;
        }
        
        /// <summary>
        /// When casting a ray from the motion controller, we don't want it to collide with
        /// ourselves. The problem is that we may want to collide with another avatar. So, we
        /// can't put every avatar on their own layer. This ray cast will take a little longer,
        /// but will ignore this avatar.
        /// 
        /// Note: This function isn't virutal to eek out ever ounce of performance we can.
        /// </summary>
        /// <param name="rRayStart"></param>
        /// <param name="rRayDirection"></param>
        /// <param name="rHitInfo"></param>
        /// <param name="rDistance"></param>
        /// <returns></returns>
        public static RaycastHit[] SafeSphereCastAll(Vector3 rRayStart, Vector3 rRayDirection, float rRadius, float rDistance, Transform rIgnore)
        {
            RaycastHit[] lHitArray = UnityEngine.Physics.SphereCastAll(rRayStart, rRadius, rRayDirection, rDistance);

            // With no hits, this is easy
            if (lHitArray.Length == 0)
            {
            }
            // With one hit, this is easy too
            else if (lHitArray.Length == 1)
            {
                if (lHitArray[0].collider.isTrigger ||
                    (rIgnore != null && rIgnore == lHitArray[0].collider.transform)
                   )
                {
                    lHitArray = new RaycastHit[0];
                }
            }
            // Find the closest hit
            else
            {
                // Order the array by distance and get rid of items that don't pass
                lHitArray.Sort(delegate (RaycastHit rLeft, RaycastHit rRight) { return (rLeft.distance < rRight.distance ? -1 : (rLeft.distance > rRight.distance ? 1 : 0)); });
                for (int i = lHitArray.Length - 1; i >= 0; i--)
                {
                    if (lHitArray[i].collider.isTrigger) { lHitArray = ArrayExt.RemoveAt(lHitArray, i); }

                    if (rIgnore != null)
                    {
                        bool lIsValidHit = true;
                        Transform lCurrentTransform = lHitArray[i].collider.transform;
                        while (lCurrentTransform != null)
                        {
                            if (lCurrentTransform == rIgnore)
                            {
                                lIsValidHit = false;
                                break;
                            }

                            lCurrentTransform = lCurrentTransform.parent;
                        }

                        if (!lIsValidHit) { lHitArray = ArrayExt.RemoveAt(lHitArray, i); }
                    }
                }
            }

            return lHitArray;
        }
        
        /// <summary>
                 /// When casting a ray from the motion controller, we don't want it to collide with
                 /// ourselves. The problem is that we may want to collide with another avatar. So, we
                 /// can't put every avatar on their own layer. This ray cast will take a little longer,
                 /// but will ignore this avatar.
                 /// 
                 /// Note: This function isn't virutal to eek out ever ounce of performance we can.
                 /// </summary>
                 /// <param name="rRayStart"></param>
                 /// <param name="rRayDirection"></param>
                 /// <param name="rHitInfo"></param>
                 /// <param name="rDistance"></param>
                 /// <returns></returns>
        public static RaycastHit[] SafeSphereCastAll(Vector3 rRayStart, Vector3 rRayDirection, float rRadius, float rDistance, int rLayerMask, Transform rIgnore)
        {
            RaycastHit[] lHitArray = UnityEngine.Physics.SphereCastAll(rRayStart, rRadius, rRayDirection, rDistance, rLayerMask);

            // With no hits, this is easy
            if (lHitArray.Length == 0)
            {
            }
            // With one hit, this is easy too
            else if (lHitArray.Length == 1)
            {
                if (lHitArray[0].collider.isTrigger ||
                    (rIgnore != null && rIgnore == lHitArray[0].collider.transform)
                   )
                {
                    lHitArray = new RaycastHit[0];
                }
            }
            // Find the closest hit
            else
            {
                // Order the array by distance and get rid of items that don't pass
                lHitArray.Sort(delegate (RaycastHit rLeft, RaycastHit rRight) { return (rLeft.distance < rRight.distance ? -1 : (rLeft.distance > rRight.distance ? 1 : 0)); });
                for (int i = lHitArray.Length - 1; i >= 0; i--)
                {
                    if (lHitArray[i].collider.isTrigger) { lHitArray = ArrayExt.RemoveAt(lHitArray, i); }

                    if (rIgnore != null)
                    {
                        bool lIsValidHit = true;
                        Transform lCurrentTransform = lHitArray[i].collider.transform;
                        while (lCurrentTransform != null)
                        {
                            if (lCurrentTransform == rIgnore)
                            {
                                lIsValidHit = false;
                                break;
                            }

                            lCurrentTransform = lCurrentTransform.parent;
                        }

                        if (!lIsValidHit) { lHitArray = ArrayExt.RemoveAt(lHitArray, i); }
                    }
                }
            }

            return lHitArray;
        }

        /// <summary>
        /// When casting a ray from the motion controller, we don't want it to collide with
        /// ourselves. The problem is that we may want to collide with another avatar. So, we
        /// can't put every avatar on their own layer. This ray cast will take a little longer,
        /// but will ignore this avatar.
        /// 
        /// Note: This function isn't virutal to eek out ever ounce of performance we can.
        /// </summary>
        /// <param name="rRayStart"></param>
        /// <param name="rRayDirection"></param>
        /// <param name="rHitInfo"></param>
        /// <param name="rDistance"></param>
        /// <returns></returns>
        public static RaycastHit[] SafeSphereCastAll(Vector3 rRayStart, Vector3 rRayDirection, float rRadius, float rDistance, List<Transform> rIgnore)
        {
            RaycastHit[] lHitArray = UnityEngine.Physics.SphereCastAll(rRayStart, rRadius, rRayDirection, rDistance);

            // With no hits, this is easy
            if (lHitArray.Length == 0)
            {
            }
            // With one hit, this is easy too
            else if (lHitArray.Length == 1)
            {
                if (lHitArray[0].collider.isTrigger ||
                    (rIgnore != null && rIgnore.Contains(lHitArray[0].collider.transform))
                   )
                {
                    lHitArray = new RaycastHit[0];
                }
            }
            // Find the closest hit
            else
            {
                // Order the array by distance and get rid of items that don't pass
                lHitArray.Sort(delegate (RaycastHit rLeft, RaycastHit rRight) { return (rLeft.distance < rRight.distance ? -1 : (rLeft.distance > rRight.distance ? 1 : 0)); });
                for (int i = lHitArray.Length - 1; i >= 0; i--)
                {
                    if (lHitArray[i].collider.isTrigger) { lHitArray = ArrayExt.RemoveAt(lHitArray, i); }

                    if (rIgnore != null)
                    {
                        bool lIsValidHit = true;
                        Transform lCurrentTransform = lHitArray[i].collider.transform;
                        while (lCurrentTransform != null)
                        {
                            if (rIgnore.Contains(lCurrentTransform))
                            {
                                lIsValidHit = false;
                                break;
                            }

                            lCurrentTransform = lCurrentTransform.parent;
                        }

                        if (!lIsValidHit) { lHitArray = ArrayExt.RemoveAt(lHitArray, i); }
                    }
                }
            }

            return lHitArray;
        }

        /// <summary>
        /// When casting a ray from the motion controller, we don't want it to collide with
        /// ourselves. The problem is that we may want to collide with another avatar. So, we
        /// can't put every avatar on their own layer. This ray cast will take a little longer,
        /// but will ignore this avatar.
        /// 
        /// Note: This function isn't virutal to eek out ever ounce of performance we can.
        /// </summary>
        /// <param name="rRayStart"></param>
        /// <param name="rRayDirection"></param>
        /// <param name="rHitInfo"></param>
        /// <param name="rDistance"></param>
        /// <returns></returns>
        public static Collider[] SafeOverlapSphere(Vector3 rPosition, float rRadius, Transform rIgnore)
        {
            // This causes 28 B of GC.
            Collider[] lHitArray = UnityEngine.Physics.OverlapSphere(rPosition, rRadius);

            // Get rid of elements we don't need
            for (int i = lHitArray.Length - 1; i >= 0; i--)
            {
                // If we hit a trigger to ignore
                if (lHitArray[i].isTrigger)
                {
                    lHitArray = ArrayExt.RemoveAt<Collider>(lHitArray, i);
                    continue;
                }

                // If we hit a transform to ignore
                if (rIgnore != null)
                {
                    Transform lCurrentTransform = lHitArray[i].transform;
                    while (lCurrentTransform != null)
                    {
                        if (lCurrentTransform == rIgnore)
                        {
                            lHitArray = ArrayExt.RemoveAt<Collider>(lHitArray, i);
                            break;
                        }

                        lCurrentTransform = lCurrentTransform.parent;
                    }
                }
            }

            // Return the rest
            return lHitArray;
        }

        /// <summary>
        /// When casting a ray from the motion controller, we don't want it to collide with
        /// ourselves. The problem is that we may want to collide with another avatar. So, we
        /// can't put every avatar on their own layer. This ray cast will take a little longer,
        /// but will ignore this avatar.
        /// 
        /// Note: This function isn't virutal to eek out ever ounce of performance we can.
        /// </summary>
        /// <param name="rRayStart"></param>
        /// <param name="rRayDirection"></param>
        /// <param name="rHitInfo"></param>
        /// <param name="rDistance"></param>
        /// <returns></returns>
        public static Collider[] SafeOverlapSphere(Vector3 rRayStart, float rRadius, int rLayerMask, Transform rIgnore)
        {
            // This causes 28 B of GC.
            Collider[] lHitArray = UnityEngine.Physics.OverlapSphere(rRayStart, rRadius, rLayerMask);

            // Get rid of elements we don't need
            for (int i = lHitArray.Length - 1; i >= 0; i--)
            {
                if (lHitArray[i].isTrigger ||
                    (rIgnore != null && rIgnore == lHitArray[i].transform))
                {
                    lHitArray = ArrayExt.RemoveAt<Collider>(lHitArray, i);
                }
            }

            // Return the rest
            return lHitArray;
        }

        /// <summary>
        /// When casting a ray from the motion controller, we don't want it to collide with
        /// ourselves. The problem is that we may want to collide with another avatar. So, we
        /// can't put every avatar on their own layer. This ray cast will take a little longer,
        /// but will ignore this avatar.
        /// 
        /// Note: This function isn't virutal to eek out ever ounce of performance we can.
        /// </summary>
        /// <param name="rRayStart"></param>
        /// <param name="rRayDirection"></param>
        /// <param name="rHitInfo"></param>
        /// <param name="rDistance"></param>
        /// <returns></returns>
        public static Collider[] SafeOverlapSphere(Vector3 rRayStart, float rRadius, List<Transform> rIgnore)
        {
            // This causes 28 B of GC.
            Collider[] lHitArray = UnityEngine.Physics.OverlapSphere(rRayStart, rRadius);

            // Get rid of elements we don't need
            for (int i = lHitArray.Length - 1; i >= 0; i--)
            {
                if (lHitArray[i].isTrigger ||
                    (rIgnore != null && rIgnore.Contains(lHitArray[i].transform)))
                {
                    lHitArray = ArrayExt.RemoveAt<Collider>(lHitArray, i);
                }
            }

            // Return the rest
            return lHitArray;
        }

        /// <summary>
        /// When casting a ray from the motion controller, we don't want it to collide with
        /// ourselves. The problem is that we may want to collide with another avatar. So, we
        /// can't put every avatar on their own layer. This ray cast will take a little longer,
        /// but will ignore this avatar.
        /// 
        /// Note: This function isn't virutal to eek out ever ounce of performance we can.
        /// </summary>
        /// <param name="rRayStart"></param>
        /// <param name="rRayDirection"></param>
        /// <param name="rHitInfo"></param>
        /// <param name="rDistance"></param>
        /// <returns></returns>
        public static Collider[] SafeOverlapSphere(Vector3 rRayStart, float rRadius, int rLayerMask, List<Transform> rIgnore)
        {
            // This causes 28 B of GC.
            Collider[] lHitArray = UnityEngine.Physics.OverlapSphere(rRayStart, rRadius, rLayerMask);

            // Get rid of elements we don't need
            for (int i = lHitArray.Length - 1; i >= 0; i--)
            {
                if (lHitArray[i].isTrigger ||
                    (rIgnore != null && rIgnore.Contains(lHitArray[i].transform)))
                {
                    lHitArray = ArrayExt.RemoveAt<Collider>(lHitArray, i);
                }
            }

            // Return the rest
            return lHitArray;
        }

        /// <summary>
        /// Determines if the "descendant" transform is a child (or grand child)
        /// of the "parent" transform.
        /// </summary>
        /// <param name="rParent"></param>
        /// <param name="rTest"></param>
        /// <returns></returns>
        private static bool IsDescendant(Transform rParent, Transform rDescendant)
        {
            Transform lParent = rDescendant;
            while (lParent != null)
            {
                if (lParent == rParent) { return true; }
                lParent = lParent.parent;
            }

            return false;
        }


        // Older function that shouldn't be used

        //public static bool SafeRaycast(Vector3 rRayStart, Vector3 rRayDirection, ref RaycastHit rHitInfo, float rDistance, Transform rIgnore)
        //{
        //    return SafeRaycast(rRayStart, rRayDirection, rDistance, rIgnore, out rHitInfo);
        //}

        public static bool SafeRaycast(Vector3 rRayStart, Vector3 rRayDirection, ref RaycastHit rHitInfo, float rDistance, int rLayerMask, Transform rIgnore)
        {
            return SafeRaycast(rRayStart, rRayDirection, rDistance, rLayerMask, rIgnore, out rHitInfo);
        }

        /// <summary>
        /// When casting a ray from the motion controller, we don't want it to collide with
        /// ourselves. The problem is that we may want to collide with another avatar. So, we
        /// can't put every avatar on their own layer. This ray cast will take a little longer,
        /// but will ignore this avatar.
        /// 
        /// Note: This function isn't virutal to eek out ever ounce of performance we can.
        /// </summary>
        /// <param name="rRayStart"></param>
        /// <param name="rRayDirection"></param>
        /// <param name="rHitInfo"></param>
        /// <param name="rDistance"></param>
        /// <param name="rIgnore">List of transforms we should ignore collisions with</param>
        /// <returns></returns>
        public static bool SafeRaycast(Vector3 rRayStart, Vector3 rRayDirection, ref RaycastHit rHitInfo, float rDistance, List<Transform> rIgnore)
        {
            int lHitCount = 0;
            float lDistanceOffset = 0f;
            Vector3 lRayStart = rRayStart;

            // Since some objects (like triggers) are invalid for collisions, we want
            // to go through them. That means we may continue a ray cast even if a hit occured
            while (lHitCount < 5 && rDistance > 0f)
            {
                // Assume the next hit to be valid
                bool lIsValidHit = true;

                // Test from the current start
                bool lHit = UnityEngine.Physics.Raycast(lRayStart, rRayDirection, out rHitInfo, rDistance);

                // Easy, just return no hit
                if (!lHit) { return false; }

                // If we hit a trigger, we'll continue testing just a tad bit beyond.
                if (rHitInfo.collider.isTrigger) { lIsValidHit = false; }
                if (rIgnore != null && rIgnore.Contains(rHitInfo.collider.transform)) { lIsValidHit = false; }

                // If we have an invalid hit, we'll continue testing by using the hit point
                // (plus a little extra) as the new start
                if (!lIsValidHit)
                {
                    lDistanceOffset += rHitInfo.distance + 0.05f;
                    lRayStart = rHitInfo.point + (rRayDirection * 0.05f);

                    rDistance -= (rHitInfo.distance + 0.05f);

                    lHitCount++;
                    continue;
                }

                // If we got here, we must have a valid hit. Update
                // the distance info incase we had to cycle through invalid hits
                rHitInfo.distance += lDistanceOffset;

                rHitInfo.distance = Vector3.Distance(rRayStart, rHitInfo.point);

                return true;
            }

            // If we got here, we exceeded our attempts and we should drop out
            return false;
        }
    }
}
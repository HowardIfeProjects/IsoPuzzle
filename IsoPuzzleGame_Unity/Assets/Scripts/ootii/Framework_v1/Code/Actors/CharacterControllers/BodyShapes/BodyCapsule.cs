using System;
using System.Collections.Generic;
using UnityEngine;
using com.ootii.Geometry;
using com.ootii.Helpers;
using com.ootii.Utilities.Debug;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace com.ootii.Actors
{
    /// <summary>
    /// Contains information that defines a sphere we'll use
    /// to help manage collisions with an actor
    /// </summary>
    [Serializable]
    public class BodyCapsule : BodyShape
    {
        /// <summary>
        /// Transform the collision sphere is tied to. This allows the
        /// sphere to change positions based on animations.
        /// </summary>
        public Transform _EndTransform = null;
        public Transform EndTransform
        {
            get { return _EndTransform; }
            set { _EndTransform = value; }
        }

        /// <summary>
        /// Relative position from the transform. If no transform
        /// is defined, it's the position from the origin.
        /// </summary>
        public Vector3 _EndOffset = Vector3.zero;
        public Vector3 EndOffset
        {
            get { return _EndOffset; }
            set { _EndOffset = value; }
        }

        /// <summary>
        /// Checks if the shape currently overlaps any colliders
        /// </summary>
        /// <param name="rPositionDelta">Movement to add to the current position</param>
        /// <param name="rLayerMask">Layer mask for determing what we'll collide with</param>
        /// <returns>Boolean that says if a hit occured or not</returns>
        public override List<BodyShapeHit> CollisionOverlap(Vector3 rPositionDelta, Quaternion rRotationDelta, int rLayerMask)
        {
            List<BodyShapeHit> lHits = new List<BodyShapeHit>();

            Vector3 lBodyShapePos1 = rPositionDelta + (_Transform == null ? _Parent.position + ((_Parent.rotation * rRotationDelta) * _Offset) : _Transform.position + ((_Transform.rotation * rRotationDelta) * _Offset));
            Vector3 lBodyShapePos2 = rPositionDelta + (_EndTransform == null ? _Parent.position + ((_Parent.rotation * rRotationDelta) * _EndOffset) : _EndTransform.position + ((_EndTransform.rotation * rRotationDelta) * _EndOffset));
            Vector3 lPosition = lBodyShapePos1 + ((lBodyShapePos2 - lBodyShapePos1) / 2f);

            float lOverlapRadius = (Vector3.Distance(lBodyShapePos1, lBodyShapePos2) / 2f) + _Radius;

            Collider[] lColliders = RaycastExt.SafeOverlapSphere(lPosition, lOverlapRadius, _Parent);
            for (int i = 0; i < lColliders.Length; i++)
            {
                Transform lCurrentTransform = lColliders[i].transform;
                if (lCurrentTransform == _Transform) { continue; }
                if (lCurrentTransform == _EndTransform) { continue; }

                // Once we get here, we have a valid collider
                Vector3 lLinePoint = Vector3.zero;
                Vector3 lColliderPoint = Vector3.zero;
                GeometryExt.ClosestPoints(lBodyShapePos1, lBodyShapePos2, _Radius, lColliders[i], ref lLinePoint, ref lColliderPoint);

                float lDistance = Vector3.Distance(lLinePoint, lColliderPoint);
                if (lDistance < _Radius + 0.001f)
                {
                    BodyShapeHit lHit = BodyShapeHit.Allocate();
                    lHit.StartPosition = lBodyShapePos1;
                    lHit.EndPosition = lBodyShapePos2;
                    lHit.HitCollider = lColliders[i];
                    lHit.HitOrigin = lLinePoint;
                    lHit.HitPoint = lColliderPoint;
                    lHit.HitDistance = lDistance - _Radius - 0.001f;

                    lHits.Add(lHit);
                }
            }

            return lHits;
        }

        /// <summary>
        /// Casts out a shape to see if a collision will occur.
        /// </summary>
        /// <param name="rPositionDelta">Movement to add to the current position</param>
        /// <param name="rDirection">Direction of the cast</param>
        /// <param name="rDistance">Distance of the case</param>
        /// <param name="rLayerMask">Layer mask for determing what we'll collide with</param>
        /// <returns>Returns an array of BodyShapeHit values representing all the hits that take place</returns>
        public override BodyShapeHit[] CollisionCastAll(Vector3 rPositionDelta, Vector3 rDirection, float rDistance, int rLayerMask)
        {
            Vector3 lBodyShapePos1 = rPositionDelta + (_Transform == null ? _Parent.position + (_Parent.rotation * _Offset) : _Transform.position + (_Transform.rotation * _Offset));
            Vector3 lBodyShapePos2 = rPositionDelta + (_EndTransform == null ? _Parent.position + (_Parent.rotation * _EndOffset) : _EndTransform.position + (_EndTransform.rotation * _EndOffset));

            // Add the additional epsilon to the distance so we can get where we really want to test.
            RaycastHit[] lRaycastHits = UnityEngine.Physics.CapsuleCastAll(lBodyShapePos1, lBodyShapePos2, _Radius, rDirection, rDistance + EPSILON, rLayerMask);

            int lBodyShapeHitsIndex = 0;
            BodyShapeHit[] lBodyShapeHits = new BodyShapeHit[lRaycastHits.Length];
            for (int i = 0; i < lRaycastHits.Length; i++)
            {
                Transform lCurrentTransform = lRaycastHits[i].collider.transform;
                if (lCurrentTransform == _Transform) { continue; }
                if (lCurrentTransform == _EndTransform) { continue; }

                // Ensure we're not colliding with a transform in our chain
                bool lIsValidHit = true;
                while (lCurrentTransform != null)
                {
                    if (lCurrentTransform == _Parent)
                    {
                        lIsValidHit = false;
                        break;
                    }

                    lCurrentTransform = lCurrentTransform.parent;
                }

                if (!lIsValidHit) { continue; }

                // Once we get here, we have a valid collider
                BodyShapeHit lBodyShapeHit = BodyShapeHit.Allocate();
                lBodyShapeHit.StartPosition = lBodyShapePos1;
                lBodyShapeHit.EndPosition = lBodyShapePos2;
                lBodyShapeHit.Shape = this;
                lBodyShapeHit.Hit = lRaycastHits[i];
                lBodyShapeHit.HitCollider = lRaycastHits[i].collider;
                lBodyShapeHit.HitPoint = lRaycastHits[i].point;
                lBodyShapeHit.HitNormal = lRaycastHits[i].normal;
                lBodyShapeHit.HitDistance = lRaycastHits[i].distance;

                // With the capsule cast all, we can recieve hits for colliders that
                // start by intruding on the capsule. In this case, the distance is "0". So,
                // we'll find the true distance ourselves.
                if (lRaycastHits[i].distance == 0f)
                {
                    Vector3 lLinePoint = Vector3.zero;
                    Vector3 lColliderPoint = Vector3.zero;

                    if (lBodyShapeHit.HitCollider is TerrainCollider)
                    {
                        GeometryExt.ClosestPoints(lBodyShapePos1, lBodyShapePos2, rDirection * rDistance, _Radius, (TerrainCollider)lBodyShapeHit.HitCollider, ref lLinePoint, ref lColliderPoint);
                    }
                    else
                    {
                        GeometryExt.ClosestPoints(lBodyShapePos1, lBodyShapePos2, _Radius, lBodyShapeHit.HitCollider, ref lLinePoint, ref lColliderPoint);
                    }

                    // If we don't have a valid point, we will skip
                    if (lColliderPoint == Vector3.zero)
                    {
                        BodyShapeHit.Release(lBodyShapeHit);
                        continue;
                    }

                    // If the hit is further than our radius, we can skip
                    Vector3 lHitVector = lColliderPoint - lLinePoint;
                    //if (lHitVector.magnitude > _Radius + EPSILON)
                    //{
                    //    BodyShapeHit.Release(lBodyShapeHit);
                    //    continue;
                    //}

                    // Setup the remaining info
                    lBodyShapeHit.HitOrigin = lLinePoint;
                    lBodyShapeHit.HitPoint = lColliderPoint;

                    // We have the distance between the origin and the surface. Now 
                    // we need to find the distances between the surfaces.
                    lBodyShapeHit.HitDistance = lHitVector.magnitude - _Radius;
                    lBodyShapeHit.HitPenetration = (lBodyShapeHit.HitDistance < 0f);

                    RaycastHit lRaycastHitInfo;
                    if (RaycastExt.SafeRaycast(lLinePoint, lHitVector.normalized, Mathf.Max(lBodyShapeHit.HitDistance + _Radius, _Radius + 0.01f), out lRaycastHitInfo))
                    {
                        lBodyShapeHit.HitNormal = lRaycastHitInfo.normal;
                    }
                    // If the ray is so close that we can't get a result we can end up here
                    else if (lBodyShapeHit.HitDistance < EPSILON)
                    {
                        lBodyShapeHit.HitNormal = (lLinePoint - lColliderPoint).normalized;
                    }
                }
                else
                {
                    lBodyShapeHit.CalculateHitOrigin();
                }

                // Add the collision info
                if (lBodyShapeHit != null)
                {
                    // We can't really trust the hit normal we have since it probably came from an edge. So, we'll
                    // shoot a ray along our movement path. This will give us a better angle to look at. However, if we're
                    // falling (probably from gravity), we don't want to replace the edge value.
                    if (rDirection != Vector3.down)
                    {
                        RaycastHit lRaycastHitInfo;
                        if (RaycastExt.SafeRaycast(lBodyShapeHit.HitPoint - (rDirection * rDistance), rDirection, rDistance + _Radius, _Parent, out lRaycastHitInfo))
                        {
                            lBodyShapeHit.HitNormal = lRaycastHitInfo.normal;
                        }
                    }

                    // TRT 11/20/15 - Given some colision oddities, I think having the hit normal always be the value
                    // between the origin and point is probably right. Will need to test to ensure.
                    lBodyShapeHit.HitNormal = (lBodyShapeHit.HitOrigin - lBodyShapeHit.HitPoint).normalized;

                    // Store the distance between the hit point and our character's root
                    lBodyShapeHit.HitRootDistance = _Parent.InverseTransformPoint(lBodyShapeHit.HitPoint).y;

                    lBodyShapeHits[lBodyShapeHitsIndex] = lBodyShapeHit;
                    lBodyShapeHitsIndex++;
                }
            }

            return lBodyShapeHits;
        }

        /// <summary>
        /// Grabs the closets contact point to this shape. During the process, we may generate a
        /// new position for the shape
        /// </summary>
        /// <param name="rCollider">Collider we want to find the closest point to</param>
        /// <param name="rMovement">Movement that this body is planning on taking</param>
        /// <param name="rProcessTerrain">Determines if we'll process TerrainColliders</param>
        /// <param name="rShapeTransform">Main transform of "this" body shape we're testing</param>
        /// <param name="rShapePosition">Planned body shape position bsed on rShapeTransform and rMovement</param>
        /// <param name="rContactPoint">Contact point found if a collision occurs or "zero" if none found</param>
        /// <returns></returns>
        public override bool ClosestPoint(Collider rCollider, Vector3 rMovement, bool rProcessTerrain, out Vector3 rShapePoint, out Vector3 rContactPoint)
        {
            // Starting point of our capsule
            Transform lStartTransform = (_Transform != null ? _Transform : _Parent);
            Vector3 lStartPosition = lStartTransform.position + (lStartTransform.rotation * _Offset);

            // Ending point of our capsule
            Transform lEndTransform = (_EndTransform != null ? _EndTransform : _Transform);
            Vector3 lEndPosition = lEndTransform.position + (lEndTransform.rotation * _EndOffset);

            // Closest contact point to the shape
            rShapePoint = lStartPosition;
            rContactPoint = Vector3.zero;

            Utilities.Profiler.Start("CP");

            // Test the collider for the closest contact point
            if (rProcessTerrain && rCollider is TerrainCollider)
            {
                GeometryExt.ClosestPoints(lStartPosition, lEndPosition, rMovement.normalized, _Radius, (TerrainCollider)rCollider, ref rShapePoint, ref rContactPoint);
            }
            else
            {
                GeometryExt.ClosestPoints(lStartPosition, lEndPosition, _Radius, rCollider, ref rShapePoint, ref rContactPoint);
            }

            Utilities.Profiler.Stop("CP");

            // Report back if we have a valid contact point
            return (rContactPoint.sqrMagnitude > 0f);
        }

        // **************************************************************************************************
        // Following properties and function only valid while editing
        // **************************************************************************************************

#if UNITY_EDITOR

        /// <summary>
        /// Used to render out the inspector for the shape
        /// </summary>
        /// <returns></returns>
        public override bool OnInspectorGUI()
        {
            bool lIsDirty = false;

            EditorGUILayout.LabelField("Body Capsule", EditorStyles.boldLabel);

            if (_Parent != null)
            {
                //ActorController lController = _Parent.GetComponent<ActorController>();
                //if (lController != null)
                //{
                //    if (lController.OverlapRadius < _Radius)
                //    {
                //        Color lGUIColor = GUI.color;
                //        GUI.color = new Color(1f, 0.7f, 0.7f, 1f);
                //        EditorGUILayout.HelpBox("Overlap radius is less than this radius. Increase overlap radius so collisions can be found.", MessageType.Warning);
                //        GUI.color = lGUIColor;
                //    }

                //    if (_Offset.y <= _Radius && lController.BaseRadius != _Radius && (_Transform == null || _Transform == _Parent))
                //    {
                //        Color lGUIColor = GUI.color;
                //        GUI.color = new Color(1f, 0.7f, 0.7f, 1f);
                //        EditorGUILayout.HelpBox("This shape sits on the ground. So, the 'Grounding Radius' above should probably be the same radius as this shape.", MessageType.Warning);
                //        GUI.color = lGUIColor;
                //    }

                //    //if (lController.MaxStepHeight > Offset.y)
                //    //{
                //    //    Color lGUIColor = GUI.color;
                //    //    GUI.color = new Color(1f, 0.7f, 0.7f, 1f);
                //    //    EditorGUILayout.HelpBox("The step height is higher than the offset. This could cause low collisions to be ignored.", MessageType.Warning);
                //    //    GUI.color = lGUIColor;
                //    //}
                //}
            }

            GUILayout.Space(5);

            string lNewName = EditorGUILayout.TextField(new GUIContent("Name", ""), Name);
            if (lNewName != Name)
            {
                lIsDirty = true;
                Name = lNewName;
            }

            float lNewRadius = EditorGUILayout.FloatField(new GUIContent("Radius", ""), Radius);
            if (lNewRadius != Radius)
            {
                lIsDirty = true;
                Radius = lNewRadius;
            }

            bool lNewIsEnabledOnGround = EditorGUILayout.Toggle(new GUIContent("Enabled on ground", "Determines if the shape is valid while on the ground."), IsEnabledOnGround);
            if (lNewIsEnabledOnGround != IsEnabledOnGround)
            {
                lIsDirty = true;
                IsEnabledOnGround = lNewIsEnabledOnGround;
            }

            bool lNewIsEnabledOnSlope = EditorGUILayout.Toggle(new GUIContent("Enabled on ramp", "Determines if the shape is valid while on a slope."), IsEnabledOnSlope);
            if (lNewIsEnabledOnSlope != IsEnabledOnSlope)
            {
                lIsDirty = true;
                IsEnabledOnSlope = lNewIsEnabledOnSlope;
            }

            bool lNewIsEnabledAboveGround = EditorGUILayout.Toggle(new GUIContent("Enabled in air", "Determines if the shape is valid while in the air."), IsEnabledAboveGround);
            if (lNewIsEnabledAboveGround != IsEnabledAboveGround)
            {
                lIsDirty = true;
                IsEnabledAboveGround = lNewIsEnabledAboveGround;
            }

            GUILayout.Space(5);

            Transform lNewStartTransform = EditorGUILayout.ObjectField(new GUIContent("Bottom Transform", ""), Transform, typeof(Transform), true) as Transform;
            if (lNewStartTransform != Transform)
            {
                lIsDirty = true;
                Transform = lNewStartTransform;
            }

            Vector3 lNewStartPosition = EditorGUILayout.Vector3Field(new GUIContent("Bottom Offset", ""), Offset);
            if (lNewStartPosition != Offset)
            {
                lIsDirty = true;
                Offset = lNewStartPosition;
            }

            GUILayout.Space(5);

            Transform lNewEndTransform = EditorGUILayout.ObjectField(new GUIContent("Top Transform", ""), EndTransform, typeof(Transform), true) as Transform;
            if (lNewEndTransform != EndTransform)
            {
                lIsDirty = true;
                EndTransform = lNewEndTransform;
            }

            Vector3 lNewEndOffset = EditorGUILayout.Vector3Field(new GUIContent("Top Offset", ""), EndOffset);
            if (lNewEndOffset != EndOffset)
            {
                lIsDirty = true;
                EndOffset = lNewEndOffset;
            }

            return lIsDirty;
        }

#endif

    }
}

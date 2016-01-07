using System;
using UnityEngine;

namespace com.ootii.Geometry
{
    /// <summary>
    /// Extension for the standard Transform that allows us to add functions
    /// </summary>
    public static class TransformExt
    {
        /// <summary>
        /// Recursively searches for a bone given the name and returns it if found
        /// </summary>
        /// <param name="rParent">Parent to search through</param>
        /// <param name="rBoneName">Bone to find</param>
        /// <returns>Transform of the bone or null</returns>
        public static Transform FindTransform(this Transform rThis, HumanBodyBones rBone)
        {
            Animator lAnimator = rThis.gameObject.GetComponent<Animator>();
            if (lAnimator != null) { return lAnimator.GetBoneTransform(rBone); }

            return null;
        }

        /// <summary>
        /// Recursively searches for a bone given the name and returns it if found
        /// </summary>
        /// <param name="rParent">Parent to search through</param>
        /// <param name="rBoneName">Bone to find</param>
        /// <returns>Transform of the bone or null</returns>
        public static Transform FindTransform(this Transform rThis, string rName)
        {
            return FindChildTransform(rThis, rName);
        }

        /// <summary>
        /// Recursively search for a bone that matches the specifie name
        /// </summary>
        /// <param name="rParent">Parent to search through</param>
        /// <param name="rBoneName">Bone to find</param>
        /// <returns></returns>
        public static Transform FindChildTransform(Transform rParent, string rName)
        {
            // We found it. Get out fast
            if (rParent.name == rName) { return rParent; }

            // Handle the case where the bone name is nested in a namespace
            int lIndex = rParent.name.IndexOf(':');
            if (lIndex >= 0)
            {
                string lParentName = rParent.name.Substring(lIndex + 1);
                if (lParentName == rName) { return rParent; }
            }

            // Since we didn't find it, check the children
            for (int i = 0; i < rParent.transform.childCount; i++)
            {
                Transform lTransform = FindChildTransform(rParent.transform.GetChild(i), rName);
                if (lTransform != null) { return lTransform; }
            }

            // Return nothing
            return null;
        }
    }
}

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Microsoft.MixedReality.WorldLocking.Core
{
    public struct NearbyAnchorsData
    {
        // public int NumAnchors;
        // public int NumInvalidAnchors;
        public List<AnchorId> InnerSphereAnchorIds;
        public List<AnchorId> OuterSphereAnchorIds;
        public List<AnchorId> InnerSphereInvalidAnchorIds;
        public List<AnchorId> OuterSphereInvalidAnchorIds;
    }

    public enum AnchorQualityFunction
    {
        NumHops = 0,
        PoseLinkDistanceRatio = 1
    }

    public struct AnchorQualityMetricData
    {
        public AnchorQualityFunction AnchorQualityFunction;
        public float Threshold;
    }

    /// <summary>
    /// Create and persist a network of anchors around the camera as it moves around,
    /// and feed them into the plugin.
    /// </summary>
    public interface IAnchorManager : System.IDisposable
    {
        /// <summary>
        /// Fired when there are no anchors < 5 hops within inner sphere and
        /// we previously placed anchors
        /// </summary>
        // public event EventHandler<NearbyAnchorsData> NearbyAnchorsUnreliable;

        /// <summary>
        /// Whether the underlying anchors can be locally persisted and reloaded.
        /// </summary>
        bool SupportsPersistence { get; }

        /// <summary>
        /// Return the current number of spongy anchors.
        /// </summary>
        /// <remarks>
        /// The number of anchors known to AnchorManager should always be identical to the
        /// frozen anchors known to the engine.
        /// </remarks>
        int NumAnchors { get; }
        
        /// <summary>
        /// The number of edges connecting spongy anchors.
        /// </summary>
        int NumEdges { get; }

        /// <summary>
        /// Error string for last error, cleared at beginning of each update.
        /// </summary>
        string ErrorStatus { get; }

        /// <summary>
        /// Minimum distance of head to nearest anchor to create a new anchor.
        /// </summary>
        float MinNewAnchorDistance { get; set; }

        /// <summary>
        /// Maximum distance between two anchors to create an edge between them.
        /// </summary>
        /// <remarks>
        /// Note that the MaxAnchorEdgeLength should be longer than the MinAnchorDistance,
        /// or else anchors will not be connected into a graph as they are created.
        /// </remarks>
        float MaxAnchorEdgeLength { get; set; }

        /// <summary>
        /// Maximum number of local anchors in the internal anchor graph.
        /// </summary>
        /// <remarks>
        /// Zero or negative means unlimited anchors.
        /// </remarks>
        int MaxLocalAnchors { get; set; }

        /// <summary>
        /// Get the transform from spongy space to the space anchors are located in.
        /// </summary>
        /// <remarks>
        /// This varies according to the design of the underlying platform anchor subsystem.
        /// It may also vary over time (so don't cache this).
        /// </remarks>
        UnityEngine.Pose AnchorFromSpongy { get; }

        List<AnchorId> InnerSphereAnchorIds { get; set;  }
        List<AnchorId> OuterSphereAnchorIds { get;  set; }
        List<AnchorId> InnerSphereInvalidAnchorIds { get; set;  }
        List<AnchorId> OuterSphereInvalidAnchorIds { get;  set; }

        AnchorQualityMetricData AnchorQualityMetric { get; set; }

        /// <summary>
        /// Delete all spongy anchor objects and reset internal state
        /// </summary>
        void Reset();

        /// <summary>
        /// Create any needed anchors/edges and update plugin
        /// </summary>
        /// <returns>Whether any anchors are active and were updated.</returns>
        bool Update();

        /// <summary>
        /// Save the spongy anchors to persistent storage
        /// </summary>
        Task SaveAnchors();

        /// <summary>
        /// Load the spongy anchors from persistent storage
        /// </summary>
        /// <remarks>
        /// The set of spongy anchors loaded by this routine is defined by the frozen anchors
        /// previously loaded into the plugin.
        /// 
        /// Likewise, when a spongy anchor fails to load, this routine will delete its frozen
        /// counterpart from the plugin.
        /// </remarks>
        Task LoadAnchors();

    }
}
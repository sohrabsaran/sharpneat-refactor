﻿using SharpNeat.Network;

namespace SharpNeat.Neat.Genome
{
    /// <summary>
    /// A gene that represents a single connection between two neurons in NEAT.
    /// </summary>
    public struct ConnectionGene<T>
        where T : struct
    {
        #region Auto Properties

        public int Id { get; }
        public int SourceId { get; }
        public int TargetId { get; }
        public T Weight { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Copy constructor.
        /// </summary>
        public ConnectionGene(ConnectionGene<T> copyFrom)
        {
            this.Id = copyFrom.Id;
            this.SourceId = copyFrom.SourceId;
            this.TargetId = copyFrom.TargetId;
            this.Weight = copyFrom.Weight;
        }

        /// <summary>
        /// Construct a new ConnectionGene with the specified source and target neurons and connection weight.
        /// </summary>
        public ConnectionGene(int id, int sourceId, int targetId, T weight)
        {
            this.Id = id;
            this.SourceId = sourceId;
            this.TargetId = targetId;
            this.Weight = weight;
        }

        #endregion

        #region Public Static Methods

        public static ConnectionGene<T>[] CloneArray(ConnectionGene<T>[] connArr)
        {
            var arr = new ConnectionGene<T>[connArr.Length];
            for (int i = 0; i <connArr.Length; i++) {
                arr[i] = new ConnectionGene<T>(connArr[i]);
            }
            return arr;
        }

        #endregion
    }
}

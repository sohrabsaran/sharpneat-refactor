﻿/* ***************************************************************************
 * This file is part of SharpNEAT - Evolution of Neural Networks.
 * 
 * Copyright 2004-2019 Colin Green (sharpneat@gmail.com)
 *
 * SharpNEAT is free software; you can redistribute it and/or modify
 * it under the terms of The MIT License (MIT).
 *
 * You should have received a copy of the MIT License
 * along with SharpNEAT; if not, see https://opensource.org/licenses/MIT.
 */
using System.Collections.Generic;
using SharpNeat.Neat.Genome;
using SharpNeat.Network;

namespace SharpNeat.Neat.Reproduction.Sexual.Strategy.UniformCrossover
{
    internal partial class ConnectionGeneListBuilder<T>
        where T : struct
    {
        #region Instance Fields

        // Indicates that we are building acyclic networks.
        readonly bool _isAcyclic;
        readonly CyclicConnectionTest _cyclicTest;

        // Connection gene lists.
        List<DirectedConnection> _connList;
        List<T> _weightList;
        
        #endregion

        #region Constructor
        
        public ConnectionGeneListBuilder(bool isAcyclic, int capacity)
        {
            _isAcyclic = isAcyclic;
            if(_isAcyclic) {
                _cyclicTest = new CyclicConnectionTest();
            }

            _connList = new List<DirectedConnection>(capacity);
            _weightList = new List<T>(capacity);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Add a Gene to the builder, but only if the connection is not already present (as determined by its source and target ID endpoints).
        /// </summary>
        /// <param name="gene">The connection gene to add.</param>
        /// <param name="isSecondaryGene">Indicates if the gene is from the secondary parent.</param>
        public void TryAddGene(ConnectionGene<T> gene, bool isSecondaryGene)
        {
            // For acyclic networks, check if the connection gene would create a cycle in the new genome; if so then reject it.
            //
            // Notes. 
            // This applies only when evolving acyclic networks.
            //
            // Both parent genomes are guaranteed to be acyclic, and since we take all genes from the primary parent, the child genome
            // is essentially the primary parent plus some genes from the secondary parent, therefore this check only needs to be
            // performed when adding genes from the secondary parent.
            //
            // A cyclicity test is relatively expensive, therefore we avoid it if at all possible.
            if(isSecondaryGene && _isAcyclic && IsCyclicConnection(gene)) {
                return;
            }

            // We are free to add the gene.
            AddGene(gene);
        }

        public ConnectionGenes<T> ToConnectionGenes()
        {
            return new ConnectionGenes<T>(
                _connList.ToArray(),
                _weightList.ToArray());
        }

        public void Clear()
        {
            _connList.Clear();
            _weightList.Clear();
        }

        #endregion

        #region Private Methods

        private void AddGene(ConnectionGene<T> gene)
        {
            _connList.Add(gene.Endpoints);
            _weightList.Add(gene.Weight);
        }

        private bool IsCyclicConnection(ConnectionGene<T> gene)
        {
            return _cyclicTest.IsConnectionCyclic(_connList, gene.Endpoints);
        }

        #endregion
    }
}

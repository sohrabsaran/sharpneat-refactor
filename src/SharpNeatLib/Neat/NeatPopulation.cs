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
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Redzen.Random;
using Redzen.Sorting;
using Redzen.Structures;
using SharpNeat.EvolutionAlgorithm;
using SharpNeat.Neat.EvolutionAlgorithm;
using SharpNeat.Neat.Genome;
using SharpNeat.Neat.Speciation;
using static SharpNeat.Neat.NeatPopulationUtils;

namespace SharpNeat.Neat
{
    public class NeatPopulation<T> : Population<NeatGenome<T>>
        where T : struct
    {
        #region Consts / Statics

        // ENHANCEMENT: Consider increasing buffer capacity, and different capacities for the two different buffers.
        const int __defaultInnovationHistoryBufferSize = 0x20000; // = 131,072.

        #endregion

        #region Auto Properties

        /// <summary>
        /// NeatGenome metadata.
        /// </summary>
        public MetaNeatGenome<T> MetaNeatGenome { get; }

        /// <summary>
        /// NeatGenome builder.
        /// </summary>
        public INeatGenomeBuilder<T> GenomeBuilder { get; }

        /// <summary>
        /// Genome ID sequence; for obtaining new genome IDs.
        /// </summary>
        public Int32Sequence GenomeIdSeq { get; }

        /// <summary>
        /// Innovation ID sequence; for obtaining new innovation IDs.
        /// </summary>
        public Int32Sequence InnovationIdSeq { get; }

        /// <summary>
        /// A history buffer of added nodes.
        /// Used when adding new nodes to check if an identical node has been added to a genome elsewhere in the population.
        /// This allows re-use of the same innovation ID for like nodes.
        /// </summary>
        public AddedNodeBuffer AddedNodeBuffer { get; }

        /// <summary>
        /// Species array.
        /// </summary>
        public Species<T>[] SpeciesArray { get; set; }

        #endregion

        #region Auto Properties [Population Statistics]

        /// <summary>
        /// Index of the best genome.
        /// </summary>
        public int BestGenomeIdx { get; set; }

        /// <summary>
        /// Index of the species that the best genome is within.
        /// </summary>
        public int BestGenomeSpeciesIdx { get; set; }

        /// <summary>
        /// Sum of species fitness means.
        /// </summary>
        public double TotalSpeciesMeanFitness { get; set; }

        #endregion

        #region Constructors

        public NeatPopulation(
            MetaNeatGenome<T> metaNeatGenome,
            INeatGenomeBuilder<T> genomeBuilder,
            List<NeatGenome<T>> genomeList) 
            : base(genomeList)
        {
            GetMaxObservedIds(genomeList, out int maxGenomeId, out int maxInnovationId);

            this.MetaNeatGenome = metaNeatGenome ?? throw new ArgumentNullException(nameof(metaNeatGenome));
            this.GenomeBuilder = genomeBuilder ?? throw new ArgumentNullException(nameof(genomeBuilder));
            this.GenomeIdSeq = new Int32Sequence(maxGenomeId + 1);
            this.InnovationIdSeq = new Int32Sequence(maxInnovationId + 1);
            this.AddedNodeBuffer = new AddedNodeBuffer(__defaultInnovationHistoryBufferSize);            
        }

        public NeatPopulation(
            MetaNeatGenome<T> metaNeatGenome,
            INeatGenomeBuilder<T> genomeBuilder,
            List<NeatGenome<T>> genomeList,
            Int32Sequence genomeIdSeq,
            Int32Sequence innovationIdSeq)
        : this(metaNeatGenome, genomeBuilder, genomeList, genomeIdSeq, innovationIdSeq, __defaultInnovationHistoryBufferSize, __defaultInnovationHistoryBufferSize)
        {}

        public NeatPopulation(
            MetaNeatGenome<T> metaNeatGenome,
            INeatGenomeBuilder<T> genomeBuilder,
            List<NeatGenome<T>> genomeList,
            Int32Sequence genomeIdSeq,
            Int32Sequence innovationIdSeq,
            int addedConnectionHistoryBufferSize,
            int addedNodeHistoryBufferSize)
        : base(genomeList)
        {
            this.MetaNeatGenome = metaNeatGenome ?? throw new ArgumentNullException(nameof(metaNeatGenome));
            this.GenomeBuilder = genomeBuilder ?? throw new ArgumentNullException(nameof(genomeBuilder));
            this.GenomeIdSeq = genomeIdSeq ?? throw new ArgumentNullException(nameof(genomeIdSeq));
            this.InnovationIdSeq = innovationIdSeq ?? throw new ArgumentNullException(nameof(innovationIdSeq));;
            this.AddedNodeBuffer = new AddedNodeBuffer(addedNodeHistoryBufferSize);

            // Assert that the ID sequences have a current IDs higher than any existing ID.
            Debug.Assert(ValidateIdSequences(genomeList, genomeIdSeq, innovationIdSeq));
        }

        #endregion

        #region Public Methods

        // TODO: Make this private and call from constructor?
        public void InitialiseSpecies(
            ISpeciationStrategy<NeatGenome<T>,T> speciationStrategy,
            int speciesCount,
            IRandomSource rng)
        {
            // Allocate the genomes to species.
            Species<T>[] speciesArr = speciationStrategy.SpeciateAll(this.GenomeList, speciesCount, rng);
            if(null == speciesArr || speciesArr.Length != speciesCount) {
                throw new Exception("Species array is null or has incorrect length.");
            }
            this.SpeciesArray = speciesArr;

            // Sort the genomes in each species. Highest fitness first, then secondary sorted by youngest genomes first.
            foreach(Species<T> species in speciesArr) {
                SortUtils.SortUnstable(species.GenomeList, GenomeFitnessAndAgeComparer<T>.Singleton, rng);
            }
        }

        #endregion
    }
}

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
using System.Linq;
using SharpNeat.Neat.DistanceMetrics;
using SharpNeat.Neat.Genome;

namespace SharpNeat.Neat.Speciation.Parallelized
{
    /// <summary>
    /// Static utility methods related to speciation.
    /// 
    /// Parallel execution versions of methods in SpeciationUtils class.
    /// </summary>
    public static class SpeciationUtilsParallel
    {
        #region Public Methods [Empty Species Handling]

        public static void PopulateEmptySpecies<T>(
            IDistanceMetric<T> distanceMetric,
            Species<T>[] emptySpeciesArr,
            Species<T>[] speciesArr)
        where T : struct
        {
            foreach(Species<T> emptySpecies in emptySpeciesArr)
            {
                // Get and remove a genome from a species with many genomes.
                var genome = GetGenomeForEmptySpecies(distanceMetric, speciesArr);

                // Add the genome to the empty species.
                emptySpecies.GenomeById.Add(genome.Id, genome);

                // Update the centroid. There's only one genome so it is the centroid.
                emptySpecies.Centroid = genome.ConnectionGenes;
            }
        }

        private static NeatGenome<T> GetGenomeForEmptySpecies<T>(
            IDistanceMetric<T> distanceMetric,
            Species<T>[] speciesArr)
        where T : struct
        {
            // Get the species with the highest number of genomes.
            Species<T> species = speciesArr.Aggregate((x, y) => x.GenomeById.Count > y.GenomeById.Count ?  x : y);

            // Get the genome furthest from the species centroid.
            var genome = species.GenomeById.Values.AsParallel().Aggregate((x, y) => distanceMetric.CalcDistance(species.Centroid, x.ConnectionGenes) > distanceMetric.CalcDistance(species.Centroid, y.ConnectionGenes) ? x : y);

            // Remove the genome from its current species.
            species.GenomeById.Remove(genome.Id);

            // Update the species centroid.
            species.Centroid = distanceMetric.CalculateCentroid(species.GenomeById.Values.Select(x => x.ConnectionGenes));

            // Return the selected genome.
            return genome;
        }

        #endregion
    }
}

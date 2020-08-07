﻿using System.Text.Json;
using SharpNeat.Neat.EvolutionAlgorithm;
using Xunit;

namespace SharpNeatLib.Tests.EvolutionAlgorithm
{
    public class NeatEvolutionAlgorithmSettingsJsonReaderTests
    {
        [Fact]
        public void Read()
        {
            JsonDocument jdoc = JsonDocument.Parse(
@"{
    ""speciesCount"":1111,
    ""elitismProportion"":0.11,
    ""selectionProportion"":0.22,
    ""offspringAsexualProportion"":0.33,
    ""offspringSexualProportion"":0.44,
    ""interspeciesMatingProportion"":0.55,
    ""statisticsMovingAverageHistoryLength"":2222
}");
            // Init a default settings object.
            var eaSettings = new NeatEvolutionAlgorithmSettings();

            // Read json properties into the settings object.
            NeatEvolutionAlgorithmSettingsJsonReader.Read(eaSettings, jdoc.RootElement);

            // Assert the expected values.
            Assert.Equal(1111, eaSettings.SpeciesCount);
            Assert.Equal(0.11, eaSettings.ElitismProportion);
            Assert.Equal(0.22, eaSettings.SelectionProportion);
            Assert.Equal(0.33, eaSettings.OffspringAsexualProportion);
            Assert.Equal(0.44, eaSettings.OffspringSexualProportion);
            Assert.Equal(0.55, eaSettings.InterspeciesMatingProportion);
            Assert.Equal(2222, eaSettings.StatisticsMovingAverageHistoryLength);
        }
    }
}
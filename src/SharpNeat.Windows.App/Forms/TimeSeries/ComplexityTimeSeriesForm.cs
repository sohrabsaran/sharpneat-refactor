﻿/* ***************************************************************************
 * This file is part of SharpNEAT - Evolution of Neural Networks.
 * 
 * Copyright 2004-2020 Colin Green (sharpneat@gmail.com)
 *
 * SharpNEAT is free software; you can redistribute it and/or modify
 * it under the terms of The MIT License (MIT).
 *
 * You should have received a copy of the MIT License
 * along with SharpNEAT; if not, see https://opensource.org/licenses/MIT.
 */
using System.Drawing;
using SharpNeat.EvolutionAlgorithm;
using ZedGraph;
using static SharpNeat.Windows.App.Forms.ZedGraphUtils;

namespace SharpNeat.Windows.App.Forms.TimeSeries
{
    public class ComplexityTimeSeriesForm : StatsGraphForm
    {
        const int __HistoryLength = 1_000;
        readonly RollingPointPairList _bestPpl;
        readonly RollingPointPairList _meanPpl;

        #region Constructor

        public ComplexityTimeSeriesForm()
            : base("Complexity (Best and Mean)", "Generation", "Complexity", null)
        {
            _bestPpl = new RollingPointPairList(__HistoryLength);
            LineItem lineItem = _graphPane.AddCurve("Best",  _bestPpl, Color.FromArgb(0x00, 0x5b, 0x31), SymbolType.None);
            ApplyLineStyle(lineItem);

            _meanPpl = new RollingPointPairList(__HistoryLength);
            lineItem = _graphPane.AddCurve("Mean",  _meanPpl, Color.FromArgb(0x9c, 0xd0, 0x88), SymbolType.None);
            ApplyLineStyle(lineItem);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Update the time series data.
        /// </summary>
        /// <param name="eaStats">Evolution algorithm statistics object.</param>
        /// <param name="popStats">Population statistics object.</param>
        public override void UpdateData(
            EvolutionAlgorithmStatistics eaStats,
            PopulationStatistics popStats)
        {
            _bestPpl.Add(eaStats.Generation, popStats.BestComplexity);
            _meanPpl.Add(eaStats.Generation, popStats.MeanComplexity);
            RefreshGraph();
        }

        /// <summary>
        /// Clear the time series data.
        /// </summary>
        public override void Clear()
        {
            _bestPpl.Clear();
            _meanPpl.Clear();
            RefreshGraph();
        }

        #endregion
    }
}

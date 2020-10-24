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
using System.Windows.Forms;
using SharpNeat.EvolutionAlgorithm;
using ZedGraph;

namespace SharpNeat.Windows.App.Forms
{
    /// <summary>
    /// Form for displaying a graph plot of time series data (e.g. best genome fitness per generation).
    /// </summary>
    public partial class GraphForm : Form
    {
        readonly protected GraphPane _graphPane;

        #region Constructor

        /// <summary>
        /// Construct the form with the provided details and data sources.
        /// </summary>
        public GraphForm(
            string title,
            string xAxisTitle,
            string y1AxisTitle,
            string y2AxisTitle)
        {
            InitializeComponent();

            this.Text = $"SharpNEAT - {title}";

            _graphPane = zed.GraphPane;
            _graphPane.Title.Text = title;

			_graphPane.XAxis.Title.Text = xAxisTitle;
			_graphPane.XAxis.MajorGrid.IsVisible = true;

			_graphPane.YAxis.Title.Text = y1AxisTitle;
			_graphPane.YAxis.MajorGrid.IsVisible = true;

            if(y2AxisTitle is object)
            {
                _graphPane.Y2Axis.Title.Text = y2AxisTitle;
			    _graphPane.Y2Axis.MajorGrid.IsVisible = false;
                _graphPane.Y2Axis.IsVisible = true;
            }
        }

        #endregion

        #region Public Methods

        // Note. These methods could be defined as abstract, but that would prevent the Window Forms UI designer from working;
        // so instead they are defined as virtuals method with no implementation.

        /// <summary>
        /// Update the time series data.
        /// </summary>
        /// <param name="eaStats">Evolution algorithm statistics object.</param>
        /// <param name="popStats">Population statistics object.</param>
        public virtual void UpdateData(
            EvolutionAlgorithmStatistics eaStats,
            PopulationStatistics popStats)
        {
        }

        /// <summary>
        /// Clear the time series data.
        /// </summary>
        public virtual void Clear()
        {

        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Recalc the axis scales based on the current data, and call Refresh() to redraw the graph.
        /// </summary>
        protected void RefreshGraph()
        {
            _graphPane.AxisChange();
            zed.Refresh();
        }

        #endregion
    }
}
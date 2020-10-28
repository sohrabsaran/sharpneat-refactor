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

namespace SharpNeat.Windows.App
{
    public struct Point2DDouble
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Point2DDouble(double x, double y)
        {
            X = x;
            Y = y;
        }
    }
}

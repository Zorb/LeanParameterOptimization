﻿using ChartJs.Blazor.ChartJS.Common.Enums;
using System.Collections.Generic;

namespace ChartJs.Blazor.ChartJS.LineChart
{
    /// <summary>
    /// This is a helper class containing pre-made display formats to use in <see cref="TimeOptions.DisplayFormats"/> for certain locales.
    /// <para>Those <see cref="TimeMeasurement"/>s that are not defined, will use the default format from https://www.chartjs.org/docs/latest/axes/cartesian/time.html#display-formats </para>
    /// </summary>
    public static class TimeDisplayFormats
    {

        public static Dictionary<TimeMeasurement, string> Default => new Dictionary<TimeMeasurement, string>
        {
            { TimeMeasurement.Millisecond, "HH:mm:ss.SSS" },
            { TimeMeasurement.Second, "HH:mm:ss" },
            { TimeMeasurement.Minute, "HH:mm" },
            { TimeMeasurement.Hour, "HH:00" },
            { TimeMeasurement.Day, "DD/MM/YYYY hh:mm" }
        };
    }
}

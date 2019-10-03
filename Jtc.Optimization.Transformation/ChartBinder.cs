﻿using ChartJs.Blazor.ChartJS.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Jtc.Optimization.Transformation
{
    public class ChartBinder
    {

        private double? Min { get; set; }
        private double? Max { get; set; }

        public async Task<Dictionary<string, List<Point>>> Read(StreamReader reader, int sampleRate = 1, bool disableNormalization = false, DateTime? minimumDate = null)
        {
            minimumDate = minimumDate ?? DateTime.MinValue;

            var data = new Dictionary<string, List<Point>>();
            var rand = new Random();
            string line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                if (sampleRate > 1 && rand.Next(0, sampleRate) != 0)
                {
                    continue;
                }

                try
                {
                    var split = line.Split(',').Select(s => s.Trim()).ToArray();
                    var time = DateTime.Parse(split[0].Substring(0, 24));

                    if (time < minimumDate)
                    {
                        continue;
                    }

                    foreach (var item in split.Skip(1))
                    {
                        if (item.Contains(": ") && !item.StartsWith("Start") && !item.StartsWith("End"))
                        {
                            var pair = item.Split(new[] { ": " }, StringSplitOptions.RemoveEmptyEntries);
                            if (!double.TryParse(pair[1], out var parsed))
                            {
                                continue;
                            }
                            if (!data.ContainsKey(pair[0]))
                            {
                                data.Add(pair[0], new List<Point>());
                            }
                            data[pair[0]].Add(new Point(time.Ticks, double.Parse(pair[1])));
                        }

                    }
                    //System.Diagnostics.Debug.WriteLine("Processing...");
                }
                catch (Exception)
                {
                    Console.WriteLine("Failed to parse:" + line);
                }
            }

            if (!disableNormalization)
            {
                var fitness = data.Last().Value;
                var nonEmpty = data.Take(data.Count() - 1).Where(d => d.Value.Any());

                //on second pass reuse min/max
                if (Max == null || Min == null)
                {
                    Max = fitness.Max(m => m.y);
                    Min = fitness.Min(m => m.y);
                }
                var normalizer = new SharpLearning.FeatureTransformations.Normalization.LinearNormalizer();

                foreach (var list in nonEmpty)
                {

                    var oldMax = list.Value.Max(m => m.y);
                    var oldMin = list.Value.Min(m => m.y);
                    foreach (var point in list.Value)
                    {
                        point.y = normalizer.Normalize(Min.Value, Max.Value, oldMin, oldMax, point.y);
                    }
                }

                data = nonEmpty.Concat(new[] { data.Last() }).ToDictionary(k => k.Key, v => v.Value);
            }

            return data;
        }

    }
}

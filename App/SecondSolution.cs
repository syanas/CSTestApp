using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace App
{
    public class SecondSolution
    {
        public class FullDataRow
        {
            public UInt64 Timestamp { get; set; }
            public Double Amp { get; set; }
            public Int64 Temperature { get; set; }
        }
        public class OutDataRow
        {
            public UInt64 Timestamp { get; set; }
            public Double Amp { get; set; }
        }

        //-----IO-----
        public class IO {
            public static void ReadContents(char split_symbol, string input_fname, List<FullDataRow> data_out)
            {
                using (var reader = new StreamReader(input_fname))
                {
                    string s;
                    s = reader.ReadLine();
                    if (s != null) s = reader.ReadLine(); // пропустить header

                    while (s != null)
                    {
                        var x = s.Split(split_symbol);

                        data_out.Add(new FullDataRow
                        {
                            Timestamp = UInt64.Parse(x[0]),
                            Amp = Double.Parse(x[1]),
                            Temperature = Int64.Parse(x[2])
                        });

                        s = reader.ReadLine();
                    }
                }
            }

            public static void WriteContents(char split_symbol, string output_fname, List<OutDataRow> data_in)
            {
                using (var writer = new StreamWriter(output_fname))
                {
                    writer.WriteLine(string.Format("{0}" + split_symbol + "{1}", "timestamp", "amp"));
                    data_in.ForEach(delegate (OutDataRow elem) {
                        writer.WriteLine(string.Format("{0}" + split_symbol + "{1}", elem.Timestamp, elem.Amp));
                    });
                }
            }

        }

        //-----Smooth-----
        public class DataManipulator {
            
            static Double SmoothingFunction(Double a, Double x_n, Double x_n_plus_1)
            {
                System.Diagnostics.Debug.Assert(a >= 0 && a <= 1);
                return a * x_n + (1 - a) * x_n_plus_1;
            }

            public static void SmoothData(Double a, List<Double> input_data, List<Double> output_data)
            {
                for (int i = 0; i < input_data.Count - 1; ++i)
                {
                    Double elem = input_data[i];
                    Double elem_next = input_data[i + 1];
                    output_data.Add(SmoothingFunction(a, elem, elem_next));
                }
                Double elemlast = input_data[input_data.Count - 1];
                output_data.Add(elemlast);
            }
        }

        //-----Extrema-----
        public class ExtremaFinder {
            static int getMeanIndex(int index_first, int index_last) {
                return (index_first + index_last) / 2;
            }

            static void ProcessStepForMin<T>(T previous_elem, T current_elem,
            int current_index, int last_index, List<int> min_candidates, List<int> minimums) where T : IComparable<T>
            {
                int comparison_result = current_elem.CompareTo(previous_elem);
                if (current_index == 1 && comparison_result > 0) //проверка нулевого 
                {
                    minimums.Add(0);
                }
                if (current_index == last_index && comparison_result < 0) //проверка последнего 
                {
                    minimums.Add(last_index);
                }
                if (min_candidates.Count != 0)
                {
                    if (comparison_result < 0) //начало плато
                    {
                        min_candidates.Clear();
                        min_candidates.Add(current_index);
                    }
                    // (comparison_result == 0) //середина плато
                    else if (comparison_result > 0) //конец плато
                    {
                        int mean_index = getMeanIndex(min_candidates[0], min_candidates[min_candidates.Count - 1]);
                        minimums.Add(mean_index);
                        min_candidates.Clear();
                    }
                }
                else
                {
                    if (comparison_result < 0)
                    {
                        min_candidates.Add(current_index);
                    }
                }
            }

            static void ProcessStepForMax<T>(T previous_elem, T current_elem,
                int current_index, int last_index, List<int> max_candidates, List<int> maximums) where T : IComparable<T>
            {
                int comparison_result = current_elem.CompareTo(previous_elem);
                if (current_index == 1 && comparison_result < 0) //проверка нулевого 
                {
                    maximums.Add(0);
                }
                if (current_index == last_index && comparison_result > 0) //проверка последнего 
                {
                    maximums.Add(last_index);
                }
                if (max_candidates.Count != 0)
                {
                    if (comparison_result > 0) //начало плато
                    {
                        max_candidates.Clear();
                        max_candidates.Add(current_index);
                    }
                    // (comparison_result == 0) //середина плато
                    else if (comparison_result < 0) //конец плато
                    {
                        int mean_index = getMeanIndex(max_candidates[0], max_candidates[max_candidates.Count-1]);
                        maximums.Add(mean_index);
                        max_candidates.Clear();
                    }
                }
                else
                {
                    if (comparison_result > 0)
                    {
                        max_candidates.Add(current_index);
                    }
                }
            }

            public static void GetNonStrictExtremaIndices<T>(List<T> data_sequence, List<int> extrema_indices) where T : IComparable<T>
            {
                if (data_sequence.Count < 2) return;

                List<int> minimums = new List<int>();
                List<int> min_candidates = new List<int>();

                List<int> maximums = new List<int>();
                List<int> max_candidates = new List<int>();

                int data_sequence_last_index = data_sequence.Count - 1;
                for (int i = 1; i < data_sequence.Count; ++i)
                {
                    T previous_elem = data_sequence[i - 1];
                    T current_elem = data_sequence[i];
                    ProcessStepForMin(previous_elem, current_elem, i, data_sequence_last_index, min_candidates, minimums);
                    ProcessStepForMax(previous_elem, current_elem, i, data_sequence_last_index, max_candidates, maximums);
                }

                extrema_indices.Clear();
                extrema_indices.AddRange(minimums);
                extrema_indices.AddRange(maximums);
                extrema_indices.Sort();
            }
        }

        //-----Preparations for output-----
        static void GetElemsWithExtremaFromInitAndSmoothedProjection(List<int> extrema_indices_in_init, List<FullDataRow> init_data,
            List<Double> smoothed_init_data_projection, List<OutDataRow> res_data)
        {
            res_data.Clear();
            for (int i = 0; i < extrema_indices_in_init.Count; ++i)
            {
                res_data.Add(new OutDataRow
                {
                    Timestamp = init_data[extrema_indices_in_init[i]].Timestamp,
                    Amp = smoothed_init_data_projection[extrema_indices_in_init[i]]
                });
            }
        }

        //-----Solution External Interface-----
        public static void ProcessFileToFindNonStrictExtrema(String input_fname, String output_fname, 
            Double smoothing_parameter, char csv_split_symbol)
        {
            var init_data = new List<FullDataRow>();
            IO.ReadContents(csv_split_symbol, input_fname, init_data);

            var list_projection = new List<Double>();
            init_data.ForEach(delegate (FullDataRow elem) {
                list_projection.Add(elem.Amp);
            });

            if (init_data.Count > 1)  
            {
                var smoothed_data = new List<Double>();
                DataManipulator.SmoothData(smoothing_parameter, list_projection, smoothed_data);

                var extrema_indices = new List<int>();
                ExtremaFinder.GetNonStrictExtremaIndices(smoothed_data, extrema_indices);

                var res_data = new List<OutDataRow>();
                GetElemsWithExtremaFromInitAndSmoothedProjection(extrema_indices, init_data, smoothed_data, res_data);

                IO.WriteContents(csv_split_symbol, output_fname, res_data);
            }
            // считаю, что у 1 точки нет окрестности
        }


    }
}

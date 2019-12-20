using SeqAlign.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using static SequenceExt;

namespace SeqAlign.Models
{
    public class RegionsModel
    {
        public HashSet<Region> Regions { get; set; } = new HashSet<Region>();
        public List<List<Region>> Rows => Regions.Any() ? GetRows() : new List<List<Region>>();
        public decimal MinRegionStart => Regions.Min(x => x.Start);
        public decimal MaxRegionEnd => Regions.Max(x => x.End);
        public string FileName { get; set; }

        private List<List<Region>> GetRows()
        {
            var rows = new List<List<Region>>();

            var regionsBucket = Regions
                .ToList()
                .ConvertAll(x =>
                new Region()
                {
                    Id = x.Id,
                    Start = x.Start,
                    End = x.End
                });

            var firstRow = new List<Region>();
            firstRow.Add(regionsBucket.OrderBy(x=>x.Start).FirstOrDefault());
            regionsBucket.Remove(regionsBucket.FirstOrDefault());
            rows.Add(firstRow);
            var row = rows.FirstOrDefault();

            while (regionsBucket.Any())
            {
                var next = row.Last().FindNextClosestNonOverlappingRegion(regionsBucket);
                if (next is null)
                {
                    rows.Add(new List<Region>(row));
                    row.Clear();
                    row.Add(regionsBucket.OrderBy(x=>x.Start).FirstOrDefault());
                    regionsBucket.Remove(regionsBucket.OrderBy(x => x.Start).FirstOrDefault());
                    continue;
                }
                row.Add(next);                    
                regionsBucket.Remove(next);
            }
            
            return rows
                .OrderBy(x=>x.Min(r=>r.Start))
                .ThenBy(y=>y.Min(r=>r.End-r.Start))
                .ToList();

        }
    }

    public static class RegionExt
    {
        public static Region FindNextClosestNonOverlappingRegion(this Region region, List<Region> list)
        {
            var nearest = list
                .Where(r => r.Start > region.End) // non overlapping
                .OrderByDescending(r => region.End - r.Start).FirstOrDefault();  // find nearest region
                //.ThenByDescending(r => r.End - r.Start);      // then prefer longest

            return nearest;
        }

        public static bool PositionIsInRow(this decimal position, List<Region> row)
        {
            return row.Any(reg => position >= reg.Start && position <= reg.End);
        }
    }
}



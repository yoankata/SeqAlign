using SeqAlign.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SeqAlign.Models
{
    public class RegionsModel
    {
        public StackingMethod Method { get; set; } = StackingMethod.RegularStacking;
        public HashSet<Region> Regions { get; set; } = new HashSet<Region>();
        public List<List<Region>> Rows { get; } = new List<List<Region>>();
        public decimal MinRegionStart => Regions.Min(x => x.Start);
        public decimal MaxRegionEnd => Regions.Max(x => x.End);

        public RegionsModel()
        {

        }

        public RegionsModel(ICollection<Region> regions, StackingMethod method = StackingMethod.RegularStacking)
        {
            if (regions is null || !regions.Any())
                throw new ArgumentException("Regions are required to instantiate a regions model!");

            Regions = regions.ToHashSet();
            Method = method;
            Rows = Method == StackingMethod.RegularStacking ? GetRows() : GetSegments();
        }

        private List<List<Region>> GetRows()
        {
            var rows = new List<List<Region>>();

            var regionsBucket = Regions
                .ToList()
                .ConvertAll(x => new Region(x.Id, x.Start, x.End));

            var firstRow = new List<Region>();
            firstRow.Add(regionsBucket.OrderBy(x => x.Start).FirstOrDefault());
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
                    row.Add(regionsBucket.OrderBy(x => x.Start).FirstOrDefault());
                    regionsBucket.Remove(regionsBucket.OrderBy(x => x.Start).FirstOrDefault());
                    continue;
                }
                row.Add(next);
                regionsBucket.Remove(next);
            }

            return rows
                .OrderBy(x => x.Min(r => r.Start))
                .ThenBy(y => y.Min(r => r.End - r.Start))
                .ToList();
        }


        private List<List<Region>> GetSegments()
        {
            //there can be no overlapping regions in a single row 
            if (Regions.Count < 2) 
                return new List<List<Region>> { Regions.ToList() };

            var nonOverlappingSegments = new List<Region>();
            
            var regions = Regions
                .OrderBy(x=>x.Start)
                .ThenBy(x=>x.End)
                .ThenBy(x => x.Start - x.End)
                .ToList();
            var r1 = 0;
            var r2 = 1;

            while (OverlappingRegionsExist(regions))
            {
                r1 = 0;
                r2 = 1;
                while (r1 < regions.Count)
                {
                    while (r2 < regions.Count)
                    {
                        if (r1 == r2)
                        {
                            r2++;
                            continue;
                        }

                        if (regions[r1].IsOverlappingRegion(regions[r2]))
                        {
                            nonOverlappingSegments = GetNonOverlappingSegmentsFromRegions(regions[r1], regions[r2]).ToList();
                            var reg1 = regions[r1];
                            var reg2 = regions[r2];
                            regions.Remove(reg1);
                            regions.Remove(reg2);
                            regions.AddRange(nonOverlappingSegments);
                            regions = regions
                                .OrderBy(x => x.Start)
                                .ThenBy(x => x.End)
                                .ThenBy(x => x.Start - x.End)
                                .ToList();

                        }
                        
                        r2++;
                    }

                    r1++;
                    r2 = 0;
                }
            }

            return new List<List<Region>> { regions };
        }

        private bool OverlappingRegionsExist(List<Region> regions)
        {
            foreach (var reg in regions)
            {
                var exists = OverlappingRegionsExistFor(reg, regions);
                if (exists) return true;
            }

            return false;
        }

        private bool OverlappingRegionsExistFor(Region reg, List<Region> regions)
        {
            var otherRegions = regions.ToList();
            otherRegions.Remove(reg);
            if (otherRegions.Exists(r => r.IsOverlappingRegion(reg)))
                return true;

            return false;
        }
        private static IEnumerable<Region> GetNonOverlappingSegmentsFromRegions(Region r1, Region r2)
        {
            var nonOverlappingRegions = new List<Region>();

            if (r1.IsContainedIn(r2)) // r1 is contained in r2
            {
                nonOverlappingRegions = new List<Region>
                {
                    new Region(r2.Start, r1.Start-1, r2.Overlap),
                    new Region(r1.Start, r1.End-1, r1.Overlap + r2.Overlap),
                    new Region(r1.End, r2.End, r2.Overlap)
                };
            }
            else if (r2.IsContainedIn(r1)) // r2 is contained in r1
            {
                nonOverlappingRegions = new List<Region>
                {
                    new Region(r1.Start, r2.Start-1, r1.Overlap),
                    new Region(r2.Start, r2.End-1, r1.Overlap + r2.Overlap),
                    new Region(r2.End, r1.End, r1.Overlap)
                };
            }
            else if (r1.IsAheadOfAndOverlapping(r2)) // r1 is ahead of r2
            {
                nonOverlappingRegions = new List<Region>
                {
                    new Region(r1.Start, r2.Start-1, r1.Overlap),
                    new Region(r2.Start, r1.End-1, r1.Overlap + r2.Overlap),
                    new Region(r1.End, r2.End, r2.Overlap)
                };
            }
            else if (r2.IsAheadOfAndOverlapping(r1)) // r1 is behind r2
            {
                nonOverlappingRegions = new List<Region>
                {
                    new Region(r2.Start, r1.Start, r2.Overlap),
                    new Region(r1.Start, r2.End, r1.Overlap + r2.Overlap),
                    new Region(r2.End, r1.End, r1.Overlap)
                };
            }
            else // no overlap, return original regions
            {
                nonOverlappingRegions = new List<Region>() { r1, r2 };
            }

            return nonOverlappingRegions.Where(r => !r.IsEmpty()); // remove degenerate regions
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



using SeqAlign.Models;

namespace SeqAlign.Utilities
{
    public static class RegionExt
    {
        // r1 is within r2
        public static bool IsContainedIn(this Region r1, Region r2)
        {
            // r1 is contained in r2
            if (r1.Start >= r2.Start && r1.End <= r2.End)
                return true;

            return false;
        }

        // r1 is ahead of r2 and overlapping
        public static bool IsAheadOfAndOverlapping(this Region r1, Region r2)
        {
            if (r1.Start <= r2.Start && r1.End <= r2.End && r1.End >= r2.Start)
                return true;

            return false;
        }

        public static bool IsOverlappingRegion(this Region r, Region reg)
        {
            return r.IsContainedIn(reg)
                || reg.IsContainedIn(r)
                || r.IsAheadOfAndOverlapping(reg)
                || reg.IsAheadOfAndOverlapping(r);
        }
        // r start and end are the same
        public static bool IsEmpty(this Region r)
        {
            if (r.End - r.Start < 0)
                return true;

            return false;
        }
    }

}

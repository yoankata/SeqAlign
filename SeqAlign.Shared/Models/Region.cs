namespace SeqAlign.Shared.Models
{
    public class Region
    {
        public int Id { get; set; }
        public decimal Start { get; set; }
        public decimal End { get; set; }
        public int Overlap { get; set; }

        public Region(int id, decimal start, decimal end)
        {
            Id = id;
            Start = start;
            End = end;
            Overlap = 1;
        }

        public Region(decimal start, decimal end, int overlappingRegions)
        {
            Id = -1;
            Start = start;
            End = end;
            Overlap = overlappingRegions;
        }
    }
}
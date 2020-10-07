using System;

namespace BentleyOttman
{
    public class ResultStructure
    {
        public Guid? Guid { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }

        public override bool Equals(object? obj)
        {
            return this.Guid == ((ResultStructure)obj).Guid 
                && this.StartDateTime == ((ResultStructure)obj).StartDateTime 
                && this.EndDateTime == ((ResultStructure)obj).EndDateTime;
        }
    }
}

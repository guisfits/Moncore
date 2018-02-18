using System.Collections.Generic;

namespace Moncore.CrossCutting.Helpers
{
    public class PropertyMappingValue
    {
        public PropertyMappingValue (IEnumerable<string> destinationProperties, bool revert = false)
        {
            this.DestinationProperties = destinationProperties;
            this.Revert = revert;
        }
        
        public IEnumerable<string> DestinationProperties { get; set; }
        public bool Revert { get; set; }
    }
}
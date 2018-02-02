using System.Collections.Generic;

namespace Moncore.Api.Services {
    public class PropertyMappingValue
    {
        public IEnumerable<string> DestinationProperties { get; set; }
        public bool ReverseOrder { get; set; }

        public PropertyMappingValue (IEnumerable<string> destinationProperties, bool reverseOrder = false) 
        {
            ReverseOrder = reverseOrder;
            DestinationProperties = destinationProperties;
        }
    }
}
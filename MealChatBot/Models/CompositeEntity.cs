using Newtonsoft.Json;
using System.Collections.Generic;

namespace MealChatBot.Models
{
    /// <summary>
    /// Composite entity class 
    /// </summary>
    public class CompositeEntity
    {
        [JsonProperty(PropertyName = "parentType")]
        public string ParentType { get; set; }

        [JsonProperty(PropertyName = "value")]
        public string value { get; set; }

        [JsonProperty(PropertyName = "children")]
        public IList<CompositeChild> Children { get; set; }
    }

    /// <summary>
    /// The composite child class
    /// </summary>
    public class CompositeChild
    {
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }
    }
}
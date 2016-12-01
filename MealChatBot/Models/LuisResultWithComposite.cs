using Microsoft.Bot.Builder.Luis.Models;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MealChatBot.Models
{
    public class LuisResultWithComposite : LuisResult
    {
        /// <summary>
        /// Initializes a new instance of the LuisResult class.
        /// </summary>
        public LuisResultWithComposite(string query, IList<IntentRecommendation> intents, IList<EntityRecommendation> entities, IList<CompositeEntity> compositeEntities)
        {
            Query = query;
            Intents = intents;
            Entities = entities;
            CompositeEntities = compositeEntities;
        }

        /// <summary>
        /// The composite entities found in the result.
        /// </summary>
        [JsonProperty(PropertyName = "compositeEntities")]
        public IList<CompositeEntity> CompositeEntities { get; set; }
    }
}
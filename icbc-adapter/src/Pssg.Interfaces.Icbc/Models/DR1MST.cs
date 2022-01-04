// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Pssg.Interfaces.Icbc.Models
{
    using Microsoft.Rest;
    using Microsoft.Rest.Serialization;
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public partial class DR1MST
    {
        /// <summary>
        /// Initializes a new instance of the DR1MST class.
        /// </summary>
        public DR1MST()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the DR1MST class.
        /// </summary>
        public DR1MST(int? mSCD = default(int?), IList<int?> rSCD = default(IList<int?>), System.DateTime? rRDT = default(System.DateTime?), int? lNUM = default(int?), DR1STAT dR1STAT = default(DR1STAT), int? lCLS = default(int?), IList<DR1MEDNITEM> dR1MEDN = default(IList<DR1MEDNITEM>))
        {
            MSCD = mSCD;
            RSCD = rSCD;
            RRDT = rRDT;
            LNUM = lNUM;
            DR1STAT = dR1STAT;
            LCLS = lCLS;
            DR1MEDN = dR1MEDN;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "MSCD")]
        public int? MSCD { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "RSCD")]
        public IList<int?> RSCD { get; set; }

        /// <summary>
        /// </summary>
        [JsonConverter(typeof(DateJsonConverter))]
        [JsonProperty(PropertyName = "RRDT")]
        public System.DateTime? RRDT { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "LNUM")]
        public int? LNUM { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "DR1STAT")]
        public DR1STAT DR1STAT { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "LCLS")]
        public int? LCLS { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "DR1MEDN")]
        public IList<DR1MEDNITEM> DR1MEDN { get; set; }

    }
}
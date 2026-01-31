using App.Modules.Base.Substrate.Models.Messages._TOREVIEW.Entities.Enums;

namespace App.Modules.Base.Substrate.Attributes
{
    /// <summary>
    /// Attribute to attach to Models to *Hint* (it depends on additional factors too, but it's a start) 
    /// as to the risks associated to exposure.
    /// </summary>
    /// <remarks>
    /// Construtor
    /// </remarks>
    /// <param name="dataClassification"></param>
#pragma warning disable CA1711 // Identifiers should not have incorrect suffix
    public class DataClassificationAttribute(NZDataClassification dataClassification)
#pragma warning restore CA1711 // Identifiers should not have incorrect suffix
    {

        /// <summary>
        /// Get/Set the DataClassification of the type.
        /// </summary>
        public NZDataClassification DataClassification { get; set; } = dataClassification;
    }
}

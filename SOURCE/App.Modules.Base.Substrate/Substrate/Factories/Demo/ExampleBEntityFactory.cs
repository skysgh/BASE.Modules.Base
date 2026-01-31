using App.Modules.Base.Substrate.Attributes;
using App.Modules.Base.Substrate.ExtensionMethods;
using App.Modules.Base.Substrate.Models.Entities.Demos;
using App.Modules.Base.Substrate.Contracts.Models.Enums;

namespace App.Modules.Base.Substrate.Factories.Demo
{


    /// <summary>
    /// Static Factory to develop simple
    /// <see cref="ExampleBEntity"/> entities
    /// that have scalar properties, and
    /// child single referenced/category records and
    /// collections of records
    /// </summary>
    [ForDemoOnly]
    internal static class ExampleBEntityFactory
    {

        /// <summary>
        /// Static method to build a
        /// <see cref="ExampleBEntity"/>
        /// and return it.
        /// </summary>
        /// <param name="index"></param>
        public static ExampleBEntity Build(int index)
        {
            ExampleBReferenceTypeEntity categoryRecord = ExampleCEntityFactory.Build(index);


            ExampleBEntity result = new()
            {
                //Timestamp
                RecordState = RecordPersistenceState.Active,
                Id = index.ToGuid(),
                CreatedByPrincipalId = "{P-whatever}",
                CreatedOnUtc = DateTime.UtcNow,
                LastModifiedByPrincipalId = "{P-whatever}",
                LastModifiedOnUtc = DateTime.UtcNow,
                //DeletedByPrincipalId  
                //DeletedOnUtc
                Title = "Some Title...",
                Description = "Some Description...",
                //----- 
                SingleProperty = categoryRecord,
                SinglePropertyFK = categoryRecord.Id,
                //----- 
            };

            return result;

        }
    }
}

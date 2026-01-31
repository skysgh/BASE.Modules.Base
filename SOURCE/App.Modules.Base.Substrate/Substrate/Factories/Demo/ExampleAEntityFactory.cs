using App.Modules.Base.Substrate.Attributes;
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Text;
// using System.Threading.Tasks;
using App.Modules.Base.Substrate.ExtensionMethods;
using App.Modules.Base.Substrate.Models.Entities.Demos;
using App.Modules.Base.Substrate.Contracts.Models.Enums;
// using App.Modules.Base.Substrate.Attributes;

namespace App.Modules.Base.Substrate.Factories.Demo
{
    /// <summary>
    /// Static Factory to develop simple
    /// <see cref="ExampleAEntity"/> entities
    /// that have just scalar properties, and no
    /// child records or collections of records
    /// </summary>
    [ForDemoOnly]
    internal static partial class ExampleAEntityFactory
    {

        /// <summary>
        /// Static method to build a
        /// <see cref="ExampleAEntity"/>
        /// and return it.
        /// </summary>
        /// <param name="index"></param>
        public static ExampleAEntity Build(int index)
        {
            ExampleAEntity result = new()
            {
                RecordState = RecordPersistenceState.Active,
                Id = index.ToGuid(),
                Title = "Some Displayable Text",
                Description = "Some Description of the item...blah, blah...",
            };
            return result;
        }
    }
}

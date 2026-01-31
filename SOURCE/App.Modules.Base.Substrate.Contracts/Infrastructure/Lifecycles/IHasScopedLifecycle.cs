using System;
using System.Collections.Generic;
using System.Text;

namespace App.Modules.Base.Infrastructure.Lifecycles
{
    /// <summary>
    /// A Scoped version of <see cref="IHasLifecycle"/>.
    /// </summary>
    public interface IHasScopedLifecycle: IHasLifecycle
    {
    }
}

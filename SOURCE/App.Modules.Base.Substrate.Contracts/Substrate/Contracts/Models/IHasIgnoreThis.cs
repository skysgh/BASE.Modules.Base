namespace App.Modules.Base.Substrate.Contracts.Models
{
    /// <summary>
    /// Contract that can be attached to classes 
    /// that need to be ignored (eg: by Reflection)
    /// <para>
    /// Have used to ignore some Entity Framework Schema 
    /// definitions, etc.
    /// </para>
    /// <para>
    /// TODO: But it's not dynamic enough (ie, one should
    /// be able to configure what exatly is ignored by using 
    /// an app.setting or some other flag that specifies
    /// type *prefix* to ignore... 
    /// </para>
    /// </summary>
    public interface IHasIgnoreThis
    {
    }
}

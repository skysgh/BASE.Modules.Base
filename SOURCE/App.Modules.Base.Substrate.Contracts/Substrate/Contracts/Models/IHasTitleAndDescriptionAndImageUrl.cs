namespace App.Modules.Base.Substrate.Contracts.Models
{
    /// <summary>
    /// Contract for displable objects that have a 
    /// displayable Title and 
    /// Description 
    /// <para>
    /// Note that it is intentional that it not 
    /// inherit from <c>IHasDescriptionNullable</c>
    /// as Descrptions on list items
    /// decrease Training and Tech Support costs.
    /// </para>
    /// <para>
    /// (And icons make them look better too).
    /// </para>
    /// </summary>
    public interface IHasTitleAndDescriptionAndImageUrlAndDisplayHints :
        IHasTitleAndDescription,
        IHasImageUrl,
        IHasDisplayHints
    {

    }

}
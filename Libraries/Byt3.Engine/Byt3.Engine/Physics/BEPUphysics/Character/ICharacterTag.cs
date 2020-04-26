namespace Byt3.Engine.Physics.BEPUphysics.Character
{
    /// <summary>
    /// Defines a class which uniquely identifies a character.
    /// Exposes an identifier for use in ordering character locks to ensure multithreaded safety.
    /// </summary>
    public interface ICharacterTag
    {
        /// <summary>
        /// Gets the unique instance identifier for this character.
        /// </summary>
        long InstanceId { get; }
    }
}
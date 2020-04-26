using Byt3.Engine.Physics.BEPUphysics.Entities;

namespace Byt3.Engine.Physics.BEPUphysics.Character
{
    /// <summary>
    /// Links the body of the character to the character controller for locking.  Default implementation of the ICharacterTag.
    /// </summary>
    public class CharacterSynchronizer : ICharacterTag
    {
        private readonly Entity body;

        /// <summary>
        /// Constructs a new character tag.
        /// </summary>
        /// <param name="body">Body of the character.</param>
        public CharacterSynchronizer(Entity body)
        {
            this.body = body;
        }

        /// <summary>
        /// Gets the unique instance identifier for this character.
        /// </summary>
        public long InstanceId => body.InstanceId;
    }
}
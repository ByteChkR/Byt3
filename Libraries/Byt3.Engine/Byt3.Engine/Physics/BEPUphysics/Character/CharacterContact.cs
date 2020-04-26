using Byt3.Engine.Physics.BEPUphysics.BroadPhaseEntries;
using Byt3.Engine.Physics.BEPUphysics.CollisionTests;

namespace Byt3.Engine.Physics.BEPUphysics.Character
{
    /// <summary>
    /// A contact generated between a character and a stored collidable.
    /// </summary>
    public struct CharacterContact
    {
        /// <summary>
        /// Core information about the contact.
        /// </summary>
        public ContactData Contact;

        /// <summary>
        /// Object that created this contact with the character.
        /// </summary>
        public Collidable Collidable;
    }
}
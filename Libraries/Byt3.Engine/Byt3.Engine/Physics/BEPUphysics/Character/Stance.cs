namespace Byt3.Engine.Physics.BEPUphysics.Character
{
    /// <summary>
    /// Stance of a cylindrical character.
    /// </summary>
    public enum Stance
    {
        /// <summary>
        /// Tallest stance.
        /// </summary>
        Standing,

        /// <summary>
        /// Middle-height stance; must be taller than prone and shorter than standing.
        /// </summary>
        Crouching,

        /// <summary>
        /// The shortest stance; the character is essentially on the ground. Note that the width and length of the character do not change while prone.
        /// </summary>
        Prone
    }
}
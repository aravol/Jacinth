using System;

namespace Jacinth.Entities
{
    /// <summary>
    /// Handles the generation of specific types of SubEntities
    /// </summary>
    public static class SubEntityFactory
    {
        #region Delegates

        /// <summary>
        /// Delegate used to attempt generation of a SubEntity
        /// </summary>
        /// <typeparam name="T">The Type of SubEntity to create</typeparam>
        /// <param name="entity">The Entity against which to create this SubEntity</param>
        /// <param name="subEntity">The resulting SubEntity</param>
        /// <returns>True if this type of SubEntity if valid against the given Entity, False otherwise</returns>
        public delegate bool TryGenerateSubEntityHandler<T>(Entity entity, out T subEntity) where T : SubEntity;

        #endregion

        #region Subclass

        private static class GenericFactory<T>
            where T : SubEntity
        {
            private static TryGenerateSubEntityHandler<T> _generationMethod = DefaultGenerationMethod;

            public static TryGenerateSubEntityHandler<T> GenerationMethod
            {
                get { return _generationMethod; }
                set
                {
                    if (_generationMethod == DefaultGenerationMethod) _generationMethod = value;
                    else throw new InvalidOperationException("SubEntity Generation has already been registered for type " + typeof(T).Name);
                }
            }

            private static bool DefaultGenerationMethod(Entity entity, out T subEntity)
            {
                throw new NotImplementedException("SubEntity Generation has not been registered for type " + typeof(T).Name);
            }
        }
        #endregion

        #region Methods

        /// <summary>
        /// Registers the generation method for a given type of SubEntity
        /// </summary>
        /// <typeparam name="T">The Type of SubEntity to register for</typeparam>
        /// <param name="method">The method to use for generation</param>
        /// <exception cref="InvalidOperationException">Throws an exception if this type already has a registered generation method</exception>
        public static void RegisterSubEntityFactory<T>(TryGenerateSubEntityHandler<T> method)
            where T : SubEntity
        {
            GenericFactory<T>.GenerationMethod = method;
        }

        internal static bool TryGenerateSubEntity<T>(Entity entity, out T subEntity)
            where T : SubEntity
        {
            return GenericFactory<T>.GenerationMethod.Invoke(entity, out subEntity);
        }
        #endregion
    }
}

namespace SharpDev.Messaging
{
    public enum TranslationResultType
    {
        /// <summary>
        /// Command has been translated correctly
        /// </summary>
        Ok, // Continue ... OK or Accepted
        /// <summary>
        /// A new version of this command is available, although given version is still supported and translated
        /// </summary>
        NewVersionSuggested, // Continue ... Ok or Accepted with new version information included in response
        /// <summary>
        /// Request body could not be parsed correctly
        /// </summary>
        BadRequest, // BadRequest(ModelState)
        /// <summary>
        /// Command was not found or invalid version was given
        /// </summary>
        NotFound,
        /// <summary>
        /// Command was recognized but its version is no longer supported. If a new version is available, it will be indicated in the response body
        /// </summary>
        Obsolete,

    }
}
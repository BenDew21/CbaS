namespace CBaSCore.Framework.Business
{
    /// <summary>
    /// ProgressChangedObject - Object passed between BaseProgressAwareTasks and the calling thread
    /// </summary>
    public class ProgressChangedObject
    {
        /// <summary>
        /// The message to send back to the calling thread
        /// </summary>
        public string Message { get; set; }
        
        /// <summary>
        /// An action to execute on the calling thread
        /// </summary>
        public ThreadAction Action { get; set; }
    }
}
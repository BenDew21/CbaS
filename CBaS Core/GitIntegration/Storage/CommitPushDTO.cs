using System.Collections.Generic;

namespace CBaSCore.GitIntegration.Storage
{
    /// <summary>
    /// CommitPushDTO - DTO passed to the CommitPush task containing required details
    /// </summary>
    public class CommitPushDTO
    {
        /// <summary>
        /// The repository path
        /// </summary>
        public string RepoPath { get; set; }
        
        /// <summary>
        /// The files to commit
        /// </summary>
        public List<string> ChangedFiles { get; set; } = new();
        
        /// <summary>
        /// The commit message
        /// </summary>
        public string CommitMessage { get; set; }
        
        /// <summary>
        /// Whether the task should push the changes to the remote
        /// </summary>
        public bool ShouldPush { get; set; }
    }
}
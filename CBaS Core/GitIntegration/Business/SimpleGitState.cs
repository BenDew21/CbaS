using System;
using LibGit2Sharp;

namespace CBaSCore.GitIntegration.Business
{
    /// <summary>
    /// SimpleGitState - Simple states a file can be in
    /// </summary>
    public enum SimpleGitState
    {
        Created,
        Modified,
        Removed,
        NoChange
    }

    /// <summary>
    /// SimpleGitStateHelper - Convert LibGit2Sharp FileStatus to a SimpleGitState
    /// </summary>
    public static class SimpleGitStateHelper
    {
        /// <summary>
        /// Get the SimpleGitState for a given FileStatus
        /// </summary>
        /// <param name="state">The FileStatus</param>
        /// <returns>The associated SimpleGitState</returns>
        public static SimpleGitState GetState(FileStatus state)
        {
            switch (state)
            {
                case FileStatus.NewInIndex:
                case FileStatus.NewInWorkdir:
                    return SimpleGitState.Created;
                case FileStatus.ModifiedInIndex:
                case FileStatus.ModifiedInWorkdir:
                    return SimpleGitState.Modified;
                case FileStatus.DeletedFromIndex:
                case FileStatus.DeletedFromWorkdir:
                    return SimpleGitState.Removed;
                default:
                    return SimpleGitState.NoChange;
            }
        }
    }
}
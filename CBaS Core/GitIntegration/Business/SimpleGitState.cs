using System;
using LibGit2Sharp;

namespace CBaSCore.GitIntegration.Business
{
    public enum SimpleGitState
    {
        Created,
        Modified,
        Removed,
        NoChange
    }

    public static class SimpleGitStateHelper
    {
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
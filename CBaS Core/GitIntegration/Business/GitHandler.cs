using System;
using System.Collections.Generic;
using System.Diagnostics;
using CBaSCore.Framework.Business;
using CBaSCore.GitIntegration.Storage;
using CBaSCore.GitIntegration.UI;
using CBaSCore.GitIntegration.UI.Nodes;
using LibGit2Sharp;
using Microsoft.Alm.Authentication;

namespace CBaSCore.GitIntegration.Business
{
    /// <summary>
    /// GitHandler - Singleton class to act as entry point for all Git related actions
    /// </summary>
    public class GitHandler
    {
        /// <summary>
        /// Static reference for this class
        /// </summary>
        private static GitHandler _instance = null;

        /// <summary>
        /// The Git control
        /// </summary>
        private GitDockContent _control;
        
        /// <summary>
        /// The Git console control
        /// </summary>
        private GitConsoleControl _console;
        
        /// <summary>
        /// The repository path
        /// </summary>
        private string _repoPath;
        
        /// <summary>
        /// Get the singleton instance
        /// </summary>
        /// <returns>The singleton instance</returns>
        public static GitHandler GetInstance()
        {
            return _instance ??= new GitHandler();
        }

        /// <summary>
        /// Set the GitDockContent control
        /// </summary>
        /// <param name="control">The GitDockContent control</param>
        public void SetControl(GitDockContent control)
        {
            _control = control;
        }

        /// <summary>
        /// Set the GitConsoleControl
        /// </summary>
        /// <param name="control">The GitConsoleControl</param>
        public void SetConsoleControl(GitConsoleControl control)
        {
            _console = control;
        }

        /// <summary>
        /// Set the repository path
        /// </summary>
        /// <param name="path">The repository path</param>
        public void SetRepository(string path)
        {
            _repoPath = path;
            _console.Log("Setting repository to: " + path);
            
            UpdateChanges();
        }

        /// <summary>
        /// Log a given message to the console
        /// </summary>
        /// <param name="message">The message to log</param>
        public void LogToConsole(string message)
        {
            _console.Log(message);
        }
        
        /// <summary>
        /// Update the git changes
        /// </summary>
        public void UpdateChanges()
        {
            var defaultChangelist = new List<BaseGitNode>();
            var unversionedFiles = new List<BaseGitNode>();
            
            using (var repo = new Repository(_repoPath))
            {
                foreach (var item in repo.RetrieveStatus())
                {
                    // Convert the FileState into a SimpleGitState
                    var gitStatus = SimpleGitStateHelper.GetState(item.State);
                    var node = new BaseGitNode(item.FilePath, false, gitStatus);

                    if (gitStatus == SimpleGitState.Created)
                    {
                        unversionedFiles.Add(node);
                    }
                    else
                    {
                        defaultChangelist.Add(node);
                    }
                }
            }
            
            // Tell the control to update the change lists visually
            _control.UpdateChanges(defaultChangelist, unversionedFiles);
        }

        /// <summary>
        /// Commit/Push the provided changes
        /// </summary>
        /// <param name="changes">List of changed files</param>
        /// <param name="commitMessage">The commit message</param>
        /// <param name="shouldPush">Whether the commit should be pushed to the remote</param>
        public void CommitChanges(List<string> changes, string commitMessage, bool shouldPush)
        {
            var dto = new CommitPushDTO()
            {
                RepoPath = _repoPath,
                ChangedFiles = changes,
                CommitMessage = commitMessage,
                ShouldPush = shouldPush
            };

            var task = new CommitPushTask();
            task.Run(dto);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CBaSCore.GitIntegration.UI;
using CBaSCore.GitIntegration.UI.Nodes;
using CBaSCore.Project.Resources;
using LibGit2Sharp;
using LibGit2Sharp.Handlers;
using Microsoft.Alm.Authentication;

namespace CBaSCore.GitIntegration.Business
{
    public class GitHandler
    {
        private static GitHandler _instance = null;

        private GitDockContent _control;

        private GitConsoleControl _console;
        
        private string _repoPath;
        
        public static GitHandler GetInstance()
        {
            return _instance ??= new GitHandler();
        }

        public void SetControl(GitDockContent control)
        {
            _control = control;
        }

        public void SetConsoleControl(GitConsoleControl control)
        {
            _console = control;
        }

        /// <summary>
        /// DeletedFromWorkdir = Red, Changes
        /// ModifiedInWorkdir = Blue, Changes
        /// NewInWorkdir = Green, Unversioned
        /// </summary>
        /// <param name="path"></param>
        public void SetRepository(string path)
        {
            _repoPath = path;
            _console.Log("Setting repository to: " + path);
            
            UpdateChanges();
        }

        public void CreateTest()
        {
            if (!Repository.IsValid(_repoPath))
            {
                Debug.WriteLine("Path not valid");
                return;
            }
            using (var repo = new Repository(_repoPath))
            {
                var commit = repo.Head.Tip;
                Debug.WriteLine("");
                Debug.WriteLine("Author: " + commit.Author.Name);
                Debug.WriteLine("Message: " + commit.MessageShort);
                Debug.WriteLine("");
                foreach (var item in repo.RetrieveStatus())
                {
                    Debug.WriteLine("Path: " + item.FilePath);
                    Debug.WriteLine("State: " + item.State);
                    Debug.WriteLine("HeadToIndexRenameDetails: " + item.HeadToIndexRenameDetails);
                    Debug.WriteLine("IndexToWorkDirRenameDetails: " + item.IndexToWorkDirRenameDetails);

                    Debug.WriteLine("");
                }
            }
        }
        
        public void UpdateChanges()
        {
            var defaultChangelist = new List<BaseGitNode>();
            var unversionedFiles = new List<BaseGitNode>();
            
            using (var repo = new Repository(_repoPath))
            {
                foreach (var item in repo.RetrieveStatus())
                {
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
            
            _control.UpdateChanges(defaultChangelist, unversionedFiles);
        }

        public void CommitChanges(List<string> changes, string commitMessage, bool shouldPush)
        {
            using (var repo = new Repository(_repoPath))
            {
                try
                {
                    Commands.Stage(repo, changes);
                    var config = repo.Config;
                    var author = config.BuildSignature(DateTimeOffset.Now);

                    repo.Commit(commitMessage, author, author);
                    if (shouldPush)
                    {
                        var secrets = new SecretStore("git");
                        var auth = new BasicAuthentication(secrets);
                        var creds = auth.GetCredentials(new TargetUri("https://github.com"));
                    
                        var remote = repo.Network.Remotes["origin"];
                        var options = new PushOptions();

                        options.CredentialsProvider = (url, fromUrl, types) => new UsernamePasswordCredentials
                        {
                            Username = creds.Username,
                            Password = creds.Password
                        };

                        var pushRefSpec = repo.Head.CanonicalName;
                        repo.Network.Push(remote, pushRefSpec, options);
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }
            }
        }
    }
}